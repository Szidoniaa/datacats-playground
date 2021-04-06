using System;
using System.Net;

namespace Equinor.OmniaDataCatalogApi.Common.Exceptions
{
    public class HttpException : Exception
    {
        public HttpException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}