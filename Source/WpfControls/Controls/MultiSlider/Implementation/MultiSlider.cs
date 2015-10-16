/**
 * Restless Animal Development
 * This program is provided freely without restrictions, but please retain this notice in any distributions, modifications ,etc.
 * http://dev.restlessanimal.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media;

namespace Xam.Wpf.Controls
{
    /// <summary>
    /// Represents a slider control that can have multiple slider points.
    /// </summary>
    public class MultiSlider : Control
    {
        #region Public Fields
        /// <summary>
        /// Gets the minimum number of sliders that the control supports.
        /// </summary>
        public const int MinSliderCount = 2;
        /// <summary>
        /// Gets the maximum number of sliders that the control suuports.
        /// </summary>
        public const int MaxSliderCount = 16;
        /// <summary>
        /// Gets the default number of sliders.
        /// </summary>
        public const int DefaultSliderCount = 4;
        /// <summary>
        /// Gets the default vale for the Minimum property.
        /// </summary>
        public const double DefaultMinimum = 0.0d;
        /// <summary>
        /// Gets the default value for the Maximum property.
        /// </summary>
        public const double DefaultMaximum = 1.0d;
        /// <summary>
        /// Gets the minimum allowed cushion value.
        /// </summary>
        public const double MinCushion = 0.005;
        /// <summary>
        /// Gets the maximum allowed cushion value.
        /// </summary>
        public const double MaxCushion = 0.100;
        /// <summary>
        /// Gets the default value for the Cushion property.
        /// </summary>
        public const double DefaultCushion = 0.020;
        #endregion

        /************************************************************************/

        #region Private Fields and Vars
        private List<SupportiveSlider> sliders;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Registers a dependency property as backing store for the SliderCount property
        /// </summary>
        public static readonly DependencyProperty SliderCountProperty =
            DependencyProperty.Register("SliderCount", typeof(int), typeof(MultiSlider),
            new FrameworkPropertyMetadata(DefaultSliderCount, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnSliderCountPropertyChanged)
                ), new ValidateValueCallback(IsValidSliderCount));

        /// <summary>
        /// Gets or sets the slider count.
        /// </summary>
        public int SliderCount
        {
            get { return (int)GetValue(SliderCountProperty); }
            set { SetValue(SliderCountProperty, value); }
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Minimum property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(MultiSlider),
            new FrameworkPropertyMetadata(DefaultMinimum, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnMinimumPropertyChanged), 
                new CoerceValueCallback(MinimumPropertyCoerce)
                ), new ValidateValueCallback(IsValidMinimum));

        /// <summary>
        /// Gets or sets the minumum value for the multi-slider.
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Maximum property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(MultiSlider),
            new FrameworkPropertyMetadata(DefaultMaximum, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnMaximumPropertyChanged),
                new CoerceValueCallback(MaximumPropertyCoerce)
                ), new ValidateValueCallback(IsValidMaximum));

        /// <summary>
        /// Gets or sets the maximum value for the multi-slider.
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Cushion property
        /// </summary>
        public static readonly DependencyProperty CushionProperty =
            DependencyProperty.Register("Cushion", typeof(double), typeof(MultiSlider),
            new FrameworkPropertyMetadata(DefaultCushion, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnCushionPropertyChanged)
                ), new ValidateValueCallback(IsValidCushion));

        /// <summary>
        /// Gets or sets the cushion value (MinCushion - MaxCushion),
        /// a percentage that indicates how close one slider can get to another.
        /// </summary>
        public double Cushion
        {
            get { return (double)GetValue(CushionProperty); }
            set { SetValue(CushionProperty, value); }
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Orientation property
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(MultiSlider),
            new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnOrientationPropertyChanged)));

        /// <summary>
        /// Gets or sets the orientation of the multi-slider.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Registers a dependency property as backing store for the IsDirectionReversed property
        /// </summary>
        public static readonly DependencyProperty IsDirectionReversedProperty =
            DependencyProperty.Register("IsDirectionReversed", typeof(bool), typeof(MultiSlider),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnIsDirectionReversedPropertyChanged)));

        /// <summary>
        /// Gets or sets the direction of increasing value.
        /// </summary>
        public bool IsDirectionReversed
        {
            get { return (bool)GetValue(IsDirectionReversedProperty); }
            set { SetValue(IsDirectionReversedProperty, value); }
        }

        /// <summary>
        /// Registers a dependency property as backing store for the TrackBrush property
        /// </summary>
        public static readonly DependencyProperty TrackBrushProperty =
            DependencyProperty.Register("TrackBrush", typeof(Brush), typeof(MultiSlider),
            new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.None));

        /// <summary>
        /// Gets or sets the track brush of the multi-slider,
        /// </summary>
        public Brush TrackBrush
        {
            get { return (Brush)GetValue(TrackBrushProperty); }
            set { SetValue(TrackBrushProperty, value); }
        }

        /// <summary>
        /// Gets a snapshot list of current slider values.
        /// </summary>
        public List<double> SliderValues
        {
            get 
            {
                List<double> values = new List<double>();
                for (int k = 0; k < SliderCount; k++)
                {
                    values.Add(sliders[k].Value);
                }
                return values;
            }
        }
        #endregion

        /************************************************************************/

        #region Routed Events
        /// <summary>
        ///  Identifies the SliderSet routed event.
        /// </summary>
        public static readonly RoutedEvent SliderSetEvent =
            EventManager.RegisterRoutedEvent("SliderSet", RoutingStrategy.Bubble, typeof(MultiSliderSetRoutedEventHandler), typeof(MultiSlider));

        /// <summary>
        /// Occurs when the sliders are set. Sliders are set at initialization time, 
        /// each time the SliderCount property changes, and when Minimum, Maximum, or Cushion properties change.
        /// </summary>
        public event MultiSliderSetRoutedEventHandler SliderSet
        {
            add
            {
                AddHandler(MultiSlider.SliderSetEvent, value);
            }
            remove
            {
                RemoveHandler(MultiSlider.SliderSetEvent, value);
            }
        }

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = 
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(MultiSliderRoutedEventHandler), typeof(MultiSlider));

        /// <summary>
        /// Occurs when one of the values on the multi-slider changes.
        /// </summary>
        public event MultiSliderRoutedEventHandler ValueChanged
        {
            add
            {
                AddHandler(MultiSlider.ValueChangedEvent, value);
            }
            remove
            {
                RemoveHandler(MultiSlider.ValueChangedEvent, value);
            }
        }

        /// <summary>
        /// Identifies the SliderSelected routed event.
        /// </summary>
        public static readonly RoutedEvent SliderSelectedEvent =
            EventManager.RegisterRoutedEvent("SliderSelected", RoutingStrategy.Bubble, typeof(MultiSliderRoutedEventHandler), typeof(MultiSlider));

        /// <summary>
        /// Occurs when one of the slider points is selected.
        /// </summary>
        public event MultiSliderRoutedEventHandler SliderSelected
        {
            add
            {
                AddHandler(MultiSlider.SliderSelectedEvent, value);
            }
            remove
            {
                RemoveHandler(MultiSlider.SliderSelectedEvent, value);
            }
        }
        #endregion
        
        /************************************************************************/

        #region Constructors
        static MultiSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSlider), new FrameworkPropertyMetadata(typeof(MultiSlider)));
        }

        public MultiSlider()
        {
            sliders = new List<SupportiveSlider>();
            for (int k = 0; k < MaxSliderCount; k++)
            {
                sliders.Add(new SupportiveSlider(this, k));
                sliders[k].Minimum = Minimum;
                sliders[k].Maximum = Maximum;
                sliders[k].Cushion = Cushion;
                sliders[k].IsParticipant = false;
                sliders[k].SupportiveValueChanged += new EventHandler(SupportiveSliderSupportiveValueChanged);
                sliders[k].GotMouseCapture += new System.Windows.Input.MouseEventHandler(MultiSliderSliderGotMouseCapture);
                sliders[k].Orientation = Orientation.Horizontal;
                sliders[k].IsDirectionReversed = false;
            }
            for (int k = 0; k < MaxSliderCount; k++)
            {
                if (k > 0) sliders[k].LowerPeer = sliders[k - 1];
                if (k < MaxSliderCount - 1) sliders[k].UpperPeer = sliders[k + 1];
            }
        }
        #endregion

        /************************************************************************/

        #region Public Methods
        /// <summary>
        /// Places the first slider at Minimum, the last slider at Maximum
        /// and the others spread evenly between.
        /// </summary>
        public void SpreadSliders()
        {
            double increment = (Maximum - Minimum) / (SliderCount - 1);
            double value = Minimum;
            for (int k = 0; k < SliderCount; k++)
            {
                sliders[k].SuspendValueChanged();
                sliders[k].Value = value;
                sliders[k].ResumeValueChanged();
                value += increment;
            }
            RaiseSliderSetEvent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InsertSliders(MaxSliderCount);
            InitializeSliders(SliderCount);
        }
        #endregion
        
        /************************************************************************/

        #region Protected Methods
        #endregion

        /************************************************************************/

        #region Other Methods (private)
        /// <summary>
        /// Inserts the sliders into the template. This method is called
        /// when the template is applied.
        /// </summary>
        /// <param name="totalSliderCount">The total number of possible sliders.</param>
        private void InsertSliders(int totalSliderCount)
        {
            Grid sliderGrid = Template.FindName("PART_SliderGrid", this) as Grid;
            if (sliderGrid == null) return;

            sliderGrid.Children.Clear();
            
            for (int k = 0; k < totalSliderCount; k++)
            {
                sliderGrid.Children.Add(sliders[k]);
            }
        }

        /// <summary>
        /// Initializes the sliders. This method is called when the template
        /// is applied and each time the SliderCount property changes.
        /// Sets the IsParticipant property of each active slider,
        /// spreads the slider values evenly, and selects the first slider.
        /// </summary>
        /// <param name="sliderCount">The number of active sliders.</param>
        private void InitializeSliders(int sliderCount)
        {
            for (int k = 0; k < MaxSliderCount; k++)
            {
                sliders[k].IsParticipant = (k < sliderCount);
            }
            SpreadSliders();
            SelectSlider(0);
        }

        private void SupportiveSliderSupportiveValueChanged(object sender, EventArgs e)
        {
            RaiseValueChangedEvent((SupportiveSlider)sender);
        }

        private void MultiSliderSliderGotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SupportiveSlider s = sender as SupportiveSlider;
            if (s != null)
            {
                SelectSlider(s.Position);
                RaiseSliderSelectedEvent(s);
            }
        }

        private void SelectSlider(int position)
        {
            foreach (SupportiveSlider s in sliders)
            {
                s.IsSelected = s.Position == position;
            }
        }

        /// <summary>
        /// Recalibrate the sliders in response to parameter change, Minimum, Maximum, or Cushion.
        /// TODO: Support Cushion change. Currently, if the cushion is increased while two sliders
        /// are within a smaller cushion value, they don't spread apart.
        /// </summary>
        private void RecalibrateSliders()
        {
            List<double> percent = new List<double>();
            for (int j = 0; j < SliderCount; j++)
            {
                percent.Add(sliders[j].ValuePercentage);
            }

            foreach (var s in sliders)
            {
                s.SuspendValueChanged();
                s.Minimum = Minimum;
                s.Maximum = Maximum;
                s.Cushion = Cushion;
                s.ResumeValueChanged();
            }

            for (int j = 0; j < SliderCount; j++)
            {
                sliders[j].SuspendValueChanged();
                sliders[j].ValuePercentage = percent[j];
                sliders[j].ResumeValueChanged();
            }

            RaiseSliderSetEvent();
        }

        /// <summary>
        /// Raises the SliderSet event.
        /// </summary>
        private void RaiseSliderSetEvent()
        {
            var mve = new MultiSliderSetRoutedEventArgs(SliderSetEvent, SliderValues);
            RaiseEvent(mve);
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="s">The slider that changed its value.</param>
        private void RaiseValueChangedEvent(SupportiveSlider s)
        {
            var mve = new MultiSliderRoutedEventArgs(ValueChangedEvent, SliderValues, s.Position);
            RaiseEvent(mve);
        }

        /// <summary>
        /// Raises the SliderSelected event.
        /// </summary>
        /// <param name="s">The slider that was selected.</param>
        private void RaiseSliderSelectedEvent(SupportiveSlider s)
        {
            var mve = new MultiSliderRoutedEventArgs(SliderSelectedEvent, SliderValues, s.Position);
            RaiseEvent(mve);
        }
        #endregion

        /************************************************************************/

        #region Dependency Property methods (private)

        private static bool IsValidSliderCount(object value)
        {
            int v = (int)value;
            return v >= MinSliderCount && v <= MaxSliderCount;
        }

        private static void OnSliderCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSlider ms = (MultiSlider)d;
            ms.InitializeSliders((int)e.NewValue); 
        }

        private static bool IsValidMinimum(object value)
        {
            return IsValidDouble(value);
        }

        private static void OnMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSlider ms = (MultiSlider)d;
            ms.CoerceValue(MaximumProperty);
            ms.RecalibrateSliders();
        }

        private static object MinimumPropertyCoerce(DependencyObject d, object value)
        {
            double max = ((MultiSlider)d).Maximum;
            double min = (double)value;
            return Math.Min(max, min);
        }

        private static bool IsValidMaximum(object value)
        {
            return IsValidDouble(value);
        }

        private static void OnMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSlider ms = (MultiSlider)d;
            ms.CoerceValue(MinimumProperty);
            ms.RecalibrateSliders();
        }

        private static object MaximumPropertyCoerce(DependencyObject d, object value)
        {
            double min = ((MultiSlider)d).Minimum;
            double max = (double)value;
            return Math.Max(max, min);
        }

        private static bool IsValidCushion(object value)
        {
            Double v = (Double)value;
            return (v >= MinCushion && v <= MaxCushion);
        }

        private static bool IsValidDouble(object value)
        {
            Double v = (Double)value;
            return !Double.IsInfinity(v) && !Double.IsNaN(v);
        }

        private static void OnCushionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSlider ms = (MultiSlider)d;
            ms.RecalibrateSliders();
        }

        private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSlider ms = (MultiSlider)d;
            foreach (var s in ms.sliders)
            {
                s.Orientation = (Orientation)e.NewValue;
            }
        }

        private static void OnIsDirectionReversedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSlider ms = (MultiSlider)d;
            foreach (var s in ms.sliders)
            {
                s.IsDirectionReversed = (bool)e.NewValue;
            }
        }

        #endregion
    }
}
