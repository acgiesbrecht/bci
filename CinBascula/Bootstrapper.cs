using Caliburn.Micro;
using System.Windows;

namespace CinBascula
{
    
        public class Bootstrapper : BootstrapperBase
        {
            public Bootstrapper()
            {
                Initialize();
            }

            protected override void OnStartup(object sender, StartupEventArgs e)
            {
                DisplayRootViewFor<MainViewModel>();
            }
        }    
}
