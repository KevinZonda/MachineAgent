using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevinZonda.MachineAgent.ConsoleApp;

internal class ActionQueue
{
    private Queue<Action> _todo, _done;
    public int Interval { get; }

    public ActionQueue(int interval)
    {
        Interval = interval;
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

    private void Reset()
    {
        while (_done.Count > 0)
        {
            _todo.Enqueue(_done.Dequeue());
        }
    }

    private void Start()
    {
        while (true)
        {
            Produce();
            Thread.Sleep(Interval);
        }

    }


}
