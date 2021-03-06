using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using UdpSender_XamarinForms.Messages;
using UdpSender_XamarinForms.Model;
using DataModelFromPhone;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace UdpSender_XamarinForms.Tasks
{
    public class TaskCounter
    {
        public async Task RunCounter(CancellationToken token)
        {
            var udpSender = new UDPSender(RemoteHostInformation.IPAddress, RemoteHostInformation.Port);

            //GPSの精度をHighに
            //2つめの引数で取得間隔を設定できる。timeoutってなってるけど。
            var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(1));

            await Task.Run(async () =>
            {
                for (long i = 0; i < long.MaxValue; i++)
                {
                    token.ThrowIfCancellationRequested();


                    //ここから
                    var location = await Geolocation.GetLocationAsync(request);

                    var locationInfo = new GeolocationInfo
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Altitude = location.Altitude
                    };

                    var serializeOption = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                        WriteIndented = true,
                    };

                    string locationInfoJson = JsonSerializer.Serialize(locationInfo, serializeOption);

                    var message = new TickedMessage
                    {
                        //Message = $"Count : {i.ToString()}, Lat = {location.Latitude}, Lon = {location.Longitude}, Alt={location.Altitude}"
                        Message = locationInfoJson
                    };
                    //ここまで

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send<TickedMessage>(message, nameof(TickedMessage));
                    });

                    udpSender.Send(message.Message);
                }
            }, token);
        }
    }
}
