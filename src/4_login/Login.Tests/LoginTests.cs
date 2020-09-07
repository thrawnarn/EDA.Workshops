using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Login.Tests
{
    public class LoginTests
    {
        [Fact]
        public void ThreeFailedAttemptsLast16MinutesSucceceeds()
        {
            var app = new App(() => DateTime.Parse("2020-06-01 13:30"));
            var time = DateTime.Parse("2020-06-01 13:15");

            //Given
            app.Given(new[] {
                new AuthenticationAttemptFailed { Time = time.AddMinutes(-1) },
                new AuthenticationAttemptFailed { Time = time.AddMinutes(1)  },
                new AuthenticationAttemptFailed { Time = time.AddMinutes(12)  }
            });

            //When
            app.Handle(new Login());
        }

        [Fact]
        public void ThreeFailedAttemptsLast15minutesThrows()
        {
            var app = new App(() => DateTime.Parse("2020-06-01 13:30"));
            var time = DateTime.Parse("2020-06-01 13:15");

            //Given
            app.Given(new[] {
                new AuthenticationAttemptFailed { Time = time.AddMinutes(1) },
                new AuthenticationAttemptFailed { Time = time.AddMinutes(5)  },
                new AuthenticationAttemptFailed { Time = time.AddMinutes(12)  }
            });

            //When
            Assert.Throws<AuthenticationException>(
                () => app.Handle(new Login())
            );
        }
    }


}
