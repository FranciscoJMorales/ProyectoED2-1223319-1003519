using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Processors
{
    public static class DiffieHellman
    {
        public static int GenerateKey(int key1, int key2)
        {
                   
            return int.Parse(Key(key1,IndividualKey(key2))); 
            throw new NotImplementedException();
        }

        private static BigInteger IndividualKey(int key) {

            BigInteger A = 1;

            for (int i = 1; i <= key; i++)
            {
                A *= 171; 
            }
            return A % 1021;
                throw new NotImplementedException();
        }

        private static string Key(int privkey, BigInteger pubkey)
        {

            BigInteger K = 1;

            for (int i = 1; i <= privkey; i++)
            {
                K *= pubkey;
            }
           
            return (K % 1021).ToString();
            throw new NotImplementedException();
        }
    }
}
