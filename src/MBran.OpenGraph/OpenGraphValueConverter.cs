using System;
using System.Collections.Generic;
using System.Linq;
using MBran.OpenGraph.Models;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace MBran.OpenGraph
{
    [PropertyValueType(typeof(IEnumerable<OpenGraphMetaData>))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class OpenGraphValueConverter : IPropertyValueConverter
    {
        private readonly UmbracoHelper _umbHelper;

        public OpenGraphValueConverter()
        {
            _umbHelper = new UmbracoHelper(UmbracoContext.Current);
        }
        public bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.Equals("MBran.OpenGraph",
                StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            return Convert.ToString(source);
        }

        public object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            var opengraph = JsonConvert.DeserializeObject<Models.OpenGraph>(source as string);
            var media = _umbHelper.Media(opengraph.ImageId);
            var mediaUrl = media.Url;

            var model = new List<OpenGraphMetaData>();
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
            
            model.AddRange(opengraph.Metadata);
            return model;
        }

        public object ConvertSourceToXPath(PublishedPropertyType propertyType, object source, bool preview)
        {
            throw new NotImplementedException();
        }
    }
}