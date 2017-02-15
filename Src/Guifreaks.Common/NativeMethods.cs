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
using System.Runtime.InteropServices;

namespace Guifreaks.Common
{
    public static class NativeMethods
    {
        public const int KeyeventfKeyup = 0x0002;
        public const int WmActivateapp = 0x001C;
        public const int WmLbuttondown = 0x201;
        public const int WmLbuttonup = 0x202;
        public const int WmMbuttondown = 0x207;
        public const int WmNcactivate = 0x0086;
        public const int WmNclbuttondown = 0x0A1;
        public const int WmNcmbuttondown = 0x0A7;
        public const int WmNcrbuttondown = 0x0A4;
        public const int WmRbuttondown = 0x204;

        public static int HiWord(int n) => (n >> 16) & 0xffff;

        public static int HiWord(IntPtr n) => HiWord(unchecked((int) (long) n));

        public static int LoWord(int n) => n & 0xffff;

        public static int LoWord(IntPtr n) => LoWord(unchecked((int) (long) n));

        [DllImport("user32")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr handle, int msg, int wParam, IntPtr lParam);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr handle, int msg, int wParam, IntPtr lParam);
    }
}
