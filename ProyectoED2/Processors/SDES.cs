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
            var content = Encoding.UTF8.GetBytes(text);
            var result = new byte[content.Length];
            var key1 = ConvertToBitArray(k1, 8);
            var key2 = ConvertToBitArray(k2, 8);
            for (int i = 0; i < content.Length; i++)
                result[i] = CipherByte(ConvertToBitArray(content[i]), key1, key2);
            return Encoding.UTF8.GetString(result);
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
            var final = InvertedPI(Merge(round2, xor1));
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
            for (int i = size; i < array.Length; i++)
                aux[i] = array[i];
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
                aux[4] = aux2;
            }
            return aux;
        }

        /*
        public static string Cipher(string text, int key)
        {
            var keys = GenerateKeys(key);

            return ShowCipher(text, keys[0], keys[1], true);
        }

        public static string Decipher(string text, int key)
        {
            var keys = GenerateKeys(key);

            return ShowCipher(text, keys[0], keys[1], false);
        }

        private static string[] GenerateKeys(int key)
        {
            string[] keys = new string[2];
            string entrada = ConvertToBinary(key, 10);

            entrada = P10(entrada.ToCharArray());
            char[] leftS1 = entrada.Substring(0, 5).ToCharArray();
            char[] leftS2 = entrada.Substring(5, 5).ToCharArray();
            leftS1 = LeftShift(leftS1);
            leftS2 = LeftShift(leftS2);
            keys[0] = P8(leftS1, leftS2);
            leftS1 = LeftShift(leftS1);
            leftS1 = LeftShift(leftS1);

            leftS2 = LeftShift(leftS2);
            leftS2 = LeftShift(leftS2);
            keys[1] = P8(leftS1, leftS2);

            return keys;
            throw new NotImplementedException();
        }

        private static string ShowCipher(string text, string k1, string k2, bool cipher)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            string ciphertext = "";
            foreach (byte b in bytes)
            {
                string entrada = IP(ConvertToBinary(int.Parse(b.ToString()), 8).ToCharArray());
                for (int i = 0; i < 2; i++)
                {
                    char[] left = entrada.Substring(0, 4).ToCharArray();
                    char[] right = entrada.Substring(4, 4).ToCharArray();

                    string xor;
                    if (i == 0)
                    {
                        if (cipher)
                        {
                            xor = XOR(EP(right).ToCharArray(), k1.ToCharArray());
                        }
                        else
                        {
                            xor = XOR(EP(right).ToCharArray(), k2.ToCharArray());
                        }


                    }
                    else
                    {
                        if (cipher)
                        {
                            xor = XOR(EP(right).ToCharArray(), k2.ToCharArray());

                        }
                        else
                        {
                            xor = XOR(EP(right).ToCharArray(), k1.ToCharArray());
                        }



                    }
                    char[] xorleft = xor.Substring(0, 4).ToCharArray();
                    char[] xorright = xor.Substring(4, 4).ToCharArray();
                    xor = Matriz(xorleft, true) + Matriz(xorright, false);
                    xor = XOR(P4(xor.ToCharArray()).ToCharArray(), left);
                    if (i == 0)
                    {
                        entrada = right[0].ToString() + right[1].ToString() + right[2].ToString() + right[3].ToString() + xor;
                    }
                    else
                    {
                        entrada = IPI((xor + right[0].ToString() + right[1].ToString() + right[2].ToString() + right[3].ToString()).ToCharArray());
                    }

                }
                ciphertext += Convert.ToChar(ConvertToInt(entrada)).ToString();


            }
            return ciphertext;
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



        private static string ConvertToBinary(int binary, int digits)
        {

            string aux = "";
            for (int i = 0; i < digits; i++)
            {
                aux = binary % 2 + aux;
                binary /= 2;
            }
            return aux;
        }

        private static int ConvertToInt(string binary)
        {
            int value = 0;
            while (binary.Length > 0)
            {
                value *= 2;
                value += int.Parse(binary.Substring(0, 1));
                binary = binary.Remove(0, 1);
            }
            return value;
        }

        private static string XOR(char[] c1, char[] c2)
        {
            string text = "";
            for (int i = 0; i < c1.Length; i++)
            {
                if (c1[i].Equals(c2[i]))
                {
                    text += "0";
                }
                else
                {
                    text += "1";
                }
            }

            return text;
        }

        private static string Matriz(char[] c1, bool izq)
        {

            string[,] s0 = new string[4, 4] { { "01", "00", "11", "10" },
                                              { "11", "10", "01", "00" },
                                              { "00", "10", "01", "11" },
                                              { "11", "01", "11", "10" }};

            string[,] s1 = new string[4, 4] { { "00", "01", "10", "11" },
                                              { "10", "00", "01", "11" },
                                              { "11", "00", "01", "00" },
                                              { "10", "01", "00", "11" }};

            string text = c1[0].ToString() + c1[3].ToString();
            int fila = ConvertToInt(text);
            text = c1[1].ToString() + c1[2].ToString();
            int columna = ConvertToInt(text);
            if (izq)
            {
                text = s0[fila, columna];

            }
            else
            {
                text = s1[fila, columna];

            }
            return text;
        }

        private static char[] LeftShift(char[] ls)
        {
            char aux = ls[0];
            ls[0] = ls[1];
            ls[1] = ls[2];
            ls[2] = ls[3];
            ls[3] = ls[4];
            ls[4] = aux;
            return ls;
        }

        private static string P10(char[] entrada)
        {
            //orden = 7, 4, 1, 9, 10, 2, 6, 3, 8, 5 
            string Key = entrada[6].ToString() + entrada[3].ToString() + entrada[0].ToString() + entrada[8].ToString() + entrada[9].ToString()
                + entrada[1].ToString() + entrada[5].ToString() + entrada[2].ToString() + entrada[7].ToString() + entrada[4].ToString();

            return Key;
        }
        private static string P8(char[] ls1, char[] ls2)
        {
            //3, 10, 2, 8, 5, 4, 1, 7
            string Key = ls1[2].ToString() + ls2[4].ToString() + ls1[1].ToString() + ls2[2].ToString() + ls1[4].ToString() + ls1[3].ToString() + ls1[0].ToString() + ls2[1].ToString();
            return Key;
        }

        private static string IP(char[] b)
        {
            //4, 7, 1, 2, 6, 5, 8, 3
            string Key = b[3].ToString() + b[6].ToString() + b[0].ToString() + b[1].ToString() + b[5].ToString() + b[4].ToString() + b[7].ToString() + b[2].ToString();
            return Key;
        }

        private static string EP(char[] b)
        {
            //4, 2, 3, 1, 2, 1, 4, 3
            string Key = b[3].ToString() + b[1].ToString() + b[2].ToString() + b[0].ToString() + b[1].ToString() + b[0].ToString() + b[3].ToString() + b[2].ToString();
            return Key;
        }

        private static string P4(char[] b)
        {
            //3, 2, 4, 1
            string Key = b[2].ToString() + b[1].ToString() + b[3].ToString() + b[0].ToString();
            return Key;
        }

        private static string IPI(char[] b)
        {
            //3, 4,  8, 1, 6, 5, 2, 7
            string Key = b[2].ToString() + b[3].ToString() + b[7].ToString() + b[0].ToString()
                + b[5].ToString() + b[4].ToString() + b[1].ToString() + b[6].ToString();
            return Key;
        }
        */
    }
}
