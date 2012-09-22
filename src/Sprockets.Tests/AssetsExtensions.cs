namespace Sprockets.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class AssetsExtensions
    {
        public static bool SequenceEqual(this IList<Node<FileMetadata>> resolved, string assetsPath, string[] fileNames)
        {
            var expected = fileNames.Select(file => Path.GetFullPath(Path.Combine(assetsPath, file))).ToList();

            return resolved.Select(r => r.Name).SequenceEqual(expected);
        }
    }
}