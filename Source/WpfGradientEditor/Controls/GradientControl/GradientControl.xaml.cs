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

namespace Xam.Applications.GradientEditor.Controls
{
    /// <summary>
    /// Interaction logic for GradientControl.xaml
    /// </summary>
    public partial class GradientControl : UserControl, INotifyPropertyChanged
    {
        #region Private Vars
        private const int minBands = 2;
        private const int maxBands = 16;
        private LinearGradientBrush currentBrush;
        private string xamlOutput;
        private int waitForSizeChangeCount;
        private int sizeChangeCount;
        private double startPointX;
        private double startPointY;
        private double endPointX;
        private double endPointY;
        private bool shiftColorsButtons;
        private bool displayBandDividers;
        private bool isTwoWay;
        #endregion

        /************************************************************************/

        #region Public Fields
        /// <summary>
        /// Provides the property name for the CurrentBrush property.
        /// </summary>
        public const string CurrentBrushPropertyName = "CurrentBrush";
        /// <summary>
        /// Provides the property name for the TotalColumns property.
        /// </summary>
        public const string TotalColumnsPropertyName = "TotalColumns";
        /// <summary>
        /// Provides the property name for the CurrenEditBand property.
        /// </summary>
        public const string CurrentEditBandPropertyName = "CurrentEditBand";
        /// <summary>
        /// Provides the property name for the XamlOutput property.
        /// </summary>
        public const string XamlOutputPropertyName = "XamlOutput";
        #endregion

        /************************************************************************/
        
        #region Public Properties
        /// <summary>
        /// Gets the Bands object that represents band information about the available gradient bands.
        /// </summary>
        public GradientBands Bands
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether the shift colors buttons are available on the control.
        /// The default is true.
        /// </summary>
        public bool ShiftColorsButtons
        {
            get { return shiftColorsButtons; }
            set
            {
                shiftColorsButtons = value;
                OnPropertyChanged("ShiftColorsButtons");
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that determines whether the control is two-way, 
        /// i.e. it can accept text input to create the brush.
        /// This feature is not currently implemented; setting this property has no effect.
        /// </summary>
        public bool IsTwoWay
        {
            get { return isTwoWay; }
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
        /// Gets the total number of columns currently in the control.
        /// </summary>
        public int TotalColumns
        {
            get { return Bands.SelectedBandCount + Bands.SelectedBandCount - 1; }
        }

        /// <summary>
        /// Gets the current editing band in a friendly one-based manner. 
        /// </summary>
        public int CurrentEditBand
        {
            get { return Bands.CurrentEditBand + 1; }
        }

        /// <summary>
        /// Gets the current brush
        /// </summary>
        public LinearGradientBrush CurrentBrush
        {
            get { return currentBrush; }
        }

        /// <summary>
        /// Gets or sets a value that determines if the grid band dividers are visible.
        /// </summary>
        public bool DisplayBandDividers
        {
            get { return displayBandDividers; }
            set
            {
                displayBandDividers = value;
                SetBandDividersVisibility(displayBandDividers);
                OnPropertyChanged("DisplayGridDividers");
            }
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
        #endregion

        /************************************************************************/

        #region Constructor
        public GradientControl()
        {
            InitializeComponent();
            DataContext = this;

            Bands = new GradientBands(minBands, maxBands);
            Bands.PropertyChanged += new PropertyChangedEventHandler(BandsPropertyChanged);

            InitializeGrid(maxBands);

            currentBrush = new LinearGradientBrush();

            StartPointX = 0.0;
            StartPointY = 0.0;
            EndPointX = 1.0;
            EndPointY = 1.0;

            ShiftColorsButtons = true;
            ShiftColorsLeftCommand = new RelayCommand(ShiftColorsLeftExecute, null, Strings.CommandDescriptionShiftColorsLeft);
            ShiftColorsRightCommand = new RelayCommand(ShiftColorsRightExecute, null, Strings.CommandDescriptionShiftColorsRight);

            Bands.SelectedBandCount = 3;
            SetCurrentEditBandVisual(Bands.CurrentEditBand);

            DisplayBandDividers = true;
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
        private void InitializeGrid(int maxBands)
        {
            // Create the selected-band indicators
            for (int k = 0; k < maxBands; k++)
            {
                Border border = new Border();
                Grid.SetColumn(border, k * 2);
                Grid.SetRow(border, 1);
                DisplayGrid.Children.Add(border);
            }

            // Create the grid splitters
            for (int k = 0; k < maxBands - 1; k++)
            {
                GridSplitter splitter = new GridSplitter();
                Grid.SetColumn(splitter, k * 2 + 1);
                Grid.SetRowSpan(splitter, 2);
                DisplayGrid.Children.Add(splitter);
            }
        }

        private void CreateColumnDefinitions(int numBands)
        {
            var widthDescriptor = DependencyPropertyDescriptor.FromProperty(ColumnDefinition.WidthProperty, typeof(ItemsControl));
            
            DisplayGrid.ColumnDefinitions.Clear();
            int totalCols = numBands * 2 - 1;
            for (int colIdx = 0; colIdx < totalCols; colIdx++)
            {
                ColumnDefinition def = new ColumnDefinition();
                if (colIdx % 2 == 1)
                {
                    // splitter column
                    def.Width = GridLength.Auto;
                }
                else
                {
                    // not a splitter column
                    def.Width = new GridLength(1, GridUnitType.Star);
                    widthDescriptor.AddValueChanged(def, WidthChanged);

                }
                DisplayGrid.ColumnDefinitions.Add(def);
            }

            int splitterCount = 0;
            foreach (GridSplitter splitter in DisplayGrid.Children.OfType<GridSplitter>())
            {
                splitterCount++;
                splitter.Visibility = (splitterCount < numBands && displayBandDividers) ? Visibility.Visible : Visibility.Collapsed;
            }

            OnPropertyChanged(TotalColumnsPropertyName);
            waitForSizeChangeCount = numBands;
            sizeChangeCount = 0;
            Bands.CurrentEditBand = 0;
        }

        private void SetCurrentEditBandVisual(int currentEditBand)
        {
            int borderCount = 0;
            foreach (Border splitter in DisplayGrid.Children.OfType<Border>())
            {
                splitter.Visibility = (borderCount == currentEditBand) ? Visibility.Visible : Visibility.Collapsed;
                borderCount++;
            }
        }

        private void WidthChanged(object sender, EventArgs e)
        {
            ColumnDefinition def = sender as ColumnDefinition;
            if (def != null)
            {
                sizeChangeCount++;
                if (sizeChangeCount == waitForSizeChangeCount)
                {
                    waitForSizeChangeCount = 2;
                    sizeChangeCount = 0;
                    UpdateCurrentBrush(Bands.SelectedBandCount);
                }
            }
        }

        private void UpdateCurrentBrush(int numBands)
        {
            var offsets = GetOffsets(numBands);
            while (currentBrush.GradientStops.Count > numBands)
            {
                currentBrush.GradientStops.RemoveAt(currentBrush.GradientStops.Count - 1);
            }

            while (currentBrush.GradientStops.Count < numBands)
            {
                currentBrush.GradientStops.Add(new GradientStop());
            }

            for (int k = 0; k < numBands; k++)
            {
                currentBrush.GradientStops[k].Color = Bands.Colors[k];
                currentBrush.GradientStops[k].Offset = offsets[k];
            }

            OnPropertyChanged(CurrentBrushPropertyName);
            CreateXamlOutput(currentBrush);
        }

        private List<double> GetOffsets(int numBands)
        {
            List<double> widths = GetColumnWidthsSansSplitters();

            //Debug.WriteLine("Widths sum is {0}", widths.Sum());

            // Before any resize, each col def will have a value of 1.0
            if (widths.Sum() == numBands)
            {
                return GetDefaultOffsets(numBands);
            }

            List<double> list = new List<double>();
            list.Add(0.0);
            double wip = 0.0;
            
            for (int k = 1; k < widths.Count; k++)
            {
                wip += widths[k-1];
                double offset = wip / widths.Sum();
                list.Add(offset);
            }

            return list;
        }

        /// <summary>
        /// Gets default offsets, evenly divided between the bands.
        /// </summary>
        /// <returns></returns>
        private List<double> GetDefaultOffsets(int numBands)
        {
            List<double> list = new List<double>();
            list.Add(0.0);
            for (int k = 1; k < numBands; k++)
            {
                list.Add(1.0 / numBands * k);
            }
            return list;
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

        private void MainRectangleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePoint = e.GetPosition(MainRectangle);
            double mouseOffset = mousePoint.X / MainRectangle.ActualWidth;
            List<double> offsets = GetOffsets(Bands.SelectedBandCount);
            int editBand = 0;

            for (int k = 1; k < offsets.Count; k++)
            {
                if (mouseOffset < offsets[k])
                {
                    Bands.CurrentEditBand = editBand;
                    return;
                }
                editBand = Math.Min(editBand + 1, Bands.SelectedBandCount - 1);
            }
            // set to last band
            Bands.CurrentEditBand = editBand;
        }

        private List<double> GetColumnWidthsSansSplitters()
        {
            List<double> widths = new List<double>();

            for (int colIdx = 0; colIdx < DisplayGrid.ColumnDefinitions.Count; colIdx++)
            {
                if (colIdx % 2 == 0)
                {
                    // not a splitter column
                    widths.Add(DisplayGrid.ColumnDefinitions[colIdx].Width.Value);
                }
            }

            return widths;
        }

        private void BandsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GradientBands.SelectedBandCountPropertyName)
            {
                CreateColumnDefinitions(Bands.SelectedBandCount);
                UpdateCurrentBrush(Bands.SelectedBandCount);
            }

            if (e.PropertyName == GradientBands.SelectedColorPropertyName)
            {
                UpdateCurrentBrush(Bands.SelectedBandCount);
            }

            if (e.PropertyName == GradientBands.CurrentEditBandPropertyName)
            {
                SetCurrentEditBandVisual(Bands.CurrentEditBand);
                OnPropertyChanged(CurrentEditBandPropertyName);
            }
        }

        private void ShiftColorsLeftExecute(object o)
        {
            Color temp = Bands.Colors[0];
            for (int k = 0; k < Bands.SelectedBandCount - 1; k++)
            {
                Bands.Colors[k] = Bands.Colors[k + 1];
            }
            Bands.Colors[Bands.SelectedBandCount - 1] = temp;
            UpdateCurrentBrush(Bands.SelectedBandCount);
            Bands.SelectedColor = Bands.Colors[Bands.CurrentEditBand];
        }

        private void ShiftColorsRightExecute(object o)
        {
            Color temp = Bands.Colors[Bands.SelectedBandCount - 1];
            for (int k = Bands.SelectedBandCount - 1; k > 0; k--)
            {
                Bands.Colors[k] = Bands.Colors[k - 1];
            }
            Bands.Colors[0] = temp;
            UpdateCurrentBrush(Bands.SelectedBandCount);
            Bands.SelectedColor = Bands.Colors[Bands.CurrentEditBand];
        }

        private void SetBandDividersVisibility(bool visible)
        {
            GridSplitter[] splitters =  DisplayGrid.Children.OfType<GridSplitter>().ToArray<GridSplitter>();
            for (int k = 0; k < Bands.SelectedBandCount - 1; k++)
            {
                splitters[k].Visibility = (visible) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion
    }
}
