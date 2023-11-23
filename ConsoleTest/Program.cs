using GHIS.Claims.Data.NPPES_API;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;
using GHIS.Claims.Domain.Request;
using GHIS.Claims.Domain.Results;
using GHIS.Claims.Domain.Services;
using Newtonsoft.Json;

Console.WriteLine("Referring Provider Validation");
ClaimPreProcessData DataObj = new ClaimPreProcessData();
Console.WriteLine("Enter ClaimID: clmID123");
DataObj.ClaimID = "clmID123"; // Console.ReadLine()!;
Console.WriteLine("Enter DCN: dcn12");
DataObj.DCN = "dcn12"; //Console.ReadLine()!;


Console.WriteLine("Enter FacilityNPI: 1003078049");
DataObj.FacilityNPI = "1003078049";
Console.WriteLine("Enter Referring Provider NPI: ");
DataObj.ReferringProviderNPI = Console.ReadLine()!;
ClaimPrevalidationService obj = new ClaimPrevalidationService();
PreValidationResult objEx = await obj.PerformPrevalidation(DataObj,3)!;
//Data= Console.ReadLine()!;

string json1 = JsonConvert.SerializeObject(objEx);
Console.WriteLine(json1 + "\n");



/*Console.WriteLine("API call started:");
Console.WriteLine(new NPPESApiClient().GetAPIResponse().ToString());
*/

/*
using GHIS.Claims.Domain.Request;
using GHIS.Claims.Domain.Results;
using GHIS.Claims.Domain.Services;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using InterSystems.Data.IRISClient;
using InterSystems.Data.IRISClient.ADO;

*/

/*Console.WriteLine("Facility Validation");
ClaimPreProcessData DataObj = new ClaimPreProcessData();
Console.WriteLine("Enter ClaimID: clmID123");
DataObj.ClaimID = "clmID123"; // Console.ReadLine()!;
Console.WriteLine("Enter DCN: dcn12");
DataObj.DCN = "dcn12"; //Console.ReadLine()!;

Console.WriteLine("Enter Claim Format (Institutional or Professional) : (Institutional)");
string claimFormat=Console.ReadLine()!;
if (claimFormat=="")
{
    claimFormat = "Institutional";
}
else
{
    claimFormat = "Professional";
}
DataObj.ClaimFormat = claimFormat;

if (claimFormat== "Professional")
{
    Console.WriteLine("Enter POS: ");
    DataObj.POS = Console.ReadLine()!;
}
else
{
    Console.WriteLine("Enter BillType: (32)");
    DataObj.BillTYpe = Console.ReadLine()!;
}
Console.Write("Enter FacilityNPI: ");
DataObj.FacilityNPI = Console.ReadLine()!;
Console.Write("Enter Referring Provider NPI: ");
DataObj.ReferringProviderNPI = Console.ReadLine()!;
ClaimPrevalidationService obj= new ClaimPrevalidationService();
PreValidationResult objEx= await obj.PerformPrevalidation(DataObj)!;
//Data= Console.ReadLine()!;

string json1 = JsonConvert.SerializeObject(objEx);
Console.WriteLine(json1 + "\n");
*/
/*
Console.WriteLine("Referring Provider Validation");
ClaimPreProcessData DataObj = new ClaimPreProcessData();
Console.WriteLine("Enter ClaimID: clmID123");
DataObj.ClaimID = "clmID123"; // Console.ReadLine()!;
Console.WriteLine("Enter DCN: dcn12");
DataObj.DCN = "dcn12"; //Console.ReadLine()!;

Console.Write("Enter Referring Provider NPI: ");
DataObj.ReferringProviderNPI = Console.ReadLine()!;
ClaimPrevalidationService obj = new ClaimPrevalidationService();
PreValidationResult objEx = await obj.PerformPrevalidation(DataObj)!;
//Data= Console.ReadLine()!;

string json1 = JsonConvert.SerializeObject(objEx);
Console.WriteLine(json1 + "\n");
*/

