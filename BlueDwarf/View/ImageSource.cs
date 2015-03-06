// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.View
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using DependencyProperty = System.Windows.DependencyProperty;

    public class ImageSource
    {
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        // TODO: this does not work...
        //[Attached(Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
        //public static string Uri { get; set; }

        public static readonly DependencyProperty UriProperty = DependencyProperty.RegisterAttached(
            "Uri", typeof (string), typeof (ImageSource), new PropertyMetadata(default(string),OnUriChanged));

        public static void SetUri(DependencyObject element, string value)
        {
            element.SetValue(UriProperty, value);
        }

        public static string GetUri(DependencyObject element)
        {
            return (string) element.GetValue(UriProperty);
        }

        /// <summary>
        /// Called when [source changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = d as Image;
            if (image != null)
            {
                if (e.NewValue == null)
                    image.Source = null;
                else
                    image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri((string)e.NewValue));
            }
        }
    }
}
