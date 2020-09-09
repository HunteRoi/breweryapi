using AutoMapper;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.V1;
using DTO;
using DTOs = DTO.V1.BeerRequest;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class BeersController : BaseController<BeersController>
    {
        private readonly RepositoryBase<Beer> _repository;

        public BeersController(ILogger<BeersController> logger, IMapper mapper, RepositoryBase<Beer> repository) 
            : base(logger, mapper)
        {
            _repository = repository;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Returns a certain number of beers.",
            Description = "Requests a page of beers not to load a lot of beers on one request. The index and the page size are optional. The request returns an array of beers based on the parameters."
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Returns an array of beers.", typeof(IEnumerable<DTOs.Beer>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll(string filter = null, int pageIndex = Constants.PageIndex, int pageSize = Constants.PageSize)
        {
            var totalCount = await _repository.CountAsync();

            var entities = await _repository.ReadAllWithFilterAsync(filter, pageIndex, pageSize);
            if (entities is null) return NotFound();

            Request.HttpContext.Response.Headers.Add("X-TotalCount", totalCount.ToString());
            Request.HttpContext.Response.Headers.Add("X-PageIndex", pageIndex.ToString());
            Request.HttpContext.Response.Headers.Add("X-PageSize", pageSize.ToString());

            return Ok(entities.Select(Mapper.Map<DTOs.Beer>));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Gets a single beer.",
            Description = "Gets a single beer."
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "The beer was successfully retrieved.", typeof(DTOs.Beer))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "The beer does not exist.")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repository.ReadAsync(id);
            if (entity is null) return NotFound();
            return Ok(Mapper.Map<DTOs.Beer>(entity));
        }
    }
}
