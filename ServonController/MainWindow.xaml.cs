using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using dweis.Rover;
using ServonController.Annotations;

namespace ServonController
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window, INotifyPropertyChanged
   {
      private SerialPortAddress[] _availableSerialPorts;

      public SerialPortAddress[] AvailableSerialPorts
      {
         get { return _availableSerialPorts; }
         set
         {
            _availableSerialPorts = value;
            OnPropertyChanged();
         }
      }


      public SerialPortAddress SelectedPort { get; set; }

      private SerialPort _port;

      public MainWindow()
      {
         AvailableSerialPorts = SerialPortService.GetSerialPorts();
         InitializeComponent();
         DataContext = this;
      }

      private void ServoControl_OnNewServovalue(object sender, int value)
      {
         ServoControl control = (ServoControl)sender;
         string servoIndex = control.ServoIndex.ToString();
         _port?.Write($"{{0 {servoIndex} {value}}}");
         System.Diagnostics.Debug.WriteLine($"S0 {servoIndex} {value}E");
      }

      private void OnConnect(object sender, RoutedEventArgs e)
      {
         try
         {
            _port?.Close();
            _port?.Dispose();
         }
         catch (Exception )
         {
         }
         _port = new SerialPort(SelectedPort.Name);
         _port.Open();
         Task.Run(() =>
         {
            try
            {
               while (true)
               {
                  System.Diagnostics.Debug.WriteLine(_port.ReadLine());
               }
            }
            catch (Exception)
            {
            }
         });
      }

      private void OnRefresh(object sender, RoutedEventArgs e)
      {
         AvailableSerialPorts = SerialPortService.GetSerialPorts();

      }

      public event PropertyChangedEventHandler PropertyChanged;

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
   }
}
