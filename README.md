# Sprockets.NET

Sprockets.NET is a sprockets style dependency resolver for .NET inspired by inspired by [Sprockets](https://github.com/sstephenson/sprockets)
and [Snockets](https://github.com/TrevorBurnham/snockets).

## Install

    Install-Package Sprockets

## Usage (script-side)

In your CoffeeScript files, write Sprockets-style comments to indicate dependencies, e.g.

    #= require dependency

(or `//= require dependency` in JavaScript). If you want to bring in a whole folder of scripts, use

    #= require_tree dir

## Usage (.NET-side)

```c#
var sprockets = new Sprockets();
var node = sprockets.Scan(@"C:\assets\app.js");

try
{
	var resolved = node.ResolveDependencies();
    foreach (var file in resolved)
    {
        Console.WriteLine(file.Data.AbsolutePath);
        Console.WriteLine(file.Data.Contents);
    }
}
catch(CircularDependencyException<FileMetadata> ex) 
{
	Console.WriteLine("Circular reference detected: {0} -> {1}", ex.A.Name, ex.B.Name);
}
```

## Using with System.Web.Optimization

    Install-Package Sprockets.Web.Optimization


Registering sprockets file using `IncludeFromSprocketFile`.

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

Registering in global.asax

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

In aspx page

    <%@ Import Namespace="System.Web.Optimization" %>
    <%= Scripts.Render("~/Scripts/jquery") %>
    <%= Scripts.Render("~/Scripts/app") %>

## License

©2012 Prabir Shrestha and available under the [MIT license](http://www.opensource.org/licenses/mit-license.php):

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
