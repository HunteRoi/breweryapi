using AutoMapper;
using DAL.Repositories;
using DTO;
using DTOs = DTO.V1.WholesalerRequest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Business;

namespace API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class WholesalersController : BaseController<WholesalersController>
    {
        private readonly WholesalerRepository _repository;
        private readonly BeerRepository _beerRepository;

        public WholesalersController(ILogger<WholesalersController> logger, IMapper mapper, WholesalerRepository repository, BeerRepository beerRepository) 
            : base(logger, mapper)
        {
            _repository = repository;
            _beerRepository = beerRepository;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Returns a certain number of wholesalers.",
            Description = "Requests a page of wholesalers not to load a lot of wholesalers on one request. The index and the page size are optional. The request returns an array of wholesalers based on the parameters."
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Returns an array of wholesalers.", typeof(IEnumerable<DTOs.Wholesaler>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(ClientSideError))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll(string filter = null, int pageIndex = Constants.PageIndex, int pageSize = Constants.PageSize)
        {
            var totalCount = await _repository.CountAsync();

            var entities = await _repository.ReadAllWithFilterAsync(filter, pageIndex, pageSize);
            if (entities is null) return NotFound();

            Request.HttpContext.Response.Headers.Add("X-TotalCount", totalCount.ToString());
            Request.HttpContext.Response.Headers.Add("X-PageIndex", pageIndex.ToString());
            Request.HttpContext.Response.Headers.Add("X-PageSize", pageSize.ToString());

            return Ok(entities.Select(Mapper.Map<DTOs.Wholesaler>));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Gets a single wholesaler.",
            Description = "Gets a single wholesaler."
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "The wholesaler was successfully retrieved.", typeof(DTOs.Wholesaler))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "The wholesaler does not exist.")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repository.ReadAsync(id);
            if (entity is null) return NotFound();
            return Ok(Mapper.Map<DTOs.Wholesaler>(entity));
        }

        [HttpPost("{wholesalerId}/orders")]
        [SwaggerOperation(Summary = "Place an order to a wholesaler.", Description = "Asks the wholesaler for an quote of an order")]
        [SwaggerResponse((int)HttpStatusCode.OK, "The order was successfully placed.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "The wholesaler does not exist.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "The beer does not exist.")]
        public async Task<IActionResult> PlaceOrder(int wholesalerId, [FromBody] DTOs.Order order)
        {
            var wholesaler = await _repository.ReadAsync(wholesalerId);
            if (wholesaler == null) return NotFound();

            await BusinessRequirements.EnsureAllRequirementsAsync(wholesalerId, order, _repository);

            var beers = await _beerRepository.ReadAllFromListAsync(order.OrderDetails.Select(od => od.BeerId));
            return Ok(new DTOs.Quote(order, beers));
        }

        [HttpPost("{wholesalerId}/beers")]
        [SwaggerOperation(Summary = "Adds a beer to a wholesaler.", Description = "Adds a beer to a wholesaler.")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "The beer was successfully placed.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "The wholesaler does not exist.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "The beer does not exist.")]
        public async Task<IActionResult> AddBeerToWholesaler(int wholesalerId, [FromBody] DTOs.Stock newBeer)
        {
            var wholesaler = await _repository.ReadAsync(wholesalerId);
            if (wholesaler == null) return NotFound();

            var beer = await _beerRepository.ReadAsync(newBeer.BeerId);
            if (beer == null) return NotFound();

            await BusinessRequirements.EnsureBeersAreNotSoldByWholesalerAsync(wholesalerId, new List<DTOs.Stock> { newBeer }, _repository);

            wholesaler.AddStock(newBeer.Quantity, beer);
            await _repository.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("{wholesalerId}/beers/{beerId}")]
        [SwaggerOperation(Summary = "Edit a beer's quantity", Description = "Remove the provided quantity of a beer from a wholesaler's stock")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "The quantity was successfully edited.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "The wholesaler does not exist.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "The beer does not exist.")]
        public async Task<IActionResult> EditBeersQuantityOfWholesaler(int wholesalerId, int beerId, [FromBody] DTOs.UpdateStock updateBeer)
        {
            var wholesaler = await _repository.ReadAsync(wholesalerId);
            if (wholesaler == null) return NotFound();

            var newBeer = updateBeer.Clone().SetBeerId(beerId);
            var beer = await _beerRepository.ReadAsync(newBeer.BeerId);
            if (beer == null) return NotFound();

            await BusinessRequirements.EnsureBeersAreSoldByWholesalerAsync(wholesalerId, new List<DTOs.Stock> { newBeer }, _repository);
            await BusinessRequirements.EnsureQuantitiesAreNotGreaterThanStockAsync(wholesalerId, new List<DTOs.Stock> { newBeer }, _repository);

            wholesaler.AddQuantity(-1 * newBeer.Quantity, beer);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
