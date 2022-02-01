using MediatR;

namespace Cofi.Handlers;

public interface CofiRequestHandler<TRequest> : IRequestHandler<TRequest, CofiResponse> where TRequest : CofiRequest { }