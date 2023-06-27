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

        public void updateResponse(int statusCode, string message)
        {
                Status = statusCode == 200 ? true : false;
                StatusCode = statusCode;
                Message = message;
        }

        public T? Data { get; set; }
        public int StatusCode { get; set; }

        public bool Status { get; set; }

        public string Message { get; set; }

        public string accessToken { get; set; }
    }
}
