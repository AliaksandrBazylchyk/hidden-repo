using Commands;
using Core;
using Microsoft.AspNetCore.Mvc;
using MovieService.Commands;
using MovieService.DTO;
using Queries;

namespace MovieService.Controllers
{
    [Route("api/movies")]
    public class MovieController : Controller
    {
        private readonly IQueryBus _queryBus;
        private readonly ICommandBus _commandBus;

        public MovieController(IQueryBus queryBus, ICommandBus commandBus)
        {
            _queryBus = queryBus;
            _commandBus = commandBus;
        }

        [HttpPost, Route("")]
        public async Task<ActionResult<CreateMovieCommand.Result>> CreateMovie([FromBody] CreateMovieDto movie, CancellationToken cancellationToken)
        {
            var command = Mapping.Map<CreateMovieDto, CreateMovieCommand.Command>(movie);

            var result = await _commandBus.Send(command, cancellationToken);

            return Created("{id}", new { id = result.Id });
        }
    }
}
