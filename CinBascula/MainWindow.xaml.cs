using CinBascula.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace CinBascula
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();

            this.WhenActivated(disposableRegistration =>
            {

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.InventoryItemsCollection,
                    view => view.InventoryItemsAutoCompleteComboBox.ItemsSource)
                    .DisposeWith(disposableRegistration);

                this.Bind(ViewModel,
                    viewModel => viewModel.SelectedPesada.InventoryItem,
                    view => view.InventoryItemsAutoCompleteComboBox.SelectedItem)
                    .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.TipoActividadCollection,
                    view => view.TipoActividadAutoCompleteComboBox.ItemsSource)
                    .DisposeWith(disposableRegistration);

                this.Bind(ViewModel,
                    viewModel => viewModel.SelectedPesada.TIPO_ACTIVIDAD,
                    view => view.TipoActividadAutoCompleteComboBox.SelectedItem)
                    .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.OrganisationCollection,
                    view => view.OrganizacionAutoCompleteComboBox.ItemsSource)
                    .DisposeWith(disposableRegistration);

                this.Bind(ViewModel,
                    viewModel => viewModel.SelectedPesada.ORGANIZATION_ID,
                    view => view.OrganizacionAutoCompleteComboBox.SelectedItem)
                    .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.PuntoDeDescargaFilteredList,
                    view => view.PuntoDeDescargaAutoCompleteComboBox.ItemsSource)
                    .DisposeWith(disposableRegistration);

                this.Bind(ViewModel,
                    viewModel => viewModel.Peso,
                    view => view.PesoTextBox.Text)
                    .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.SelectedPesada.PESO_BRUTO,
                    view => view.PesoBrutoLabel.Content)
                    .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.SelectedPesada.PESO_TARA,
                    view => view.PesoTaraLabel.Content)
                    .DisposeWith(disposableRegistration);

               /* this.OneWayBind(ViewModel,
                    viewModel => viewModel.SelectedPesada.TIPO_ACTIVIDAD.TipoEstablecimiento,
                    view => view.TipoEstablecimientoLabel.Content)
                    .DisposeWith(disposableRegistration);
                    */
                this.OneWayBind(ViewModel,
        viewModel => viewModel.visibilityContrato,
        view => view.ContratoPanel.Visibility,
        value => BoolToVisibilityConverter(value))
        .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
        viewModel => viewModel.visibilityLote,
        view => view.LotePanel.Visibility,
        value => BoolToVisibilityConverter(value))
        .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
        viewModel => viewModel.SelectedPesada,
        view => view.BtnBruto.IsEnabled,
        value => NullToEnabledConverter(value))
        .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
        viewModel => viewModel.SelectedPesada,
        view => view.BtnTara.IsEnabled,
        value => NullToEnabledConverter(value))
        .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
        viewModel => viewModel.SelectedPesada,
        view => view.BtnGuardar.IsEnabled,
        value => NullToEnabledConverter(value))
        .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
        viewModel => viewModel.PesadasPendientesList,
        view => view.PendientesDataGrid.ItemsSource)
        .DisposeWith(disposableRegistration);

                

                this.BindCommand(
                    ViewModel,
                    x => x.NewPesada,
                    x => x.BtnNewPesada);

                this.BindCommand(
                    ViewModel,
                    x => x.SetBruto,
                    x => x.BtnBruto);

                this.BindCommand(
                    ViewModel,
                    x => x.SetTara,
                    x => x.BtnTara);

                this.BindCommand(
                    ViewModel,
                    x => x.Guardar,
                    x => x.BtnGuardar);

            });
        }

        private Visibility BoolToVisibilityConverter(bool isVisible)
        {
            if (isVisible)
            {return Visibility.Visible;}
            else
            {return Visibility.Hidden;}
        }

        private bool NullToEnabledConverter(object obj)
        {
            return obj != null;
        }

    }
}
