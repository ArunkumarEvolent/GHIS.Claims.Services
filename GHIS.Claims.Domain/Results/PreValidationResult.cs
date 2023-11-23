using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHIS.Claims.Domain.Results
{

    public class PreValidationResult
    {
        public bool Success { get; set; }
        public int ErrorQueue { get; set; }
        public string ErrorString { get; set; }

        /*public PreValidationResult(bool SuccessVal)
        {
            this.Success = SuccessVal;
        }
        public PreValidationResult(bool SuccessVal, int ErrorQueueVal, string ErrorStringVal)
        {
            this.Success = SuccessVal;
            this.ErrorQueue = ErrorQueueVal;
            this.ErrorString = ErrorStringVal;
        }*/
    }
}
