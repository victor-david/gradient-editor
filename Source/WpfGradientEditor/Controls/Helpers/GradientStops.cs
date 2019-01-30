using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace Xam.Applications.GradientEditor.Controls
{
    /// <summary>
    /// Provides helper services for managing gradient bands and current band selection.
    /// </summary>
    public class GradientStops : INotifyPropertyChanged
    {
        #region Private Vars
        private int selectedStopsCount;
        private int currentEditStop;
        #endregion

        /************************************************************************/
        
        #region Public Fields
        /// <summary>
        /// Provides the property name for the SelectedStopsCount property.
        /// </summary>
        public const string SelectedStopsCountPropertyName = "SelectedStopsCount";
        /// <summary>
        /// Provides the property name for the CurrentEditStop property.
        /// </summary>
        public const string CurrentEditStopPropertyName = "CurrentEditStop";
        /// <summary>
        /// Provides the property name for the SelectedColor property.
        /// </summary>
        public const string SelectedColorPropertyName = "SelectedColor";
        #endregion

        /************************************************************************/

        #region Public Properties
        /// <summary>
        /// Gets the minimum number of allowed gradient stops.
        /// </summary>
        public int MinStops
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the maximum number of allowed gradient stops.
        /// </summary>
        public int MaxStops
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of available stops.
        /// </summary>
        public int[] AvailableStops
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the selected stops count.
        /// </summary>
        public int SelectedStopsCount
        {
            get { return selectedStopsCount; }
            set
            {
                if (value != selectedStopsCount)
                {
                    if (value < MinStops || value > MaxStops)
                    {
                        throw new ArgumentOutOfRangeException(Strings.ArgumentException_SelectedStopsCount, innerException: null);
                    }
                    selectedStopsCount = value;
                    OnPropertyChanged(SelectedStopsCountPropertyName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the zero-based integer that represents the gradient band that is currently being edited.
        /// </summary>
        public int CurrentEditStop
        {
            get { return currentEditStop; }
            set
            {
                if (value != currentEditStop)
                {
                    if (value < 0 || value > SelectedStopsCount - 1)
                    {
                        throw new ArgumentOutOfRangeException(String.Format(Strings.ArgumentException_CurrentEditStop, 0, SelectedStopsCount - 1), innerException: null);
                    }
                    currentEditStop = value;
                    OnPropertyChanged(SelectedColorPropertyName);
                    OnPropertyChanged(CurrentEditStopPropertyName);
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
            get { return Colors[currentEditStop]; }
            set
            {
                if (value.HasValue)
                {
                    Colors[currentEditStop] = (Color)value;
                }
                OnPropertyChanged(SelectedColorPropertyName);
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor

        public GradientStops(int minStops, int maxStops)
        {
            if (minStops < 2) throw new ArgumentOutOfRangeException(Strings.ArgumentException_MinimumStops, innerException: null);
            if (maxStops < minStops + 1) throw new ArgumentOutOfRangeException(Strings.ArgumentException_MaximumStops, innerException: null);
            MinStops = minStops;
            MaxStops = maxStops;
            AvailableStops = Enumerable.Range(MinStops, MaxStops - 1).ToArray();

            // The first 6 colors are simply predefined.
            Colors = new List<Color>();
            Colors.Add(System.Windows.Media.Colors.DarkBlue);
            Colors.Add(System.Windows.Media.Colors.Orange);
            Colors.Add(System.Windows.Media.Colors.DarkBlue);
            Colors.Add(System.Windows.Media.Colors.LightGray);
            Colors.Add(System.Windows.Media.Colors.MintCream);
            Colors.Add(System.Windows.Media.Colors.MediumVioletRed);

            // After 6 colors, we'll just grab some random ones.
            if (maxStops > 6)
            {
                Random rand = new Random();
                byte a = 0, r = 0, g = 0, b = 0;

                for (int k = 0; k < maxStops - 6; k++)
                {
                    a = (byte)rand.Next(127, 256);
                    r = (byte)rand.Next(0, 256);
                    g = (byte)rand.Next(0, 256);
                    b = (byte)rand.Next(0, 256);

                    Colors.Add(System.Windows.Media.Color.FromArgb(a, r, g, b));
                }
            }

            // must set the private backing var here to avoid triggering the property change.
            selectedStopsCount = minStops;
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
