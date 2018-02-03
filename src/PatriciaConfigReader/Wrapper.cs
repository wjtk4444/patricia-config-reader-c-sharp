using System;

namespace PatriciaConfigReader
{
    public class Wrapper<T>
    {
        public Wrapper() { }
        public Wrapper(T value) { this.value = value; }
        public Wrapper(Wrapper<T> w) { value = w.value; }

        public T value { get; set; }


        static public explicit operator Wrapper<T>(T v)
        {
            return new Wrapper<T>(v);
        }
        static public implicit operator T(Wrapper<T> w)
        {
            return w.value;
        }
    }
}

