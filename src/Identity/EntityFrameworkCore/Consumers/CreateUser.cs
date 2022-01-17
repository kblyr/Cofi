using Cofi.Identity.Utilities;

namespace Cofi.Identity.Consumers;

sealed class CreateUserConsumer : IConsumer<CreateUser>
{
    readonly IDbContextFactory<UserDbContext> _contextFactory;
    readonly IMapper _mapper;
    readonly IBus _bus;
    readonly IUserPasswordCryptoHash _passwordHash;
    readonly ICurrentAuditInfoProvider _currentAuditInfoProvider;

    public CreateUserConsumer(IDbContextFactory<UserDbContext> contextFactory, IMapper mapper, IBus bus, IUserPasswordCryptoHash passwordHash, ICurrentAuditInfoProvider currentAuditInfoProvider)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
        _bus = bus;
        _passwordHash = passwordHash;
        _currentAuditInfoProvider = currentAuditInfoProvider;
    }

    public async Task Consume(ConsumeContext<CreateUser> context)
    {
        using var dbContext = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        using var transaction = await dbContext.Database.BeginTransactionAsync().ConfigureAwait(false);
        var auditInfo = await _currentAuditInfoProvider.GetCurrent().ConfigureAwait(false);

        if (await dbContext.Users.UsernameExists(context.Message.Username).ConfigureAwait(false))
        {
            var usernameAlreadyExists = _mapper.Map<CreateUser, UsernameAlreadyExists>(context.Message);
            var response = _mapper.Map<UsernameAlreadyExists, CreateUserFailed>(usernameAlreadyExists);
            await context.RespondAsync(response).ConfigureAwait(false);
            return;
        }

        if (await dbContext.Users.EmailAddressExists(context.Message.EmailAddress).ConfigureAwait(false))
        {
            var emailAddressAlreadyExists = _mapper.Map<CreateUser, UserEmailAddressAlreadyExists>(context.Message);
            var response = _mapper.Map<UserEmailAddressAlreadyExists, CreateUserFailed>(emailAddressAlreadyExists);
            await context.RespondAsync(response).ConfigureAwait(false);
            return;
        }

        var user = _mapper.Map<CreateUser, User>(context.Message) with 
        { 
            HashedPassword = _passwordHash.ComputeHash(context.Message.Password),
            InsertedById = auditInfo.UserId,
            InsertedOn = auditInfo.Timestamp
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        await transaction.CommitAsync().ConfigureAwait(false);

        var userCreated = _mapper.Map<User, UserCreated>(user);
        await _bus.Publish(userCreated).ConfigureAwait(false);
    }
}