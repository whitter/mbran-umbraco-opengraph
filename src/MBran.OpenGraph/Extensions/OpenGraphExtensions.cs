using System.Collections.Generic;
using MBran.OpenGraph.Models;
using Umbraco.Web;

namespace MBran.OpenGraph.Extensions
{
    public static class OpenGraphExtensions
    {
        public static IEnumerable<OpenGraphMetaData> ToList(this Models.OpenGraph opengraph)
        {
            var umbHelper = new UmbracoHelper(UmbracoContext.Current);
            var model = new List<OpenGraphMetaData>();

            if (opengraph == null) return model;

            if (opengraph.ImageId != null)
            {
                var media = umbHelper.Media(opengraph.ImageId);
                var mediaUrl = media.Url;
                if (!string.IsNullOrEmpty(mediaUrl))
                {
                    var url = UmbracoContext.Current
                        .HttpContext.Request.Url?.AbsoluteUri
                        .TrimEnd('/');
                    model.Add(new OpenGraphMetaData
                    {
                        Metadata = "og:image",
                        Value = url + mediaUrl
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(opengraph.Title))
                model.Add(new OpenGraphMetaData
                {
                    Metadata = "og:title",
                    Value = opengraph.Title
                });
            if (!string.IsNullOrWhiteSpace(opengraph.Type))
                model.Add(new OpenGraphMetaData
                {
                    Metadata = "og:type",
                    Value = opengraph.Type
                });
            if (!string.IsNullOrWhiteSpace(opengraph.Description))
                model.Add(new OpenGraphMetaData
                {
                    Metadata = "og:description",
                    Value = opengraph.Description
                });

            model.AddRange(opengraph.Metadata);
            return model;
        }
    }
}