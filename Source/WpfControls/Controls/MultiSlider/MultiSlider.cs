using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;

namespace Xam.Wpf.Controls
{
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
        /// Gets the default value for the CushionPercentage property.
        /// </summary>
        public const double DefaultCushionPercentage = 0.01;

        #endregion

        /************************************************************************/

        #region Private Fields and Vars
        private List<SupportiveSlider> sliders;
        private List<double> sliderValues;
        private bool initializingSliders;
        private bool constrainingSliders;
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
            set 
            { 
                SetValue(SliderCountProperty, value);
                InitializeSliders(value); 
            }
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Minimum property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(MultiSlider),
            new FrameworkPropertyMetadata(DefaultMinimum, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnMinimumPropertyChanged)
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
                new PropertyChangedCallback(OnMaximumPropertyChanged)
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
        /// Registers a dependency property as backing store for the CushionPercentage property
        /// </summary>
        public static readonly DependencyProperty CushionPercentageProperty =
            DependencyProperty.Register("CushionPercentage", typeof(double), typeof(MultiSlider),
            new FrameworkPropertyMetadata(DefaultCushionPercentage, FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnCushionPercentagePropertyChanged)
                ), new ValidateValueCallback(IsValidCushionPercentage));

        /// <summary>
        /// Gets or sets the cushion percentage value for the multi-slider,
        /// i.e. the closet one slider can get to another.
        /// </summary>
        public double CushionPercentage
        {
            get { return (double)GetValue(CushionPercentageProperty); }
            set { SetValue(CushionPercentageProperty, value); }
        }

        /// <summary>
        /// Gets a list of current slider values.
        /// </summary>
        public List<double> SliderValues
        {
            get { return sliderValues.Take<double>(SliderCount).ToList<double>(); }
        }
        #endregion

        /************************************************************************/

        #region Events

        public static readonly RoutedEvent SliderSetEvent =
            EventManager.RegisterRoutedEvent("SliderSet", RoutingStrategy.Bubble, typeof(MultiSliderSetRoutedEventHandler), typeof(MultiSlider));

        /// <summary>
        /// Occurs when the sliders are set. Sliders are set at initialization time 
        /// and each time the SliderCount property changes.
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

        public static readonly RoutedEvent SliderSelectedEvent =
            EventManager.RegisterRoutedEvent("SliderSelected", RoutingStrategy.Bubble, typeof(MultiSliderRoutedEventHandler), typeof(MultiSlider));

        /// <summary>
        /// Occurs when one of the slider points is activated.
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

        //public static readonly RoutedEvent SliderDeselectedEvent =
        //    EventManager.RegisterRoutedEvent("SliderDeselected", RoutingStrategy.Bubble, typeof(MultiSliderRoutedEventHandler), typeof(MultiSlider));

        ///// <summary>
        ///// Occurs when one of the slider points is deselected.
        ///// </summary>
        //public event MultiSliderRoutedEventHandler SliderDeselected
        //{
        //    add
        //    {
        //        AddHandler(MultiSlider.SliderDeselectedEvent, value);
        //    }
        //    remove
        //    {
        //        RemoveHandler(MultiSlider.SliderDeselectedEvent, value);
        //    }
        //}


        #endregion
        
        /************************************************************************/

        #region Constructors
        static MultiSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSlider), new FrameworkPropertyMetadata(typeof(MultiSlider)));
        }

        public MultiSlider()
        {
            sliderValues = new List<double>();
            sliders = new List<SupportiveSlider>();
            for (int k = 0; k < MaxSliderCount; k++)
            {
                sliders.Add(new SupportiveSlider(k));
                sliders[k].Minimum = Minimum;
                sliders[k].Maximum = Maximum;
                sliders[k].Visibility = System.Windows.Visibility.Collapsed;
                sliders[k].ValueChanged += new RoutedPropertyChangedEventHandler<double>(MultiSliderSliderValueChanged);
                sliders[k].GotMouseCapture += new System.Windows.Input.MouseEventHandler(MultiSliderSliderGotMouseCapture);
                //sliders[k].LostMouseCapture += new System.Windows.Input.MouseEventHandler(MultiSliderSliderLostMouseCapture);
                sliderValues.Add(Double.NaN);
            }
        }
        #endregion

        /************************************************************************/

        #region Public Methods
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

        private void InitializeSliders(int sliderCount)
        {
            initializingSliders = true;

            double increment = (Maximum - Minimum) / (sliderCount - 1);
            double value = 0.0;
            for (int k = 0; k < sliderCount; k++)
            {
                sliders[k].Value = value;
                sliders[k].Visibility = System.Windows.Visibility.Visible;
                sliderValues[k] = value;
                value += increment;
            }

            for (int k = sliderCount; k < MaxSliderCount; k++)
            {
                sliders[k].Visibility = System.Windows.Visibility.Collapsed;
            }
            SelectSlider(0);
            // Note: SliderCount property returns only [sliderCount] values.
            var mve = new MultiSliderSetRoutedEventArgs(SliderSetEvent, SliderValues);
            RaiseEvent(mve);
            initializingSliders = false;
        }

        private void MultiSliderSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!initializingSliders && !constrainingSliders)
            {
                constrainingSliders = true;
                SupportiveSlider s = sender as SupportiveSlider;
                if (s != null)
                {
                    ConstrainSliderUpward(s, s.Position + 1);
                    ConstrainSliderDownward(s, s.Position - 1);
                    if (s.LastValue != s.Value)
                    {
                        s.LastValue = s.Value;
                        sliderValues[s.Position] = s.Value;
                        // Note: SliderCount property returns only [sliderCount] values.
                        var mve = new MultiSliderRoutedEventArgs(ValueChangedEvent, SliderValues, s.Position);
                        RaiseEvent(mve);
                    }
                }
                constrainingSliders = false;
            }
        }
        
        private void ConstrainSliderUpward(Slider s, int nextIdx)
        {
            if (nextIdx > SliderCount - 1) return;

            double cushion = (Maximum - Minimum) * CushionPercentage;
            if (s.Value > sliders[nextIdx].Value - cushion)
            {
                s.Value = sliders[nextIdx].Value - cushion;
            }
        }

        private void ConstrainSliderDownward(Slider s, int prevIdx)
        {
            if (prevIdx < 0) return;

            double cushion = (Maximum - Minimum) * CushionPercentage;
            if (s.Value < sliders[prevIdx].Value + cushion)
            {
                s.Value = sliders[prevIdx].Value + cushion;
            }
        }

        private void MultiSliderSliderGotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SupportiveSlider s = sender as SupportiveSlider;
            if (s != null)
            {
                SelectSlider(s.Position);
                // Note: SliderCount property returns only [sliderCount] values.
                var mve = new MultiSliderRoutedEventArgs(SliderSelectedEvent, SliderValues, s.Position);
                RaiseEvent(mve);
            }
        }

        private void SelectSlider(int position)
        {
            foreach (SupportiveSlider s in sliders)
            {
                s.IsSelected = s.Position == position;
            }
        }
        //private void MultiSliderSliderLostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    SupportiveSlider s = sender as SupportiveSlider;
        //    if (s != null)
        //    {
        //        // Note: SliderCount property returns only [sliderCount] values.
        //        var mve = new MultiSliderRoutedEventArgs(SliderDeselectedEvent, SliderValues, s.Position);
        //        RaiseEvent(mve);
        //    }
        //}
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
            //d.CoerceValue(MinReadingProperty);
            //d.CoerceValue(MaxReadingProperty);
            Debug.WriteLine("**** Slider count property changed to {0}", e.NewValue);
        }


        private static bool IsValidMinimum(object value)
        {
            Double v = (Double)value;
            return (!v.Equals(Double.NegativeInfinity) && !v.Equals(Double.PositiveInfinity));
        }

        private static void OnMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //d.CoerceValue(MinReadingProperty);
            //d.CoerceValue(MaxReadingProperty);
            Debug.WriteLine("**** Minimum property changed to {0}", e.NewValue);
        }

        private static bool IsValidMaximum(object value)
        {
            return IsValidMinimum(value);
        }

        private static void OnMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //d.CoerceValue(MinReadingProperty);
            //d.CoerceValue(MaxReadingProperty);
            
            Debug.WriteLine("**** Maximum property changed to {0}", e.NewValue);
        }

        private static bool IsValidCushionPercentage(object value)
        {
            Double v = (Double)value;
            return (v >= 0.0 && v <= 1.0);
        }

        private static void OnCushionPercentagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("**** Cushion percentage property changed to {0}", e.NewValue);
        }

        #endregion
    }
}
