using Cofi.Identity.Utilities;

namespace Cofi.Identity.Consumers;

sealed class CreateUserConsumer : IConsumer<CreateUser>
{
    readonly ILogger<CreateUserConsumer> _logger;
    readonly IDbContextFactory<UserDbContext> _contextFactory;
    readonly IMapper _mapper;
    readonly IBus _bus;
    readonly IUserPasswordCryptoHash _passwordHash;
    readonly ICurrentAuditInfoProvider _currentAuditInfoProvider;

    public CreateUserConsumer(ILogger<CreateUserConsumer> logger, IDbContextFactory<UserDbContext> contextFactory, IMapper mapper, IBus bus, IUserPasswordCryptoHash passwordHash, ICurrentAuditInfoProvider currentAuditInfoProvider)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _mapper = mapper;
        _bus = bus;
        _passwordHash = passwordHash;
        _currentAuditInfoProvider = currentAuditInfoProvider;
    }

    public async Task Consume(ConsumeContext<CreateUser> context)
    {
        _logger.DbContext().Creating<UserDbContext>();
        using var dbContext = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        using var transaction = await dbContext.Database.BeginTransactionAsync().ConfigureAwait(false);
        var auditInfo = await _currentAuditInfoProvider.GetCurrent().ConfigureAwait(false);

        _logger.TryLogTrace("Checking if username already exists");
        if (await dbContext.Users.UsernameExists(context.Message.Username).ConfigureAwait(false))
        {
            _logger.TryLogTrace("Username already exists");
            var usernameAlreadyExists = _mapper.Map<CreateUser, UsernameAlreadyExists>(context.Message);
            var response = _mapper.Map<UsernameAlreadyExists, CreateUserFailed>(usernameAlreadyExists);
            await context.RespondAsync(response).ConfigureAwait(false);
            return;
        }

        _logger.TryLogTrace("Checking if user email address already exists");
        if (await dbContext.Users.EmailAddressExists(context.Message.EmailAddress).ConfigureAwait(false))
        {
            _logger.TryLogTrace("User email address already exists");
            var emailAddressAlreadyExists = _mapper.Map<CreateUser, UserEmailAddressAlreadyExists>(context.Message);
            var response = _mapper.Map<UserEmailAddressAlreadyExists, CreateUserFailed>(emailAddressAlreadyExists);
            await context.RespondAsync(response).ConfigureAwait(false);
            return;
        }

        var user = _mapper.Map<CreateUser, User>(context.Message);
        user = user with 
        { 
            HashedPassword = _passwordHash.ComputeHash(context.Message.Password),
            InsertedById = auditInfo.UserId,
            InsertedOn = auditInfo.Timestamp
        };

        dbContext.Users.Add(user);
        _logger.DbContext().SavingChanges();
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

        _logger.DbContext().CommittingTransaction();
        await transaction.CommitAsync().ConfigureAwait(false);

        var userCreated = _mapper.Map<User, UserCreated>(user);
        await _bus.Publish(userCreated).ConfigureAwait(false);
    }
}