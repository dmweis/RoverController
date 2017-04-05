using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dweis.Rover
{
   public class SerialPortAddress : INotifyPropertyChanged
   {
      private string _name;

      public string Name
      {
         get { return _name; }
         set
         {
            _name = value;
            OnPropertyChanged();
         }
      }

      private string _description;

      public string Description
      {
         get { return _description; }
         set
         {
            _description = value;
            OnPropertyChanged();
         }
      }

      public SerialPortAddress(string name)
      {
         Name = name;
         Description = string.Empty;
      }

      public event PropertyChangedEventHandler PropertyChanged;

      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
   }
}
