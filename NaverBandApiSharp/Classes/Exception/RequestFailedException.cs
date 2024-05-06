public class RequestFailedException : Exception
{
    public RequestFailedException()
    {
    }

    public RequestFailedException(string message) : base(message)
    {
    }

    public int code { get; set; }
    public string response { get; set; }
}