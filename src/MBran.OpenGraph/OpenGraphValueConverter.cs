using System;
using System.Collections.Generic;
using MBran.OpenGraph.Extensions;
using MBran.OpenGraph.Models;
using Newtonsoft.Json;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace MBran.OpenGraph
{
    [PropertyValueType(typeof(IEnumerable<OpenGraphMetaData>))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class OpenGraphValueConverter : IPropertyValueConverter
    {
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
            return opengraph == null ? new List<OpenGraphMetaData>() : opengraph.ToList();
        }

        public object ConvertSourceToXPath(PublishedPropertyType propertyType, object source, bool preview)
        {
            throw new NotImplementedException();
        }
    }
}