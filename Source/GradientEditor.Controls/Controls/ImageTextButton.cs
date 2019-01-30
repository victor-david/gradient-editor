using System.Windows;
/**
 * Restless Animal Development
 * This program is provided freely without restrictions, but please retain this notice in any distributions, modifications ,etc.
 * https://restlessanimal.com
 */
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.GradientEditor.Controls
{
    /// <summary>
    /// Represents a Button control that accepts an image and text.
    /// </summary>
    public class ImageTextButton : Button
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the<see cref="ImageTextButton"/> class.
        /// </summary>
        public ImageTextButton()
        {
        }

        static ImageTextButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageTextButton), new FrameworkPropertyMetadata(typeof(ImageTextButton)));
        }
        #endregion

        /************************************************************************/

        #region ImageSource
        /// <summary>
        /// Gets or sets the image source for this instance.
        /// </summary>
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ImageSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register
            (
                nameof(ImageSource), typeof(ImageSource), typeof(ImageTextButton), new FrameworkPropertyMetadata(null)
            );

        #endregion

        /************************************************************************/

        #region ImageWidth
        /// <summary>
        /// Gets or sets the image width for this instance.
        /// </summary>
        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ImageWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register
            (
                nameof(ImageWidth), typeof(double), typeof(ImageTextButton), new FrameworkPropertyMetadata(48.0)
            );
        #endregion

        /************************************************************************/

        #region ImageHeight
        /// <summary>
        /// Gets or sets the image height for this instance.
        /// </summary>
        public double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ImageHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register
            (
                nameof(ImageHeight), typeof(double), typeof(ImageTextButton), new FrameworkPropertyMetadata(48.0)
            );
        #endregion

        /************************************************************************/

        #region Text
        /// <summary>
        /// Gets or sets the text for this instance.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
            (
                nameof(Text), typeof(string), typeof(ImageTextButton), new PropertyMetadata(null)
            );
        #endregion
    }
}
