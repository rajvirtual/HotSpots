// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.Text;
using System.Linq;
using System.Windows.Data;
using System.Xml.Linq;
using System.Globalization;

namespace HotSpots.Converters
{
    /// <summary>
    /// Data binding converter for converting an itinerary text to plain text.
    /// </summary>
    public class ItineraryTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textBuilder = new StringBuilder();

            // Since value represents not well formatted XML which has no root element,
            // and the VirtualEarth prefix is not mapped to any namespace, we're
            // adding a fictitious root element which also maps the VirtualEarth prefix.
            string validXmlText = string.Format("<Directives xmlns:VirtualEarth=\"http://BingMaps\">{0}</Directives>", value);
            XDocument.Parse(validXmlText)
                     .Elements()
                     .Select(e => e.Value)
                     .ToList()
                     .ForEach(v => textBuilder.Append(v));

            return textBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}