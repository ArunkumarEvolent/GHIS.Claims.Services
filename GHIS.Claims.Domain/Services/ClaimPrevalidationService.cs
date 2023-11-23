using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GHIS.Claims.Data.NPPES_API;
using GHIS.Claims.Domain.Common;
using GHIS.Claims.Domain.Interfaces;
using GHIS.Claims.Domain.Request;
using GHIS.Claims.Domain.Results;
using InterSystems.Data.IRISClient;
using InterSystems.Data.IRISClient.ADO;
using GHIS.Claims.Data.NPPES_API.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GHIS.Claims.Domain.Services
{
    public class ClaimPrevalidationService : IClaimPrevalidationService
    {
        //public ClaimPreProcessData ValidationObject;

        public ClaimPreProcessData? ValidationObject { get; private set; }

        public Task<PreValidationResult> PerformPrevalidation(ClaimPreProcessData ValidationObject, int ReValidateStartQueue = 0)
        {
            PreValidationResult result = new PreValidationResult();
            if (ReValidateStartQueue <= 1)
            {
                result = MemberValidation(ValidationObject);
                if (result.Success == false) { return Task.FromResult(result); }
            }
            if (ReValidateStartQueue <= 2)
            {
                result = FacilityValidation(ValidationObject);
                if (result.Success == false) { return Task.FromResult(result); }
            }
            if (ReValidateStartQueue <= 3)
            {
                result = ReferringProviderValidation(ValidationObject);
                if (result.Success == false) { return Task.FromResult(result); }
            }
            return Task.FromResult(new PreValidationResult() { Success = true });
        }

        private PreValidationResult MemberValidation(ClaimPreProcessData ValidationObject)
        {
            if (ValidationObject is not null)
            {
                /// Inprogress - Shailesh Cache code need to be called.
            }
            return new PreValidationResult() { Success = true };
        }
        private PreValidationResult FacilityValidation(ClaimPreProcessData ValidationObject)
        {
            if (ValidationObject is not null)
            {
                IRIS NativeAPI = CommonFunctions.GetNativeAPI();
                string POS = ValidationObject.POS;
                if (ValidationObject.ClaimFormat == "Institutional")
                {
                    if (NativeAPI.IsDefined("BILLTYPEPOS", ValidationObject.BillTYpe) > 0)
                    {
                        //byte[] EffDate = (byte[]) NativeAPI.NextSubscript(false, "BILLTYPEPOS", ValidationObject.BillTYpe);
                        //if (EffDate != null)
                        //{
                        string tmpStr = NativeAPI.GetString("BILLTYPEPOS", ValidationObject.BillTYpe, "66840");
                        string[] POSstr = tmpStr.Split("^", StringSplitOptions.RemoveEmptyEntries);
                        POS = POSstr[2];
                        //}
                    }
                }
                if ("/11/12/41/42/02/".Contains(POS))
                {
                    return new PreValidationResult() { Success = true };
                }
                else
                {
                    if (ValidationObject.FacilityNPI.Trim() == "")
                    {
                        return new PreValidationResult() { Success = false, ErrorQueue = 2, ErrorString = "Missing Facility NPI" };
                    }
                    else
                    {
                        if (ValidationObject.FacilityNPI.Trim().Length == 10)
                        {
                            if (NativeAPI.IsDefined("FACNPI", ValidationObject.FacilityNPI.Trim()) > 0)
                            {
                                return new PreValidationResult() { Success = true };
                            }
                            else
                            {
                                return new PreValidationResult() { Success = false, ErrorQueue = 2, ErrorString = "Missing Facility NPI" };
                            }
                        }
                        else
                        {
                            return new PreValidationResult() { Success = false, ErrorQueue = 2, ErrorString = "Wrong Facility NPI" };
                        }
                    }
                }
            }
            else
            {
                return new PreValidationResult() { Success = false, ErrorQueue = 2, ErrorString = "Wrong Facility NPI" };
            }
        }
        private PreValidationResult ReferringProviderValidation(ClaimPreProcessData ValidationObject)
        {
            if (ValidationObject is not null)
            {
                IRIS NativeAPI = CommonFunctions.GetNativeAPI();
                if (ValidationObject.ReferringProviderNPI.Trim() == "")
                {
                    return new PreValidationResult() { Success = false, ErrorQueue = 2, ErrorString = "Missing Referring Provider NPI" };
                }
                else
                {
                    if (ValidationObject.ReferringProviderNPI.Trim().Length == 10)
                    {
                        if (NativeAPI.IsDefined("NPI", "RIDF", ValidationObject.ReferringProviderNPI.Trim()) > 0)
                        {
                            return new PreValidationResult() { Success = true };
                        }
                        else
                        {
                            /// To check with NEPS API
                            NPPESApiClient APIobj = new NPPESApiClient();
                            string APIresponse = APIobj.GetAPIResponse(ValidationObject.ReferringProviderNPI.Trim());
                            if (APIresponse.Split("^")[0] == "1")
                            {
                                string FirstName = APIresponse.Split("^")[1];
                                string LastName = APIresponse.Split("^")[2];
                                string stringVal = NativeAPI.ClassMethodString("GHIS.EDI.API.RESTAPI", "AddNewReferringNPI", ValidationObject.ReferringProviderNPI.Trim(), FirstName, LastName);
                                return new PreValidationResult() { Success = true };
                            }
                            return new PreValidationResult() { Success = false, ErrorQueue = 2, ErrorString = "Unable to add new Referring Provider NPI into GHIS." };
                        }
                    }
                    else
                    {
                        return new PreValidationResult() { Success = false, ErrorQueue = 2, ErrorString = "Wrong Referring Provider NPI" };
                    }
                }
            }
            return new PreValidationResult() { Success = true };
        }
        public PreValidationResult ValidateDOS(ClaimPreProcessData ValidationObject)
        {
            Int32 isValidFromDOS, isValidToDOS;
            DateTime objFromDOS, objToDOS;
            string fromDOS, toDOS;
            Int32 count = ValidationObject.ServiceLines.Count;

            for (int i = 0; i < count; i++)
            {
                fromDOS = ValidationObject.ServiceLines[0].DOSFrom;
                toDOS = ValidationObject.ServiceLines[0].DOSTo;
                isValidFromDOS = ValidateDate(fromDOS, out objFromDOS);
                isValidToDOS = ValidateDate(toDOS, out objToDOS);
                if ((isValidFromDOS == 0) | (isValidToDOS == 0))
                {
                    //return 0;
                    return new PreValidationResult() { Success = false, ErrorQueue = 3, ErrorString = "Invalid Date" };
                }
            }
            return new PreValidationResult() { Success = true };
        }

        //Date Validation
        public int ValidateDate(String date, out DateTime objDate)
        {
            if (!DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out objDate))
            {
                return 0;
            }
            return 1;
        }

        public PreValidationResult ValidateNotificationDate(ClaimPreProcessData ValidationObject)
        {
            string notificationDate = ValidationObject.NotificationDate;
            Int32 activeErrorQueue = 8;                     //Notification date Error Queue ID

            // Check NotificationDate is Valid
            DateTime objNotificationDate;
            int isValidNotificationDate = ValidateDate(notificationDate, out objNotificationDate);
            if (isValidNotificationDate == 0)
            {
                return new PreValidationResult() { Success = false, ErrorQueue = activeErrorQueue, ErrorString = "Invalid Date" };
            }

            //Fetch FromDOS and ToDOS to validate against Notfication Date
            string fromDOS, toDOS;
            int isValidFromDOS = 0, isValidToDOS = 0;
            DateTime objFromDOS, objToDOS;
            Int32 count = ValidationObject.ServiceLines.Count;

            for (int i = 0; i < count; i++)
            {
                fromDOS = ValidationObject.ServiceLines[0].DOSFrom;
                toDOS = ValidationObject.ServiceLines[0].DOSTo;
                isValidFromDOS = ValidateDate(fromDOS, out objFromDOS);
                isValidToDOS = ValidateDate(toDOS, out objToDOS);

                if ((objFromDOS > objNotificationDate) | (objToDOS > objNotificationDate))
                {
                    return new PreValidationResult() { Success = false, ErrorQueue = activeErrorQueue, ErrorString = "Not in Date range" };
                }
            }
            return new PreValidationResult() { Success = true };
        }

        public PreValidationResult ValidateRenderingProvider(ClaimPreProcessData ValidationObject)
        {
            int activeErrorQueue = 3;
            string[] val = ValidationObject.ClaimID.Split("||");
            string processID =  val[0];
            string DCN = ValidationObject.DCN;
            string renderingNPI = ValidationObject.RenderingProviderNPI;
            string attendingNPI = ValidationObject.AttendingProviderNPI;
            //string batchID = "1";
            //DBConnection();

            //IRIS NativeAPI = IRIS.CreateIRIS(IRISConnect);
            IRIS NativeAPI = CommonFunctions.GetNativeAPI();
            if ((renderingNPI == "") & (attendingNPI == ""))
            {
                String stringVal = NativeAPI.ClassMethodString("GHIS.EDI.API.RESTAPI", "UpdateEDIErrorQueue", ValidationObject.ClaimID, activeErrorQueue);
                NativeAPI.Close();
                return new PreValidationResult() { Success = false, ErrorQueue = activeErrorQueue, ErrorString = "Empty NPI value" };
            }
            if ((ValidationObject.ClaimType == "837P") & (renderingNPI == "")) renderingNPI = attendingNPI;
            if ((ValidationObject.ClaimType == "837I") & (attendingNPI != "")) renderingNPI = attendingNPI;

            int isDefined = NativeAPI.IsDefined("^NPI", "IDF", renderingNPI);

            if (isDefined != 10)
            {
                //UpdateEDIErrorQueue(activeErrorQueue, processID, batchID, DCN);
                String stringVal = NativeAPI.ClassMethodString("GHIS.EDI.API.RESTAPI", "UpdateEDIErrorQueue", ValidationObject.ClaimID, activeErrorQueue);
                NativeAPI.Close();
                return new PreValidationResult() { Success = false, ErrorQueue = activeErrorQueue, ErrorString = "NPI not found in GHIS." };
            }
            NativeAPI.Close();
            return new PreValidationResult() { Success = true };
        }
        public PreValidationResult ValidateBillingTaxID(ClaimPreProcessData ValidationObject)
        {
            //DBConnection();
            //IRIS NativeAPI = IRIS.CreateIRIS(IRISConnect);
            IRIS NativeAPI = CommonFunctions.GetNativeAPI();

            Int32 isValidBillingTaxID = 0;
            string fromDOS;
            DateTime objFromDOS;
            int isValidFromDOS = 0;
            string taxID = ValidationObject.BillingProviderTIN;
            String[] taxIDARR = taxID.Split("-");
            if (taxIDARR.Length < 2) { taxID = taxID.Substring(0, 2) + "-" + taxID.Substring(2); }
            string patientID = ValidationObject.PatientID;
            fromDOS = ValidationObject.ServiceLines[0].DOSFrom;
            isValidFromDOS = ValidateDate(fromDOS, out objFromDOS);
            Int32 count = ValidationObject.ServiceLines.Count;

            if ((taxID == null) | (patientID == "") | (isValidFromDOS == 0))
            {
                return new PreValidationResult() { Success = false, ErrorQueue = 5, ErrorString = "Invalid Inputs." };
            }
            string patPattern = @"\w{3}";
            Match match1 = Regex.Match(patientID, patPattern);


            string taxPattern = @"^(\d{2}[-]\d{7})$";
            Match match2 = Regex.Match(taxID, taxPattern);
            if (match1.Success & match2.Success) { }
            else { return new PreValidationResult() { Success = true }; }

            int isDefined = NativeAPI.IsDefined("^IDF", "TAXID", patientID);
            if (isDefined != 10)
            {
                return new PreValidationResult() { Success = false, ErrorQueue = 5, ErrorString = "TaxID Not exist in GHIS." };
            }
            IRISIterator iter1, iter2;
            //string sub1 = "TAXID";
            iter1 = NativeAPI.GetIRISIterator("^IDF", "TAXID", patientID);
            iter1.StartFrom("0");
            foreach (var v1 in iter1)
            {
                object sub1 = iter1.CurrentSubscript;
                string val1 = (string)NativeAPI.GetString("^IDF", "TAXID", patientID, sub1);
                string[] val1Arr = val1.Split("/");
                if (val1Arr[0] != taxID) { continue; }

                iter2 = NativeAPI.GetIRISIterator("^IDF", "TAXID", patientID, sub1);
                iter2.StartFrom("0");
                foreach (var v2 in iter2)
                {
                    object sub2 = iter2.CurrentSubscript;
                    string val2 = (string)NativeAPI.GetString("^IDF", "TAXID", patientID, sub1, sub2);
                    string[] val2Arr = val2.Split("/");
                    if (val2Arr[2] != "A") { continue; }
                    DateTime startDate = IRISList.HorologToDate(Int32.Parse(val2Arr[0]));
                    DateTime endDate = IRISList.HorologToDate(Int32.Parse(val2Arr[1]));

                    if ((startDate <= objFromDOS) & (objFromDOS <= endDate))
                    {
                        return new PreValidationResult() { Success = true };
                    }
                }
            }
            //return isValidBillingTaxID;
            return new PreValidationResult() { Success = false, ErrorQueue = 5, ErrorString = "Invalid Inputs." };
        }
    }
}
