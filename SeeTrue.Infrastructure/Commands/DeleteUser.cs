using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SeeTrue.Infrastructure.Commands
{
    public static class DeleteUser
    {
        public record Command(Guid UserId) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            public Handler()
            {

            }

            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
