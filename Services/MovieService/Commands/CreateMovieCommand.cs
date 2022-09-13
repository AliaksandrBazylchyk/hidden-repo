using Commands;
using Core;
using Core.Aggregate;
using Models.Entities;
using Models.OutboxEntities;

namespace MovieService.Commands
{
    public class CreateMovieCommand
    {
        public class Command : ICommand<Result>
        {
            public string Title { get; set; }
            public int Year { get; set; }
        }
        public class Result
        {
            public Guid Id { get; set; }
        }

        public class Handler : ICommandHandler<Command, Result>
        {
            private readonly IAggregate<Movie, MovieOutbox> _aggregate;

            public Handler(IAggregate<Movie, MovieOutbox> aggregate)
            {
                _aggregate = aggregate;
            }
            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var createdEntity = await _aggregate.CreateAsync(Mapping.Map<Command, Movie>(command));

                var result = new Result
                {
                    Id = createdEntity.Id
                };

                return result;
            }
        }

    }
}
