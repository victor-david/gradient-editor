/**
 * Restless Animal Development
 * This program is provided freely without restrictions, but please retain this notice in any distributions, modifications ,etc.
 * http://dev.restlessanimal.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;

namespace Xam.Wpf.Controls.Core
{
    public class CoreResourceDictionary : ResourceDictionary, ISupportInitialize
    {
        #region Public Fields
        public const string DefaultAssemblyName = "Xam.Wpf.Controls";
        #endregion

        /************************************************************************/
        
        #region Private vars
        private int initCount;
        private string assemblyName;
        private string sourcePath;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets the assembly name for this resource dictionary.
        /// </summary>
        public string AssemblyName
        {
            get { return assemblyName; }
            set
            {
                ValidateInit();
                assemblyName = value;
            }
        }

        /// <summary>
        /// Gets or sets the source path for this resource dictionary.
        /// </summary>
        public string SourcePath
        {
            get { return sourcePath; }
            set
            {
                ValidateInit();
                sourcePath = value;
            }
        }
        #endregion

        /************************************************************************/

        #region Constrcutors
        public CoreResourceDictionary(string assemblyName, string sourcePath)
        {
            ((ISupportInitialize)this).BeginInit();
            AssemblyName = assemblyName;
            SourcePath = sourcePath;
            ((ISupportInitialize)this).EndInit();
        }

        public CoreResourceDictionary(string sourcePath)
            : this(DefaultAssemblyName, sourcePath)
        {
        }

        public CoreResourceDictionary()
        {
        }
        #endregion

        /************************************************************************/

        #region Private Methods
        private void ValidateInit()
        {
            if (initCount <= 0)
            {
                throw new InvalidOperationException("VersionResourceDictionary properties can only be set while initializing");
            }
        }
        #endregion

        /************************************************************************/

        #region ISupportInitialize
        void ISupportInitialize.BeginInit()
        {
            base.BeginInit();
            initCount++;
        }

        void ISupportInitialize.EndInit()
        {
            initCount--;
            if (initCount <= 0)
            {
                if (Source != null)
                {
                    throw new InvalidOperationException("CoreResourceDictionay: Do not set Source property");
                }

                if (String.IsNullOrEmpty(AssemblyName))
                {
                    assemblyName = DefaultAssemblyName;
                }

                if (String.IsNullOrEmpty(SourcePath))
                {
                    throw new InvalidOperationException("SourcePath must be set during initialization");
                }

                string uriStr = string.Format(@"pack://application:,,,/{0};component/{1}", AssemblyName, SourcePath);
                Source = new Uri(uriStr, UriKind.Absolute);
            }
            base.EndInit();
        }
        #endregion
    }
}