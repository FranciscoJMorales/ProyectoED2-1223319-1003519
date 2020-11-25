using System;
using System.Collections.Generic;
using System.Text;

namespace Processors
{
    // agregar el static
    public  class SDES
    {
        //pasarlo a privado
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
                string entrada = IP(ConvertToBinary(int.Parse(b.ToString()),8).ToCharArray());
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
                        else {
                            xor = XOR(EP(right).ToCharArray(), k2.ToCharArray());
                        }
                        

                    }
                    else {
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
                    else {
                        entrada = IPI((xor+ right[0].ToString() + right[1].ToString() + right[2].ToString() + right[3].ToString()).ToCharArray());
                    }
                    
                }
                ciphertext += Convert.ToChar(ConvertToInt(entrada)).ToString();

                
            }
                return ciphertext;
            throw new NotImplementedException();
        }

        // agregaqrt el static3
        public  string CipherMessage(string message, int key1, int key2)
        {
            return Cipher(message, DiffieHellman.GenerateKey(key1, key2));
            //return Cipher(message, 857);
        }

        //static
        public  string DecipherMessage(string message, int key1, int key2)
        {
            
            return Decipher(message, DiffieHellman.GenerateKey(key1, key2));
           // return Decipher(message, 857);
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

        private static long ConvertToInt(string binary)
        {
            long value = 0;
            while (binary.Length > 0)
            {
                value *= 2;
                value += int.Parse(binary.Substring(0, 1));
                binary = binary.Remove(0, 1);
            }
            return value;
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
           
            string[,] s0 = new string[4, 4] { { "01", "00", "11", "10" },
                                              { "11", "10", "01", "00" },
                                              { "00", "10", "01", "11" },
                                              { "11", "01", "11", "10" }};

            string[,] s1 = new string[4, 4] { { "00", "01", "10", "11" },
                                              { "10", "00", "01", "11" },
                                              { "11", "00", "01", "00" },
                                              { "10", "01", "00", "11" }};
            string text = c1[0].ToString() + c1[3].ToString();

            //int fila = Convert.ToInt32(text, 2);
            long fila = ConvertToInt(text); 
            text = c1[1].ToString() + c1[2].ToString();
            //int columna = Convert.ToInt32(text, 2); 
            long columna = ConvertToInt(text);
            if (izq)
            {
                text = s0[fila, columna];
                //text = s0[ columna, fila];
            }
            else {
                text = s1[fila, columna];
               // text = s1[columna, fila];
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
    }
}
