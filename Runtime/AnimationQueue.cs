using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QueuemationPackage
{
    public class AnimationQueue : Waitable, IPlayable
    {
        public int Count { get => queuemations.Count; }
        private Queue<IPlayable> queuemations;

        public AnimationQueue() : base()
        {
            queuemations = new Queue<IPlayable>();
        }

        public AnimationQueue(string name) : base(name)
        {
            queuemations = new Queue<IPlayable>();
        }
    
        protected override void HandleAdd(IPlayable queuemation)
        {
            switch (Status)
            {
                case QueuemationStatus.COMPLETED:
                    Debug.LogWarning($"Could not enqueue \"{queuemation.Name}\" to completed queue \"{Name}\".");
                    break;
                case QueuemationStatus.CANCELED:
                    Debug.LogWarning($"Could not enqueue \"{queuemation.Name}\" to cancelled queue \"{Name}\".");
                    break;
                default:
                    queuemations.Enqueue(queuemation);
                    break;
            }
        }

        protected override async UniTask Run()
        {
            while (queuemations.Count > 0)
            {
                controller.CT.ThrowIfCancellationRequested();
                await queuemations.Peek().Play();
                queuemations.Dequeue();
            }
        }

        protected override IEnumerable<IPlayable> GetChildren()
        {
            return queuemations;
        }

        public async UniTask Play()
        {
            await PlayInternal();
        }

        public Queuemation GetQueuemation()
        {
            return this;
        }
    }
}
