using Labo06;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        PaintA,
    }

    public class ThreadMachine
    {

        private static readonly Lazy<ThreadMachine> _instance = new Lazy<ThreadMachine>(() => new ThreadMachine());
        private Thread _thread;
        const string ipaddress = "127.0.0.1";
        const int port = 9999;
        private ObservableCollection<Batch> _ShareBatches = new ObservableCollection<Batch>();
        private ThreadMachine()
        {
            _thread = new Thread(Run);
            _thread.Start();
            
        }
        private void Run() {
            MachineState machineState = MachineState.waiting;
            MachinePainting machinePainting = new MachinePainting(ipaddress, port);


            while (StopThread)
            {
                switch (machineState) {
                    case MachineState.waiting:
                        if (Start == true)
                        {
                            IndexBatch = 0;
                            NumberBucket = 0;
                            machineState = MachineState.askbucket;
                            Start = false;
                        }

                        break;

                    case MachineState.askbucket:
                        machinePainting.ConveyorOn = true;
                        machineState = MachineState.waitingBucket;
                        break;

                    case MachineState.waitingBucket:
                        if (machinePainting.BucketLocked)
                        {
                            BucketPresent = true;
                            machineState = MachineState.PaintA;
                        }
                        break;

                    case MachineState.PaintA:
                        if (Baches.Count > 1)
                        {

                            dispensePaint(PigmentType.A, (int)(Baches[IndexBatch].Recipe.PigmentA * 100), machinePainting);
                            dispensePaint(PigmentType.B, (int)(Baches[IndexBatch].Recipe.PigmentB * 100), machinePainting);
                            dispensePaint(PigmentType.C, (int)(Baches[IndexBatch].Recipe.PigmentC * 100), machinePainting);
                            dispensePaint(PigmentType.D, (int)(Baches[IndexBatch].Recipe.PigmentD * 100), machinePainting);
                            if (NumberBucket < Baches[0].BucketCount - 1)
                            {
                                NumberBucket++;
                                NumberMadeBucket++;
                                machineState = MachineState.askbucket;
                            }
                            else
                            {
                                IndexBatch++;
                                NumberMadeBucket++;
                                if (IndexBatch < Baches.Count)
                                {
                                    machineState = MachineState.askbucket;
                                }
                                else
                                {
                                    machineState = MachineState.waiting;
                                }

                            }
                        }
                        else
                        {
                            machineState = MachineState.waiting;
                        }
                        break;


                }
                Thread.Sleep(100);
                Connected = machinePainting.Connected;
            }
        }

        private void dispensePaint(PigmentType type, int sleepDelay, MachinePainting machinePainting)
        {
            if (sleepDelay > 0)
            {
                machinePainting.PigmentDispenced = type;
                Thread.Sleep(sleepDelay);
                machinePainting.PigmentDispenced = PigmentType.None;
            }
        }

        private readonly object _lockIndexBatch = new object();
        private int _indexBatch;
        public int IndexBatch
        {
            get
            {
                lock (_lockIndexBatch)
                {
                    return _indexBatch;
                }
            }
            set
            {
                lock (_lockIndexBatch)
                {
                    _indexBatch = value;
                }
            }
        }


        private readonly object _lockNumberBucket = new object();
        private int _numberBucket;
        public int NumberBucket
        {
            get
            {
                lock (_lockNumberBucket)
                {
                    return _numberBucket;
                }
            }
            set
            {
                lock (_lockNumberBucket)
                {
                    _numberBucket = value;
                }
            }
        }

        private readonly object _lockNumberMadeBucket = new object();
        private int _numberMadeBucket;

        public int NumberMadeBucket
        {
            get
            {
                lock (_lockNumberMadeBucket)
                {
                    return _numberMadeBucket;
                }
            }
            set
            {
                lock (_lockNumberMadeBucket)
                {
                    _numberMadeBucket = value;
                }
            }
        }

        private readonly object _lockStop = new object();
        private bool _stopThread = true;
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

        private readonly object _lockBaches = new object();

        public ObservableCollection<Batch> Baches
        {
            get
            {
                lock (_lockBaches)
                {
                    return _ShareBatches;
                }
            }
            set
            {
                lock (_lockBaches)
                {
                    _ShareBatches = value;
                }
            }
        }

        public int TotalBucketBatch
        {
            get {
                int total = 0;
                foreach (var batch in Baches)
                {
                    total += batch.BucketCount;
                }
                return total;
            }
        }


        public static ThreadMachine Instance => _instance.Value;


        ~ThreadMachine(){
            StopThread = false;
        }
    }
}
