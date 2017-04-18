using System;
using System.Linq;
using System.Reflection;

namespace ImplementingObserverPattern
{
    public class Listener
    {
        public Delegate CallBack { get; }

        public int Priority { get; }

        public bool stopPropagation { get; set; }

        private bool _once  { get; set; }

        private int _calls;

        public Listener(Delegate callBack, int priority = 0)
        {
            CallBack = callBack;
            Priority = priority;
            _once = false;
        }

        public void Handle(object[] args)
        {
            if(_once && _calls > 0) return;
            _calls++;
            var methodParameterNumber = CallBack.GetMethodInfo().GetParameters().Length;
            CallBack?.DynamicInvoke(args.Take(methodParameterNumber).ToArray());
        }

        public Listener Once()
        {
            _once = true;
            return this;
        }

        public Listener StopPropagation()
        {
            stopPropagation = true;
            return this;
        }
    }
}
