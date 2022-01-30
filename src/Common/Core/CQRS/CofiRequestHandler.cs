namespace Cofi;

public interface CofiRequestHandler<TRequest> : IRequestHandler<TRequest, CofiResponse> where TRequest : IRequest<CofiResponse> { }