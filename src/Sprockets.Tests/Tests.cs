namespace Sprockets.Tests
{
    using System;
    using System.IO;
    using System.Reflection;
    using Should;
    using Xunit;

    public class Tests
    {
        private static readonly string AssetsFolder;

        static Tests()
        {
            string codeBase = Assembly.GetCallingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            AssetsFolder = Path.Combine(Path.GetDirectoryName(path), "assets");
        }

        [Fact]
        public void IndependentJsFilesHasNoDependencies()
        {
            var sprockets = new Sprockets();

            var result = sprockets.Scan(Path.Combine(AssetsFolder, "b.js"));
            result.Edges.Count.ShouldEqual(0);
        }

        [Fact]
        public void SingleStepDependenciesAreCorrectlyRecorded()
        {
            var sprockets = new Sprockets();

            var node = sprockets.Scan(Path.Combine(AssetsFolder, "a.coffee"));
            var resolved = node.ResolveDependencies();
            resolved.Count.ShouldEqual(2);

            node.Edges.Count.ShouldEqual(1);
            node.Edges[0].Edges.Count.ShouldEqual(0);

            resolved.SequenceEqual(AssetsFolder, new[] { "b.js", "a.coffee" }).ShouldBeTrue();
        }

        [Fact]
        public void DependenciesWithMultipleExtensionsAreAccepted()
        {
            var sprockets = new Sprockets();

            var node = sprockets.Scan(Path.Combine(AssetsFolder, "testing.js"));
            var resolved = node.ResolveDependencies();

            resolved.SequenceEqual(AssetsFolder, new[] { "1.2.3.coffee", "testing.js" }).ShouldBeTrue();
        }

        [Fact]
        public void DependenciesCanHaveSubDirectoryRelativePaths()
        {
            var sprockets = new Sprockets();

            var node = sprockets.Scan(Path.Combine(AssetsFolder, "song/loveAndMarriage.js"));
            var resolved = node.ResolveDependencies();

            resolved.SequenceEqual(AssetsFolder, new[] { "horseAndCarriage.coffee", "loveAndMarriage.js" });
            //.ShouldBeTrue();
        }

        [Fact(Skip = "not supported")]
        public void MultipleDependenciesCanBeDeclaredInOneRequireDirective()
        {
            var sprockets = new Sprockets();

            var node = sprockets.Scan(Path.Combine(AssetsFolder, "poly.coffee"));
            var resolved = node.ResolveDependencies();

            //resolved.SequenceEqual(AssetsFolder, new[] { "loveAndMarriage.js", "horseAndCarriage.coffee" });
        }

        [Fact]
        public void ChainedDependenciesAreCorrectlyOrdered()
        {
            var sprockets = new Sprockets();

            var node = sprockets.Scan(Path.Combine(AssetsFolder, "z.coffee"));
            var resolved = node.ResolveDependencies();

            resolved.SequenceEqual(AssetsFolder, new[] { "x.coffee", "y.js", "z.coffee" }).ShouldBeTrue();
        }

        [Fact]
        public void DependencyCyclesDoesNotThrowErrorDuringScanning()
        {
            var sprockets = new Sprockets();

            var node = sprockets.Scan(Path.Combine(AssetsFolder, "yin.js"));
        }

        [Fact]
        public void DependencyCyclesThrowsErrorDuringResolvingDependencies()
        {
            var sprockets = new Sprockets();

            var node = sprockets.Scan(Path.Combine(AssetsFolder, "yin.js"));

            Assert.Throws<CircularDependencyException<FileMetadata>>(() => node.ResolveDependencies());
            Assert.Throws<CircularDependencyException<FileMetadata>>(() => node.Edges[0].ResolveDependencies());
        }

        [Fact]
        public void RequireTreeWorksForSameDirectory()
        {
            var sprockets = new Sprockets();

            var node = sprockets.Scan(Path.Combine(AssetsFolder, "branch/center.coffee"));
            var resolved = node.ResolveDependencies();

            resolved.SequenceEqual(AssetsFolder,
                                   new[]
                                       {
                                           "branch/edge.coffee",
                                           "branch/periphery.js",
                                           "branch/unwanted",
                                           "branch/subbranch/leaf.js",
                                           "branch/center.coffee"
                                       })
                                       .ShouldBeTrue();
        }

        [Fact]
        public void RequireWorksForIncludesThatAreRelativeToOriginalFile()
        {
            var sprockets = new Sprockets();

            var node = sprockets.Scan(Path.Combine(AssetsFolder, "first/syblingFolder.js"));
            var resolved = node.ResolveDependencies();

            resolved.SequenceEqual(AssetsFolder,
                                   new[]
                                       {
                                           "sybling/sybling.js",
                                           "first/syblingFolder.js"
                                       }).ShouldBeTrue();
        }

        [Fact]
        public void RequireTreeWorksForNestedDirectories()
        {
            var sprockets = new Sprockets();

            var node = sprockets.Scan(Path.Combine(AssetsFolder, "fellowship.js"));
            var resolved = node.ResolveDependencies();

            resolved.SequenceEqual(AssetsFolder,
                                   new[]
                                       {
                                           "middleEarth/legolas.coffee",
                                           "middleEarth/shire/bilbo.js",
                                           "middleEarth/shire/frodo.coffee",
                                           "fellowship.js"
                                       }).ShouldBeTrue();
        }

        [Fact]
        public void RequireTreeWorksForRedundantDirectories()
        {
            var sprockets = new Sprockets();

            var node = sprockets.Scan(Path.Combine(AssetsFolder, "trilogy.coffee"));
            var resolved = node.ResolveDependencies();

            resolved.SequenceEqual(AssetsFolder,
                                   new[]
                                       {
                                           "middleEarth/shire/bilbo.js",
                                           "middleEarth/shire/frodo.coffee",
                                           "middleEarth/legolas.coffee",
                                           "trilogy.coffee"
                                       }).ShouldBeTrue();
        }
    }
}