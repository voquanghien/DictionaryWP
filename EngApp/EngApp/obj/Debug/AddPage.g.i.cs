﻿#pragma checksum "C:\Users\Võ\Documents\Visual Studio 2013\Projects\EngApp\EngApp\AddPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2338E5167F80A2A842585154857A9CB5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
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


namespace EngApp {
    
    
    public partial class AddPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ApplicationBar Bar;
        
        internal System.Windows.Controls.TextBox NameTextBox;
        
        internal System.Windows.Controls.TextBox WordsTextBox;
        
        internal System.Windows.Controls.TextBlock DescriptionTextBlock;
        
        internal Microsoft.Phone.Controls.LongListSelector wtf;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/EngApp;component/AddPage.xaml", System.UriKind.Relative));
            this.Bar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("Bar")));
            this.NameTextBox = ((System.Windows.Controls.TextBox)(this.FindName("NameTextBox")));
            this.WordsTextBox = ((System.Windows.Controls.TextBox)(this.FindName("WordsTextBox")));
            this.DescriptionTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("DescriptionTextBlock")));
            this.wtf = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("wtf")));
        }
    }
}
