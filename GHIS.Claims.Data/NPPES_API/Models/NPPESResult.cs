using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHIS.Claims.Data.NPPES_API.Models
{
    /// A search result returned from the NPPES API.
    /// </summary>
    public class NPPESResult
    {
        /// <summary>
        /// The registered NPI number.
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// Basic information regarding the search result.
        /// </summary>
        public NPPESBasic? Basic { get; set; }
    }
}
