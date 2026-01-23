using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QueuemationPackage
{   
    public class QueuedBatch : QueueContainer, IPlayable
    {
        public int Count { get => queuemations.Count; }
        private List<IPlayable> queuemations;

        public QueuedBatch() : base()
        {
            queuemations = new List<IPlayable>();
        }

        public QueuedBatch(string name) : base(name)
        {
            queuemations = new List<IPlayable>();
        }

        protected override void HandleAdd(IPlayable queuemation)
        {
            switch (Status)
            {
                case QueuemationStatus.IN_PROGRESS:
                    Debug.LogWarning($"Could not enqueue \"{queuemation.Name}\" to batch \"{Name}\" since it is already in progress.");
                    break;
                case QueuemationStatus.COMPLETED:
                    Debug.LogWarning($"Could not enqueue \"{queuemation.Name}\" to completed batch \"{Name}\".");
                    break;
                case QueuemationStatus.CANCELED:
                    Debug.LogWarning($"Could not enqueue \"{queuemation.Name}\" to cancelled batch \"{Name}\".");
                    break;
                default:
                    queuemations.Add(queuemation);
                    break;
            }
        }

        protected override async UniTask Run()
        {
            List<UniTask> tasks = queuemations
                .Select(queuemation => queuemation.Play())
                .ToList();
            await UniTask.WhenAll(tasks);
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
