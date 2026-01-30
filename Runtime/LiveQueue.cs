using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QueuemationPackage
{
    public class LiveQueue : Waitable
    {
        private Queue<IPlayable> queuemations;
        private bool live = false;

        public LiveQueue() : base()
        {
            queuemations = new Queue<IPlayable>();
        }

        public LiveQueue(string name) : base(name) 
        { 
            queuemations = new Queue<IPlayable>();
        }

        protected override IEnumerable<IPlayable> GetChildren()
        {
            return queuemations;
        }

        protected override void HandleAdd(IPlayable queuemation)
        {
            switch (Status)
            {
                case QueuemationStatus.CANCELED:
                    Debug.LogWarning($"Could not enqueue \"{queuemation.Name}\" to cancelled queue \"{Name}\".");
                    break;
                default:
                    queuemations.Enqueue(queuemation);
                    if (!live) RunLive().Forget();
                    break;
            }
        }

        private async UniTask RunLive()
        {
            live = true;

            while (queuemations.Count > 0)
            {
                controller.CT.ThrowIfCancellationRequested();
                await queuemations.Peek().Play();
                queuemations.Dequeue();
            }

            live = false;
        }
        

        protected override UniTask Run() 
        { 
            return new UniTask();
        }
    }
}
