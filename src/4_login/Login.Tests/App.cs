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
}
