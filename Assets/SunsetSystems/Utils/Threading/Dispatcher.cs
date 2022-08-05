using System;
using System.Collections.Generic;
using UnityEngine;

namespace SunsetSystems.Utils.Threading
{
    [RequireComponent(typeof(Tagger))]
    public class Dispatcher : Singleton<Dispatcher>
    {
        private readonly List<Action> pending = new();

        public void Invoke(Action fn)
        {
            lock (this.pending)
            {
                this.pending.Add(fn);
            }
        }

        private void InvokePending()
        {
            lock (this.pending)
            {
                foreach (Action action in this.pending)
                {
                    action();
                }

                this.pending.Clear();
            }
        }

        private void Update()
        {
            this.InvokePending();
        }
    }
}
