﻿#region License and Copyright
/*
 
Copyright (c) Guifreaks - Jacob Mesu
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the Guifreaks nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/
#endregion

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Guifreaks.Common
{
   /// <summary>
   /// A class to assist in creating popup windows like Combo Box drop-downs and Menus.
   /// This class includes functionality to keep the title bar of the popup owner form
   /// active whilst the popup is displayed, and to automatically cancel the popup
   /// whenever the user clicks outside the popup window or shifts focus to another 
   /// application.
   /// </summary>
   public class PopupWindowHelper : NativeWindow
   {
      #region Member Variables
      /// <summary>
      /// Event Handler to detect when the popup window is closed
      /// </summary>
      private EventHandler popClosedHandler = null;
      /// <summary>
      /// Message filter to detect mouse clicks anywhere in the application
      /// whilst the popup window is being displayed.
      /// </summary>
      private PopupWindowHelperMessageFilter filter = null;
      /// <summary>
      /// The popup form that is being shown.
      /// </summary>
      private Form popup = null;
      /// <summary>
      /// The owner of the popup form that is being shown:
      /// </summary>
      private Form owner = null;
      /// <summary>
      /// Whether the popup is showing or not.
      /// </summary>
      private bool popupShowing = false;
      /// <summary>
      /// Whether the popup has been cancelled, notified by PopupCancel,
      /// rather than closed.
      /// </summary>
      private bool skipClose = false;
      #endregion

      /// <summary>
      /// Raised when the popup form is closed.
      /// </summary>
      public event PopupClosedEventHandler PopupClosed;
      /// <summary>
      /// Raised when the Popup Window is about to be cancelled.  The
      /// <see cref="PopupCancelEventArgs.Cancel"/> property can be
      /// set to <c>true</c> to prevent the form from being cancelled.
      /// </summary>
      public event PopupCancelEventHandler PopupCancel;

      /// <summary>
      /// Shows the specified Form as a popup window, keeping the
      /// Owner's title bar active and preparing to cancel the popup
      /// should the user click anywhere outside the popup window.
      /// <para>Typical code to use this message is as follows:</para>
      /// <code>
      ///    frmPopup popup = new frmPopup();
      ///    Point location = this.PointToScreen(new Point(button1.Left, button1.Bottom));
      ///    popupHelper.ShowPopup(this, popup, location);
      /// </code>
      /// <para>Put as much initialisation code as possible
      /// into the popup form's constructor, rather than the <see cref="System.Windows.Forms.Load"/>
      /// event as this will improve visual appearance.</para>
      /// </summary>
      /// <param name="owner">Main form which owns the popup</param>
      /// <param name="popup">Window to show as a popup</param>
      /// <param name="location">Location relative to the screen to show the popup at.</param>
      public void ShowPopup(Form owner, Form popup, Point location)
      {

         this.owner = owner;
         this.popup = popup;

         // Start checking for the popup being cancelled
         Application.AddMessageFilter(filter);

         // Set the location of the popup form:
         popup.StartPosition = FormStartPosition.Manual;
         popup.Location = location;
         // Make it owned by the window that's displaying it:
         owner.AddOwnedForm(popup);
         // Respond to the Closed event in case the popup
         // is closed by its own internal means
         popClosedHandler = new EventHandler(popup_Closed);
         popup.Closed += popClosedHandler;

         // Show the popup:
         this.popupShowing = true;
         popup.Show();
         popup.Activate();

         // A little bit of fun.  We've shown the popup,
         // but because we've kept the main window's
         // title bar in focus the tab sequence isn't quite
         // right.  This can be fixed by sending a tab,
         // but that on its own would shift focus to the
         // second control in the form.  So send a tab,
         // followed by a reverse-tab.

         // Send a Tab command:
         NativeMethods.keybd_event((byte)Keys.Tab, 0, 0, 0);
         NativeMethods.keybd_event((byte)Keys.Tab, 0, NativeMethods.KEYEVENTF_KEYUP, 0);

         // Send a reverse Tab command:
         NativeMethods.keybd_event((byte)Keys.ShiftKey, 0, 0, 0);
         NativeMethods.keybd_event((byte)Keys.Tab, 0, 0, 0);
         NativeMethods.keybd_event((byte)Keys.Tab, 0, NativeMethods.KEYEVENTF_KEYUP, 0);
         NativeMethods.keybd_event((byte)Keys.ShiftKey, 0, NativeMethods.KEYEVENTF_KEYUP, 0);


         // Start filtering for mouse clicks outside the popup
         filter.Popup = popup;

      }

      /// <summary>
      /// Responds to the <see cref="System.Windows.Forms.Form.Closed"/>
      /// event from the popup form.
      /// </summary>
      /// <param name="sender">Popup form that has been closed.</param>
      /// <param name="e">Not used.</param>
      private void popup_Closed(object sender, EventArgs e)
      {
         ClosePopup();
      }

      /// <summary>
      /// Subclasses the owning form's existing Window Procedure to enables the 
      /// title bar to remain active when a popup is show, and to detect if
      /// the user clicks onto another application whilst the popup is visible.
      /// </summary>
      /// <param name="m">Window Procedure Message</param>
      protected override void WndProc(ref Message m)
      {
         base.WndProc(ref m);
         if (this.popupShowing)
         {
            // check for WM_ACTIVATE and WM_NCACTIVATE
            if (m.Msg == NativeMethods.WM_NCACTIVATE)
            {
               // Check if the title bar will made inactive:
               if (((int)m.WParam) == 0)
               {
                  // If so reactivate it.
                  NativeMethods.SendMessage(this.Handle, NativeMethods.WM_NCACTIVATE, 1, IntPtr.Zero);

                  // Note it's no good to try and consume this message;
                  // if you try to do that you'll end up with windows
                  // that don't respond.
               }

            }
            else if (m.Msg == NativeMethods.WM_ACTIVATEAPP)
            {
               // Check if the application is being deactivated.
               if ((int)m.WParam == 0)
               {
                  // It is so cancel the popup:
                  ClosePopup();
                  // And put the title bar into the inactive state:
                  NativeMethods.PostMessage(this.Handle, NativeMethods.WM_NCACTIVATE, 0, IntPtr.Zero);
               }
            }
         }
      }

      /// <summary>
      /// Called when the popup is being hidden.
      /// </summary>
      public void ClosePopup()
      {
         if (this.popupShowing)
         {
            if (!skipClose)
            {
               // Raise event to owner
               OnPopupClosed(new PopupClosedEventArgs(this.popup));
            }
            skipClose = false;

            // Make sure the popup is closed and we've cleaned
            // up:
            this.owner.RemoveOwnedForm(this.popup);
            this.popupShowing = false;
            this.popup.Closed -= popClosedHandler;
            this.popClosedHandler = null;
            this.popup.Close();
            // No longer need to filter for clicks outside the
            // popup.
            Application.RemoveMessageFilter(filter);

            // If we did something from the popup which shifted
            // focus to a new form, like showing another popup
            // or dialog, then Windows won't know how to bring
            // the original owner back to the foreground, so
            // force it here:
            this.owner.Activate();

            // Null out references for GC
            this.popup = null;
            this.owner = null;
         }
      }

      /// <summary>
      /// Raises the <see cref="PopupClosed"/> event.
      /// </summary>
      /// <param name="e"><see cref="PopupClosedEventArgs"/> describing the
      /// popup form that is being closed.</param>
      protected virtual void OnPopupClosed(PopupClosedEventArgs e)
      {
         if (this.PopupClosed != null)
         {
            this.PopupClosed(this, e);
         }
      }

      private void popup_Cancel(object sender, PopupCancelEventArgs e)
      {
         OnPopupCancel(e);
      }

      /// <summary>
      /// Raises the <see cref="PopupCancel"/> event.
      /// </summary>
      /// <param name="e"><see cref="PopupCancelEventArgs"/> describing the
      /// popup form that about to be cancelled.</param>
      protected virtual void OnPopupCancel(PopupCancelEventArgs e)
      {
         if (this.PopupCancel != null)
         {
            this.PopupCancel(this, e);
            if (!e.Cancel)
            {
               skipClose = true;
            }
         }
      }

      /// <summary>
      /// Default constructor.
      /// </summary>
      /// <remarks>Use the <see cref="System.Windows.Forms.NativeWindow.AssignHandle"/>
      /// method to attach this class to the form you want to show popups from.</remarks>
      public PopupWindowHelper()
      {
         filter = new PopupWindowHelperMessageFilter(this);
         filter.PopupCancel += new PopupCancelEventHandler(popup_Cancel);
      }
   }
}