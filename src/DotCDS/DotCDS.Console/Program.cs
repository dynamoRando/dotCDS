using System;
using DotCDS;

namespace DotCDS.Console // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static Process _process;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Welcome to dotCDS!");
            System.Console.WriteLine("Press enter to start.");
            var input = System.Console.ReadLine();
            System.Console.WriteLine(("Starting dotCDS..."));

            _process = new Process();
            _process.Start();

            while (!string.Equals(input, "q", System.StringComparison.OrdinalIgnoreCase))
            {
                System.Console.WriteLine("Waiting for input. Press 'q' to quit");
                input = System.Console.ReadLine();
            }

            System.Console.WriteLine("Quitting dotCDS...");
        }
    }
}