using System;
using System.Collections.Generic;
using System.Text;

namespace ezHelper.Logging
{
    public interface ILogger
    {
        void Log(string message, string fileName = "Log");
        void Log(Exception exception, string fileName = "Log");
    }
}
