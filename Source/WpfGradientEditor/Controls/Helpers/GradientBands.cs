using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;

namespace Xam.Applications.GradientEditor.Controls
{
    /// <summary>
    /// Provides helper services for managing gradient bands and current band selection.
    /// </summary>
    public class GradientBands : INotifyPropertyChanged
    {
        #region Private Vars
        private int selectedBandCount;
        private int currentEditBand;
        #endregion

        /************************************************************************/
        
        #region Public Fields
        /// <summary>
        /// Provides the property name for the SelectedBandCount property.
        /// </summary>
        public const string SelectedBandCountPropertyName = "SelectedBandCount";
        /// <summary>
        /// Provides the property name for the CurrentEditBand property.
        /// </summary>
        public const string CurrentEditBandPropertyName = "CurrentEditBand";
        /// <summary>
        /// Provides the property name for the SelectedColor property.
        /// </summary>
        public const string SelectedColorPropertyName = "SelectedColor";
        #endregion

        /************************************************************************/

        #region Public Properties
        /// <summary>
        /// Gets the minimum number of allowed gradient bands.
        /// </summary>
        public int MinBands
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the maximum number of allowed gradient bands.
        /// </summary>
        public int MaxBands
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of available bands.
        /// </summary>
        public int[] AvailableBands
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the selected band count.
        /// </summary>
        public int SelectedBandCount
        {
            get { return selectedBandCount; }
            set
            {
                if (value != selectedBandCount)
                {
                    if (value < MinBands || value > MaxBands)
                    {
                        throw new ArgumentOutOfRangeException(Strings.ArgumentException_SelectedBandCount, innerException: null);
                    }
                    selectedBandCount = value;
                    OnPropertyChanged(SelectedBandCountPropertyName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the zero-based integer that represents the gradient band that is currently being edited.
        /// </summary>
        public int CurrentEditBand
        {
            get { return currentEditBand; }
            set
            {
                if (value != currentEditBand)
                {
                    if (value < 0 || value > SelectedBandCount - 1)
                    {
                        throw new ArgumentOutOfRangeException(String.Format(Strings.ArgumentException_CurrentEditBand, 0, SelectedBandCount - 1), innerException: null);
                    }
                    currentEditBand = value;
                    OnPropertyChanged(SelectedColorPropertyName);
                    OnPropertyChanged(CurrentEditBandPropertyName);
                }
            }
        }

        /// <summary>
        /// Gets the available colors indexed by band.
        /// </summary>
        public List<Color> Colors
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the currently selected color for CurrentEditBand.
        /// </summary>
        public Color? SelectedColor
        {
            get { return Colors[currentEditBand]; }
            set
            {
                if (value.HasValue)
                {
                    Colors[currentEditBand] = (Color)value;
                }
                OnPropertyChanged(SelectedColorPropertyName);
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor

        public GradientBands(int minBands, int maxBands)
        {
            if (minBands < 2) throw new ArgumentOutOfRangeException(Strings.ArgumentException_MinimumBands, innerException: null);
            if (maxBands < minBands + 1) throw new ArgumentOutOfRangeException(Strings.ArgumentException_MaximumBands, innerException: null);
            MinBands = minBands;
            MaxBands = maxBands;
            AvailableBands = Enumerable.Range(MinBands, MaxBands - 1).ToArray();

            // The first 6 colors are simply predefined.
            Colors = new List<Color>();
            Colors.Add(System.Windows.Media.Colors.DarkBlue);
            Colors.Add(System.Windows.Media.Colors.Orange);
            Colors.Add(System.Windows.Media.Colors.DarkBlue);
            Colors.Add(System.Windows.Media.Colors.LightGray);
            Colors.Add(System.Windows.Media.Colors.MintCream);
            Colors.Add(System.Windows.Media.Colors.MediumVioletRed);

            // After 6 colors, we'll just grab some random ones.
            if (maxBands > 6)
            {
                Random rand = new Random();
                byte a = 0, r = 0, g = 0, b = 0;

                for (int k = 0; k < maxBands - 6; k++)
                {
                    a = (byte)rand.Next(127, 256);
                    r = (byte)rand.Next(0, 256);
                    g = (byte)rand.Next(0, 256);
                    b = (byte)rand.Next(0, 256);

                    Colors.Add(System.Windows.Media.Color.FromArgb(a, r, g, b));
                }
            }

            // must set the private backing var here to avoid triggering the property change.
            selectedBandCount = minBands;
        }
        #endregion

        /************************************************************************/

        #region Public Methods

        #endregion

        /************************************************************************/

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        #endregion
    }
}
