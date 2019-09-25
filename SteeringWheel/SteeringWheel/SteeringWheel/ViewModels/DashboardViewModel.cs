using Plugin.BLE.Abstractions.Contracts;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SteeringWheel.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private IAdapter _btAdapter;
        private IDevice _btDevice;
        private ICharacteristic _characteristic;

        private string _direction;
        public string Direction
        {
            get { return _direction; }
            set
            {
                SetProperty(ref _direction, value);
                RaisePropertyChanged(nameof(LeftImageSrc));
                RaisePropertyChanged(nameof(RightImageSrc));
            }
        }


        private bool _buzzerStatus;
        public bool BuzzerStatus
        {
            get { return _buzzerStatus; }
            set
            {
                SetProperty(ref _buzzerStatus, value);
                RaisePropertyChanged(nameof(BuzzerImageSrc));
            }
        }

        public string LeftImageSrc
        {
            get
            {
                return Direction == "left" ? "on.png" : "off.png";
            }
        }

        public string BuzzerImageSrc
        {
            get
            {
                return BuzzerStatus ? "on.png" : "off.png";
            }
        }

        public string RightImageSrc
        {
            get
            {
                return Direction == "right" ? "on.png" : "off.png";
            }
        }

        public DashboardViewModel(IAdapter btAdapter)
        {
            _btAdapter = btAdapter;
            LeftCommand = new DelegateCommand(LeftCommandExecute);
            BuzzerCommand = new DelegateCommand(BuzzerCommandExecute);
            RightCommand = new DelegateCommand(RightCommandExecute);
        }

        private async void LeftCommandExecute()
        {
            Direction = "left";

            await SendDataAsync("c1", "1");

            await Task.Delay(100);

            await SendDataAsync("c3", "0");
        }

        private async void BuzzerCommandExecute()
        {
            BuzzerStatus = !BuzzerStatus;

            string value = "0";
            if (BuzzerStatus)
                value = "1";

            await SendDataAsync("c2", value);
        }

        private async void RightCommandExecute()
        {
            Direction = "right";

            await SendDataAsync("c3", "1");

            await Task.Delay(100);

            await SendDataAsync("c1", "0");
        }

        private async Task SendDataAsync(string componentCode, string value)
        {
            //Prefix
            string data = "#$";

            data += componentCode + value + "%";
            await _characteristic?.WriteAsync(Encoding.ASCII.GetBytes(data));
        }

        public ICommand LeftCommand { get; set; }
        public ICommand BuzzerCommand { get; set; }
        public ICommand RightCommand { get; set; }

        public async Task ConnectToDeviceAsync(IDevice bleDevice)
        {
            await _btAdapter.ConnectToDeviceAsync(bleDevice);
            var service = await bleDevice.GetServiceAsync(new Guid("0000ffe0-0000-1000-8000-00805f9b34fb"));

            if (service != null)
            {
                //Read Write Property
                var characteristic = await service.GetCharacteristicAsync(new Guid("0000ffe1-0000-1000-8000-00805f9b34fb"));

                if (characteristic != null)
                {
                    _characteristic = characteristic;
                }
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            _btDevice = parameters["Device"] as IDevice;
            ConnectToDeviceAsync(_btDevice);
        }
    }
}
