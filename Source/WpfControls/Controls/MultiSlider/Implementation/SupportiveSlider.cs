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

namespace Xam.Wpf.Controls
{
    /// <summary>
    /// Represents a slider within the MultiSlider control.
    /// </summary>
    internal class SupportiveSlider : Slider
    {
        #region Private vars
        // the percentage
        private double cushion;
        // the actual calculated cushion value.
        private double cushionValue;
        // the last value this instance had when it raised the SupportiveValueChanged event.
        private double lastValue;
        // the MultiSlider that owns this instance changes this value 
        // via SuspendValueChanged() and ResumeValueChanged().
        private bool isSuspendedValueChanged;
        // used internally to prevent re-entry into the ValueChanged handler
        private bool isInProgressValueChanged;
        // the multi slider that owns this instance
        private MultiSlider owner;

        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Registers a dependency property as backing store for the IsSelected property
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(SupportiveSlider),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));


        /// <summary>
        /// Gets or sets a boolean value that indicates if this instance 
        /// is currently particpating in the multi-slider.
        /// </summary>
        public bool IsParticipant
        {
            get { return Visibility == System.Windows.Visibility.Visible; }
            set
            {
                Visibility = (value) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates if this slider is selected.
        /// A slider remains selected until another silder is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the peer slider that is one lower in the multi-slider than this one.
        /// </summary>
        public SupportiveSlider LowerPeer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the peer slider that is one higher in the multi-slider than this one.
        /// </summary>
        public SupportiveSlider UpperPeer
        {
            get;
            set;
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
        /// Gets or sets the cushion percentage value for the multi-slider,
        /// i.e. the closet one slider can get to another.
        /// </summary>
        public double Cushion
        {
            get { return cushion; }
            set
            {
                if (value != cushion)
                {
                    cushion = value;
                    CalculateCushionValue();
                }
            }
        }

        /// <summary>
        /// Gets or sets the value as a percentage
        /// </summary>
        public double ValuePercentage
        {
            get
            {
                double spread = Maximum - Minimum;
                if (spread > 0)
                {
                    double valMinSpread = Value - Minimum;
                    return  valMinSpread / spread;
                }
                return 0.0;
            }
            set
            {
                if (value < 0.0 || value > 1.0)
                {
                    throw new ArgumentOutOfRangeException("ValuePercentage must be 0.0 - 1.0", innerException: null);
                }
                Value = Minimum + ((Maximum - Minimum) * value);
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        public SupportiveSlider(MultiSlider owner, int position)
            : base()
        {
            this.owner = owner;
            Position = position;
            lastValue = Double.NaN;
        }
        #endregion

        /************************************************************************/
        
        #region Methods
        /// <summary>
        /// Suspends handling of the ValueChanged event.
        /// </summary>
        public void SuspendValueChanged()
        {
            isSuspendedValueChanged = true;
        }

        /// <summary>
        /// Resumes handling of the ValueChanged event.
        /// </summary>
        public void ResumeValueChanged()
        {
            isSuspendedValueChanged = false;
        }

        /// <summary>
        /// Forces this instance to constrain its Value
        /// </summary>
        public void ConstrainValue()
        {
            ConstrainValue(Value - 0.1, Value);
            ConstrainValue(Value + 0.1, Value);
        }
        #endregion

        /************************************************************************/
        
        #region Events
        /// <summary>
        /// Occurs when the Value property changes. Unlike the base ValueChanged event,
        /// this event fires after constraints have been applied. If applying constaints 
        /// reuslts in the Value property returning to its previous value (constained),
        /// this event does not fire.
        /// </summary>
        public event EventHandler SupportiveValueChanged;
        #endregion

        /************************************************************************/

        #region Protected Methods
        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            CalculateCushionValue();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            CalculateCushionValue();
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            // Prevent re-entry if we need to constrain a value
            if (!isSuspendedValueChanged && !isInProgressValueChanged)
            {
                isInProgressValueChanged = true;
                ConstrainValue(oldValue, newValue);
                isInProgressValueChanged = false;
            }
        }
        #endregion

        /************************************************************************/
        
        #region Private Methods

        private void ConstrainValue(double oldValue, double newValue)
        {
            if (newValue > oldValue)
            {
                ConstrainToUpper();
            }
            else
            {
                ConstrainToLower();
            }
            if (Value != lastValue)
            {
                lastValue = Value;
                RaiseSupportiveValueChangedEvent();
            }
        }

        private void ConstrainToLower()
        {
            if (LowerPeer != null && Value < LowerPeer.Value + cushionValue)
            {
                Value = LowerPeer.Value + cushionValue;
            }
        }

        private void ConstrainToUpper()
        {
            if (Position < owner.SliderCount - 1 && UpperPeer != null && Value > UpperPeer.Value - cushionValue)
            {
                
                Value = UpperPeer.Value - cushionValue;
            }
        }

        private void RaiseSupportiveValueChangedEvent()
        {
            var eventSig = SupportiveValueChanged;
            if (eventSig != null)
            {
                eventSig(this, EventArgs.Empty);
            }
        }

        private void CalculateCushionValue()
        {
            cushionValue = (Maximum - Minimum) * cushion;
        }
        #endregion
    }
}
