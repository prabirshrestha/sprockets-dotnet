namespace Sprockets
{
    using System.Collections.Generic;

    public class Node<T>
    {
        private readonly string name;
        private readonly IList<Node<T>> edges;

        public Node(string name)
        {
            this.name = name;
            this.edges = new List<Node<T>>();
        }

        public string Name { get { return this.name; } }
        public IList<Node<T>> Edges { get { return this.edges; } }

        public void DependsOn(params Node<T>[] on)
        {
            if (on != null)
            {
                foreach (var node in on)
                {
                    this.edges.Add(node);
                }
            }
        }

        public T Data { get; set; }

        public static void ResolveDependencies(Node<T> node, List<Node<T>> resolved)
        {
            ResolveDependenciesInternal(node, resolved, new List<Node<T>>());
        }

        private static void ResolveDependenciesInternal(Node<T> node, List<Node<T>> resolved, List<Node<T>> unresolved)
        {
            // A software package can be installed when all of its dependencies have been installed, or when it doesn't have any dependencies at all.
            // When a package has already been resolved, we don't need to visit it again.
            // A circular dependency is occurring when we see a software package more than once, unless that software package has all its dependencies resolved.

            unresolved.Add(node);
            foreach (var edge in node.Edges)
            {
                if (!resolved.Contains(edge))
                {
                    if (unresolved.Contains(edge))
                        throw new CircularDependencyException<T>(node, edge);
                    ResolveDependenciesInternal(edge, resolved, unresolved);
                }
            }

            resolved.Add(node);
            unresolved.Remove(node);
        }
    }
}