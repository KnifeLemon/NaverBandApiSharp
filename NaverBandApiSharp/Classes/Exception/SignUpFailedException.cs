public class SignUpFailedException : Exception
{
    public SignUpFailedException()
    {
    }

    public SignUpFailedException(string message) : base(message)
    {
    }

    public int code { get; set; }
    public string response { get; set; }
}