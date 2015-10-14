using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using Xceed.Wpf.Toolkit;
using Xam.Wpf.Controls;

namespace Xam.Applications.GradientEditor.Controls
{
    /// <summary>
    /// Interaction logic for GradientControl.xaml
    /// </summary>
    public partial class GradientControl : UserControl, INotifyPropertyChanged
    {
        #region Private Vars
        private const int minStops = MultiSlider.MinSliderCount;
        private const int maxStops = MultiSlider.MaxSliderCount;
        private LinearGradientBrush currentBrush;
        private string xamlOutput;
        private double startPointX;
        private double startPointY;
        private double endPointX;
        private double endPointY;
        private string operationMessage;
        private bool operationMessageCreated;
        #endregion

        /************************************************************************/

        #region Public Fields
        /// <summary>
        /// Provides the property name for the CurrentBrush property.
        /// </summary>
        public const string CurrentBrushPropertyName = "CurrentBrush";
        /// <summary>
        /// Provides the property name for the CurrenEditStop property.
        /// </summary>
        public const string CurrentEditStopPropertyName = "CurrentEditStop";
        /// <summary>
        /// Provides the property name for the XamlOutput property.
        /// </summary>
        public const string XamlOutputPropertyName = "XamlOutput";
        /// <summary>
        /// Provides the property name for the OperationMessage property
        /// </summary>
        public const string OperationMessagePropertyName = "OperationMessage";
        /// <summary>
        /// Provides the property name for the OperationMessageCreated property
        /// </summary>
        public const string OperationMessageCreatedPropertyName = "OperationMessageCreated";
        /// <summary>
        /// Gets the amount of expansion that is applied when invoking the IncreaseGradientRangeCommand.
        /// </summary>
        public const double RangeIncreaseAmount = 0.1;
        /// <summary>
        /// Gets the amount of contraction that is applied when invoking the DecreaseGradientRangeCommand.
        /// </summary>
        public const double RangeDecreaseAmount = 0.1;

        #endregion

        /************************************************************************/
        
        #region Public Properties
        /// <summary>
        /// Gets the GradientStops object that represents band information about the available gradient bands.
        /// </summary>
        public GradientStops Stops
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a boolean value that determines whether the control is two-way, 
        /// i.e. it can accept text input to create the brush.
        /// This feature is not currently implemented; setting this property has no effect.
        /// </summary>
        public bool IsTwoWay
        {
            get { return false; }
            set
            {
                //isTwoWay = value;
                //OnPropertyChanged("IsTwoWay");
            }
        }

        /// <summary>
        /// Gets the command to shift the gradient colors to the left.
        /// </summary>
        public RelayCommand ShiftColorsLeftCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command to shift the gradient colors to the right.
        /// </summary>
        public RelayCommand ShiftColorsRightCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command to increase the gradient range.
        /// </summary>
        public RelayCommand IncreaseGradientRangeCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command to decrease the gradient range.
        /// </summary>
        public RelayCommand DecreaseGradientRangeCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command to spread the gradient stops evenly throughout the gradient range.
        /// </summary>
        public RelayCommand SpreadGradientStopsCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command to reset the control to its beginning state.
        /// </summary>
        public RelayCommand ResetCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command to copy the Xaml output to the clipboard.
        /// </summary>
        public RelayCommand CopyXamlCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the starting X cooridinate
        /// </summary>
        public double StartPointX
        {
            get { return startPointX; }
            set
            {
                if (value != startPointX)
                {
                    startPointX = value;
                    currentBrush.StartPoint = new Point(startPointX, startPointY);
                    CreateXamlOutput(currentBrush);
                    OnPropertyChanged(CurrentBrushPropertyName);
                    OnPropertyChanged("StartPointX");
                }
            }
        }

        /// <summary>
        /// Gets or sets the starting Y cooridinate
        /// </summary>
        public double StartPointY
        {
            get { return startPointY; }
            set
            {
                if (value != startPointY)
                {
                    startPointY = value;
                    currentBrush.StartPoint = new Point(startPointX, startPointY);
                    CreateXamlOutput(currentBrush);
                    OnPropertyChanged(CurrentBrushPropertyName);
                    OnPropertyChanged("StartPointY");
                }
            }
        }

        /// <summary>
        /// Gets or sets the ending X cooridinate
        /// </summary>
        public double EndPointX
        {
            get { return endPointX; }
            set
            {
                if (value != endPointX)
                {
                    endPointX = value;
                    currentBrush.EndPoint = new Point(endPointX, endPointY);
                    CreateXamlOutput(currentBrush);
                    OnPropertyChanged(CurrentBrushPropertyName);
                    OnPropertyChanged("EndPointX");
                }
            }
        }

        /// <summary>
        /// Gets or sets the ending Y cooridinate
        /// </summary>
        public double EndPointY
        {
            get { return endPointY; }
            set
            {
                if (value != endPointY)
                {
                    endPointY = value;
                    currentBrush.EndPoint = new Point(endPointX, endPointY);
                    CreateXamlOutput(currentBrush);
                    OnPropertyChanged(CurrentBrushPropertyName);
                    OnPropertyChanged("EndPointY");
                }
            }
        }

        /// <summary>
        /// Gets the current editing band in a friendly one-based manner. 
        /// </summary>
        public int CurrentEditStop
        {
            get { return Stops.CurrentEditStop + 1; }
        }

        /// <summary>
        /// Gets the current brush
        /// </summary>
        public LinearGradientBrush CurrentBrush
        {
            get { return currentBrush; }
        }

        /// <summary>
        /// Gets the xaml output for the current brush.
        /// </summary>
        public string XamlOutput
        {
            get { return xamlOutput; }
            private set
            {
                xamlOutput = value;
                OnPropertyChanged(XamlOutputPropertyName);
            }
        }

        /// <summary>
        /// Gets the operation message from the control
        /// </summary>
        public string OperationMessage
        {
            get { return operationMessage; }
            private set
            {
                operationMessage = value;
                OnPropertyChanged(OperationMessagePropertyName);
                OperationMessageCreated = true;
                OperationMessageCreated = false;

            }
        }

        /// <summary>
        /// Gets the operation message from the control
        /// </summary>
        public bool OperationMessageCreated
        {
            get { return operationMessageCreated; }
            private set
            {
                operationMessageCreated = value;
                OnPropertyChanged(OperationMessageCreatedPropertyName);
            }
        }

        #endregion

        /************************************************************************/

        #region Constructor
        public GradientControl()
        {
            InitializeComponent();
            DataContext = this;

            Stops = new GradientStops(minStops, maxStops);
            Stops.PropertyChanged += new PropertyChangedEventHandler(StopsPropertyChanged);

            currentBrush = new LinearGradientBrush();

            StartPointX = 0.0;
            StartPointY = 0.0;
            EndPointX = 1.0;
            EndPointY = 1.0;

            ShiftColorsLeftCommand = new RelayCommand(ShiftColorsLeftExecute, null, Strings.CommandDescriptionShiftColorsLeft);
            ShiftColorsRightCommand = new RelayCommand(ShiftColorsRightExecute, null, Strings.CommandDescriptionShiftColorsRight);
            IncreaseGradientRangeCommand = new RelayCommand(IncreaseGradientRangeExecute, IncreaseGradientRangeCanExecute, Strings.CommandDescriptionIncreaseGradientRange);
            DecreaseGradientRangeCommand = new RelayCommand(DecreaseGradientRangeExecute, DecreaseGradientRangeCanExecute, Strings.CommandDescriptionDecreaseGradientRange);
            SpreadGradientStopsCommand = new RelayCommand(SpreadGradientStopsExecute, null, Strings.CommandDescriptionSpreadGradientStops);
            ResetCommand = new RelayCommand(ResetExecute, null, Strings.CommandDescriptionReset);
            CopyXamlCommand = new RelayCommand(CopyXamlExecute, null, Strings.CommandDescriptionCopyXaml);

            Stops.SelectedStopsCount = MultiSlider.DefaultSliderCount;

            StopsControl.SliderSet += StopsControlSlidersSet;
            StopsControl.ValueChanged += StopsControlValueChanged;
            StopsControl.SliderSelected += StopsControlSliderSelected;
            ToolBar b;
        }
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


        /************************************************************************/

        #region Private Methods
        private void UpdateCurrentBrush(List<double> offsets)
        {
            int numStops = offsets.Count;
            while (currentBrush.GradientStops.Count > numStops)
            {
                currentBrush.GradientStops.RemoveAt(currentBrush.GradientStops.Count - 1);
            }

            while (currentBrush.GradientStops.Count < numStops)
            {
                currentBrush.GradientStops.Add(new GradientStop());
            }

            double div = 1.0 / numStops;

            for (int k = 0; k < numStops; k++)
            {
                currentBrush.GradientStops[k].Color = Stops.Colors[k];
                currentBrush.GradientStops[k].Offset = offsets[k];
            }

            OnPropertyChanged(CurrentBrushPropertyName);
            CreateXamlOutput(currentBrush);
        }


        private void StopsControlSlidersSet(object sender, MultiSliderSetRoutedEventArgs e)
        {
            UpdateCurrentBrush(e.SliderValues);
        }

        private void StopsControlValueChanged(object sender, MultiSliderRoutedEventArgs e)
        {
            currentBrush.GradientStops[e.Position].Offset = e.SliderValues[e.Position];
            OnPropertyChanged(CurrentBrushPropertyName);
            CreateXamlOutput(currentBrush);
        }

        private void StopsControlSliderSelected(object sender, MultiSliderRoutedEventArgs e)
        {
            Stops.CurrentEditStop = e.Position;
        }

        private void CreateXamlOutput(LinearGradientBrush brush)
        {
            StringBuilder b = new StringBuilder(512);
            b.AppendLine(String.Format("<LinearGradientBrush x:Key=\"MyBrush\" StartPoint=\"{0},{1}\" EndPoint=\"{2},{3}\">", 
                brush.StartPoint.X.ToString("F2"), brush.StartPoint.Y.ToString("F2"), 
                brush.EndPoint.X.ToString("F2"), brush.EndPoint.Y.ToString("F2")));
            b.AppendLine("\t<GradientBrush.GradientStops>");
            b.AppendLine("\t\t<GradientStopCollection>");

            foreach (var stop in brush.GradientStops)
            {
                b.AppendLine(String.Format("\t\t\t<GradientStop Color=\"{0}\" Offset=\"{1}\"/>", stop.Color, stop.Offset.ToString("F3")));
            }

            b.AppendLine("\t\t</GradientStopCollection>");
            b.AppendLine("\t</GradientBrush.GradientStops>");
            b.AppendLine("</LinearGradientBrush>");

            XamlOutput = b.ToString();
        }

        private void StopsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GradientStops.SelectedStopsCountPropertyName)
            {
                StopsControl.SliderCount = Stops.SelectedStopsCount;
            }

            if (e.PropertyName == GradientStops.SelectedColorPropertyName)
            {
                UpdateCurrentBrush(StopsControl.SliderValues);
            }

            if (e.PropertyName == GradientStops.CurrentEditStopPropertyName)
            {
                OnPropertyChanged(CurrentEditStopPropertyName);
            }
        }

        private void ShiftColorsLeftExecute(object o)
        {
            Color temp = Stops.Colors[0];
            for (int k = 0; k < Stops.SelectedStopsCount - 1; k++)
            {
                Stops.Colors[k] = Stops.Colors[k + 1];
            }
            Stops.Colors[Stops.SelectedStopsCount - 1] = temp;
            UpdateCurrentBrush(StopsControl.SliderValues);
            Stops.SelectedColor = Stops.Colors[Stops.CurrentEditStop];
        }

        private void ShiftColorsRightExecute(object o)
        {
            Color temp = Stops.Colors[Stops.SelectedStopsCount - 1];
            for (int k = Stops.SelectedStopsCount - 1; k > 0; k--)
            {
                Stops.Colors[k] = Stops.Colors[k - 1];
            }
            Stops.Colors[0] = temp;
            UpdateCurrentBrush(StopsControl.SliderValues);
            Stops.SelectedColor = Stops.Colors[Stops.CurrentEditStop];
        }

        private bool IncreaseGradientRangeCanExecute(object o)
        {
            return StopsControl.Minimum > -0.6d;
        }

        private void IncreaseGradientRangeExecute(object o)
        {
            StopsControl.Minimum = Math.Round(StopsControl.Minimum - RangeIncreaseAmount, 1);
            StopsControl.Maximum = Math.Round(StopsControl.Maximum + RangeIncreaseAmount, 1);
        }

        private bool DecreaseGradientRangeCanExecute(object o)
        {
            return StopsControl.Minimum < 0.0d;
        }

        private void DecreaseGradientRangeExecute(object o)
        {
            StopsControl.Minimum = Math.Round(StopsControl.Minimum + RangeDecreaseAmount, 1);
            StopsControl.Maximum = Math.Round(StopsControl.Maximum - RangeDecreaseAmount, 1);
        }

        private void SpreadGradientStopsExecute(object o)
        {
            StopsControl.SpreadSliders();
            OperationMessage = null;
        }

        private void ResetExecute(object o)
        {
            StopsControl.Minimum = 0.0;
            StopsControl.Maximum = 1.0;
            StartPointX = 0.0;
            StartPointY = 0.0;
            EndPointX = 1.0;
            EndPointY = 1.0;
            StopsControl.SpreadSliders();
            OperationMessage = Strings.OperationMessageReset;
        }

        private void CopyXamlExecute(object o)
        {
            Clipboard.SetText(XamlOutput);
            OperationMessage = Strings.OperationMessageXamlCopiedToClipboard;
        }


        #endregion
    }
}
