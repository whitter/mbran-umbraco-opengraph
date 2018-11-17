using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBran.OpenGraph.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace MBran.OpenGraph.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString OpenGraph(this HtmlHelper helper,
            Models.OpenGraph opengraph)
        {
            return helper.OpenGraph(opengraph.ToList());
        }

        public static MvcHtmlString OpenGraph(this HtmlHelper helper, string propertyName,
            Models.OpenGraph opengraph)
        {
            var curPage = GetCurrentPage();
            return helper.OpenGraph(curPage, propertyName, opengraph.ToList());
        }

        public static MvcHtmlString OpenGraph(this HtmlHelper helper,
            IEnumerable<OpenGraphMetaData> defaultMetadata = null)
        {
            var curPage = GetCurrentPage();
            var cacheName = string.Join("_", nameof(HtmlHelperExtensions), nameof(OpenGraph),
                curPage.DocumentTypeAlias);

            var currentCachedPropertyName = (string) ApplicationContext.Current
                .ApplicationCache
                .RuntimeCache
                .GetCacheItem(cacheName);

            //clear if there is previous cache but property is gone
            if (!string.IsNullOrWhiteSpace(currentCachedPropertyName) &&
                !curPage.HasProperty(currentCachedPropertyName))
                ApplicationContext.Current
                    .ApplicationCache
                    .RuntimeCache
                    .ClearCacheItem(cacheName);

            //retry if no cache available or if previous cached property does not exist
            if (string.IsNullOrWhiteSpace(currentCachedPropertyName)
                || !curPage.HasProperty(currentCachedPropertyName))
                currentCachedPropertyName = (string) ApplicationContext.Current
                    .ApplicationCache
                    .RuntimeCache
                    .GetCacheItem(cacheName, () => GetPropertyName(curPage));

            return helper.OpenGraph(curPage, currentCachedPropertyName, defaultMetadata);
        }

        public static MvcHtmlString OpenGraph(this HtmlHelper helper, string propertyName,
            IEnumerable<OpenGraphMetaData> defaultMetadata = null)
        {
            var curPage = GetCurrentPage();
            return helper.OpenGraph(curPage, propertyName, defaultMetadata);
        }

        private static MvcHtmlString OpenGraph(this HtmlHelper helper,
            IPublishedContent content,
            string propertyName,
            IEnumerable<OpenGraphMetaData> defaultMetadata = null)
        {
            var metaData = !string.IsNullOrWhiteSpace(propertyName) && content.HasProperty(propertyName)
                ? content.GetPropertyValue<List<OpenGraphMetaData>>(propertyName)
                : new List<OpenGraphMetaData>();

            var defaultMeta = defaultMetadata?
                .Where(d => !metaData.Any(m =>
                    m.Metadata.Equals(d.Metadata, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();

            if (defaultMeta != null) metaData.AddRange(defaultMeta);

            var htmlString = metaData
                .Select(m => $@"<meta property=""{m.Metadata}"" content=""{HttpUtility.HtmlEncode(m.Value)}"">")
                .ToList();


            return MvcHtmlString.Create(string.Join(string.Empty, htmlString));
        }

        private static string GetPropertyName(IPublishedContent content)
        {
            return content.Properties
                .FirstOrDefault(p => p.GetValue<List<OpenGraphMetaData>>() != null)
                ?.PropertyTypeAlias;
        }

        private static IPublishedContent GetCurrentPage()
        {
            var umbHelper = new UmbracoHelper(UmbracoContext.Current);
            return umbHelper.UmbracoContext.PublishedContentRequest.PublishedContent;
        }
    }
}