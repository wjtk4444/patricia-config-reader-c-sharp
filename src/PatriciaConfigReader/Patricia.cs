using System;
using System.Collections.Generic;

namespace PatriciaConfigReader
{
    class Patricia
    {
        public Patricia() { }
        Dictionary<char, Node> map = new Dictionary<char, Node>();
        private class Node
        {
            public Node() { }
            public Node(string v)
            {
                value = v;
            }
            public Node(string v, string data)
            {
                value = v;
                nodeData = data;
            }

            public Node(string v, Dictionary<char, Node> m)
            {
                value = v;
                map = new Dictionary<char, Node>(m);
            }

            Dictionary<char, Node> map = new Dictionary<char, Node>();

            string nodeData = null;
            bool last = true;       //true = end of a logged word
            string value = null;    //suffix of a word

            //adds a value to the trie
            public void add(string v, string data)
            {
                int i;
                //compare requested value to the logged one; break on failed comparison
                for (i = 0; i < Math.Min(value.Length, v.Length); i++)
                {
                    if (value[i] != v[i])
                        break;
                }

                if (i == value.Length)
                {
                    //the added word is an exiating subword
                    if (v.Length - value.Length == 0)
                    {
                        last = true;
                        nodeData = data;
                    }
                    else
                    {
                        v = v.Substring(i, v.Length - value.Length);

                        //check further
                        if (map.ContainsKey(v[0]))
                            map[v[0]].add(v, data);
                        //the added word is a superword of an existing subword
                        else
                            map.Add(v[0], new Node(v, data));
                    }
                }
                else
                {
                    Node tmp = new Node(value.Substring(i, value.Length - i), map);
                    dataTransfer(ref tmp);
                    map = new Dictionary<char, Node>();
                    map.Add(value[i], tmp);

                    //the added word is a new subword
                    if (i == v.Length)
                    {
                        value = v;
                        nodeData = data;
                    }
                    //the added word is a new branch of trie
                    else
                    {
                        last = false;

                        value = value.Substring(0, i);
                        v = v.Substring(i, v.Length - value.Length);

                        map.Add(v[0], new Node(v, data));
                    }
                }
            }
            public int edit(string v, string data)
            {
                if (v.Length < value.Length)
                    return 1;

                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] != v[i])
                        return 1;
                }

                v = v.Substring(value.Length, v.Length - value.Length);

                if (v == "")
                {
                    if (last)
                    {
                        nodeData = data;
                    }
                }
                else
                {
                    if (map.ContainsKey(v[0])) return map[v[0]].edit(v, data);
                }

                return 1;
            }

            //bool that says if requested value exists
            public bool find(string v)
            {
                if (v.Length < value.Length)
                    return false;

                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] != v[i])
                        return false;
                }

                v = v.Substring(value.Length, v.Length - value.Length);

                if (v == "")
                {
                    if (last) return true;
                }
                else
                {
                    if (map.ContainsKey(v[0])) return map[v[0]].find(v);
                }

                return false;
            }
            public Node findAux(string v)
            {
                if (v.Length < value.Length)
                    return null;

                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] != v[i])
                        return null;
                }

                v = v.Substring(value.Length, v.Length - value.Length);

                if (v == "")
                {
                    if (last) return this;
                }
                else
                {
                    if (map.ContainsKey(v[0])) return map[v[0]].findAux(v);
                }

                return null;
            }

            public string getData(string v)
            {
                Node tmp = findAux(v);
                if (tmp == null) return null;
                else return tmp.nodeData;
            }

            private void dataTransfer(ref Node node)
            {
                node.nodeData = nodeData;
                nodeData = null;
            }
        }

        //wrappers for Node methods
        public void add(string v, string data = null)
        {
            if (v == "") return;

            if (map.ContainsKey(v[0]))
                map[v[0]].add(v, data);
            else
                map.Add(v[0], new Node(v, data));
        }

        public int edit(string v, string data)
        {
            if (map.ContainsKey(v[0]))
                return map[v[0]].edit(v, data);
            else
                return 1;
        }

        public bool find(string v)
        {
            if (v == "")
                return true;

            else if (map.ContainsKey(v[0]))
                return map[v[0]].find(v);

            return false;
        }

        public string getNodeData(string v)
        {
            if (v == "")
                return null;

            else if (map.ContainsKey(v[0]))
                return map[v[0]].getData(v);

            return null;
        }

        public ConfigData.SortBy getSortByData(string valName, ConfigData.SortBy defVal)
        {
            string tmp;
            if ((tmp = getNodeData(valName)) == null)
            {
                return defVal;
            }
            else
            {
                return Parser.parseSortBy(tmp, defVal);
            }
        }

        public bool getBoolData(string valName, bool defVal)
        {
            string tmp;
            if ((tmp = getNodeData(valName)) == null)
            {
                return defVal;
            }
            else
            {
                return Parser.parseBool(tmp, defVal);
            }
        }

        public int getIntData(string valName, int defVal)
        {
            string tmp;
            if ((tmp = getNodeData(valName)) == null)
            {
                return defVal;
            }
            else
            {
                return Parser.parseInt(tmp, defVal);
            }
        }

        public int getUintData(string valName, int defVal)
        {
            string tmp;
            if ((tmp = getNodeData(valName)) == null)
            {
                return defVal;
            }
            else
            {
                return Parser.parseUint(tmp, defVal);
            }
        }

        public string getWordData(string valName, string defVal)
        {
            string tmp;
            if ((tmp = getNodeData(valName)) == null)
            {
                return defVal;
            }
            else
            {
                return Parser.parseWord(tmp, defVal);
            }
        }

        public string getStringData(string valName, string defVal)
        {
            string tmp;
            if ((tmp = getNodeData(valName)) == null)
            {
                return defVal;
            }
            else
            {
                return Parser.parseString(tmp, defVal);
            }
        }

        public string[] getStringArrData(string valName, string[] defVal)
        {
            string tmp;
            if ((tmp = getNodeData(valName)) == null)
            {
                return defVal;
            }
            else
            {
                return Parser.parseStringArr(tmp, defVal);
            }
        }
    }
}