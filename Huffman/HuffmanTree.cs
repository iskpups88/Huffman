using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HuffmanTest;

namespace Huffman
{
    class HuffmanTree
    {
        private List<Node> nodes = new List<Node>();
        public Node Root { get; set; }
        public Dictionary<string, int> Frequencies = new Dictionary<string, int>();
        public Dictionary<string, List<bool>> encodedSymbols = new Dictionary<string, List<bool>>();
        public Dictionary<string, double> ConditionProb = new Dictionary<string, double>();

        public void Build(string source)
        {
            Frequencies = source.GroupBy(c => c).ToDictionary(q => q.Key.ToString(), q => q.Count());
            foreach (KeyValuePair<string, int> symbol in Frequencies)
            {
                nodes.Add(new Node() {Symbol = symbol.Key, Frequency = symbol.Value});
            }

            while (nodes.Count > 1)
            {
                List<Node> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList<Node>();

                if (orderedNodes.Count >= 2)
                {
                    // Take first two items
                    List<Node> taken = orderedNodes.Take(2).ToList<Node>();

                    // Create a parent node by combining the frequencies
                    Node parent = new Node()
                    {
                        Symbol = "^",
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                this.Root = nodes.FirstOrDefault();

            }

        }

        public void BuildForTwoSymbols(string source)
        {
            Frequencies = source.GroupBy(c => c).ToDictionary(q => q.Key.ToString(), q => q.Count());
            Dictionary<string, double> Probability = Frequencies.ToDictionary(item => item.Key, item => (double)item.Value / source.Length);
            List<String> pairList = new List<string>();
            Dictionary<string, double> payrsP = new Dictionary<string, double>();
            for (int i = 0; i < source.Length - 1; i++)
            {
                pairList.Add(string.Concat(source[i], source[i + 1]));
            }
            Dictionary<string, double> payrs = pairList.GroupBy(c => c)
                .ToDictionary(g => g.Key, g => (double)g.Count() / source.Length);
            ConditionProb = new Dictionary<string, double>();
            double p = 0;
            foreach (var pair in payrs)
            {
                foreach (var symbol in Probability)
                {
                    if (pair.Key[0].ToString() == symbol.Key)
                    {
                        p = pair.Value / Probability[pair.Key[1].ToString()];
                        ConditionProb.Add(pair.Key, p);
                        break;
                    }
                }
            }
            foreach (KeyValuePair<string, double> symbol in ConditionProb)
            {
                nodes.Add(new Node() { Symbol = symbol.Key, Frequency = symbol.Value });
            }
            while (nodes.Count > 1)
            {
                List<Node> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList<Node>();

                if (orderedNodes.Count >= 2)
                {
                    // Take first two items
                    List<Node> taken = orderedNodes.Take(2).ToList<Node>();

                    // Create a parent node by combining the frequencies
                    Node parent = new Node()
                    {
                        Symbol = "^",
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                this.Root = nodes.FirstOrDefault();
            }
        }

        public List<bool> Encode(string text)
        {
            List<bool> result = new List<bool>();
            foreach (var item in ConditionProb)
            {
                encodedSymbols.Add(item.Key, this.Root.Traverse(item.Key, new List<bool>()));
            }
            for (int i = 0; i < text.Length - 1; i++)
            {
                result.AddRange(encodedSymbols[text[i].ToString() + text[i+1]]);
                i++;

            }
            return result;
        }

        public string Decode(List<bool> bits)
        {
            string decoded = "";
            //List<string> current = new List<string>();
            //Dictionary<List<string>, string> test = encodedSymbols.ToDictionary(item => item.Value, item => item.Key);

            //String[] arr = bits.ToArray();
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    current.Add("111");                
            //    if (test.ContainsKey(current))
            //    {
            //        var myKey = encodedSymbols.FirstOrDefault(x => x.Value == current).Key;
            //        decoded += myKey;
            //        current.Clear();
            //    }

            //}
            Node current = this.Root;
            foreach (bool bit in bits)
            {
                if (bit == true)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }
                if (current.Left == null && current.Right == null)
                {
                    decoded += current.Symbol;
                    current = this.Root;
                }
            }
            return decoded;
        }
    }
}
