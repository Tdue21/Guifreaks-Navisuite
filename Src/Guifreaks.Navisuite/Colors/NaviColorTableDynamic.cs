// ********************************************************************
// * Copyright © 2008 - 2016 Scanvaegt Nordic A/S
// *
// * This file is the property of Scanvaegt Nordic A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System.Drawing;
using System.Xml.Linq;

namespace Guifreaks.Navisuite
{
    public class NaviColorTableDynamic : NaviColorTable
    {
        private Color _border;
        private Color _borderInner;
        private Color _text;
        private Color _shapesFront;
        private Color _background;
        private Color _dashedLineColor;
        private Color _bandCollapsedBgColor1;
        private Color _bandCollapsedBgColor2;
        private Color _bandCollapsedHoveredColor1;
        private Color _bandCollapsedClickedColor1;
        private Color _popupBandBackground1;
        private Color _popupBandBackground2;
        private Color _headerColor1;
        private Color _headerColor2;
        private Color _headerText;
        private Color _overflowColor1;
        private Color _overflowColor2;
        private Color _buttonNormalColor1;
        private Color _buttonNormalColor2;
        private Color _buttonNormalColor3;
        private Color _buttonNormalColor4;
        private Color _buttonHoveredColor1;
        private Color _buttonHoveredColor2;
        private Color _buttonHoveredColor3;
        private Color _buttonHoveredColor4;
        private Color _buttonActiveColor1;
        private Color _buttonActiveColor2;
        private Color _buttonActiveColor3;
        private Color _buttonActiveColor4;
        private Color _buttonClickedColor1;
        private Color _buttonClickedColor2;
        private Color _buttonClickedColor3;
        private Color _buttonClickedColor4;
        private Color _naviClientareaBgColor1;
        private Color _naviClientareaBgColor2;
        private Color _collapseButtonNormalColor1;
        private Color _collapseButtonNormalColor2;
        private Color _collapseButtonHoveredColor1;
        private Color _collapseButtonHoveredColor2;
        private Color _collapseButtonClickedColor1;
        private Color _collapseButtonClickedColor2;
        private Color _buttonCollapseFront;
        private Color _buttonCollapseActive;
        private Color _groupColor1;
        private Color _groupColor2;
        private Color _groupHoveredColor1;
        private Color _groupHoveredColor2;
        private Color _groupBorderLight;
        private Color _groupExpandedColor1;
        private Color _groupExpandedColor2;
        private Color _splitterColor1;
        private Color _splitterColor2;
        private Color _splitterColor3;

        public NaviColorTableDynamic(string fileName)
        {
            var doc = XDocument.Load(fileName, LoadOptions.None);
            var root = doc.Root;
            var ns = root.GetDefaultNamespace();

            Name = root.Attribute("Name").Value;

            _background                  = Color.FromArgb(int.Parse(root.Element(ns + "Background").Value));
            _bandCollapsedBgColor1       = Color.FromArgb(int.Parse(root.Element(ns + "BandCollapsedBgColor1").Value));
            _bandCollapsedBgColor2       = Color.FromArgb(int.Parse(root.Element(ns + "BandCollapsedBgColor2").Value));
            _bandCollapsedClickedColor1  = Color.FromArgb(int.Parse(root.Element(ns + "BandCollapsedClickedColor1").Value));
            _bandCollapsedHoveredColor1  = Color.FromArgb(int.Parse(root.Element(ns + "BandCollapsedHoveredColor1").Value));
            _border                      = Color.FromArgb(int.Parse(root.Element(ns + "Border").Value));
            _borderInner                 = Color.FromArgb(int.Parse(root.Element(ns + "BorderInner").Value));
            _buttonActiveColor1          = Color.FromArgb(int.Parse(root.Element(ns + "ButtonActiveColor1").Value));
            _buttonActiveColor2          = Color.FromArgb(int.Parse(root.Element(ns + "ButtonActiveColor2").Value));
            _buttonActiveColor3          = Color.FromArgb(int.Parse(root.Element(ns + "ButtonActiveColor3").Value));
            _buttonActiveColor4          = Color.FromArgb(int.Parse(root.Element(ns + "ButtonActiveColor4").Value));
            _buttonClickedColor1         = Color.FromArgb(int.Parse(root.Element(ns + "ButtonClickedColor1").Value));
            _buttonClickedColor2         = Color.FromArgb(int.Parse(root.Element(ns + "ButtonClickedColor2").Value));
            _buttonClickedColor3         = Color.FromArgb(int.Parse(root.Element(ns + "ButtonClickedColor3").Value));
            _buttonClickedColor4         = Color.FromArgb(int.Parse(root.Element(ns + "ButtonClickedColor4").Value));
            _buttonCollapseActive        = Color.FromArgb(int.Parse(root.Element(ns + "ButtonCollapseActive").Value));
            _buttonCollapseFront         = Color.FromArgb(int.Parse(root.Element(ns + "ButtonCollapseFront").Value));
            _buttonHoveredColor1         = Color.FromArgb(int.Parse(root.Element(ns + "ButtonHoveredColor1").Value));
            _buttonHoveredColor2         = Color.FromArgb(int.Parse(root.Element(ns + "ButtonHoveredColor2").Value));
            _buttonHoveredColor3         = Color.FromArgb(int.Parse(root.Element(ns + "ButtonHoveredColor3").Value));
            _buttonHoveredColor4         = Color.FromArgb(int.Parse(root.Element(ns + "ButtonHoveredColor4").Value));
            _buttonNormalColor1          = Color.FromArgb(int.Parse(root.Element(ns + "ButtonNormalColor1").Value));
            _buttonNormalColor2          = Color.FromArgb(int.Parse(root.Element(ns + "ButtonNormalColor2").Value));
            _buttonNormalColor3          = Color.FromArgb(int.Parse(root.Element(ns + "ButtonNormalColor3").Value));
            _buttonNormalColor4          = Color.FromArgb(int.Parse(root.Element(ns + "ButtonNormalColor4").Value));
            _collapseButtonClickedColor1 = Color.FromArgb(int.Parse(root.Element(ns + "CollapseButtonClickedColor1").Value));
            _collapseButtonClickedColor2 = Color.FromArgb(int.Parse(root.Element(ns + "CollapseButtonClickedColor2").Value));
            _collapseButtonHoveredColor1 = Color.FromArgb(int.Parse(root.Element(ns + "CollapseButtonHoveredColor1").Value));
            _collapseButtonHoveredColor2 = Color.FromArgb(int.Parse(root.Element(ns + "CollapseButtonHoveredColor2").Value));
            _collapseButtonNormalColor1  = Color.FromArgb(int.Parse(root.Element(ns + "CollapseButtonNormalColor1").Value));
            _collapseButtonNormalColor2  = Color.FromArgb(int.Parse(root.Element(ns + "CollapseButtonNormalColor2").Value));
            _dashedLineColor             = Color.FromArgb(int.Parse(root.Element(ns + "DashedLineColor").Value));
            _groupBorderLight            = Color.FromArgb(int.Parse(root.Element(ns + "GroupBorderLight").Value));
            _groupColor1                 = Color.FromArgb(int.Parse(root.Element(ns + "GroupColor1").Value));
            _groupColor2                 = Color.FromArgb(int.Parse(root.Element(ns + "GroupColor2").Value));
            _groupExpandedColor1         = Color.FromArgb(int.Parse(root.Element(ns + "GroupExpandedColor1").Value));
            _groupExpandedColor2         = Color.FromArgb(int.Parse(root.Element(ns + "GroupExpandedColor2").Value));
            _groupHoveredColor1          = Color.FromArgb(int.Parse(root.Element(ns + "GroupHoveredColor1").Value));
            _groupHoveredColor2          = Color.FromArgb(int.Parse(root.Element(ns + "GroupHoveredColor2").Value));
            _headerColor1                = Color.FromArgb(int.Parse(root.Element(ns + "HeaderColor1").Value));
            _headerColor2                = Color.FromArgb(int.Parse(root.Element(ns + "HeaderColor2").Value));
            _headerText                  = Color.FromArgb(int.Parse(root.Element(ns + "HeaderText").Value));
            _naviClientareaBgColor1      = Color.FromArgb(int.Parse(root.Element(ns + "NaviClientareaBgColor1").Value));
            _naviClientareaBgColor2      = Color.FromArgb(int.Parse(root.Element(ns + "NaviClientareaBgColor2").Value));
            _overflowColor1              = Color.FromArgb(int.Parse(root.Element(ns + "OverflowColor1").Value));
            _overflowColor2              = Color.FromArgb(int.Parse(root.Element(ns + "OverflowColor2").Value));
            _popupBandBackground1        = Color.FromArgb(int.Parse(root.Element(ns + "PopupBandBackground1").Value));
            _popupBandBackground2        = Color.FromArgb(int.Parse(root.Element(ns + "PopupBandBackground2").Value));
            _shapesFront                 = Color.FromArgb(int.Parse(root.Element(ns + "ShapesFront").Value));
            _splitterColor1              = Color.FromArgb(int.Parse(root.Element(ns + "SplitterColor1").Value));
            _splitterColor2              = Color.FromArgb(int.Parse(root.Element(ns + "SplitterColor2").Value));
            _splitterColor3              = Color.FromArgb(int.Parse(root.Element(ns + "SplitterColor3").Value));
            _text                        = Color.FromArgb(int.Parse(root.Element(ns + "Text").Value));
        }

        public string Name { get; set; }
        public override Color Border { get { return _border; } }
        public override Color BorderInner { get { return _borderInner; } }
        public override Color Text { get { return _text; } }
        public override Color ShapesFront { get { return _shapesFront; } }
        public override Color Background { get { return _background; } }
        public override Color DashedLineColor { get { return _dashedLineColor; } }
        public override Color BandCollapsedBgColor1 { get { return _bandCollapsedBgColor1; } }
        public override Color BandCollapsedBgColor2 { get { return _bandCollapsedBgColor2; } }
        public override Color BandCollapsedHoveredColor1 { get { return _bandCollapsedHoveredColor1; } }
        public override Color BandCollapsedClickedColor1 { get { return _bandCollapsedClickedColor1; } }
        public override Color PopupBandBackground1 { get { return _popupBandBackground1; } }
        public override Color PopupBandBackground2 { get { return _popupBandBackground2; } }
        public override Color HeaderColor1 { get { return _headerColor1; } }
        public override Color HeaderColor2 { get { return _headerColor2; } }
        public override Color HeaderText { get { return _headerText; } }
        public override Color OverflowColor1 { get { return _overflowColor1; } }
        public override Color OverflowColor2 { get { return _overflowColor2; } }
        public override Color ButtonNormalColor1 { get { return _buttonNormalColor1; } }
        public override Color ButtonNormalColor2 { get { return _buttonNormalColor2; } }
        public override Color ButtonNormalColor3 { get { return _buttonNormalColor3; } }
        public override Color ButtonNormalColor4 { get { return _buttonNormalColor4; } }
        public override Color ButtonHoveredColor1 { get { return _buttonHoveredColor1; } }
        public override Color ButtonHoveredColor2 { get { return _buttonHoveredColor2; } }
        public override Color ButtonHoveredColor3 { get { return _buttonHoveredColor3; } }
        public override Color ButtonHoveredColor4 { get { return _buttonHoveredColor4; } }
        public override Color ButtonActiveColor1{get { return _buttonActiveColor1; }}
        public override Color ButtonActiveColor2{get { return _buttonActiveColor2; }}
        public override Color ButtonActiveColor3{get { return _buttonActiveColor3; }}
        public override Color ButtonActiveColor4{get { return _buttonActiveColor4; }}
        public override Color ButtonClickedColor1{get { return _buttonClickedColor1; }}
        public override Color ButtonClickedColor2{get { return _buttonClickedColor2; }}
        public override Color ButtonClickedColor3{get { return _buttonClickedColor3; }}
        public override Color ButtonClickedColor4{get { return _buttonClickedColor4; }}
        public override Color NaviClientareaBgColor1{get { return _naviClientareaBgColor1; }}
        public override Color NaviClientareaBgColor2{get { return _naviClientareaBgColor2; }}
        public override Color CollapseButtonNormalColor1{get { return _collapseButtonNormalColor1; }}
        public override Color CollapseButtonNormalColor2{get { return _collapseButtonNormalColor2; }}
        public override Color CollapseButtonHoveredColor1
        {
            get { return _collapseButtonHoveredColor1; }
        }
        public override Color CollapseButtonHoveredColor2
        {
            get { return _collapseButtonHoveredColor2; }
        }
        public override Color CollapseButtonClickedColor1
        {
            get { return _collapseButtonClickedColor1; }
        }
        public override Color CollapseButtonClickedColor2
        {
            get { return _collapseButtonClickedColor2; }
        }
        public override Color ButtonCollapseFront
        {
            get { return _buttonCollapseFront; }
        }
        public override Color ButtonCollapseActive
        {
            get { return _buttonCollapseActive; }
        }
        public override Color GroupColor1
        {
            get { return _groupColor1; }
        }
        public override Color GroupColor2
        {
            get { return _groupColor2; }
        }
        public override Color GroupHoveredColor1
        {
            get { return _groupHoveredColor1; }
        }
        public override Color GroupHoveredColor2
        {
            get { return _groupHoveredColor2; }
        }

        public override Color GroupBorderLight
        {
            get { return _groupBorderLight; }
        }

        public override Color GroupExpandedColor1
        {
            get { return _groupExpandedColor1; }
        }

        public override Color GroupExpandedColor2
        {
            get { return _groupExpandedColor2; }
        }

        public override Color SplitterColor1
        {
            get { return _splitterColor1; }
        }

        public override Color SplitterColor2
        {
            get { return _splitterColor2; }
        }

        public override Color SplitterColor3
        {
            get { return _splitterColor3; }
        }
    }
}