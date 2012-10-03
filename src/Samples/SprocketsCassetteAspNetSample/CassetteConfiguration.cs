
namespace SprocketsCassetteAspNetSample
{
    using System.Linq;
    using System.Web.Hosting;
    using Cassette;
    using Cassette.Scripts;
    using Sprockets;

    /// <summary>
    /// Configures the Cassette asset bundles for the web application.
    /// </summary>
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection bundles)
        {
            bundles.Add<ScriptBundle>("assets/javascripts/vendors/jquery-1.8.2.js", b => b.PageLocation = "jquery");

            var sprockets = new Sprockets();
            var node = sprockets.Scan(HostingEnvironment.MapPath("~/assets/javascripts/main.js"));
            var deps = node.ResolveDependencies();

            bundles.Add<ScriptBundle>("assets/javascripts/main.js", deps.Select(d => d.Data.AbsolutePath).ToArray(), b => b.PageLocation = "app");
        }
    }
}