namespace Sprockets
{
    using System;

    public class CircularDependencyException<T> : Exception
    {
        private readonly Node<T> a;
        private readonly Node<T> b;

        public CircularDependencyException(Node<T> a, Node<T> b)
        {
            this.a = a;
            this.b = b;
        }

        public Node<T> A { get { return this.a; } }
        public Node<T> B { get { return this.b; } }
    }
}