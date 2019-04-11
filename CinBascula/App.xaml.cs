using System;
using System.Windows;
using System.Windows.Threading;

namespace CinBascula
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {

        AppDomain currentDomain = AppDomain.CurrentDomain;
        public event DispatcherUnhandledExceptionEventHandler UnhandledException;

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ToString());
            // Process unhandled exception

            // Prevent default unhandled exception processing
            e.Handled = true;
        }       

        
    }
}
