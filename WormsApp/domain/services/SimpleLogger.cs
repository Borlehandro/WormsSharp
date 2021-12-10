using System.IO;

namespace WormsApp.Domain.Services
{
    public class SimpleLogger : ILogger
    {
        private StreamWriter _writer;

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
            _writer.Dispose();
        }
    }
}