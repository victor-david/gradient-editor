/**
 * Restless Animal Development
 * This program is provided freely without restrictions, but please retain this notice in any distributions, modifications ,etc.
 * http://dev.restlessanimal.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Xam.Wpf.Controls
{
    /// <summary>
    /// Represents the arguments for the MultiSlider events that involve a single position on the MultiSlider.
    /// </summary>
    public class MultiSliderRoutedEventArgs : MultiSliderSetRoutedEventArgs
    {
        #region Properties
        /// <summary>
        /// Gets the zero-based position of the value that changed.
        /// </summary>
        public int Position
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructors (internal)

        internal MultiSliderRoutedEventArgs(RoutedEvent routedEvent, object source, List<double> sliderValues, int position)
            : base(routedEvent, source, sliderValues)
        {
            Position = position;
        }

        internal MultiSliderRoutedEventArgs(RoutedEvent routedEvent, List<double> sliderValues, int position)
            : this(routedEvent, null, sliderValues, position)
        {
        }
        #endregion
    }
}
