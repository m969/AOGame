namespace AO
{
    using ET;
    using System.Net;

    public class AppConfig
    {
        public string IP;
        public int Zone = 1;

        public long Id;
        public string Type;
        public int Port;
        public string DBConnection;

        public IPEndPoint EndPoint
        {
            get
            {
                return new IPEndPoint(IPAddress.Parse(IP), Port);
            }
        }
    }
}