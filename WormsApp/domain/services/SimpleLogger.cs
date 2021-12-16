using System;
using System.IO;

namespace WormsApp.Domain.Services
{
    public class SimpleLogger : ILogger
    {
        private readonly StreamWriter _writer;

        public SimpleLogger(StreamWriter writer)
        {
            _writer = writer;
        }

        public void Log(string content)
        {
            _writer.WriteLine(content);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _writer.Dispose();
        }
    }
}