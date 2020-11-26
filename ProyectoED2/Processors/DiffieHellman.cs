using System;
using System.Collections.Generic;
using System.Text;

namespace Processors
{
    public static class DiffieHellman
    {
        public static int GenerateKey(int key1, int key2)
        {
            int aux = 137;
            for (int i = 1; i < key1; i++)
            {
                aux *= 137;
                aux %= 1021;
            }
            int key = aux;
            for (int i = 1; i < key2; i++)
            {
                key *= aux;
                key %= 1021;
            }
            return key;
        }
    }
}
