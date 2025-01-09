using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Labo08_Chevalley_Michaud
{
    public enum MachineState
{
        waiting,
        waitingBucket,
        askbucket,
}

    public class ThreadMachine
    {

        private static readonly Lazy<ThreadMachine> _instance = new Lazy<ThreadMachine>(() => new ThreadMachine());
        private Thread _thread;
        const string ipaddress = "127.0.0.1";
        const int port = 9999;

        private ThreadMachine()
        {
            _thread = new Thread(Run);
            _thread.Start();
        }
        private void Run() {
            MachineState machineState = MachineState.waitingBucket;
            MachinePainting machinePainting = new MachinePainting(ipaddress, port);

            while (StopThread)
            {
                switch (machineState){
                    case MachineState.waiting:
                        if(Start==true)
                            machineState = MachineState.askbucket;
                            Start = false;
                        break;
                    case MachineState.waitingBucket:
                        if(machinePainting.BucketLocked)
                            BucketPresent = true;
                            machineState = MachineState.waiting;
                        break;

                    case MachineState.askbucket:
                        machinePainting.ConveyorOn=true;
                        machineState=MachineState.waitingBucket;
                        break;
                }
                Thread.Sleep(100);
                Connected = machinePainting.Connected;
                
            }

        }

        private readonly object _lockStop = new object();
        private  bool _stopThread = true;
        public bool StopThread
        {
            get
            {
                lock (_lockStop)
                {
                    return _stopThread;
                }
            }
            set
            {
                lock (_lockStop)
                {
                    _stopThread = value;
                }
                if (value == false)
                {
                    _stopThread = false;
                    _thread.Join();
                }
            }
        }
        private readonly object _lockConnect = new object();
        private bool _connected;
        public bool Connected
        {
            get
            {
                lock (_lockConnect)
                {
                    return _connected;
                }
            }
            set
            {
                lock (_lockConnect)
                {
                    _connected = value;
                }

            }
        }

        private readonly object _lockBucketPresent = new object();
        private bool _bucketPresent;
        public bool BucketPresent
        {
            get
            {
                lock (_lockBucketPresent)
                {
                    return _bucketPresent;
                }
            }
            set
            {
                lock (_lockBucketPresent)
                {
                    _bucketPresent = value;
                }
            }
        }

        private readonly object _lockStart = new object();
        private bool _start;
        public bool Start
        {
            get
            {
                lock (_lockStart)
                {
                    return _start;
                }
            }
            set
            {
                lock (_lockStart)
                {
                    _start = value;
                }
            }
        }

        
        public static ThreadMachine Instance => _instance.Value;


        ~ThreadMachine(){
            StopThread = false;
        }
    }
}
