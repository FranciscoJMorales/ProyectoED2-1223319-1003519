﻿using System;
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

        private static int[] GenerateKeys(int k)
        {
            var key = ConvertToBitArray(k, 10);
            var p10 = P10(key);
            var left = Ci(DivideLeft(p10), 1);
            var right = Ci(DivideRight(p10), 1);
            var k1 = P8(Merge(left, right));
            var left2 = Ci(left, 2);
            var right2 = Ci(right, 2);
            var k2 = P8(Merge(left2, right2));
            int[] keys = { ConvertToInt(k1), ConvertToInt(k2) };
            return keys;
        }

        private static string ShowCipher(string text, int k1, int k2)
        {
            var content = ConvertToByteArray(text);
            var result = new byte[content.Length];
            var key1 = ConvertToBitArray(k1, 8);
            var key2 = ConvertToBitArray(k2, 8);
            for (int i = 0; i < content.Length; i++)
                result[i] = CipherByte(ConvertToBitArray(content[i]), key1, key2);
            return ConvertToString(result);
        }

        private static byte CipherByte(bool[] text, bool[] k1, bool[] k2)
        {
            var ip = PI(text);
            var left = DivideLeft(ip);
            var right = DivideRight(ip);
            var round1 = Round(right, k1);
            var xor1 = Xor(left, round1);
            var round2 = Round(xor1, k2);
            var xor2 = Xor(right, round2);
            var final = InvertedPI(Merge(xor2, xor1));
            return ConvertToByte(final);
        }

        public static string CipherMessage(string message, int key1, int key2)
        {
            return Cipher(message, DiffieHellman.GenerateKey(key1, key2));
        }

        public static string DecipherMessage(string message, int key1, int key2)
        {
            return Decipher(message, DiffieHellman.GenerateKey(key1, key2));
        }

        private static bool[] Xor(bool[] a, bool[] b)
        {
            bool[] final = new bool[a.Length];
            for (int i = 0; i < final.Length; i++)
            {
                final[i] = a[i] != b[i];
            }
            return final;
        }

        private static bool[] Round(bool[] text, bool[] key)
        {
            var ep = EP(text);
            var xor = Xor(ep, key);
            var left = S0(DivideLeft(xor));
            var right = S1(DivideRight(xor));
            return P4(Merge(left, right));
        }

        private static string ConvertToString(byte[] array)
        {
            string text = "";
            foreach (var item in array)
                text += Convert.ToString(Convert.ToChar(item));
            return text;
        }

        private static byte[] ConvertToByteArray(string text)
        {
            byte[] array = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
                array[i] = Convert.ToByte(text[i]);
            return array;
        }

        private static bool[] ConvertToBitArray(byte n)
        {
            bool[] aux = new bool[8];
            for (int i = 7; i >= 0; i--)
            {
                aux[i] = (n % 2 == 1);
                n /= 2;
            }
            return aux;
        }

        private static bool[] ConvertToBitArray(int n, int size)
        {
            bool[] aux = new bool[size];
            for (int i = size - 1; i >= 0; i--)
            {
                aux[i] = (n % 2 == 1);
                n /= 2;
            }
            return aux;
        }

        private static int ConvertToInt(bool[] array)
        {
            int val = 0;
            for (int i = 0; i < array.Length; i++)
            {
                val *= 2;
                if (array[i])
                    val += 1;
            }
            return val;
        }

        private static byte ConvertToByte(bool[] array)
        {
            byte val = 0;
            for (int i = 0; i < array.Length; i++)
            {
                val *= 2;
                if (array[i])
                    val += 1;
            }
            return val;
        }

        private static bool[] DivideLeft(bool[] array)
        {
            int size = array.Length / 2;
            bool[] aux = new bool[size];
            for (int i = 0; i < size; i++)
                aux[i] = array[i];
            return aux;
        }

        private static bool[] DivideRight(bool[] array)
        {
            int size = array.Length / 2;
            bool[] aux = new bool[size];
            for (int i = 0; i < size; i++)
                aux[i] = array[i + size];
            return aux;
        }

        private static bool[] Merge(bool[] first, bool[] last)
        {
            int size = first.Length + last.Length;
            bool[] aux = new bool[size];
            for (int i = 0; i < first.Length; i++)
                aux[i] = first[i];
            for (int i = 0; i < last.Length; i++)
                aux[first.Length + i] = last[i];
            return aux;
        }

        private static bool[] P10(bool[] item)
        {
            bool[] aux = { item[6], item[3], item[0], item[8], item[9], item[1], item[5], item[2], item[7], item[4] };
            return aux;
        }

        private static bool[] P8(bool[] item)
        {
            bool[] aux = { item[2], item[9], item[1], item[7], item[4], item[3], item[0], item[6] };
            return aux;
        }

        private static bool[] P4(bool[] item)
        {
            bool[] aux = { item[2], item[1], item[3], item[0] };
            return aux;
        }

        private static bool[] EP(bool[] item)
        {
            bool[] aux = { item[3], item[1], item[2], item[0], item[1], item[0], item[3], item[2] };
            return aux;
        }

        private static bool[] PI(bool[] item)
        {
            bool[] aux = { item[3], item[6], item[0], item[1], item[5], item[4], item[7], item[2] };
            return aux;
        }

        private static bool[] InvertedPI(bool[] item)
        {
            bool[] aux = { item[2], item[3], item[7], item[0], item[5], item[4], item[1], item[6] };
            return aux;
        }

        private static bool[] S0(bool[] item)
        {
            int[,] s0 = new int[4, 4] { { 1, 0, 3, 2 },
                                        { 3, 2, 1, 0 },
                                        { 0, 2, 1, 3 },
                                        { 3, 1, 3, 2 }};
            bool[] first = { item[0], item[3] };
            bool[] last = { item[1], item[2] };
            int row = ConvertToInt(first);
            int column = ConvertToInt(last);
            int val = s0[row, column];
            return ConvertToBitArray(val, 2);
        }

        private static bool[] S1(bool[] item)
        {
            int[,] s1 = new int[4, 4] { { 0, 1, 2, 3 },
                                        { 2, 0, 1, 3 },
                                        { 3, 0, 1, 0 },
                                        { 2, 1, 0, 3 }};
            bool[] first = { item[0], item[3] };
            bool[] last = { item[1], item[2] };
            int row = ConvertToInt(first);
            int column = ConvertToInt(last);
            int val = s1[row, column];
            return ConvertToBitArray(val, 2);
        }

        private static bool[] Ci(bool[] item, int times)
        {
            bool[] aux = (bool[])item.Clone();
            for (int i = 0; i < times; i++)
            {
                bool aux2 = aux[0];
                aux[0] = aux[1];
                aux[1] = aux[2];
                aux[2] = aux[3];
                aux[3] = aux[4];
                aux[4] = aux2;
            }
            return aux;
        }
    }
}
