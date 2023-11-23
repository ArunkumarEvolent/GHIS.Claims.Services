using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GHIS.Claims.Domain.Interfaces;
using GHIS.Claims.Domain.Results;
using GHIS.Claims.Domain.Request;

namespace GHIS.Claims.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrevalidationController : ControllerBase
    {

        private readonly IClaimPrevalidationService _claimValidation;
        private readonly ILogger<PrevalidationController> _logger;

        public PrevalidationController(ILogger<PrevalidationController> logger, IClaimPrevalidationService claimValidation)
        {
            _logger = logger;
            _claimValidation = claimValidation;
        }

        [Route("PreValidation"), HttpPost]
        [ProducesResponseType(statusCode: 200, type: typeof(PreValidationResult))]
        public async Task<IActionResult> PreValidation([FromBody] ClaimPreProcessData obj, int ReValidationStartQueue=0)
        {
            return Ok(await _claimValidation.PerformPrevalidation(obj,ReValidationStartQueue));
        }
    }
}
