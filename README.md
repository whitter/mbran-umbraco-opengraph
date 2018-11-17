# mbran-umbraco-opengraph
An Umbraco plugin which adds open graph metadata to Umbraco powered sites

Nuget: [click here](https://www.nuget.org/packages/MBran.OpenGraph/) to visit NuGet link.

Umbraco: [click here]() to download as Umbraco package:

1. Install the package either from Nuget or Umbraco package
2. Create a new data type from "MBran.OpenGraph" type
3. Add new property to your doctype and select the data type you have created from step#2.
4. Edit your page from the backoffice and fill the necessary details for the Open Graph. 
5. Save your changes.
4. Add the following snippet on your html template:

```csharp
/* This will search for the first property that is of type MBran.OpenGraph */
@Html.OpenGraph()

```
OR

```csharp
/* This will get the page property "docTypePropertyName" */
@Html.OpenGraph("docTypePropertyName")
```

5. Load your html page and view the code for metadata. It should look something like the below:

```html
<meta property="og:title" content="My Website">
<meta property="og:type" content="website">
<meta property="og:description" content="This is  the description that will show  when sharing this link to  social media.">
<meta property="og:site_name" content="My site">
```

6. If you wish to have default values for opengraph metadata, you can pass an argument as an IEnumerable. The values specified on this argument will be overwritten by any matching key from the property value.

```csharp
@Html.OpenGraph("docTypePropertyName", new []
{
    new { key = "og:site_name", value="My site" }
})
```
OR

```csharp
@Html.OpenGraph("docTypePropertyName", new List<OpenGraphMetaData>
{
    new OpenGraphMetaData{ Key = "og:site_name", Value = "My site"}
})
```
