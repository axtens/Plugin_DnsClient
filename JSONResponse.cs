using System;

namespace DnsClient
{
    [Serializable]
    internal class JSONResponse
    {
        public object Cargo { get; set; }
        public object Error { get; set; }
        public object Crew { get; set; }
    }
}