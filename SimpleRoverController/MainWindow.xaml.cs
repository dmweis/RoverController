using System;
using System.Collections.Generic;
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
      private Rover _rover;

      public MainWindow()
      {
         InitializeComponent();
         foreach (var port in SerialPortService.GetSerialPorts())
         {
            SelectorBox.Items.Add(port);
         }
      }

      private void Button_Turn90(object sender, RoutedEventArgs e)
      {
         _rover?.TurnServos90();
      }

      private void Button_Turn180(object sender, RoutedEventArgs e)
      {
         _rover?.TurnServos180();
      }

      private void Button_Forward(object sender, RoutedEventArgs e)
      {
         _rover?.FullForward();
      }

      private void Button_STOP(object sender, RoutedEventArgs e)
      {
         _rover?.StopMotors();
      }

      private void Button_Backwards(object sender, RoutedEventArgs e)
      {
         _rover?.FullBackwards();
      }

      private void Button_ParallelLeft(object sender, RoutedEventArgs e)
      {
         _rover?.ParallelLeft();
      }

      private void Button_ParallelRight(object sender, RoutedEventArgs e)
      {
         _rover?.ParallelRight();
      }

      private void SelectorBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         SerialPortAddress address = e.AddedItems[0] as SerialPortAddress;
         _rover?.Close();
         _rover = new Rover(address);
      }
   }
}
