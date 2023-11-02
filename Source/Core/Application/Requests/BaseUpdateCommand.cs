using MediatR;

namespace Application.Requests
{
    internal class BaseUpdateCommand<T> : IRequest<T>
    {
    }
}
