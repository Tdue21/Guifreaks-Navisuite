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

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Guifreaks.Navisuite;

namespace Guifreaks.Design
{
    public class NaviGroupDesigner : ParentControlDesigner
    {
        private IComponentChangeService _changeService;
        private NaviGroup _designingControl;
        private ISelectionService _selectionService;

        public static int HiWord(int dwValue) => (dwValue >> 16) & 0xFFFF;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            var control = component as NaviGroup;
            if (control != null)
            {
                _designingControl = control;
            }
            InitializeServices();
        }

        public static int LoWord(int dwValue) => dwValue & 0xFFFF;

        protected override void WndProc(ref Message m)
        {
            if (m.HWnd == Control.Handle && m.Msg == 0x202)
            {
                CheckHeaderClick(new Point(LoWord((int) m.LParam), HiWord((int) m.LParam)));
            }
            base.WndProc(ref m);
        }

        private void CheckHeaderClick(Point location)
        {
            if (_designingControl != null && _designingControl.HeaderRegion.IsVisible(location))
            {
                if (_selectionService.PrimarySelection == _designingControl)
                {
                    SetControlProperty("Expanded", !_designingControl.Expanded);
                }
            }
        }

        private void InitializeServices()
        {
            if (_changeService == null)
            {
                _changeService = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            }

            if (_selectionService == null)
            {
                _selectionService = GetService(typeof(ISelectionService)) as ISelectionService;
            }
        }

        private void SetControlProperty(string propName, object value)
        {
            var propDesc = TypeDescriptor.GetProperties(_designingControl)[propName];

            // Raise event that we are about to change
            _changeService?.OnComponentChanging(_designingControl, propDesc);

            // Change to desired value
            var oldValue = propDesc.GetValue(_designingControl);
            propDesc.SetValue(_designingControl, value);

            // Raise event that the component has been changed
            _changeService?.OnComponentChanged(_designingControl, propDesc, oldValue, value);
        }
    }
}
