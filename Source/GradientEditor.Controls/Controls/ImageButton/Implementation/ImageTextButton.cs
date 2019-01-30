using System.Windows;
/**
 * Restless Animal Development
 * This program is provided freely without restrictions, but please retain this notice in any distributions, modifications ,etc.
 * https://restlessanimal.com
 */
using System.Windows.Controls;
using System.Windows.Media;

namespace Xam.Wpf.Controls
{
    /// <summary>
    /// Represents a Button control that accepts an image and text.
    /// </summary>
    public class ImageTextButton : Button
    {
        #region Properties
        /// <summary>
        /// Registers a dependency property as backing store for the ImageSource property.
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageTextButton), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

        /// <summary>
        /// Gets or sets the image source for this instance.
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }


        /// <summary>
        /// Registers a dependency property as backing store for the ImageWidth property.
        /// </summary>
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImageTextButton),
            new FrameworkPropertyMetadata(48.0, FrameworkPropertyMetadataOptions.None));

        /// <summary>
        /// Gets or sets the image width for this instance.
        /// </summary>
        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }


        /// <summary>
        /// Registers a dependency property as backing store for the ImageWidth property.
        /// </summary>
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImageTextButton),
            new FrameworkPropertyMetadata(48.0, FrameworkPropertyMetadataOptions.None));

        /// <summary>
        /// Gets or sets the image height for this instance.
        /// </summary>
        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Text property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ImageTextButton),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

        /// <summary>
        /// Gets or sets the text for this instance.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        /************************************************************************/

        #region Constructors
        static ImageTextButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageTextButton), new FrameworkPropertyMetadata(typeof(ImageTextButton)));
        }

        public ImageTextButton()
        {
        }
        #endregion
    }
}
