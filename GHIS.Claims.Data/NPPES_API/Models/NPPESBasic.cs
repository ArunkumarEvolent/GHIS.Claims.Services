using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHIS.Claims.Data.NPPES_API.Models
{
    /// <summary>
    /// Basic information regarding the NPPES entry.
    /// </summary>
    public class NPPESBasic
    {
        /// <summary>
        /// The registered individual's first name.
        /// </summary>
        public string first_name { get; set; }

        /// <summary>
        /// The registered individual's last name.
        /// </summary>
        public string last_name { get; set; }
    }
}
