using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using UdpSender_XamarinForms.Messages;
using UdpSender_XamarinForms.Model;

namespace UdpSender_XamarinForms
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        private string _remoteIPAddress;
        public string RemoteIPAddress
        {
            get => _remoteIPAddress;
            set
            {
                _remoteIPAddress = value;
                OnPropertyChanged();
            }
        }

        private string _remotePort;
        public string RemotePort
        {
            get => _remotePort;
            set
            {
                _remotePort = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = this;

            RemoteIPAddress = "192.168.121.25";
            RemotePort = "4602";

            MessagingCenter.Subscribe<TickedMessage>(this, nameof(TickedMessage), message =>
            {
                Message = message.Message;
            });
        }

        private async void Button_LongRunningTaskStart_Clicked(object sender, EventArgs e)
        {
            //位置情報取得の許可状態を確認
            var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            if (status != PermissionStatus.Granted)
            {
                //許可されていなかった場合はユーザーに確認する
                status = await Permissions.RequestAsync<Permissions.LocationAlways>();//Alwaysを承認してもらわないと困るんだな。バックグラウンドだから。
                //ユーザーが拒否した場合は(´・ω・`)
                if (status != PermissionStatus.Granted)
                    return;
            }

            RemoteHostInformation.IPAddress = RemoteIPAddress;
            RemoteHostInformation.Port = int.Parse(RemotePort);

            var message = new StartLongRunningTaskMessage();
            MessagingCenter.Send(message, nameof(StartLongRunningTaskMessage));
        }

        private void Button_LongRunningTaskStop_Clicked(object sender, EventArgs e)
        {
            var message = new StopLongRunningTaskMessage();
            MessagingCenter.Send(message, nameof(StopLongRunningTaskMessage));
        }
    }
}
