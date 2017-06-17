using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Crossword_Completer
{
    public static class WebViewExtensions
    {
        public static String GetUriSource(WebView view)
        {
            return (String)view.GetValue(UriSourceProperty);
        }
        public static void SetUriSource(WebView view, String value)
        {
            view.SetValue(UriSourceProperty, value);
        }
        public static readonly DependencyProperty UriSourceProperty = 
            DependencyProperty.RegisterAttached("UriSource", typeof(String), typeof(WebViewExtensions), new PropertyMetadata(null, OnUriSourcePropertyChanged));
        public static void OnUriSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            WebView webView = (WebView)sender;
            if (webView == null)
                throw new NotSupportedException();
            if(e.NewValue != null)
                webView.Navigate(new Uri(e.NewValue.ToString()));
        }
    }
}
