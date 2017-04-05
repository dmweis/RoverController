using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ServonController.Annotations;

namespace ServonController
{
   /// <summary>
   /// Interaction logic for ServoControl.xaml
   /// </summary>
   public partial class ServoControl : UserControl, INotifyPropertyChanged
   {
      private int _min;
      public int Min
      {
         get { return _min; }
         set
         {
            _min = value;
            OnPropertyChanged();
         }
      }

      private int _max;
      public int Max
      {
         get { return _max; }
         set
         {
            _max = value;
            OnPropertyChanged();
         }
      }

      private int _value;
      public int Value
      {
         get { return _value; }
         set
         {
            _value = value;
            OnPropertyChanged();
         }
      }

      private int _servoIndex;
      public int ServoIndex
      {
         get { return _servoIndex; }
         set
         {
            _servoIndex = value;
            OnPropertyChanged();
         }
      }

      private int _lastValue;

      public ServoControl()
      {
         Min = 150;
         Max = 600;
         Value = (600 - 150) / 2 + 150;
         InitializeComponent();
         DataContext = this;
      }

      public event EventHandler<int> NewServovalue; 

      public event PropertyChangedEventHandler PropertyChanged;

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }


      private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {
         if ( Math.Abs( _lastValue - Value) > 5)
         {
            _lastValue = Value;
            NewServovalue?.Invoke(this, Value);
         }
      }

      private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
      {
         _lastValue = Value;
         NewServovalue?.Invoke(this, Value);
      }
   }
}
