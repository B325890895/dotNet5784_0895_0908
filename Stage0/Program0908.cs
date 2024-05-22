// See https://aka.ms/new-console-template for more information
using System;
namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome0908();
            Welcome0895();
            Console.ReadKey();
        }

        static partial void Welcome0895();
        private static void Welcome0908()
        {
            Console.WriteLine("enter your name:");
            string? name = Console.ReadLine();
            Console.WriteLine(name + " welcome to my first console application!");
        }
    }
}

