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
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Guifreaks.Common;
using Guifreaks.Navisuite;

namespace Guifreaks.Design
{
    [ToolboxItemFilter("System.Windows.Forms", ToolboxItemFilterType.Custom)]
    public class NaviBarDesigner : ParentControlDesigner
    {
        private NaviBar _designingControl;
        private ISelectionService _selectionService;

        public override DesignerVerbCollection Verbs
        {
            get
            {
                var verbs = new[] { new DesignerVerb("Add band..", AddBandVerbClicked) };
                return new DesignerVerbCollection(verbs);
            }
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            _designingControl = component as NaviBar;
            _selectionService = GetService(typeof(ISelectionService)) as ISelectionService;
            if (_selectionService != null)
            {
                _selectionService.SelectionChanged += SelectionServiceSelectionChanged;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_selectionService != null)
            {
                _selectionService.SelectionChanged -= SelectionServiceSelectionChanged;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WmLbuttonup)
            {
                if (!HandleClickEvent(NativeMethods.LoWord(m.LParam), NativeMethods.HiWord(m.LParam)))
                {
                    base.WndProc(ref m);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void AddBandVerbClicked(object sender, EventArgs e)
        {
            var designerHost = (IDesignerHost) GetService(typeof(IDesignerHost));
            var componentService = (IComponentChangeService) GetService(typeof(IComponentChangeService));
            var designerTransaction = designerHost.CreateTransaction("Add band");
            componentService.OnComponentChanging(Control, TypeDescriptor.GetProperties(Control)["Controls"]);

            var band = designerHost.CreateComponent(typeof(NaviBand)) as NaviBand;
            if (band != null)
            {
                Control.Controls.Add(band);
                (Control as NaviBar).ActiveBand = band;
            }

            componentService.OnComponentChanged(Control, TypeDescriptor.GetProperties(Control)["Controls"], null, null);
            designerTransaction.Commit();
        }

        private bool HandleClickEvent(int x, int y)
        {
            if (_designingControl != null)
            {
                foreach (NaviBand band in _designingControl.Bands)
                {
                    if ((band.Button != null) && (band.Button.Bounds.Contains(x, y)))
                    {
                        var list = new ArrayList {band};
                        if (_selectionService != null)
                        {
                            _designingControl.SetActiveBand(_selectionService.PrimarySelection as NaviBand);
                            _selectionService.SetSelectedComponents(list);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void SelectionServiceSelectionChanged(object sender, EventArgs e)
        {
            var band = _selectionService.PrimarySelection as NaviBand;
            if (band != null)
            {
                _designingControl.SetActiveBand(band);
                _designingControl.PerformLayout();
            }
        }
    }
}
