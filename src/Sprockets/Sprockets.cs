namespace Sprockets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Sprockets
    {
        private static readonly Regex SprocketsRegex =
            new Regex(@"^(//=|#=) (?<type>(require_tree|require_self|require)).'?(?<path>.*)'?",
                      RegexOptions.Compiled | RegexOptions.Multiline);

        public Node<FileMetadata> Scan(string file)
        {
            var root = GetNode(file, new Dictionary<string, Node<FileMetadata>>());
            root.Data.Type = "root";

            return root;
        }

        private Node<FileMetadata> GetNode(string file, IDictionary<string, Node<FileMetadata>> resolved)
        {
            Node<FileMetadata> node;

            file = NormalizePath(file);
            if (resolved.TryGetValue(file, out node))
                return node;

            node = new Node<FileMetadata>(file);
            resolved.Add(file, node);
            var contents = GetContents(file);

            node.Data = new FileMetadata
                                {
                                    AbsolutePath = file,
                                    Contents = contents
                                };

            var match = SprocketsRegex.Match(contents);
            if (!match.Success)
                return node;

            foreach (Match itemMatch in SprocketsRegex.Matches(contents))
            {
                var dependencyPath = itemMatch.Groups["path"].Value;
                if (dependencyPath.EndsWith("'"))
                    dependencyPath = dependencyPath.TrimEnd('\'');

                switch (itemMatch.Groups["type"].Value)
                {
                    case "require_tree":
                        var files = GetFiles(CombinePath(GetDirectoryName(file), dependencyPath));
                        if (dependencyPath == ".")
                            files = files.Where(f => f != file).ToArray();
                        foreach (var f in files)
                        {
                            if (f == file)
                                continue;
                            var dep = GetNode(f, resolved);
                            node.Edges.Add(dep);
                        }
                        break;
                    case "require_self":
                        throw new NotImplementedException();
                        break;
                    case "require":
                        var dependencyAbsolutePath = GetAbsolutePath(file, dependencyPath);
                        var depNode = GetNode(dependencyAbsolutePath, resolved);

                        depNode.Data.Type = "require";
                        depNode.Data.Value = dependencyPath;
                        node.Edges.Add(depNode);

                        break;
                }
            }

            return node;
        }

        protected virtual string[] GetFiles(string path)
        {
            return
                Directory.GetFiles(DirectoryExists(path) ? path : GetDirectoryName(path), "*", SearchOption.AllDirectories)
                .Select(NormalizePath)
                .ToArray();
        }

        protected virtual string NormalizePath(string file)
        {
            return Path.GetFullPath(file);
        }

        protected virtual string GetContents(string file)
        {
            return File.ReadAllText(file);
        }

        protected virtual string GetDirectoryName(string file)
        {
            return Path.GetDirectoryName(file);
        }

        protected virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        protected virtual bool FileExists(string path)
        {
            return File.Exists(path);
        }

        protected virtual string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        protected virtual string CombinePath(params string[] paths)
        {
            return Path.Combine(paths);
        }

        protected virtual string GetAbsolutePath(string root, string file)
        {
            var ext = GetExtension(root);
            var dir = GetDirectoryName(root);

            file = file.Replace("\r", string.Empty);

            var path = CombinePath(dir, file);
            if (!FileExists(path))
            {
                if (!string.IsNullOrEmpty(ext))
                {
                    var tempPath = path + ext;
                    if (!FileExists(tempPath))
                    {
                        // try js or coffee
                        if (ext == ".js")
                        {
                            // try coffee
                            path = path + ".coffee";
                        }
                        else
                        {
                            // try js
                            path = path + ".js";
                        }
                    }
                    else
                    {
                        path = tempPath;
                    }
                }
            }

            return path;
        }
    }
}