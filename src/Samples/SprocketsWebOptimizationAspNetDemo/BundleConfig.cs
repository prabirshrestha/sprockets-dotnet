namespace SprocketsWebOptimizationAspNetDemo
{
    using System.Web.Optimization;
    using Sprockets.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/jquery")
                .Include("~/assets/javascripts/vendors/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/Scripts/app")
                .IncludeFromSprocketFile("~/assets/javascripts/main.js"));
        }
    }
}