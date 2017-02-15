// *********************************************************************
// * Copyright © Guifreaks - Jacob Mesu
// *
// * Permission is hereby granted, free of charge, to any person obtaining a copy
// * of this software and associated documentation files (the "Software"), to deal
// * in the Software without restriction, including without limitation the rights
// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// * copies of the Software, and to permit persons to whom the Software is
// * furnished to do so, subject to the following conditions:
// * 
// * The above copyright notice and this permission notice shall be included in
// * all copies or substantial portions of the Software.
// * 
// * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// * THE SOFTWARE.
// ********************************************************************

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace Guifreaks.Common
{
    public class LargeImageIndexConverter : ImageIndexConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var result = -1;
                var text = value.ToString();
                if (!text.Equals("(none)") && !(string.IsNullOrEmpty(text)))
                {
                    int.TryParse(text, out result);
                }
                return result;
            }
            return null;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is int)
            {
                var number = (int) value;
                return number >= 0 ? number.ToString() : "(none)";
            }
            return null;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ImageList imageList = null;

            var propertyCollection = TypeDescriptor.GetProperties(context.Instance);

            PropertyDescriptor property;
            if ((property = propertyCollection.Find("LargeImages", false)) != null)
            {
                imageList = (ImageList) property.GetValue(context.Instance);
            }

            if (imageList != null)
            {
                var imgIdx = new ArrayList {-1};
                for (var i = 0; i < imageList.Images.Count; i++)
                {
                    imgIdx.Add(i);
                }

                return new StandardValuesCollection(imgIdx);
            }

            return new StandardValuesCollection(new[] {-1});
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return context.Instance != null;
        }
    }
}
