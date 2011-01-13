﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIAPI.Streaming;

namespace CIAPI.IntegrationTests.Streaming
{
    public class StreamingClientBuilder
    {
        public static Client BuildStreamingClient()
        {
            const string apiUrl = "https://ciapipreprod.cityindextest9.co.uk/TradingApi/";
            const string userName = "0x234";
            const string password = "password";

            var authenticatedClient = new Rpc.Client(new Uri(apiUrl));
            authenticatedClient.LogIn(userName, password);
           
            var streamingUri = new Uri("https://pushpreprod.cityindextest9.co.uk/CITYINDEXSTREAMING");

            return new Client(streamingUri, userName, authenticatedClient.SessionId);
        }
    }
}