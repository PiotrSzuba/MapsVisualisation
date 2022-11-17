using MediatR;

namespace MapsVisualisation.Service.BuildingBlocks;

public interface ICommand : IRequest
{
}

public interface ICommand<out TResult> : IRequest<TResult>
{
}