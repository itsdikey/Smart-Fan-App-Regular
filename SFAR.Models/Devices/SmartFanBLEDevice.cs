using CommunityToolkit.Mvvm.ComponentModel;

namespace SFAR.Models.Devices
{
    public sealed class SmartFanBLEDevice : ObservableObject
    {
        private string? name;
        private string? address;

        public string Name 
        { 
            get => name??"No Name"; 
            set => SetProperty(ref name, value); 
        }
        public string Address 
        { 
            get => address??"00:00:00:00";
            set => SetProperty(ref address, value);
        }
    }
}
