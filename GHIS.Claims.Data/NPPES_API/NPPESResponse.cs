using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GHIS.Claims.Data.NPPES_API.Models;

namespace GHIS.Claims.Data.NPPES_API
{
    /// <summary>
    /// Represents a response from the NPPES NPI API.
    /// </summary>
    public class NPPESResponse
    {
        /// <summary>
        /// Number of results returned in the response.
        /// </summary>
        public int result_count { get; set; }

        /// <summary>
        /// The search results, if any matches.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public IList<NPPESResult> results { get; set; }

        /// <summary>
        /// A list of errors returned from the API, if any.
        /// </summary>
        public IList<NPPESError>? Errors { get; set; }

        /// <summary>
        /// The http status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
    }

}
