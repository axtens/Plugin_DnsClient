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
            
            LookupClient client = new LookupClient(IPAddress.Parse("8.8.8.8"), IPAddress.Parse("8.8.4.4"));
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
    public class DNSBlock
    {
        public string Address { get; set; }
        public string DomainName { get; internal set; }
        public string RecordClass { get; internal set; }
        public int InitialTimeToLive { get; internal set; }
        public int TimeToLive { get; internal set; }
        public int RawDataLength { get; internal set; }
        public string RecordType { get; internal set; }
        public AfsType SubType { get; internal set; }
        public object Flags { get; internal set; }
        public string Tag { get; internal set; }
        public string Value { get; internal set; }
        public DnsString CanonicalName { get; internal set; }
        public string OS { get; internal set; }
        public string Cpu { get; internal set; }
        public DnsString MadName { get; internal set; }
        public DnsString MgName { get; internal set; }
        public DnsString NewName { get; internal set; }
        public DnsString Exchange { get; internal set; }
        public object Preference { get; internal set; }
        public int Order { get; internal set; }
        public string RegularExpression { get; internal set; }
        public DnsString Replacement { get; internal set; }
        public string Services { get; internal set; }
        public DnsString NSDName { get; internal set; }
    }
}
