namespace Sprockets.Web.Optimization
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Optimization;

    public static class SprocketsBundleCollectionExtensions
    {
        public static Bundle IncludeFromSprocketFile(this Bundle bundle, string relativePath)
        {
            return bundle.IncludeFromSprocketFile(relativePath, HostingEnvironment.MapPath("~/"));
        }

        public static Bundle IncludeFromSprocketFile(this Bundle bundle, string relativePath, string root)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentNullException("relativePath");

            if (string.IsNullOrWhiteSpace(root))
                throw new ArgumentNullException("root");

            if (!relativePath.StartsWith("~/"))
                throw new ArgumentException("relativePath must start with ~/", relativePath);

            relativePath = relativePath.Substring(2);

            var sprockets = new Sprockets();

            var rootNode = sprockets.Scan(Path.Combine(root, relativePath));
            var resolved = rootNode.ResolveDependencies();

            return bundle.Include(resolved.Select(node => ToAbsolutePath(node, root)).ToArray());
        }

        private static string ToAbsolutePath(Node<FileMetadata> node, string rootPath)
        {
            return "~/" + node.Data.AbsolutePath.Substring(rootPath.Length).Replace('\\', '/');
        }
    }
}