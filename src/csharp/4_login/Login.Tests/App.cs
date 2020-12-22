using System;
using System.Collections.Generic;
using System.Text;

namespace Login.Tests
{

    public interface IEvent
    {
        string SourceId { get; }
        IDictionary<string, string> Meta { get; }
    }

    public interface ICommand
    { }

    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message)
        { }
    }

    public class Login : ICommand
    { }

    public class AuthenticationAttemptFailed : IEvent
    {
        public string SourceId => "auth";

        public DateTime Time { get; set; }

        public IDictionary<string, string> Meta { get; set; }

    }
}
