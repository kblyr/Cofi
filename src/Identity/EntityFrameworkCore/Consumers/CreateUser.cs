namespace Cofi.Identity.Consumers;

sealed class CreateUserConsumer : IConsumer<CreateUser>
{
    readonly ILogger<CreateUserConsumer> _logger;
    readonly IDbContextFactory<UserDbContext> _contextFactory;
    readonly IMapper _mapper;
    readonly IBus _bus;

    public CreateUserConsumer(ILogger<CreateUserConsumer> logger, IDbContextFactory<UserDbContext> contextFactory, IMapper mapper, IBus bus)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _mapper = mapper;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<CreateUser> context)
    {
        _logger.DbContext().Creating<UserDbContext>();
        using var dbContext = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        using var transaction = await dbContext.Database.BeginTransactionAsync().ConfigureAwait(false);

        _logger.LogTrace("Checking if username already exists");
        if (await dbContext.Users.UsernameExistsAsync(context.Message.Username).ConfigureAwait(false))
        {
            _logger.LogTrace("Username already exists");
            var usernameAlreadyExists = _mapper.Map<CreateUser, UsernameAlreadyExists>(context.Message);
            var response = _mapper.Map<UsernameAlreadyExists, CreateUserFailed>(usernameAlreadyExists);
            await context.RespondAsync(response).ConfigureAwait(false);
            return;
        }

        _logger.LogTrace("Checking if user email address already exists");
        if (await dbContext.Users.EmailAddressExistsAsync(context.Message.EmailAddress).ConfigureAwait(false))
        {
            _logger.LogTrace("User email address already exists");
            var emailAddressAlreadyExists = _mapper.Map<CreateUser, UserEmailAddressAlreadyExists>(context.Message);
            var response = _mapper.Map<UserEmailAddressAlreadyExists, CreateUserFailed>(emailAddressAlreadyExists);
            await context.RespondAsync(response).ConfigureAwait(false);
            return;
        }

        var user = _mapper.Map<CreateUser, User>(context.Message);

        _logger.DbContext().CommittingTransaction();
        await transaction.CommitAsync().ConfigureAwait(false);      
    }
}