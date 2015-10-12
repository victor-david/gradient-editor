using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Xam.Wpf.Controls
{
    internal class SupportiveSlider : Slider
    {
        #region Properties
        /// <summary>
        /// Registers a dependency property as backing store for the IsSelected property
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(SupportiveSlider),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));

        /// <summary>
        /// Gets or sets a value that indicates if this slider is selected.
        /// A slide remains selected until another silder is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets the zero-based position of the slider within the slider group
        /// </summary>
        public int Position
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the last value this slider had before the ValueChanged event fired.
        /// </summary>
        public double LastValue
        {
            get;
            set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        public SupportiveSlider(int position)
            : base()
        {
            Position = position;
            LastValue = Double.NaN;
        }
        #endregion
    }
}
