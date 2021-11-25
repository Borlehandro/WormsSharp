using System.IO;
using WormsApp.Domain.Services;

namespace WormsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Use game config
            new GameLauncher(new StreamWriter(args[0])).Start();
        }
    }
}