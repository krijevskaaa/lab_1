using System;
using System.Linq;

enum ProcessStatus { ready, running, waiting, terminated }

class Process
{
    private long id;
    private string name;
    private long workTime;
    private Random rand;
    public Process(long pId, long addrSpace)
    {
        id = pId;
        name = "p" + id;
        AddrSpace = addrSpace;
        Status = ProcessStatus.ready;
    }

    public long BurstTime { get; set; }
    public ProcessStatus Status { get; set; }
    public long AddrSpace { get; }
    /*public Process(long pId, long addrSpace)
    {
        id = pId;
        name = "p" + id;
        AddrSpace = addrSpace;
        Status = ProcessStatus.ready;
    }*/

    public void IncreaseWorkTime()
    {
        if (workTime < BurstTime)
        {
            workTime++;
            return;
        }
        Status = rand.Next(0, 2) == 0 ? ProcessStatus.terminated : ProcessStatus.waiting;
    }

    public void ResetWorkTime()
    {
        workTime = 0;
    }
    public int CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }
        Process otheProcess = obj as Process;
        if (otheProcess != null)
        {
            if (BurstTime < otheProcess.BurstTime)
            {
                return 1;
            }
            return BurstTime < otheProcess.BurstTime ? 1 : 0;
        }
        else
        {
            throw new ArgumentException("Object is not a Process");
        }
    }
    public override string ToString()
    {
        return "Id: " + id + "; Burst time: " + BurstTime + "; Status: " + Status.ToString();
    }
}
class Resource
{
    public Process ActiveProcess
    {
        get;// { return activeProcess;}
        set;// { activeProcess = value; }
    }
    private Process activeProcess;

    public void WorkingCycle()
    {
        if (!IsFree())
        {
            activeProcess.IncreaseWorkTime();
        }
    }
    /*public void WorkingCycle()
    {
        if (ActiveProcess != null)
        {
            ActiveProcess.IncreaseWorkTime();
        }
    }*/
    /*[Pure]*/
    public bool IsFree()
    {
        return activeProcess == null;
    }

    public void CLear()
    {
        activeProcess = null;
    }
}
class Memory
{
    public long Size {get;private set;}
    public long FreeSize
    {
        get 
        {
            return Size - occupiedSize;
        }
        private set { }
    }

    private long occupiedSize;
    public long OccupiedSize
    {
        get {return occupiedSize;}
        set {occupiedSize = value;FreeSize = Size - occupiedSize;}
    }
    public void Save(long size)
    {
        Size = size;
        OccupiedSize = 0;
    }

    public void Clear()
    {
        FreeSize = 0;
        OccupiedSize = 0;
    }

    public static implicit operator Memory(long v)
    {
        throw new NotImplementedException();
    }
}
class SystemClock
{
    private long clock;
    public long Clock
    {
        get {return clock;}
        private set {clock = value;}
    }
    public void WorkingCycle()
    {
        clock++;
    }
    public void Clear()
    {
        clock = 0;
    }
}

class CPUScheduler
{
    private Resource resource;
    private IQueryable<Process> queue;
    public CPUScheduler(Resource resource, IQueryable<Process> queue)
    {
        this.resource = resource;
        this.queue = queue;
    }
    public IQueryable<Process> Session()
    {
        Process tmpProc = queue.Single();
        tmpProc.Status = ProcessStatus.running;
        resource.ActiveProcess = tmpProc;
        return queue;
    }
}
class DeviceScheduler
{
    private Resource resource;
    private IQueryable<Process> queue;
    public void CPUScheduler(Resource resource, IQueryable<Process> queue)
    {
        this.resource = resource;
        this.queue = queue;
    }
    public IQueryable<Process> Session()
    {
        Process Proc = queue.Single();
        resource.ActiveProcess = Proc;
        return queue;
    }

}
class MemoryManager //???
{
    private Memory memory;
    public void Save(long size)
    {
        memory =  size;
    }
    public Memory Allocate (Process process)
    {
        if ( Convert.ToInt32( process) <= memory.FreeSize)
        {
            memory.OccupiedSize += Convert.ToInt64( process);
            return memory;
        }
        return null;
    }
    public Memory Free(Process process)
    {
        memory.OccupiedSize -= process.BurstTime;
        return memory;
    }
}
public class Settings
{
        public double Intensity{get;set;}
        public int MinValueOfBurstTime {get;set;}
        public int MaxValueOfBurstTime {get;set;}
        public int MinValueOfAddrSpace {get;set;}
        public int MaxValueOfAddrSpace {get;set;}
        public int ValueOfRAMSize { get;set;}
    
}
class IdGenerator
{
    public long Id
    {
        get
        {
            return id == long.MaxValue ? 0 : ++id;
        }
    }
    public IdGenerator Clear()
    {
        if (this != null)
        {
            id = 0;
        }
        return this;
    }

    private long id;
}

class HelloWorld
{
    static void Main()
    {
        Process proc = new Process(125, 1000);
        proc.BurstTime = 100;
        Process procNew = new Process(150, 2000);
        procNew.BurstTime = 200;
        /*Console.WriteLine();
        Process proc_1 = proc;
        proc_1.BurstTime = 300;
        Console.WriteLine(proc_1.BurstTime);
        Console.WriteLine(proc.BurstTime);*/
        /*int i1 = 30, i2 = 20;
        i1.CompareTo(i2);
        Console.WriteLine(i1.CompareTo(i2));*/
        //proc.CompareTo(procNew);
        Console.WriteLine(proc.CompareTo(procNew));
    }
}


