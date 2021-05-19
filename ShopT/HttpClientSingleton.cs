using System;
using System.Net.Http;

namespace ShopT
{
    public class HttpClientSingleton
    {
        private static object syncRoot = new Object();
        private static HttpClient instance;
        public static HttpClient Instance 
        {
            get 
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new HttpClient();
                        }
                        return instance;
                    }
                }
                return instance;
            }
            set 
            {
                instance = value;
            }
        }

        // This method must be in a class in a platform project, even if
        // the HttpClient object is constructed in a shared project.
        public static HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }
    }
}
