/**
 * Restless Animal Development
 * This program is provided freely without restrictions, but please retain this notice in any distributions, modifications ,etc.
 * https://restlessanimal.com
 */
using System;
using System.Windows;
using Xam.Applications.GradientEditor.ViewModel;

namespace Xam.Applications.GradientEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Window main = new MainWindow();
            MainWindowViewModel mainViewModel = new MainWindowViewModel();
            main.DataContext = mainViewModel;
            main.Width = Math.Min(System.Windows.SystemParameters.WorkArea.Width, 1176.0);
            main.Height = Math.Min(System.Windows.SystemParameters.WorkArea.Height, 822.0);
            main.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
