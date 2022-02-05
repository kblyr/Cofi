using AutoMapper;
using Cofi.Auditing;
using Cofi.Identity.Utilities;
using Cofi.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Cofi.Identity.Handlers;

sealed class SignupUser_Handler : RequestHandler<SignupUser>
{
    readonly IDbContextFactory<DatabaseContext> _contextFactory;
    readonly IMapper _mapper;
    readonly IUserPasswordHash _passwordHash;
    readonly ICurrentAuditInfoProvider _currentAuditInfoProvider;
    readonly MessageBusAdapter _bus;

    public SignupUser_Handler(IDbContextFactory<DatabaseContext> contextFactory, IMapper mapper, IUserPasswordHash passwordHash, ICurrentAuditInfoProvider currentAuditInfoProvider, MessageBusAdapter bus)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
        _passwordHash = passwordHash;
        _currentAuditInfoProvider = currentAuditInfoProvider;
        _bus = bus;
    }

    public async Task<Response> Handle(SignupUser request, CancellationToken cancellationToken)
    {
        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        if (await DoesUsernameExists(context, request.Username, cancellationToken).ConfigureAwait(false))
        {
            return new UsernameAlreadyExists(request.Username);
        }

        var auditInfo = _currentAuditInfoProvider.Current;
        var user = _mapper.Map<SignupUser, User>(request);
        user.HashedPassword = _passwordHash.ComputeHash(request.Password);
        user.Status = UserStatus.Pending;
        user.IsPasswordChangeRequired = false;
        user.IsDeleted = false;
        user.InsertedById = auditInfo.UserId;
        user.InsertedOn = auditInfo.Timestamp;
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await _bus.Publish(() => _mapper.Map<User, UserCreated>(user), cancellationToken).ConfigureAwait(false);

        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        return new SignupUser.Response(user.Id);
    }

    static async Task<bool> DoesUsernameExists(DatabaseContext context, string username, CancellationToken cancellationToken) => await context.Users
        .AsNoTracking()
        .Where(user =>
            !user.IsDeleted
            && user.Username == username
        )
        .AnyAsync(cancellationToken)
        .ConfigureAwait(false);
}
