using AutoMapper;
using Cofi.Auditing;
using Cofi.Identity.Utilities;
using Cofi.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Cofi.Identity.Handlers;

sealed class CreateUser_Handler : CofiRequestHandler<CreateUser>
{
    readonly IDbContextFactory<DatabaseContext> _contextFactory;
    readonly ICurrentAuditInfoProvider _currentAuditInfoProvider;
    readonly IMapper _mapper;
    readonly IUserPasswordHash _passwordHash;
    readonly MessageBusAdapter _bus;

    public CreateUser_Handler(IDbContextFactory<DatabaseContext> contextFactory, ICurrentAuditInfoProvider currentAuditInfoProvider, IMapper mapper, IUserPasswordHash passwordHash, MessageBusAdapter bus)
    {
        _contextFactory = contextFactory;
        _currentAuditInfoProvider = currentAuditInfoProvider;
        _mapper = mapper;
        _passwordHash = passwordHash;
        _bus = bus;
    }

    public async Task<CofiResponse> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        var auditInfo = _currentAuditInfoProvider.Current;

        var (user, insertUserFailed) = await InsertUser(context, request, auditInfo, cancellationToken).ConfigureAwait(false);

        if (insertUserFailed is not null)
            return insertUserFailed;

        var (userDomains, insertUserDomainsFailed) = await InsertUserDomains(context, user, request.DomainIds, auditInfo, cancellationToken).ConfigureAwait(false);

        if (insertUserDomainsFailed is not null)
            return insertUserDomainsFailed;

        var (userRoles, insertUserRolesFailed) = await InsertUserRoles(context, user, request.RoleIds, auditInfo, cancellationToken).ConfigureAwait(false);

        if (insertUserRolesFailed is not null)
            return insertUserRolesFailed;

        var (userPermissions, insertUserPermissionsFailed) = await InsertUserPermissions(context, user, request.PermissionIds, auditInfo, cancellationToken).ConfigureAwait(false);

        if (insertUserPermissionsFailed is not null)
            return insertUserPermissionsFailed;

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await _bus.Publish(() => _mapper.Map<User, UserCreated>(user), cancellationToken).ConfigureAwait(false);

        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        return new CreateUser.Response(user.Id);
    }

    async Task<(User, FailedResponse?)> InsertUser(DatabaseContext context, CreateUser request, AuditInfo auditInfo, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<CreateUser, User>(request);

        if (await DoesUsernameExists(context, user.Username, cancellationToken).ConfigureAwait(false))
        {
            var usernameAlreadyExists = new UsernameAlreadyExists(request.Username);
            return (user, usernameAlreadyExists);
        }

        user.HashedPassword = _passwordHash.ComputeHash(request.Password);
        user.IsDeleted = false;
        user.InsertedById = auditInfo.UserId;
        user.InsertedOn = auditInfo.Timestamp;
        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return (user, null);
    }

    async Task<(IEnumerable<UserDomain>, FailedResponse?)> InsertUserDomains(DatabaseContext context, User user, IEnumerable<short> domainIds, AuditInfo auditInfo, CancellationToken cancellationToken)
    {
        if (domainIds is null)
            return (Enumerable.Empty<UserDomain>(), null);

        var userDomains = new List<UserDomain>();

        foreach(var domainId in domainIds)
        {
            var domain = await GetDomain(context, domainId, cancellationToken).ConfigureAwait(false);

            if (domain is null)
            {
                var domainNotFound = new DomainNotFound(domainId);
                return (userDomains, domainNotFound);
            }

            var userDomain = new UserDomain
            {
                UserId = user.Id,
                DomainId = domainId,
                IsDeleted = false,
                InsertedById = auditInfo.UserId,
                InsertedOn = auditInfo.Timestamp
            };
            context.UserDomains.Add(userDomain);
            userDomains.Add(userDomain);
        }

        return (userDomains, null);
    }

    async Task<(IEnumerable<UserRole>, FailedResponse?)> InsertUserRoles(DatabaseContext context, User user, IEnumerable<int> roleIds, AuditInfo auditInfo, CancellationToken cancellationToken)
    {
        if (roleIds is null)
            return (Enumerable.Empty<UserRole>(), null);

        var userRoles = new List<UserRole>();

        foreach(var roleId in roleIds)
        {
            var role = await GetRole(context, roleId, cancellationToken).ConfigureAwait(false);

            if (role is null)
            {
                var roleNotFound = new RoleNotFound(roleId);
                return (userRoles, roleNotFound);
            }

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = roleId,
                IsDeleted = false,
                InsertedById = auditInfo.UserId,
                InsertedOn = auditInfo.Timestamp,
            };
            context.UserRoles.Add(userRole);
            userRoles.Add(userRole);
        }

        return (userRoles, null);
    }

    async Task<(IEnumerable<UserPermission>, FailedResponse?)> InsertUserPermissions(DatabaseContext context, User user, IEnumerable<int> permissionIds, AuditInfo auditInfo, CancellationToken cancellationToken)
    {
        if (permissionIds is null)
            return (Enumerable.Empty<UserPermission>(), null);

        var userPermissions = new List<UserPermission>();

        foreach (var permissionId in permissionIds)
        {
            var permission = await GetPermission(context, permissionId, cancellationToken).ConfigureAwait(false);

            if (permission is null)
            {
                var permissionNotFound = new PermissionNotFound(permissionId);
                return (userPermissions, permissionNotFound);
            }

            var userPermission = new UserPermission
            {
                UserId = user.Id,
                PermissionId = permissionId,
                IsDeleted = false,
                InsertedById = auditInfo.UserId,
                InsertedOn = auditInfo.Timestamp
            };
            context.UserPermissions.Add(userPermission);
            userPermissions.Add(userPermission);
        }

        return (userPermissions, null);
    }

    static async Task<bool> DoesUsernameExists(DatabaseContext context, string username, CancellationToken cancellationToken) => await context.Users
        .AsNoTracking()
        .Where(user => !user.IsDeleted && user.Username == username)
        .AnyAsync(cancellationToken)
        .ConfigureAwait(false);

    static async Task<Domain?> GetDomain(DatabaseContext context, short id, CancellationToken cancellationToken) => await context.Domains
        .AsNoTracking()
        .Where(domain => !domain.IsDeleted && domain.Id == id)
        .Select(domain => new Domain { Id = domain.Id })
        .SingleOrDefaultAsync(cancellationToken)
        .ConfigureAwait(false);

    static async Task<Role?> GetRole(DatabaseContext context, int id, CancellationToken cancellationToken) => await context.Roles
        .AsNoTracking()
        .Where(role => !role.IsDeleted && role.Id == id)
        .Select(role => new Role { Id = role.Id })
        .SingleOrDefaultAsync(cancellationToken)
        .ConfigureAwait(false);

    static async Task<Permission?> GetPermission(DatabaseContext context, int id, CancellationToken cancellationToken) => await context.Permissions
        .AsNoTracking()
        .Where(permission => !permission.IsDeleted && permission.Id == id)
        .Select(permission => new Permission { Id = permission.Id })
        .SingleOrDefaultAsync(cancellationToken)
        .ConfigureAwait(false);
}
