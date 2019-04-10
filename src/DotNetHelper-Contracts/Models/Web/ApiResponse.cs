using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DotNetHelper_Contracts.Enum;

namespace DotNetHelper_Contracts.Models.Web
{
    public class ApiResponse
    {

        public string Version => "1.0";

        public string Status { get; set; }
        public int StatusCode { get; set; }

        public string MessageForUser { get; set; }
        public bool Success { get; set; }

        public List<Exception> Errors { get; set; }

        public object Result { get; set; }

        public ApiResponse(int statusCode, string status, object result = null, List<Exception> errorMessage = null)
        {
            StatusCode = statusCode;
            Status = status;
            Result = result;
            Errors = errorMessage;
        }

        public ApiResponse()
        {
            
        }


     
    }
    public static class ApiResponseExtension
    {




        public static ApiResponse GetExceptionApiResponse(this ApiResponse response, Exception error, MyEnvironment enviroment)
        {

            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.Status = "Internal Server Error";
            if (enviroment == MyEnvironment.Development || enviroment == MyEnvironment.Staging)
            {
                response.Errors.Add(error);
            }

            if (error is InvalidDataException)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Status = $"Bad Request";
            }

            response.Success = false;
            return response;
        }

        public static ApiResponse GetExceptionApiResponse(this ApiResponse response, Exception error, MyEnvironment enviroment, string fromBody)
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.Status = "Internal Server Error";
            if (enviroment == MyEnvironment.Development || enviroment == MyEnvironment.Staging)
            {
                response.Errors.Add(error);
            }
            if (error is InvalidDataException)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Status = $"Bad Request";
            }

            if (string.IsNullOrEmpty(fromBody))
                response.Errors.Add(new Exception("We Didn't Recieve An Object From The Body Of Your Request"));
            response.Success = false;
            return response;
        }



    }
}
