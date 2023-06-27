namespace QuizzBankBE.Model
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
            Message = message;
            StatusCode = status == true ? 200 : 404;
            Status = status;
        }

        public T? Data { get; set; }
        public int StatusCode { get; set; } = 200;

        public bool Status { get; set; } = true;

        public string Message { get; set; }

        public string accessToken { get; set; }
    }
}
