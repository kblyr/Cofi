using AutoMapper;
using MediatR;

namespace Cofi.Messaging;

public class MappedMediatorAdapter
{
    readonly IMapper _mapper;
    readonly IMediator _mediator;

    public MappedMediatorAdapter(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<Response> Send<TFromRequest, TToRequest>(TFromRequest request, CancellationToken cancellationToken = default) where TToRequest : Request
    {
        var destinationRequest = _mapper.Map<TFromRequest, TToRequest>(request);
        return await _mediator.Send(destinationRequest, cancellationToken).ConfigureAwait(false);
    }
}