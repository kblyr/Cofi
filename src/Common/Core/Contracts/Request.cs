using MediatR;

namespace Cofi.Contracts;

public interface Request : IRequest<Response> { }