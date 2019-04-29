using BCI.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BCI
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class ErrorWindow : MetroWindow
    {        
        public Exception ActualException
        {
            get
            {
                return viewModel.ActualException;
            }
            set => viewModel.ActualException = value;            
        }

        ErrorWindowViewModel viewModel = new ErrorWindowViewModel();
        public ErrorWindow()
        {
            InitializeComponent();
            DataContext = viewModel;            
        }
    }
}
