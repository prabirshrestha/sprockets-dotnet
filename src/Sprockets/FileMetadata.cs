namespace Sprockets
{
    public class FileMetadata
    {
        public bool Root { get { return Type == "root"; } }
        public string AbsolutePath { get; set; }
        public string Contents { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}