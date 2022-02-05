using MediatR;

namespace Cofi.Handlers;

public interface RequestHandler<TRequest> : IRequestHandler<TRequest, Response> where TRequest : Request { }