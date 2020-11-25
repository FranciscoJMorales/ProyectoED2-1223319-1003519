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
            string x = cifrado.CipherMessage("Hola soy ESTUARDO", 2, 4);
           Console.WriteLine("texto cifrado" + x+"\n" +"texto descifrado " + cifrado.DecipherMessage(x, 2,4));
            
        }
    }
}
