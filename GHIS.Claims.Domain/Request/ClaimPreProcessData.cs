using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHIS.Claims.Domain.Request
{
    public class ClaimPreProcessData
    {
        public string ClaimID { get; set; }
        public string DCN { get; set; }

        public string ClaimFormat { get; set; }

        public string BillTYpe { get; set; }
        public string POS { get; set; }
        public string FacilityNPI { get; set; }
        public string ReferringProviderNPI { get; set; }

        public string ClaimType { get; set; }
        public string PatientID { get; set; }
        public string NotificationDate { get; set; }
        public string ServiceLocationNPI { get; set; }
        public string RenderingProviderNPI { get; set; }
        public string AttendingProviderNPI { get; set; }
        public int ValidateRenderingProvider { get; set; }
        public string BillingProviderNPI { get; set; }
        public string BillingProviderTIN { get; set; }
        public IList<ServiceLine> ServiceLines { get; set; }
    }

    public class ServiceLine
    {
        public string LineNumber { get; set; }
        public string DOSFrom { get; set; }
        public string DOSTo { get; set; }
        public string POS { get; set; }


    }
}
