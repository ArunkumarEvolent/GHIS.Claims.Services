using InterSystems.Data.IRISClient;
using InterSystems.Data.IRISClient.ADO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHIS.Claims.Domain.Common
{
    public class CommonFunctions
    {
        public static IRIS GetNativeAPI()
        {
            IRISConnection IRISConnect = new IRISConnection();
            IRISConnect.ConnectionString = GHIS.Claims.Data.ConstantCls.ConnectionStringCache.ToString();
            IRISConnect.Open();

            IRIS NativeAPI = IRIS.CreateIRIS(IRISConnect);
            return NativeAPI;

        }
    }
}
