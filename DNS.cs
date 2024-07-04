using DnsClient.Protocol;

using Newtonsoft.Json;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace DnsClient
{
    public static class DNS
    {
        public static string Query(string domain, string queryType, bool debug = false)
        {
            if (debug) Debugger.Launch();


            /*LookupClientOptions lookupClientOptions = new LookupClientOptions
            {
                Retries = 4,
                Timeout = TimeSpan.FromSeconds(5)
                
            };*/
            
            LookupClient client = new(IPAddress.Parse("8.8.8.8"), IPAddress.Parse("8.8.4.4"));
            IDnsQueryResponse result = null;
            try
            {
                result = client.Query(domain, (QueryType)Enum.Parse(typeof(QueryType), queryType));
            }
            catch (DnsResponseException dre)
            {
                return JsonConvert.SerializeObject(new JSONResponse() { Error = dre.Message, Crew = $"{domain},{queryType}" });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new JSONResponse() { Error = e.Message, Crew = $"{domain},{queryType}" });
            }

            if (result.HasError)
            {
                return JsonConvert.SerializeObject(new JSONResponse() { Error = result.ErrorMessage, Crew = $"{domain},{queryType}" });
            }

            return JsonConvert.SerializeObject(new JSONResponse() { Cargo = (from answer in result.Answers select answer.ToString()).ToArray() });
        }
    }
}
