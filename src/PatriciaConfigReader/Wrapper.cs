using System;

namespace PatriciaConfigReader
{
	public class Wrapper<T>
		where T : struct
	{
		public Wrapper (){}
		public Wrapper (T v) {value = v;}

		public T value { get; set; }

		static public implicit operator Wrapper<T> (T v) 
		{
			return new Wrapper<T>(v);
		}

		static public implicit operator T (Wrapper<T> w)
		{
			return w.value;
		}
	}
}

