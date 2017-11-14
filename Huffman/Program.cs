using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = File.ReadAllText(@"C:\Users\Искандер\Desktop\My.txt");
            text = text.Replace(System.Environment.NewLine, "");

            HuffmanTree huffmanTree = new HuffmanTree();
            huffmanTree.BuildForTwoSymbols(text);
            List<bool> encoded = huffmanTree.Encode(text);      
            string decoded = huffmanTree.Decode(encoded);
            Console.WriteLine((double)encoded.Count / text.Length);
            //File.WriteAllText(@"C:\Users\Искандер\source\repos\Huffman\Huffman\TextFile1.txt", decoded);
            Console.ReadLine();
        }
    }
}
