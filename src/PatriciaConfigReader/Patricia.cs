using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace PatriciaConfigReader
{
    public partial class Patricia
    {
        public Patricia()
		{ 
			root = new Node<int> ("");
		}
		INode root;

        //wrappers for Node methods

		//adds a key-value pair
        public void add<T>(string word, Wrapper<T> data)
        {
            if (word == "") return;
			root.add<T>(word, data, root);
        }

		//edits a value under the selected key, if it exists; returns true on success, false otherwise
		public bool edit<T>(string word, Wrapper<T> data)
        {
			if (word == "")
				return false;
			Node<T> node = root.findAux(word) as Node<T>;
			if (node != null) 
			{
				node.nodeData = data;
				return true;
			}
            else
                return false;
        }

		//searches for the specified key; returns true if the specified key was found, flase otherwise
        public bool find(string v)
        {
			if (v == "") return false;
			if (root.findAux (v) != null)
				return true;
			else
				return false;
        }

		//returns data from the selected key, or null if not possible
        public Wrapper<T> getNodeData<T>(string v)
        {
            if (v == "")
                return null;

			Node<T> node = root.findAux(v) as Node<T>;
			if (node != null)
				return node.nodeData;
			else
				return null;
        }
    }
}