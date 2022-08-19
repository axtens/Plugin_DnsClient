using DnsClient.Protocol;

using Newtonsoft.Json;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            LookupClient client = new LookupClient();
            IDnsQueryResponse result = client.Query(domain, (QueryType)Enum.Parse(typeof(QueryType), queryType));
            if (result.HasError)
            {
                return JsonConvert.SerializeObject(new JSONResponse() { Error = result.ErrorMessage, Crew = $"{domain},{queryType}" });
            }
            var res = new List<DNSBlock>();

            switch ((QueryType)Enum.Parse(typeof(QueryType), queryType))
            {
                case QueryType.A:
                    var ares = result.Answers.ARecords().ToArray();
                    foreach (var a in ares)
                    {
                        var blk = new DNSBlock
                        {
                            Address = a.Address.ToString(),
                            RawDataLength = a.RawDataLength,
                            DomainName = a.DomainName,
                            InitialTimeToLive = a.InitialTimeToLive,
                            RecordClass = a.RecordClass.ToString(),
                            TimeToLive = a.TimeToLive,
                            RecordType = a.RecordType.ToString()
                        };
                        res.Add(blk);
                    }

                    break;
                case QueryType.AAAA:
                    var aaaares = result.Answers.AaaaRecords().ToArray();
                    foreach (var a in aaaares)
                    {
                        var blk = new DNSBlock
                        {
                            Address = a.Address.ToString(),
                            DomainName = a.DomainName.Value,
                            RecordClass = a.RecordClass.ToString(),
                            InitialTimeToLive = a.InitialTimeToLive,
                            TimeToLive = a.TimeToLive,
                            RawDataLength = a.RawDataLength,
                            RecordType = a.RecordType.ToString()
                        };
                        res.Add(blk);
                    }
                    break;
                case QueryType.AFSDB:
                    var afsdbres = result.Answers.AfsDbRecords().ToArray();
                    foreach (var a in afsdbres)
                    {
                        var blk = new DNSBlock
                        {
                            SubType = a.SubType,
                            DomainName = a.DomainName.Value,
                            RecordClass = a.RecordClass.ToString(),
                            InitialTimeToLive = a.InitialTimeToLive,
                            TimeToLive = a.TimeToLive,
                            RawDataLength = a.RawDataLength,
                            RecordType = a.RecordType.ToString()
                        };
                        res.Add(blk);
                    }
                    break;
                //case QueryType.AXFR:
                //    res = result.Answers.A();
                //    break;
                case QueryType.CAA:
                    var carres = result.Answers.CaaRecords().ToArray();
                    foreach (var a in carres)
                    {
                        var blk = new DNSBlock
                        {
                            Flags = a.Flags,
                            Tag = a.Tag,
                            Value = a.Value,
                            DomainName = a.DomainName.Value,
                            RecordClass = a.RecordClass.ToString(),
                            InitialTimeToLive = a.InitialTimeToLive,
                            TimeToLive = a.TimeToLive,
                            RawDataLength = a.RawDataLength,
                            RecordType = a.RecordType.ToString()
                        };
                        res.Add(blk);
                    }
                    break;
                case QueryType.CNAME:
                    var cnameres = result.Answers.CnameRecords().ToArray();
                    foreach (var a in cnameres)
                    {
                        var blk = new DNSBlock
                        {
                            CanonicalName = a.CanonicalName,
                            DomainName = a.DomainName.Value,
                            RecordClass = a.RecordClass.ToString(),
                            InitialTimeToLive = a.InitialTimeToLive,
                            TimeToLive = a.TimeToLive,
                            RawDataLength = a.RawDataLength,
                            RecordType = a.RecordType.ToString()
                        };
                        res.Add(blk);
                    }
                    break;
                //case QueryType.DNSKEY:
                //    res = result.Answers;
                //    break;
                ///case QueryType.DS:
                ///    res = result.Answers;
                ///    break;
                case QueryType.HINFO:
                    var hinfores = result.Answers.HInfoRecords().ToArray();
                    foreach (var a in hinfores)
                    {
                        var blk = new DNSBlock
                        {
                            OS = a.OS,
                            Cpu = a.Cpu,
                            DomainName = a.DomainName.Value,
                            RecordClass = a.RecordClass.ToString(),
                            InitialTimeToLive = a.InitialTimeToLive,
                            TimeToLive = a.TimeToLive,
                            RawDataLength = a.RawDataLength,
                            RecordType = a.RecordType.ToString()
                        };
                        res.Add(blk);
                    }
                    break;
                case QueryType.MB:
                    var mbres = result.Answers.MbRecords().ToArray();
                    foreach (var a in mbres)
                    {
                        var blk = new DNSBlock
                        {
                            DomainName = a.DomainName,
                            InitialTimeToLive = a.InitialTimeToLive,
                            MadName = a.MadName,
                            RawDataLength = a.RawDataLength,
                            RecordClass = a.RecordClass.ToString(),
                            RecordType = a.RecordType.ToString()
                        };
                        res.Add(blk);
                    }
                    break;
                case QueryType.MG:
                    var mgres = result.Answers.MgRecords().ToArray();
                    foreach (var a in mgres)
                    {
                        var blk = new DNSBlock
                        {
                            DomainName = a.DomainName,
                            InitialTimeToLive = a.InitialTimeToLive,
                            RawDataLength = a.RawDataLength,
                            RecordClass = a.RecordClass.ToString(),
                            RecordType = a.RecordType.ToString(),
                            MgName = a.MgName
                        };
                        res.Add(blk);
                    }
                    break;
                //case QueryType.MINFO:
                //    res = result.Answers.();
                //    break;
                case QueryType.MR:
                    var mrres = result.Answers.MrRecords().ToArray();
                    foreach (var a in mrres)
                    {
                        var blk = new DNSBlock
                        {
                            DomainName = a.DomainName,
                            InitialTimeToLive = a.InitialTimeToLive,
                            RawDataLength = a.RawDataLength,
                            RecordClass = a.RecordClass.ToString(),
                            RecordType = a.RecordType.ToString(),
                            NewName = a.NewName
                        };
                        res.Add(blk);
                    }
                    break;
                case QueryType.MX:
                    var mxres = result.Answers.MxRecords().ToArray();
                    foreach (var a in mxres)
                    {

                        var blk = new DNSBlock
                        {
                            Exchange = a.Exchange,
                            Preference = a.Preference,
                            DomainName = a.DomainName,
                            InitialTimeToLive = a.InitialTimeToLive,
                            RawDataLength = a.RawDataLength,
                            RecordClass = a.RecordClass.ToString(),
                            RecordType = a.RecordType.ToString()
                        };
                        res.Add(blk);
                    }
                    break;
                case QueryType.NAPTR:
                    var naptrres = result.Answers.NAPtrRecords().ToArray();
                    foreach (var a in naptrres)
                    {
                        var blk = new DNSBlock
                        {
                            Order = a.Order,
                            RegularExpression = a.RegularExpression,
                            Flags = a.Flags,
                            Replacement = a.Replacement,
                            Services = a.Services,
                            Preference = a.Preference,
                            DomainName = a.DomainName,
                            InitialTimeToLive = a.InitialTimeToLive,
                            RawDataLength = a.RawDataLength,
                            RecordClass = a.RecordClass.ToString(),
                            RecordType = a.RecordType.ToString()
                        };
                        res.Add(blk);
                    }
                    break;
                case QueryType.NS:
                    var nsres = result.Answers.NsRecords().ToArray();
                    foreach (var a in nsres)
                    {
                        var blk = new DNSBlock
                        {
                            NSDName = a.NSDName,
                            DomainName = a.DomainName,
                            InitialTimeToLive = a.InitialTimeToLive,
                            RawDataLength = a.RawDataLength,
                            RecordClass = a.RecordClass.ToString(),
                            RecordType = a.RecordType.ToString()
                        };
                        res.Add(blk);
                    }
                    break;
                //case QueryType.NSEC:
                //    res = result.Answers();
                //    break;
                //case QueryType.NSEC3:
                //    res = result.Answers.();
                //    break;
                //case QueryType.NSEC3PARAM:
                //    res = result.Answers.ARecords();
                //    break;
                /*case QueryType.PTR:
                    res = result.Answers.PtrRecords().ToArray();
                    break;
                case QueryType.RP:
                    res = result.Answers.RpRecords().ToArray();
                    break;
                case QueryType.RRSIG:
                    res = result.Answers.RRSigRecords().ToArray();
                    break;
                case QueryType.SOA:
                    res = result.Answers.SoaRecords().ToArray();
                    break;
                //case QueryType.SPF:
                //    res = result.Answers.();
                //    break;
                case QueryType.SRV:
                    res = result.Answers.SrvRecords().ToArray();
                    break;
                //case QueryType.SSHFP:
                //    res = result.Answers.();
                //    break;
                case QueryType.TLSA:
                    res = result.Answers.TlsaRecords().ToArray();
                    break;
                case QueryType.TXT:
                    res = result.Answers.TxtRecords().ToArray();
                    break;
                case QueryType.URI:
                    res = result.Answers.UriRecords().ToArray();
                    break;
                case QueryType.WKS:
                    res = result.Answers.WksRecords().ToArray();
                    break;
                */
                default:
                    break;
            }
            return JsonConvert.SerializeObject(new JSONResponse() { Cargo = res, Crew = (from answer in result.Answers select answer.ToString()).ToArray() });
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
