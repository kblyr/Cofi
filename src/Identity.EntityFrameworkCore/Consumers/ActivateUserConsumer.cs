namespace Cofi.Identity.Consumers;

sealed class ActivateUserConsumer : IConsumer<ActivateUser>
{
    readonly ILogger<ActivateUserConsumer> _logger;
    readonly IDbContextFactory<UserDbContext> _contextFactory;

    public ActivateUserConsumer(ILogger<ActivateUserConsumer> logger, IDbContextFactory<UserDbContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task Consume(ConsumeContext<ActivateUser> context)
    {
        using (_logger.BeginScopeWithProps(context.Message.GetLoggingProps()))
        _logger.LogInformation("Activating user");

        using var dbContext = await _contextFactory.CreateDbContextAsync(context.CancellationToken).ConfigureAwait(false);
        using var transaction = await dbContext.Database.BeginTransactionAsync(context.CancellationToken).ConfigureAwait(false);

        var user = await dbContext.Users.FindAsync(new object[] { context.Message.UserId }, context.CancellationToken).ConfigureAwait(false);

        if (user is null)
        {
            await context.RespondAsync(new UserNotFound(context.Message.UserId)).ConfigureAwait(false);
            _logger.LogDebug("User does not exists");
            return;
        }

        if (user is { IsActive: true })
        {
            await context.RespondAsync(new UserAlreadyActive(context.Message.UserId)).ConfigureAwait(false);
            _logger.LogDebug("Cannot activate user, already active");
            return;
        }

        user.IsActive = true;
        await dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);
        await transaction.CommitAsync(context.CancellationToken).ConfigureAwait(false);
        _logger.LogInformation("User activated successfully");
        var userActivated = new UserActivated(context.Message.UserId, context.Message.ActivatedById);

        if (context.RequestId.HasValue)
        {
            await context.RespondAsync(userActivated).ConfigureAwait(false);
            return;
        }
        
        await context.Publish(userActivated).ConfigureAwait(false);
    }
}
