using Cofi.Identity.Utilities;

namespace Cofi.Identity.Handlers;

sealed class SignupUser_Handler : CofiRequestHandler<SignupUser>
{
    readonly IDbContextFactory<UserDbContext> _contextFactory;
    readonly ICurrentAuditInfoProvider _currentAuditInfoProvider;
    readonly IUserPasswordHash _passwordHash;
    readonly IMapper _mapper;
    readonly MessageBusAdapter _bus;

    public SignupUser_Handler(IDbContextFactory<UserDbContext> contextFactory, ICurrentAuditInfoProvider currentAuditInfoProvider, IUserPasswordHash passwordHash, IMapper mapper, MessageBusAdapter bus)
    {
        _contextFactory = contextFactory;
        _currentAuditInfoProvider = currentAuditInfoProvider;
        _passwordHash = passwordHash;
        _mapper = mapper;
        _bus = bus;
    }

    public async Task<CofiResponse> Handle(SignupUser request, CancellationToken cancellationToken)
    {
        using var context = await _contextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        if (await DoesUsernameExists(context, request.Username, cancellationToken).ConfigureAwait(false))
            return new UsernameAlreadyExists(request.Username);

        var auditInfo = _currentAuditInfoProvider.Current;

        var user = _mapper.Map<SignupUser, User>(request);
        user.HashedPassword = _passwordHash.ComputeHash(request.Password);
        user.IsActive = false;
        user.IsDeleted = false;
        user.InsertedById = auditInfo.UserId;
        user.InsertedOn = auditInfo.Timestamp;

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

        await _bus.Publish(_mapper.Map<User, UserCreated>(user), cancellationToken).ConfigureAwait(false);

        return new SignupUser.Response(user.Id);
    }

    static async Task<bool> DoesUsernameExists(UserDbContext context, string username, CancellationToken cancellationToken) => await context.Users
        .AsNoTracking()
        .AnyAsync(cancellationToken)
        .ConfigureAwait(false);
}