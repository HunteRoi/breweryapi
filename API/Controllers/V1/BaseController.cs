using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers.V1
{
    public class BaseController<TController> : ControllerBase
    {
        protected ILogger<TController> Logger { get; set; }

        protected IMapper Mapper { get; set; }
        

        public BaseController(ILogger<TController> logger, IMapper mapper)
        {
            Logger = logger;
            Mapper = mapper;
        }
    }
}
