using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PatriciaConfigReader;
using System.Net;

namespace Example
{
	class Example
	{
		public static void Main(string[] args)
		{
			Patricia patricia = new Patricia ();
			Wrapper<int> number = (Wrapper<int>)23;
			Wrapper<string> text = (Wrapper<string>)"text";
			Wrapper<double> fraction = (Wrapper<double>)0.5;
			Wrapper<bool> boolean = (Wrapper<bool>)false;

			patricia.add<int> ("test", number);
			patricia.add<string> ("test123", text);
			patricia.add<double> ("test1", fraction);
			patricia.add<bool> ("test124", boolean);

			Console.WriteLine (patricia.find ("test"));
			Console.WriteLine (patricia.getNodeData<int> ("test"));
			patricia.getNodeData<int> ("test").value = 2;
			Console.WriteLine (number);


			Console.WriteLine (patricia.find ("test123"));
			Console.WriteLine (patricia.find ("test1"));
			Console.WriteLine (patricia.find ("test124"));
			Console.WriteLine (patricia.find ("test12"));
		}
	}
}
