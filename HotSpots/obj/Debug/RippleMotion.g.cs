﻿#pragma checksum "C:\HotSpots\HotSpots\HotSpots\RippleMotion.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "42EC345A43F1E03C991A444CFA49B30D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Ripple {
    
    
    public partial class RippleMotion : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard AnimiRipple;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Shapes.Ellipse WaterRipple1;
        
        internal System.Windows.Shapes.Ellipse WaterRipple2;
        
        internal System.Windows.Shapes.Ellipse WaterRipple3;
        
        internal System.Windows.Shapes.Ellipse WaterRipple4;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/HotSpots;component/RippleMotion.xaml", System.UriKind.Relative));
            this.AnimiRipple = ((System.Windows.Media.Animation.Storyboard)(this.FindName("AnimiRipple")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.WaterRipple1 = ((System.Windows.Shapes.Ellipse)(this.FindName("WaterRipple1")));
            this.WaterRipple2 = ((System.Windows.Shapes.Ellipse)(this.FindName("WaterRipple2")));
            this.WaterRipple3 = ((System.Windows.Shapes.Ellipse)(this.FindName("WaterRipple3")));
            this.WaterRipple4 = ((System.Windows.Shapes.Ellipse)(this.FindName("WaterRipple4")));
        }
    }
}

