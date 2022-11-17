using MediatR;

namespace MapsVisualisation.Service.BuildingBlocks;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
