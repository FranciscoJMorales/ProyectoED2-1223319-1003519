using Processors;
using System;

namespace TestSdes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            SDES cifrado = new SDES();

            cifrado.CipherMessage("LL",1,2);
        }
    }
}
