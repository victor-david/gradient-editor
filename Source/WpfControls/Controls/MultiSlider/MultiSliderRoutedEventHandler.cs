using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Xam.Wpf.Controls
{
    /// <summary>
    /// Represents the delegate for for the MultiSlider events that involve a single position on the MultiSlider.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    public delegate void MultiSliderRoutedEventHandler(object sender, MultiSliderRoutedEventArgs e);
}
