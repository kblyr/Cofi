namespace Cofi.Identity.Endpoints;

static class User_Endpoints
{
    static class Routes
    {
        public const string Signup = "/user/sign-up";
    }

    public static EndpointMapper MapUser(this EndpointMapper mapper)
    {
        mapper.Builder
            .MapPost(Routes.Signup, Signup)
                .Accepts<SignupUser>(MimeTypes.Application.Json)
                .Produces<SignupUser.Response>(StatusCodes.Status201Created, MimeTypes.Application.Json)
        ;

        return mapper;
    }

    static async Task<IResult> Signup(IMediator mediator, SignupUser request, CancellationToken cancellationToken)
    {
        return await mediator.SendThenCreated<SignupUser, SignupUser.Response>(request, response => $"/user/{response.Id}", cancellationToken);
    }
}