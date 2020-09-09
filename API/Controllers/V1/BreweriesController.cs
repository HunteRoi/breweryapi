using AutoMapper;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DTO;
using DTOs = DTO.V1.BreweryRequest;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;

namespace API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class BreweriesController : BaseController<BreweriesController>
    {
        private readonly BreweryRepository _repository;

        public BreweriesController(ILogger<BreweriesController> logger, IMapper mapper, BreweryRepository repository)
            : base(logger, mapper)
        {
            _repository = repository;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Returns a certain number of breweries.",
            Description = "Requests a page of breweries not to load a lot of breweries on one request. The index and the page size are optional. The request returns an array of breweries based on the parameters, with the associated beers and wholesalers."
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Returns an array of breweries.", typeof(IEnumerable<DTOs.Brewery>))]
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

            return Ok(entities.Select(Mapper.Map<DTOs.Brewery>));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Gets a single brewery.",
            Description = "Gets a single brewery."
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "The brewery was successfully retrieved.", typeof(DTOs.Brewery))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "The brewery does not exist.")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repository.ReadAsync(id);
            if (entity is null) return NotFound();
            return Ok(Mapper.Map<DTOs.Brewery>(entity));
        }
    }
}
