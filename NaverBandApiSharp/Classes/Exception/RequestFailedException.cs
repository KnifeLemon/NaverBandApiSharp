using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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