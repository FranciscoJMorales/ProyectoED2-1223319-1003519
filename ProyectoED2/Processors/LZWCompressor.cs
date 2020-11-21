using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Processors
{
    public class LZWCompressor
    {
        private readonly string Path;


        public LZWCompressor()
        {
            Path = null;
        }

        public LZWCompressor(string path)
        {
            Path = path;
        }

        public string ShowCompress(string text)
        {
            if (text.Length > 0)
            {
                var list = new Dictionary<string, int>();
                foreach (var item in text)
                {
                    if (!list.ContainsKey(item.ToString()))
                        list.Add(item.ToString(), list.Count + 1);
                }
                string metadata = Convert.ToChar(list.Count).ToString();
                foreach (var item in list)
                    metadata += item.Key;
                var lzw = new List<int>();
                while (!list.ContainsKey(text))
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (!list.ContainsKey(text.Substring(0, i + 1)))
                        {
                            list.Add(text.Substring(0, i + 1), list.Count + 1);
                            lzw.Add(list[text.Substring(0, i)]);
                            text = text.Remove(0, i);
                            break;
                        }
                    }
                }
                lzw.Add(list[text]);
                int max = lzw[0];
                foreach (var item in lzw)
                {
                    if (item > max)
                        max = item;
                }
                int size = GetBinarySize(max);
                string binary = "";
                foreach (var item in lzw)
                    binary += ConvertToBinary(Convert.ToChar(item), size);
                while (binary.Length % 8 != 0)
                    binary += "0";
                string final = "";
                while (binary.Length > 0)
                {
                    final += Convert.ToChar(ConvertToInt(binary.Substring(0, 8)));
                    binary = binary.Remove(0, 8);
                }
                return Convert.ToChar(size) + metadata + final;
            }
            else
                return "";
        }

        public string Compress(byte[] array, string currentName, string newName)
        {
            string text = ConvertToString(array);
            string lzw = Convert.ToChar(currentName.Length) + currentName + ShowCompress(text);
            string path = Path + "\\" + newName + ".lzw";
            using StreamWriter writer = new StreamWriter(path, false);
            writer.Write(lzw);
            writer.Close();
            return path;
        }

        private int GetBinarySize(int max)
        {
            int size = 0;
            while (max > 0)
            {
                size++;
                max /= 2;
            }
            return size;
        }

        private int ConvertToInt(string binary)
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

        private string ConvertToBinary(char value, int digits)
        {
            int binary = Convert.ToInt32(value);
            string aux = "";
            for (int i = 0; i < digits; i++)
            {
                aux = binary % 2 + aux;
                binary /= 2;
            }
            return aux;
        }

        public string ShowDecompress(string text)
        {
            if (text.Length > 0)
            {
                var size = Convert.ToInt32(text[0]);
                var letters = Convert.ToInt32(text[1]);
                text = text.Remove(0, 2);
                var list = new Dictionary<int, string>();
                for (int i = 0; i < letters; i++)
                {
                    list.Add(list.Count + 1, text.Substring(i, 1));
                }
                text = text.Remove(0, letters);
                string binary = "";
                foreach (var item in text)
                    binary += ConvertToBinary(item, 8);
                var lzw = new List<int>();
                while (binary.Length >= size)
                {
                    if (binary.Substring(0, size) != new string('0', size))
                        lzw.Add(ConvertToInt(binary.Substring(0, size)));
                    binary = binary.Remove(0, size);
                }
                string final = list[lzw[0]];
                for (int i = 1; i < lzw.Count; i++)
                {
                    list.Add(list.Count + 1, list[lzw[i - 1]]);
                    if (i > 1)
                        list[list.Count - 1] += list[lzw[i - 1]].Substring(0, 1);
                    final += list[lzw[i]];
                }
                return final;
            }
            else
                return "";
        }

        public string Decompress(string text)
        {
            int titleLength = Convert.ToInt32(text[0]);
            string title = text.Substring(1, titleLength);
            text = text.Remove(0, titleLength + 1);
            string final = ShowDecompress(text);
            string path = Path + "\\" + title;
            using var file = new FileStream(path, FileMode.Create);
            file.Write(ConvertToByteArray(final), 0, final.Length);
            return path;
        }

        private string ConvertToString(byte[] array)
        {
            string text = "";
            foreach (var item in array)
                text += Convert.ToString(Convert.ToChar(item));
            return text;
        }

        private byte[] ConvertToByteArray(string text)
        {
            byte[] array = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
                array[i] = Convert.ToByte(text[i]);
            return array;
        }
    }
}
