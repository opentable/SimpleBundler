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
cssPack.CacheBustingString = "build-number"; // if you don't pass the build-number, it will use the hash of css contents as cache busting string.
cssPack.CacheBustingMethod = CacheBustingMethod.VaryByQueryString; // makes bundle request to /bundles/css/homePageCss?r=<cache-busting-string>
cssPack.Add("home.css");
cssPack.Add("/ui/ui.css");
cssPack.AsNamed("homePageCss");

```


Create a JavaScript bundle
```csharp
var jsPack = JavaScript.Css();
jsPack.BasePath = "c:/myapp/content/javascript";
jsPack.CacheBustingString = "build-number"; // if you don't pass the build-number, it will use the hash of css contents as cache busting string.
jsPack.Add("home.js");
jsPack.Add("/ui/ui.js");
jsPack.AsNamed("homePageJS");

```

Using Cache Busting String in "Url Path" vs "Query string"

By default the cache busting string is used in the Url Path. This behavior can be changed so that the string is added as a query string. 

```csharp
jsPack.CacheBustingString = "build-number"; // if you don't pass the build-number, it will use the hash of css contents as cache busting string.
jsPack.CacheBustingMethod = CacheBustingMethod.VaryByQueryString; // makes bundle request to /bundles/js/homePageJS?r=<cache-busting-string>

```

```csharp
jsPack.CacheBustingString = "build-number"; // if you don't pass the build-number, it will use the hash of css contents as cache busting string.
jsPack.CacheBustingMethod = CacheBustingMethod.VaryByUrlPath; // makes bundle request to /bundles/js/<cache-busting-string>/homePageJS
```


After the bundling set up is done, you just need to add some endpoints to your code which serve the bundle requests

```csharp
Get["/css/{appVersion}/{bundleName}"] = reqParams => Pack.Css().RenderContents(reqParams.bundleName);
Get["/js/{appVersion}/{bundleName}"] = reqParams => Pack.JavaScript().RenderContents(reqParams.bundleName);
```