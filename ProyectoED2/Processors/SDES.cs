using System;
using System.Collections.Generic;
using System.Text;

namespace Processors
{
    // agregar el static
    public  class SDES
    {
        public static string Cipher(string text, int key)
        {
            var keys = GenerateKeys(key);
            
            return ShowCipher(text, keys[0], keys[1]);
        }

        public static string Decipher(string text, int key)
        {
            var keys = GenerateKeys(key);

            return ShowCipher(text, keys[0], keys[1]);
        }

        private static string[] GenerateKeys(int key)
        {
            string[] keys = new string[2];
            // string entrada = ConvertToBinary(key, 10);
            string entrada = key.ToString();
            //
            string k1, k2;
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

        private static string ShowCipher(string text, string k1, string k2)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            foreach (byte b in bytes)
            {
                string entrada = IP(ConvertToBinary(int.Parse(b.ToString())).ToCharArray());
                
                char[] left = entrada.Substring(0, 4).ToCharArray();
                char[] right = entrada.Substring(4, 4).ToCharArray();

               

                string xor = XOR(EP(right).ToCharArray(), k1.ToCharArray());
                char[] xorleft = xor.Substring(0, 4).ToCharArray();
                char[] xorright = xor.Substring(4, 4).ToCharArray();
                xor = Matriz(xorleft, true) + Matriz(xorright, false);
                xor = XOR(P4(xor.ToCharArray()).ToCharArray(),left);
                entrada = right.ToString() + xor;

            }
           
            throw new NotImplementedException();
        }

        // agregaqrt el static3
        public  string CipherMessage(string message, int key1, int key2)
        {
            // return Cipher(message, DiffieHellman.GenerateKey(key1, key2));
            return Cipher(message, 1101011001);
        }

        public static string DecipherMessage(string message, int key1, int key2)
        {
            return Decipher(message, DiffieHellman.GenerateKey(key1, key2));
        }

      

        private static string ConvertToBinary(int binary)
        {
            
            string aux = "";
            for (int i = 0; i < 8; i++)
            {
                aux = binary % 2 + aux;
                binary /= 2;
            }
            return aux;
        }

        private static string XOR(char[] c1, char[] c2) {
            string text = "";
            for (int i = 0; i < c1.Length; i++)
            {
                if (c1[i].Equals(c2[i]))
                {
                    text += "0";
                }
                else {
                    text += "1";
                }
            }

            return text;
        }

        private static string Matriz(char[] c1, bool izq)
        {
           
            string[,] s0 = new string[4, 4] { { "01", "00", "11" ,"10"},
                                              { "11", "10", "01","00" },
                                              { "00", "10", "01","11" },
                                              { "11", "01", "11","10" }};

            string[,] s1 = new string[4, 4] { { "00", "01", "10" ,"11"},
                                              { "10", "00", "01","11" },
                                              { "11", "00", "01","00" },
                                              { "10", "01", "00","11" }};
            string text = "";
            int fila = Convert.ToInt32((c1[0] + c1[3]).ToString(), 2);
            int columna = Convert.ToInt32((c1[1] + c1[2]).ToString(), 2);

            if (izq)
            {
                text = s0[fila, columna];
            }
            else {
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
    }
}
