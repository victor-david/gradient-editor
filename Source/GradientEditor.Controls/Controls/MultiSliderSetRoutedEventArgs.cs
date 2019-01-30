/**
 * Restless Animal Development
 * This program is provided freely without restrictions, but please retain this notice in any distributions, modifications ,etc.
 * https://restlessanimal.com
 */
using System;
using System.Collections.Generic;
using System.Windows;

namespace Restless.GradientEditor.Controls
{
    /// <summary>
    /// Represents the arguments for the MultiSlider.SliderSet event.
    /// </summary>
    public class MultiSliderSetRoutedEventArgs : RoutedEventArgs
    {
        #region Properties
        /// <summary>
        /// Gets the list of slider values.
        /// </summary>
        public List<double> SliderValues
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructors (internal)

        internal MultiSliderSetRoutedEventArgs(RoutedEvent routedEvent, object source, List<double> sliderValues)
            : base(routedEvent, source)
        {
            if (sliderValues == null)
            {
                throw new ArgumentNullException("MultiSliderSetRoutedEventArgs.SliderValues");
            }
            SliderValues = sliderValues;
        }

        internal MultiSliderSetRoutedEventArgs(RoutedEvent routedEvent, List<double> sliderValues)
            : this(routedEvent, null, sliderValues)
        {
        }
        #endregion
    }
}
