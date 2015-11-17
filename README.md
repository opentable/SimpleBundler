# SimpleBundler
A simple bundling and minification framework with no dependencies to System.Web namespace (can be safely used in OWIN projects)

[![Build status](https://ci.appveyor.com/api/projects/status/xgfs9o70teq9kdqi?svg=true)](https://ci.appveyor.com/project/govin/simplebundler)

SimpleBundler
=============

A simple bundling and minification framework with no dependencies to System.Web namespace (can be safely used in OWIN projects)

Why?  Because .NET's built in bundling/minification framework has a dependency with System.Web. It cannot be used in OWIN style self host projects. 

**Nuget Installation**
```powershell
PM> Install-Package SimpleBundler
```

**Usage**

Create a Css bundle
```csharp
var cssPack = Pack.Css();
cssPack.BasePath = "c:/myapp/content/styles";
cssPack.Add("home.css");
cssPack.Add("/ui/ui.css");
cssPack.AsNamed("homePageCss");

```


Create a JavaScript bundle
```csharp
var jsPack = JavaScript.Css();
jsPack.BasePath = "c:/myapp/content/javascript";
jsPack.Add("home.js");
jsPack.Add("/ui/ui.js");
jsPack.AsNamed("homePageJS");

```

After the bundling set up is done, you need to add endpoints to your code which serve the bundle requests

By default, 

Bundle requests for css are made to /bundles/css/{hash-tag}/nameOfBundle

Bundle requests for javascript are made to /bundles/js/{hash-tag}/nameOfBundle


Using NancyFX, this would look like - 

```csharp
Get["bundles/css/{versionTag}/{bundleName}"] = reqParams => Pack.Css().RenderContents(reqParams.bundleName);
Get["bundles/js/{versionTag}/{bundleName}"] = reqParams => Pack.JavaScript().RenderContents(reqParams.bundleName);
```

Using WebAPI, this would look like - 

```csharp
[HttpGet]
[Route("bundles/css/{versionTag}/{bundleName}")]
public HttpResponseMessage GetCssBundle(string versionTag, string bundleName)
{
	return Pack.Css().RenderContents
}

```

Using Cache Busting String in "Url Path" vs "Query string"

By default the cache busting string is used in the Url Path. This behavior can be changed so that the string is added as a query string. 

```csharp
jsPack.CacheBustingMethod = CacheBustingMethod.VaryByQueryString; // makes bundle request to /bundles/js/homePageJS?r={cache-busting-string}
jsPack.CacheBustingString = "build-number"; // if you don't pass the build-number, it will use the hash of css contents as cache busting string.

```

```csharp
jsPack.CacheBustingMethod = CacheBustingMethod.VaryByUrlPath; // makes bundle request to /bundles/js/{cache-busting-string}/homePageJS
jsPack.CacheBustingString = "build-number"; // if you don't pass the build-number, it will use the hash of css contents as cache busting string.
```



