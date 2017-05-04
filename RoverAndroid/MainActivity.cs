using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Bluetooth;
using Android.Widget;
using Android.OS;
using Android.Text.Method;
using Android.Util;
using dweis.Rover.Controller;
using Java.IO;
using Java.Util;
using Console = System.Console;

namespace RoverAndroid
{
   [Activity(Label = "RoverAndroid", MainLauncher = true, Icon = "@drawable/icon")]
   public class MainActivity : Activity
   {
      private Button _buttonRotateAnti;
      private Button _buttonForward;
      private Button _buttonSteerLeft;
      private Button _buttonSteerRight;
      private Button _buttonRotateClockwise;
      private Button _buttonStrafeLeft;
      private Button _buttonStop;
      private Button _buttonStrafeRight;
      private Button _buttonBackwards;
      private Button _buttonSteerLeftBack;
      private Button _buttonSteerRightBack;

      private TextView _textView;

      private RoverConnector _roverConnector;
      private Rover _rover;

      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);
         // Set our view from the "main" layout resource
         SetContentView (Resource.Layout.Main);
         using (var stream = Assets.Open("config.json"))
         {
            using (var reader = new StreamReader(stream))
            {
               _rover = Rover.FromJson(reader.ReadToEnd());
            }
         }
         _buttonRotateAnti = FindViewById<Button>(Resource.Id.buttonRotAnti);
         _buttonForward = FindViewById<Button>(Resource.Id.buttonForward);
         _buttonSteerLeft = FindViewById<Button>(Resource.Id.buttonSteerLeft);
         _buttonSteerRight = FindViewById<Button>(Resource.Id.buttonSteerRight);
         _buttonRotateClockwise = FindViewById<Button>(Resource.Id.buttonRotateClock);
         _buttonStrafeLeft = FindViewById<Button>(Resource.Id.buttonStrafeLeft);
         _buttonStop = FindViewById<Button>(Resource.Id.buttonStop);
         _buttonStrafeRight = FindViewById<Button>(Resource.Id.buttonStrafeRight);
         _buttonBackwards = FindViewById<Button>(Resource.Id.buttonBackwards);
         _textView = FindViewById<TextView>(Resource.Id.textBox);
         _textView.MovementMethod = new ScrollingMovementMethod();
         _buttonSteerLeftBack = FindViewById<Button>(Resource.Id.buttonSteerLeftBack);
         _buttonSteerRightBack = FindViewById<Button>(Resource.Id.buttonSteerRightBack);

         _textView.Text = "Incoming:\n";

         _roverConnector = new RoverConnector();
         _rover.NewMessage += (sender, message) => _roverConnector.Enqueue(message);

         _roverConnector.NewMessage += (sender, message) => RunOnUiThread( () => _textView.Text += message);
         _buttonRotateAnti.Click += (sender, args) => _rover.RotateCounterClockwise();
         _buttonForward.Click += (sender, args) =>
         {
            _rover.TurnServosStraight();
            _rover.FullForward();
         };
         _buttonSteerLeft.Click += (sender, args) =>
         {
            _rover.TurnSlightlyLeft();
            _rover.FullForward();
         };
         _buttonSteerRight.Click += (sender, args) =>
         {
            _rover.TurnSlightlyRight();
            _rover.FullForward();
         };
         _buttonRotateClockwise.Click += (sender, args) => _rover.RotateClockwise();
         _buttonStrafeLeft.Click += (sender, args) => _rover.ParallelLeft();
         _buttonStop.Click += (sender, args) => _rover.StopMotors();
         _buttonStrafeRight.Click += (sender, args) => _rover.ParallelRight();
         _buttonBackwards.Click += (sender, args) =>
         {
            _rover.TurnServosStraight();
            _rover.FullBackwards();
         };
         _buttonSteerLeftBack.Click += (sender, args) =>
         {
            _rover.TurnSlightlyLeft();
            _rover.FullBackwards();
         };
         _buttonSteerRightBack.Click += (sender, args) =>
         {
            _rover.TurnSlightlyRight();
            _rover.FullBackwards();
         };
      }

   }

   class RoverConnector
   {
      private Thread _btThread;
      private Thread _readerThread;
      private readonly BlockingCollection<string> _messageBuffer;

      public event EventHandler<string> NewMessage;

      public RoverConnector()
      {
         _messageBuffer = new BlockingCollection<string>();
         _btThread = new Thread(BtComm);
         _btThread.Start();
      }

      public void Enqueue(string message)
      {
         _messageBuffer.Add(message);
      }

      private void BtComm()
      {
         BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
         string address = adapter.BondedDevices.FirstOrDefault(device => device.Name.Contains("Rover1"))?.Address;
         try
         {
            BluetoothDevice device = adapter.GetRemoteDevice(address);
            adapter.CancelDiscovery();
            BluetoothSocket socket =
               device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
            socket.Connect();
            Stream output = socket.OutputStream;
            _readerThread = new Thread(() => ReaderLoop(socket.InputStream));
            _readerThread.Start();
            foreach (string message in _messageBuffer.GetConsumingEnumerable())
            {
               byte[] data = Encoding.ASCII.GetBytes(message);
               output.Write(data, 0, data.Length);
               output.Flush();
            }
            _readerThread.Abort();
            Thread.Sleep(2000);
            output.Close();
            socket.Close();
            socket.Dispose();
            device.Dispose();
         }
         catch (Exception e)
         {
            Log.Error("RoverController", $"Exception {e}");
         }

      }

      private void ReaderLoop(Stream input)
      {
         try
         {
            while (true)
            {
               byte[] incomingData = new byte[128];
               int incoming = input.Read(incomingData, 0, incomingData.Length);
               string incomingMessage = Encoding.ASCII.GetString(incomingData);
               NewMessage?.Invoke(this, incomingMessage);
            }
         }
         catch
         {
            input.Dispose();
         }
      }
   }
}

