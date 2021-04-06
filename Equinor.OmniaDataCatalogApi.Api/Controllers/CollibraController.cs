using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Equinor.OmniaDataCatalogApi.Collibra.BusinessDataTypes;
using Equinor.OmniaDataCatalogApi.Collibra.Services;
using Equinor.OmniaDataCatalogApi.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Equinor.OmniaDataCatalogApi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]

    public class CollibraController : ControllerBase
    {
        private readonly ICollibraService _collibraService;


        private readonly ILogger<CollibraController> _logger;

        public CollibraController(ILogger<CollibraController> logger, ICollibraService collibraService)
        {
            _logger = logger;
            _collibraService = collibraService;
        }

        [HttpGet]
        [Route("communities")]
        [Produces(typeof(IEnumerable<Community>))]
        public async Task<ActionResult> Communities(CancellationToken cancellationToken=default)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                var results = await _collibraService.Communities(token, cancellationToken);
                return new OkObjectResult(results);
            }
            catch (HttpException e) //figure out the best ways here, might be wise to have middleware for this (?)
            {
                _logger.Log(LogLevel.Error, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                throw new HttpException(HttpStatusCode.NoContent);
            }
        }
    }
}
