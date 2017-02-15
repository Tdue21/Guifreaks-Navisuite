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

using System.Windows.Forms;

namespace Guifreaks.Common
{
    /// <summary>
    /// A Message Loop filter which detect mouse events whilst the popup form is shown
    /// and notifies the owning <see cref="PopupWindowHelper"/> class when a mouse
    /// click outside the popup occurs.
    /// </summary>
    public class PopupWindowHelperMessageFilter : IMessageFilter
    {
        /// <summary>
        /// Raised when the Popup Window is about to be cancelled.  The
        /// <see cref="PopupCancelEventArgs.Cancel"/> property can be
        /// set to <c>true</c> to prevent the form from being cancelled.
        /// </summary>
        public event PopupCancelEventHandler PopupCancel;

        /// <summary>
        /// The owning <see cref="PopupWindowHelper"/> object.
        /// </summary>
        private readonly PopupWindowHelper _owner;

        /// <summary>
        /// Constructs a new instance of this class and sets the owning
        /// object.
        /// </summary>
        /// <param name="owner">The <see cref="PopupWindowHelper"/> object
        /// which owns this class.</param>
        public PopupWindowHelperMessageFilter(PopupWindowHelper owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Gets/sets the popup form which is being displayed.
        /// </summary>
        public Form Popup { get; set; }

        /// <summary>
        /// Checks the message loop for mouse messages whilst the popup
        /// window is displayed.  If one is detected the position is
        /// checked to see if it is outside the form, and the owner
        /// is notified if so.
        /// </summary>
        /// <param name="m">Windows Message about to be processed by the
        /// message loop</param>
        /// <returns><c>true</c> to filter the message, <c>false</c> otherwise.
        /// This implementation always returns <c>false</c>.</returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (Popup != null)
            {
                switch (m.Msg)
                {
                    case NativeMethods.WmLbuttondown:
                    case NativeMethods.WmRbuttondown:
                    case NativeMethods.WmMbuttondown:
                    case NativeMethods.WmNclbuttondown:
                    case NativeMethods.WmNcrbuttondown:
                    case NativeMethods.WmNcmbuttondown:
                        OnMouseDown();
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// Raises the <see cref="PopupCancel"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PopupCancelEventArgs"/> associated 
        /// with the cancel event.</param>
        protected virtual void OnCancelPopup(PopupCancelEventArgs e)
        {
            PopupCancel?.Invoke(this, e);

            if (!e.Cancel)
            {
                _owner.ClosePopup();
                // Clear reference for GC
                Popup = null;
            }
        }

        /// <summary>
        /// Checks the mouse location and calls the OnCancelPopup method
        /// if the mouse is outside the popup form.		
        /// </summary>
        private void OnMouseDown()
        {
            // Get the cursor location
            var cursorPos = Cursor.Position;
            // Check if it is within the popup form
            if (!Popup.Bounds.Contains(cursorPos))
            {
                // If not, then call to see if it should be closed
                OnCancelPopup(new PopupCancelEventArgs(Popup, cursorPos));
            }
        }
    }
}
