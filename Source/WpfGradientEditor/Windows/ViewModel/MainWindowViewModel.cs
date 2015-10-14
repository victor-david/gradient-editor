﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;

namespace Xam.Applications.GradientEditor.ViewModel 
{
    /// <summary>
    /// Provides the logic for MainWindow.
    /// This doesn't do much right now, but is in place for future expanison.
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Private Vars
        #endregion

        /************************************************************************/

        #region Public Properties
        /// <summary>
        /// Gets the string used in the display title of the application.
        /// </summary>
        public string DisplayTitle
        {
            get;
            private set;
        }
        #endregion
        
        /************************************************************************/

        #region Constructor
        public MainWindowViewModel()
        {
            DisplayTitle = "Wpf Gradient Editor 3.0";
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

        #endregion

    }
}
