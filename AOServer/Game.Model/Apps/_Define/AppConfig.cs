namespace AO
{
    using ET;
    using System.Net;

    public class AppConfig
    {
        public string IP;

        public long Id;
        public string Type;
        public int Port;

        public IPEndPoint EndPoint
        {
            get
            {
                return new IPEndPoint(IPAddress.Parse(IP), Port);
            }
        }
    }
}