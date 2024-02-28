using SFAR.ViewModels;

namespace SFAR.App.Views;

public partial class DeviceControlPage : ContentPage, IViewBase
{
	public DeviceControlPage(ControlDevicePageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
        Loaded += DeviceControlPage_Loaded;
	}

    private void DeviceControlPage_Loaded(object? sender, EventArgs e)
    {

    }
}