﻿

#pragma checksum "C:\Users\wpackard\Documents\Visual Studio 2013\Projects\Crossword Completer\Crossword Completer\Crossword Completer.Windows\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "AA4321B4ADA862CFACADE38A258E0D87"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Crossword_Completer
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 107 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ManageCloseClicked;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 124 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.AppBar)(target)).Closed += this.cbOptionsBottom_Closed;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 150 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.AddClicked;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 151 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.UpdateClicked;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 152 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.ManageClicked;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


