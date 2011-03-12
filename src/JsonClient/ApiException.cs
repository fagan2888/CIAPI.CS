﻿using System;
using System.IO;
using System.Net;

namespace CityIndex.JsonClient
{
    /// <summary>
    /// An exception class that will retrieve the response text of the inner exception if it is a WebException
    /// </summary>
    public class ApiException : Exception
    {
        internal ApiException(Exception inner)
            : base(inner.Message, inner)
        {
            if (inner is WebException)
            {
                GetResponseText((WebException) inner);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        public ApiException(string message)
            : base(message)
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        ///<param name="inner"></param>
        public ApiException(string message, Exception inner)
            : base(message, inner)
        {
            if (inner is WebException)
            {
                GetResponseText((WebException) inner);
            }
            if (inner is ApiException)
            {
                ResponseText = ((ApiException) inner).ResponseText;
            }
        }


        ///<summary>
        ///</summary>
        public string ResponseText { get; protected set; }

        ///<summary>
        ///</summary>
        ///<param name="inner"></param>
        ///<returns></returns>
        public static ApiException Create(Exception inner)
        {
            if (inner is ApiException)
            {
                return (ApiException) inner;
            }

            return new ApiException(inner);
        }

        private void GetResponseText(WebException inner)
        {
            try
            {
                // test is breaking this
                // TODO: add response to test request?
                if (inner.Response != null)
                {
                    using (Stream stream = inner.Response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                ResponseText = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch 
            {
                // should swallow?
                ResponseText = "Could not parse exception response";
            }
        }
    }


    ///<summary>
    ///</summary>
    public class ApiSerializationException : ApiException
    {
        ///<summary>
        ///</summary>
        ///<param name="message"></param>
        ///<param name="responseText"></param>
        public ApiSerializationException(string message, string responseText)
            : base(message)
        {
            ResponseText = responseText;
        }
    }


    ///<summary>
    ///</summary>
    public class ResponseHandlerException : ApiException
    {
        internal ResponseHandlerException(Exception inner)
            : base(inner)
        {
            if (inner is ApiException)
            {
                ResponseText = ((ApiException) inner).ResponseText;
            }
        }

        internal ResponseHandlerException(string message)
            : base(message)
        {
        }

        internal ResponseHandlerException(string message, Exception inner)
            : base(message, inner)
        {
            if (inner is ApiException)
            {
                ResponseText = ((ApiException) inner).ResponseText;
            }
        }
    }
}