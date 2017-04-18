using System;
using System.Collections.Generic;

namespace ImplementingObserverPattern
{
    public class Emitter
    {

        #region Static Fields

        private static readonly object Padlock = new object();

        private static Emitter _instance;
        private Dictionary<string,List<Listener>> Listeners { get; set; }

        private Emitter()
        {
            Listeners = new Dictionary<string, List<Listener>>();
        }

        #endregion


        #region Public Properties

        public static Emitter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Emitter();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        public void Emit(string events, params object[] args)
        {
            if (HasListeners(events))
            {
                foreach (var x in Listeners[events])
                {
                    x.Handle(args);
                    if (x.stopPropagation)
                        break;
                }
            }
        }

        public Listener On(string events, Delegate callback,int priority = 0)
        {
            if (!HasListeners(events))
            {
                Listeners.Add(events, new List<Listener>());
            }
            CallBackExistForEvent(events, callback);
            var listener = new Listener(callback, priority);
            Listeners[events].Add(listener);
            SortListenersByPriority(events);
            return listener;
        }

        public Listener Once(string events, Delegate callback, int priority = 0)
        {
            return On(events, callback, priority).Once();
        }

        #endregion

        #region Private Methods

        private bool HasListeners(string events)
        {
            return Listeners.ContainsKey(events);
        }

        private void SortListenersByPriority(string events)
        {
            Listeners[events].Sort((x,y) => x.Priority.CompareTo(y.Priority));
        }

        private bool CallBackExistForEvent(string events, Delegate callback)
        {
            foreach (var x in Listeners[events])
            {
                if(x.CallBack == callback)
                    throw new Exception("DoubleEventException");
            }
            return false;
        }

        #endregion

    }
}
