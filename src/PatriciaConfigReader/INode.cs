using System;
using System.Collections.Generic;
using System.Configuration;

namespace PatriciaConfigReader
{
    public partial class Patricia
    {
        private interface INode
        {
            //add a word <-> data association; if the word already exists, the method overwrites it
            void add<T>(string word, Wrapper<T> data, INode parent);
            //returns INode that represents the 'word'; returns null if no such Node exists
            INode findAux(string word);

            Dictionary<char, INode> map { get; set; }
        }
    }
}
