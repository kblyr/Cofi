using MediatR;

namespace Cofi.Contracts;

public interface CofiRequest : IRequest<CofiResponse> { }