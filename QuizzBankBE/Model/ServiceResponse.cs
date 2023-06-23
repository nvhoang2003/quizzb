﻿namespace QuizzBankBE.Model
{
    public class ServiceResponse<T>
    {
        public ServiceResponse()
        {
        }

        public ServiceResponse(bool status)
        {
            if (!status)
            {
                Status = false;
                StatusCode = 400;
            }
        }

        public ServiceResponse(bool status, string message)
        {
            if (!status)
            {
                Status = false;
                StatusCode = 400;
                Message = message;
            }
        }

        public T? Data { get; set; }
        public int StatusCode { get; set; }

        public bool Status { get; set; }

        public string Message { get; set; }

        public string accessToken { get; set; }
    }
}
