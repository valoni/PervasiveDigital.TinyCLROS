using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Diagnostics;
using PervasiveDigital.Hardware.ESP8266;
using PervasiveDigital.Net;
using PervasiveDigital;
using PervasiveDigital.Utilities;

using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Uart;
using GHIElectronics.TinyCLR.Pins;

namespace PervasiveDigital
{
    class Program
    {
        // Change these to match your ESP8266 configuration
        private static GpioPin _rfPower;
        private static GpioPin _userLed;

        // In order to create a truly robust long-running solution, you must combine the code below
        //   with proper use of a Watchdog Timer and exception handling on the wifi calls.
        //
        // Hardware note: It has been our experience that to work at 115200 with the ESP8266 and NETMF, you need a 1024 byte serial buffer.
        //   Smaller serial buffers may result in portions of incoming TCP traffic being dropped, which can also break the protocol processing
        //   and result in hangs.

        static void Main()
        {
            _rfPower = GpioController.GetDefault().OpenPin(5);
            _rfPower.SetDriveMode(GpioPinDriveMode.Output);
            _rfPower.Write(GpioPinValue.High);

            _userLed = GpioController.GetDefault().OpenPin(45);
            _userLed.SetDriveMode(GpioPinDriveMode.Output);

            var port = UartController.FromName(SC20100.UartPort.Uart8);

            port.SetActiveSettings(115200, 8, UartParity.None, UartStopBitCount.One,UartHandshake.None);

            var wifi = new Esp8266WifiDevice(port, _rfPower, null);

            // Enable echoing of commands
            wifi.EnableDebugOutput = false;


            Debug.WriteLine("--------------------------------");
            Debug.WriteLine("----Esp 8266 Wifi TinyClr 2 ----");
            Debug.WriteLine("--------------------------------");
            wifi.Connect("valoninet", "valoni1234567");


            Debug.WriteLine("Station IP address : " + wifi.StationIPAddress.ToString());
            Debug.WriteLine("Station MAC address : " + wifi.StationMacAddress);
            Debug.WriteLine("Station Gateway address : " + wifi.StationGateway.ToString());
            Debug.WriteLine("Station netmask : " + wifi.StationNetmask.ToString());
            Debug.WriteLine("------------------------");

            Thread.Sleep(1000);

            var sntp = new SntpClient(wifi, "time1.google.com");

            sntp.SetTime();

            int iCounter = 0;

            bool state = true;

            while (true)
            {
                state = !state;

                if (state)
                {
                    _userLed.Write(GpioPinValue.High);
                }
                else
                {
                    _userLed.Write(GpioPinValue.Low);
                }

                ++iCounter;
                if (iCounter % 10 == 0)
                {
                    Debug.WriteLine("Current UTC time : " + DateTime.UtcNow);
                    Debug.WriteLine("------------------------");
                }
                // Every 15 seconds

                Thread.Sleep(500);
            }


        }
    }
}
