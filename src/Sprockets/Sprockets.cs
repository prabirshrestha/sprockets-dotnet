namespace Sprockets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    public class Sprockets
    {
        private static readonly Regex SprocketsRegex =
            new Regex(@"^//= (?<type>(require_tree|require_self|require)).(?<path>.*)",
                      RegexOptions.Compiled | RegexOptions.Multiline);

        public virtual IList<IDictionary<string, object>> Get(string file)
        {
            var contents = GetContents(file);
            throw new NotImplementedException();
        }

        protected string GetContents(string file)
        {
            return File.ReadAllText(file);
        }
    }
}