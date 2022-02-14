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
            _process = new Process();
            _process.Start();
        }
    }
}