using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            IDictionary<string,string> defaultMetadata = null)
        {

            var curPage = GetCurrentPage();
            var cacheName = string.Join("_", nameof(HtmlHelperExtensions), nameof(OpenGraph),
                curPage.DocumentTypeAlias);

            var propertyName = (string)ApplicationContext.Current
                .ApplicationCache
                .RuntimeCache
                .GetCacheItem(cacheName, () => GetPropertyName(curPage));

            //if cache exist but property has changed
            if (string.IsNullOrWhiteSpace(propertyName) || curPage.HasProperty(propertyName))
                return helper.OpenGraph(curPage, propertyName, defaultMetadata);

            ApplicationContext.Current
                .ApplicationCache
                .RuntimeCache
                .ClearCacheItem(cacheName);

            propertyName = (string)ApplicationContext.Current
                .ApplicationCache
                .RuntimeCache
                .GetCacheItem(cacheName, () => GetPropertyName(curPage));

            return helper.OpenGraph(curPage, propertyName, defaultMetadata);

        }

        public static MvcHtmlString OpenGraph(this HtmlHelper helper, string propertyName,
            IDictionary<string, string> defaultMetadata = null)
        {
            var curPage = GetCurrentPage();
            return helper.OpenGraph(curPage, propertyName, defaultMetadata);
        }

        private static MvcHtmlString OpenGraph(this HtmlHelper helper, 
            IPublishedContent content,
            string propertyName,
            IDictionary<string, string> defaultMetadata = null)
        {
            var metaData = !string.IsNullOrWhiteSpace(propertyName) && content.HasProperty(propertyName) 
                ? content.GetPropertyValue<List<OpenGraphMetaData>>(propertyName)
                : new List<OpenGraphMetaData>();

            var htmlString = metaData
                .Select(m => $@"<meta property=""{m.Metadata}"" content=""{HttpUtility.HtmlEncode(m.Value)}"">")
                .ToList();

            return MvcHtmlString.Create(string.Join(string.Empty,htmlString));
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
