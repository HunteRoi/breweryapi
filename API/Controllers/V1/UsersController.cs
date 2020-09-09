//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using AutoMapper;
//using DAL.Repositories;
//using Constants = DTO.Constants;
//using DTOs = DTO.V1;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using v1 = Models.V1;
//using Swashbuckle.AspNetCore.Annotations;

//namespace API.Controllers.V1
//{
//    [ApiController]
//    [ApiVersion("1.0")]
//    [Route("api/v{version:apiVersion}/[controller]")]
//    [Produces("application/json")]
//    public class UsersController : ControllerBase
//    {
//        private readonly ILogger<UsersController> _logger;
//        private readonly IMapper _mapper;
//        private readonly UserRepository _repository;

//        public UsersController(ILogger<UsersController> logger, IMapper mapper, UserRepository repository)
//        {
//            _logger = logger;
//            _mapper = mapper;
//            _repository = repository;
//        }

//        [HttpGet(Name = nameof(GetAll))]
//        [SwaggerOperation(
//            Summary = "Returns a certain number of users.",
//            Description = "Requests a page of users not to load a lot of users on one request. The index and the page size are optional. The request returns an array of users based on the parameters."
//        )]
//        [SwaggerResponse((int)HttpStatusCode.OK, "Returns an array of users.", typeof(IEnumerable<DTOs.User>))]
//        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
//        [SwaggerResponse((int)HttpStatusCode.NotFound)]
//        public async Task<IActionResult> GetAll(string filter = null, int pageIndex = Constants.PageIndex, int pageSize = Constants.PageSize)
//        {
//            var totalCount = await _repository.CountAsync();

//            var entities = await _repository.ReadAllWithFilterAsync(filter, pageIndex, pageSize);
//            if (entities == null) return NotFound();

//            Request.HttpContext.Response.Headers.Add("X-TotalCount", totalCount.ToString());
//            Request.HttpContext.Response.Headers.Add("X-PageIndex", pageIndex.ToString());
//            Request.HttpContext.Response.Headers.Add("X-PageSize", pageSize.ToString());

//            return Ok(entities.Select(_mapper.Map<DTOs.User>));
//        }

//        [HttpGet("{id}", Name = nameof(GetById))]
//        [SwaggerOperation(
//            Summary = "Gets a single user.",
//            Description = "Gets a single user."
//        )]
//        [SwaggerResponse((int)HttpStatusCode.OK, "The user was successfully retrieved.", typeof(DTOs.User))]
//        [SwaggerResponse((int)HttpStatusCode.NotFound, "The user does not exist.")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var user = await _repository.ReadAsync(id);
//            if (user == null) return NotFound();
//            return Ok(_mapper.Map<DTOs.User>(user));
//        }

//        [HttpPost(Name = nameof(Post))]
//        [SwaggerOperation(
//            Summary = "Places a new user.",
//            Description = "Adds a new user to the database."
//        )]
//        [SwaggerResponse((int)HttpStatusCode.Created, "The user was successfully placed.", typeof(DTOs.User))]
//        [SwaggerResponse((int)HttpStatusCode.BadRequest, "The user is invalid.")]
//        public async Task<IActionResult> Post([FromBody] DTOs.User user, ApiVersion version = null)
//        {
//            if (version == null)
//                version = ApiVersion.Default;

//            var newEntity = new v1.User(user.Firstname, user.LastName, user.Email, user.Phone);
//            await _repository.AddAsync(newEntity);
//            await _repository.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetById), new { id = newEntity.Id, version = $"{version}" }, _mapper.Map<DTOs.User>(newEntity));
//        }

//        [HttpPut("{id}", Name = nameof(Put))]
//        [SwaggerOperation(
//            Summary = "Edits a user based on their identifier.",
//            Description = "Change the entity of a requested user based on the provided identifier."
//        )]
//        [SwaggerResponse((int)HttpStatusCode.Accepted, "The user was successfully edited.", typeof(DTOs.User))]
//        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
//        [SwaggerResponse((int)HttpStatusCode.NotFound, "The user does not exist.")]
//        public async Task<IActionResult> Put(int id, [FromBody] DTOs.User user, ApiVersion version = null)
//        {
//            if (version == null)
//                version = ApiVersion.Default;

//            var entityFound = await _repository.ReadAsync(id);
//            if (entityFound == null) return NotFound();

//            var updateCommand = new v1.User(user.Firstname, user.LastName, user.Email, user.Phone);
//            entityFound.Update(updateCommand);
//            _repository.Edit(entityFound);

//            return AcceptedAtAction(nameof(GetById), new { id = entityFound.Id, version = $"{version}" }, _mapper.Map<DTOs.User>(entityFound));
//            //return Ok(_mapper.Map<DTOs.user>(entityFound));
//        }

//        [HttpDelete("{id}", Name = nameof(Delete))]
//        [SwaggerOperation(
//            Summary = "Deletes a user based on their identifier.",
//            Description = "Deletes the entity of a requested user based on the provided identifier."
//        )]
//        [SwaggerResponse((int)HttpStatusCode.NoContent, "The user was successfully deleted.")]
//        [SwaggerResponse((int)HttpStatusCode.NotFound, "The user does not exist.")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var entityFound = await _repository.ReadAsync(id);
//            if (entityFound == null) return NotFound();

//            _repository.Delete(entityFound);
//            await _repository.SaveChangesAsync();
//            return NoContent();
//        }
//    }
//}