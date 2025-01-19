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
        PaintB,
        PaintC,
        PaintD,
        Stoping,
        Empting
    }
    public enum RunningState
    {
        Empty,
        Run,
        Pause,
        Stop,
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
            int _paintAcounter = 0 , _paintBcounter = 0, _paintCcounter = 0, _paintDcounter = 0,_emptyTimer = 0;
            const int _emptyConst = 500;


            while (StopThread)
            { 
                if (RunState == RunningState.Stop)
                {
                    machineState = MachineState.waiting;
                }
                if (RunState != RunningState.Pause)
                {
                    switch (machineState)
                    {
                        case MachineState.waiting:
                            machinePainting.PigmentDispenced = PigmentType.None;
                            machinePainting.ConveyorOn=false;
                            if (RunState == RunningState.Run && Baches.Count > 0)
                            {
                                IndexBatch = 0;
                                NumberBucket = 0;
                                machineState = MachineState.askbucket;
                                RunState = RunningState.Run;
                            }
                            if (RunState == RunningState.Empty)
                            {
                                machineState = MachineState.Empting;
                            }
                            break;

                        case MachineState.askbucket:
                            _paintAcounter = 0;
                            _paintBcounter = 0;
                            _paintCcounter = 0;
                            _paintDcounter = 0;
                            machinePainting.BucketsLoadingEnabled = true;
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
                            if (dispensePaint(PigmentType.A, (int)(Baches[IndexBatch].Recipe.PigmentA), ref _paintAcounter, machinePainting))
                            {
                                machineState = MachineState.PaintB;
                            }
                            break;

                        case MachineState.PaintB:
                            if (dispensePaint(PigmentType.B, (int)(Baches[IndexBatch].Recipe.PigmentB), ref _paintBcounter, machinePainting))
                            {
                                machineState = MachineState.PaintC;
                            }
                            break;

                        case MachineState.PaintC:
                            if (dispensePaint(PigmentType.C, (int)(Baches[IndexBatch].Recipe.PigmentC), ref _paintCcounter, machinePainting))
                            {
                                machineState = MachineState.PaintD;
                            }
                            break ;

                        case MachineState.PaintD:
                            if (dispensePaint(PigmentType.D, (int)(Baches[IndexBatch].Recipe.PigmentD), ref _paintDcounter, machinePainting))
                            {
                                if (Baches.Count > 0)
                                {

                                    if (NumberBucket < Baches[IndexBatch].BucketCount - 1)
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
                                            machineState = MachineState.Stoping;
                                        }
                                    }
                                }
                                else
                                {
                                    machineState = MachineState.Stoping;
                                }
                            }
                            break;

                        case MachineState.Stoping:
                            RunState = RunningState.Stop;
                            machineState = MachineState.waiting;
                            break;

                        case MachineState.Empting:
                            _emptyTimer++;
                            machinePainting.BucketsLoadingEnabled = false;
                            machinePainting.ConveyorOn = true;
                            if (_emptyTimer > _emptyConst)
                            {
                                _emptyTimer = 0;
                                machinePainting.ConveyorOn=false;
                                machineState = MachineState.Stoping;
                            }
                            break;
                    }
                }
                else
                {
                    machinePainting.PigmentDispenced = PigmentType.None;
                }
                Thread.Sleep(100);
                Connected = machinePainting.Connected;
            }
        }

        private bool dispensePaint(PigmentType type,int CountTarget, ref int Counter, MachinePainting machinePainting)
        {
            if (Baches.Count > 0)
            {
                machinePainting.PigmentDispenced = type;
                Counter++;
                if (Counter >= CountTarget)
                {
                    machinePainting.PigmentDispenced = PigmentType.None;
                    return true;
                }
                return false;
            }
            return true;
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
        private readonly object _lockRunState = new object();
        private RunningState _runState;
        public RunningState RunState
        {
            get
            {
                lock (_lockRunState)
                {
                    return _runState;
                }
            }
            set
            {
                lock (_lockRunState)
                {
                    _runState = value;
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
