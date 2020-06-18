using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Login.Tests
{
    public class App
    {
        public App(Func<DateTime> timeProvider)
        {
            this.timeProvider = timeProvider;
        }

        List<IEvent> history = new List<IEvent>();
        readonly Func<DateTime> timeProvider;

        public void Given(params IEvent[] events) => history.AddRange(events);


        public void Handle(Login command)
        {
            if (history.ToManyAttempts(timeProvider))
                throw new AuthenticationException("To many attempts");
        }
    }

    public static class LoginRules
    {
        public static bool ToManyAttempts(this IEnumerable<IEvent> events, Func<DateTime> timeProvider)
            => events
            .OfType<AuthenticationAttemptFailed>()
            //TODO
            .Count() <= 3;
    }

    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message)
        {}
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
