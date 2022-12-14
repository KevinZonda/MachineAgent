using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevinZonda.MachineAgent.ConsoleApp;

internal class ActionQueue
{
    private Queue<Action> _todo, _done;
    private Thread _t;
    private CancellationTokenSource _source;
    private CancellationToken _token;
    public int Interval { get; }

    public ActionQueue(int interval)
    {
        Interval = interval;
        // TODO: CTS
        _source = new();
        _token = _source.Token;
        _t = new Thread(new ThreadStart(() =>
        {
            while (true)
            {
                Produce();
                Thread.Sleep(Interval);
                ResetQueue();
            }

        }));
    }

    public void Produce()
    {
        while (_todo.Count > 0)
        {
            Action action = _todo.Dequeue();
            action.Invoke();
            _done.Enqueue(action);
        }
    }

    private void ResetQueue()
    {
        while (_done.Count > 0)
        {
            _todo.Enqueue(_done.Dequeue());
        }
    }

    private void Start()
    {
        if (IsRunning) return;
        _t.Start();
    }

    public bool IsRunning => _t.ThreadState == ThreadState.Running;


}
