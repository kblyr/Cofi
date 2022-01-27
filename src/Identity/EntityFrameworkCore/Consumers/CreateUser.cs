using Cofi.Extensions.MassTransit;
using Cofi.Identity.Utilities;

namespace Cofi.Identity.Consumers;

sealed class CreateUser_Consumer : IConsumer<CreateUser>
{
    readonly IDbContextFactory<UserDbContext> _contextFactory;
    readonly IMapper _mapper;
    readonly IUserPasswordCryptoHash _passwordHash;
    readonly ICurrentAuditInfoProvider _currentAuditInfoProvider;

    public CreateUser_Consumer(IDbContextFactory<UserDbContext> contextFactory, IMapper mapper, IUserPasswordCryptoHash passwordHash, ICurrentAuditInfoProvider currentAuditInfoProvider)
    {
        _contextFactory = contextFactory;
        _mapper = mapper;
        _passwordHash = passwordHash;
        _currentAuditInfoProvider = currentAuditInfoProvider;
    }

    public async Task Consume(ConsumeContext<CreateUser> context)
    {
        using var dbContext = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        using var transaction = await dbContext.Database.BeginTransactionAsync().ConfigureAwait(false);

        if (await CheckUsername(context, dbContext, context.Message.Username).ConfigureAwait(false) == false)
            return;

        if (await CheckEmailAddress(context, dbContext, context.Message.EmailAddress).ConfigureAwait(false) == false)
            return;

        var auditInfo = await _currentAuditInfoProvider.GetCurrent().ConfigureAwait(false);
        var user = PrepareUser(context.Message, auditInfo);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
        await transaction.CommitAsync().ConfigureAwait(false);

        var userCreated = _mapper.Map<User, UserCreated>(user);
        await context.Publish(userCreated).ConfigureAwait(false);

        if (context.IsRequest())
            await context.RespondAsync(new CreateUser.Result { Id = user.Id }).ConfigureAwait(false);
    }

    async Task<bool> InsertUserDomains(UserDbContext dbContext, User user, IEnumerable<short> domainIds, AuditInfo auditInfo)
    {
        if (domainIds is null || !domainIds.Any())
            return true;

        foreach (var domainId in domainIds)
        {

        }

        return true;
    }

    async Task<bool> CheckUsername(ConsumeContext<CreateUser> context, UserDbContext dbContext, string username)
    {
        if (await dbContext.Users.UsernameExists(username).ConfigureAwait(false))
        {
            if (context.IsRequest())
            {
                var usernameAlreadyExists = new UsernameAlreadyExists { Username = username };
                await context.RespondAsync(_mapper.Map<UsernameAlreadyExists, CreateUserFailed>(usernameAlreadyExists)).ConfigureAwait(false);
            }

            return false;
        }

        return true;
    }

    async Task<bool> CheckEmailAddress(ConsumeContext<CreateUser> context, UserDbContext dbContext, string emailAddress)
    {
        if (await dbContext.Users.EmailAddressExists(emailAddress).ConfigureAwait(false))
        {
            if (context.IsRequest())
            {
                var emailAddressAlreadyExists = new UserEmailAddressAlreadyExists { EmailAddress = emailAddress };
                await context.RespondAsync(_mapper.Map<UserEmailAddressAlreadyExists, CreateUserFailed>(emailAddressAlreadyExists)).ConfigureAwait(false);
            }

            return false;
        }

        return true;
    }

    User PrepareUser(CreateUser request, AuditInfo auditInfo) => _mapper.Map<CreateUser, User>(request) with
    {
        HashedPassword = _passwordHash.ComputeHash(request.Password),
        InsertedById = auditInfo.UserId,
        InsertedOn = auditInfo.Timestamp
    };
}