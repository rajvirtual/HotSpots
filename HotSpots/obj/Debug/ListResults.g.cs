﻿#pragma checksum "C:\HotSpots\HotSpots\HotSpots\ListResults.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9D3E4C1A62BE6A510233C870134B1A41"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using HotSpots;
using Microsoft.Advertising.Mobile.UI;
using Microsoft.Phone.Controls;
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


namespace HotSpots {
    
    
    public partial class ListResults : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Controls.PhoneApplicationPage listResults;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Advertising.Mobile.UI.AdControl adListResults;
        
        internal HotSpots.WatermarkTextBox txtSearchType;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.ListBox listbox;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/HotSpots;component/ListResults.xaml", System.UriKind.Relative));
            this.listResults = ((Microsoft.Phone.Controls.PhoneApplicationPage)(this.FindName("listResults")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.adListResults = ((Microsoft.Advertising.Mobile.UI.AdControl)(this.FindName("adListResults")));
            this.txtSearchType = ((HotSpots.WatermarkTextBox)(this.FindName("txtSearchType")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.listbox = ((System.Windows.Controls.ListBox)(this.FindName("listbox")));
        }
    }
}

