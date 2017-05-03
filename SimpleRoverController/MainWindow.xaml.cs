using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using dweis.Rover;
using dweis.Rover.Connector;

namespace SimpleRoverController
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private IRoverConnector _roverConnector;
      private string _configJson;

      public MainWindow()
      {
         InitializeComponent();
         _configJson = File.ReadAllText("config.json");
         foreach (var port in SerialPortService.GetSerialPorts())
         {
            SelectorBox.Items.Add(port);
         }
      }

      private void Button_Turn90(object sender, RoutedEventArgs e)
      {
         _roverConnector?.TurnServos90();
      }

      private void Button_Turn180(object sender, RoutedEventArgs e)
      {
         _roverConnector?.TurnServos180();
      }

      private void Button_Forward(object sender, RoutedEventArgs e)
      {
         _roverConnector?.TurnServos90();
         _roverConnector?.FullForward();
      }

      private void Button_STOP(object sender, RoutedEventArgs e)
      {
         _roverConnector?.StopMotors();
      }

      private void Button_Backwards(object sender, RoutedEventArgs e)
      {
         _roverConnector?.TurnServos90();
         _roverConnector?.FullBackwards();
      }

      private void Button_ParallelLeft(object sender, RoutedEventArgs e)
      {
         _roverConnector?.TurnServos180();
         _roverConnector?.ParallelLeft();
      }

      private void Button_ParallelRight(object sender, RoutedEventArgs e)
      {
         _roverConnector?.TurnServos180();
         _roverConnector?.ParallelRight();
      }

      private async void SelectorBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         SerialPortAddress address = e.AddedItems[0] as SerialPortAddress;
         _roverConnector?.Close();
         //_roverConnector = await Task.Run(() => new RoverConnector(address));
         _roverConnector = await Task.Run(() => new NewRoverConnector(address, _configJson));
      }

      private void ButtonSetServos(object sender, RoutedEventArgs e)
      {
         //_roverConnector?.SetServo(Motors.LEFT_FRONT_SERVO, (float)LFSlider.Value);
         //_roverConnector?.SetServo(Motors.RIGHT_FRONT_SERVO, (float)RFSlider.Value);
         //_roverConnector?.SetServo(Motors.LEFT_REAR_SERVO, (float)LRSlider.Value);
         //_roverConnector?.SetServo(Motors.RIGHT_REAR_SERVO, (float)RRSlider.Value);
      }

      private void ButtonCrossLegged(object sender, RoutedEventArgs e)
      {
         _roverConnector?.TurnCrossLegs();
      }

      private void ButtonRotateCounter(object sender, RoutedEventArgs e)
      {
         _roverConnector?.TurnCrossLegs();
         _roverConnector?.RotateCounterClockwise();
      }

      private void ButtonRotateClockwise(object sender, RoutedEventArgs e)
      {
         _roverConnector?.TurnCrossLegs();
         _roverConnector?.RotateClockwise();
      }
   }
}
