using System;
using System.Collections.Generic;
using System.Text;

namespace Processors
{
    public static class SDES
    {
        public static string Cipher(string text, int key)
        {
            var keys = GenerateKeys(key);
            return ShowCipher(text, keys[0], keys[1]);
        }

        public static string Decipher(string text, int key)
        {
            var keys = GenerateKeys(key);
            return ShowCipher(text, keys[1], keys[0]);
        }

        private static int[] GenerateKeys(int key)
        {
            throw new NotImplementedException();
        }

        private static string ShowCipher(string text, int k1, int k2)
        {
            throw new NotImplementedException();
        }

        public static string CipherMessage(string message, int key1, int key2)
        {
            return Cipher(message, DiffieHellman.GenerateKey(key1, key2));
        }

        public static string DecipherMessage(string message, int key1, int key2)
        {
            return Decipher(message, DiffieHellman.GenerateKey(key1, key2));
        }
    }
}
