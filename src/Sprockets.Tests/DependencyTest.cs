namespace Sprockets.Tests
{
    using System.Collections.Generic;
    using Xunit;

    public class DependencyTest
    {
        [Fact]
        public void NoCircularDependency_DotNotThrowCircularDependencyException_Test()
        {
            var a = new Node<object>("a");
            var b = new Node<object>("b");
            var c = new Node<object>("c");
            var d = new Node<object>("d");
            var e = new Node<object>("e");

            a.Edges.Add(b); // a depends on b
            a.Edges.Add(d); // a depends on d
            b.Edges.Add(c); // b depends on c
            b.Edges.Add(e); // b depends on e
            c.Edges.Add(d); // c depends on d
            c.Edges.Add(e); // c depends on e

            var resolved = new List<Node<object>>();

            Assert.DoesNotThrow(() => Node<object>.ResolveDependencies(a, resolved));
        }

        [Fact]
        public void CircularDependency_ThrowsCircularDependencyException_Test()
        {
            var a = new Node<object>("a");
            var b = new Node<object>("b");
            var c = new Node<object>("c");
            var d = new Node<object>("d");
            var e = new Node<object>("e");

            a.Edges.Add(b); // a depends on b
            a.Edges.Add(d); // a depends on d
            b.Edges.Add(c); // b depends on c
            b.Edges.Add(e); // b depends on e
            c.Edges.Add(d); // c depends on d
            c.Edges.Add(e); // c depends on e
            d.Edges.Add(b); // d depends on b

            var resolved = new List<Node<object>>();

            CircularDependencyException<object> exception = null;
            try
            {
                Node<object>.ResolveDependencies(a, resolved);
            }
            catch (CircularDependencyException<object> ex)
            {
                exception = ex;
            }

            Assert.NotNull(exception);
            Assert.Equal(d, exception.A);
            Assert.Equal(b, exception.B);
        }
    }
}