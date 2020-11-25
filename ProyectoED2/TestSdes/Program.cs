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
            
           Console.WriteLine(cifrado.DecipherMessage(cifrado.CipherMessage("H", 2, 4), 2,4));
            
        }
    }
}
