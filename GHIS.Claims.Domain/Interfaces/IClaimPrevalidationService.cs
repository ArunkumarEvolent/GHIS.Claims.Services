using GHIS.Claims.Domain.Request;
using GHIS.Claims.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHIS.Claims.Domain.Interfaces
{
    public interface IClaimPrevalidationService
    {
        Task<PreValidationResult> PerformPrevalidation(ClaimPreProcessData obj, int ReValidateStartQueue);
    }
}
