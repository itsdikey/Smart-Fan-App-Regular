using SFAR.App.Views;
using SFAR.Models.Devices;
using SFAR.ViewModels;

namespace Smart_Fan_App_Regular
{
    public partial class MainPage : ContentPage, IViewBase
    {
        private DiscoverPageViewModel ViewModel => (DiscoverPageViewModel)this.BindingContext;

        public MainPage(DiscoverPageViewModel discoverPageViewModel)
        {
            InitializeComponent();
            this.BindingContext = discoverPageViewModel;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectionChanged(e.SelectedItem as SmartFanBLEDevice);
        }
    }

}
