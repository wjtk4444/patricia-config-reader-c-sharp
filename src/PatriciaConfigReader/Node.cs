using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace PatriciaConfigReader
{
	public partial class Patricia
	{
		private class Node <T> : INode
		{
			//------------------//
			//---CONSTRUCTORS---//
			//------------------//
			public Node() 
			{
				map = new Dictionary<char, INode>();
			}
			public Node(string path)
			{
				this.path = path;
				map = new Dictionary<char, INode>();
			}
			public Node(string path, Wrapper<T> nodeData)
			{
				this.path = path;
				this.nodeData = nodeData;
				loggedWord = true;
				map = new Dictionary<char, INode>();
			}

			public Node(string path, Dictionary<char, INode> m)
			{
				this.path = path;
				map = new Dictionary<char, INode>(m);
			}
			public Node(string path, Dictionary<char, INode> m, Wrapper<T> nodeData)
			{
				this.path = path;
				this.nodeData = nodeData;
				loggedWord = true;
				map = new Dictionary<char, INode>(m);
			}

			//------------------//
			//----DATA FIELDS---//
			//------------------//
			//using Dictionary due to the nature of the Patricia; It'll keep config data and more often
			//than not, there will be only a few branches per Node, rather than the whole alphabet
			public Dictionary<char, INode> map {get; set;}

			public Wrapper<T> nodeData = null;   //Value kept under the logged word; should be null if !loggedWord
			readonly bool loggedWord = false;   //true = end of a logged word
			string path = null;     //letters on the path to the Node


			//------------------//
			//------METHODS-----//
			//------------------//
			//adds a value to the tree; if a the word was already in the Trie, it will be overwritten
			public void add<U>(string word, Wrapper<U> data, INode parent)
			{
				int i;
				//compare the word with the path
				for (i = 0; i < Math.Min(path.Length, word.Length); i++)
				{
					if (path[i] != word[i])
						break;
				}

				//entire path matches
				if (i == path.Length)
				{
					//the added word is an existing subword
					if(word.Length == path.Length)
					{
						Node<U> node = new Node<U>(path, map, data);
						parent.map[word[0]] = node;
					}
					//the added word is longer than what was searched thus far, search further
					else
					{
						word = word.Substring(i);

						//we can go further down the tree
						if (map.ContainsKey (word [0]))
							map [word [0]].add<U> (word, data, this);
						//no more logged data, create new word and log it as a word
						else 
							map [word [0]] = new Node<U> (word, data);
					}
				}
				//comparison ended before entire path was checked
				else
				{
					//the added word is a new subword - add a node, split a path, don't add new branches
					if (i == word.Length)
					{
						Node<U> node = new Node<U> (word, data);

						parent.map[word[0]] = node;
						node.map[path[i]] = this;
						path = path.Substring(i);
					}
					//the added word is a new branch of the trie - add a node, and branch the path
					else
					{
						Node<U> node = new Node<U> (path.Substring (0, i));
						parent.map[word[0]] = node;

						path = path.Substring(i);
						word = word.Substring(i);

						node.map[path[0]] = this;
						node.map[word[0]] = new Node<U>(word, data);
					}
				}
			}

			//return an INode (or null) if a node with the requested key exists
			public INode findAux(string word)
			{
				//the word would have to be somewhere on the path -> the word does not exist
				if (word.Length < path.Length)
					return null;
				
				//compare
				for (int i = 0; i < path.Length; i++)
				{
					if (path[i] != word[i])
						return null;
				}

				word = word.Substring(path.Length);

				//the word ends exactly at the Node
				if (word == "") {
					if (loggedWord)
						return this;
				}
				//go further down the tree if possible;
				else if (map.ContainsKey(word[0]))
					return map[word[0]].findAux(word);

				//can't go further down the tree or this Node does not contain logged word, end search
				return null;
			}
		}
	}
}

