using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QueuemationPackage
{
    public abstract class Queuemation : IQueuemationData
    { 
        public static event EventHandler QueueUpdates;
        public static Queuemation MonitoredQueuemation { get; private set; }
        private static int currentID = 0;
        public string Name { get; protected set; }
        public int ID { get; }
        public QueuemationStatus Status { get; protected set; }
        public IEnumerable<IQueuemationData> Children { get => GetChildren(); }
        protected QueuemationController controller;
        private CancellationTokenSource cts;
        protected abstract IEnumerable<IPlayable> GetChildren();
        protected abstract UniTask Run();

        public Queuemation()
        {
            Name = $"Queuemation {currentID}";
            Status = QueuemationStatus.NOT_STARTED;
            cts = new CancellationTokenSource();
            controller = new QueuemationController(cts.Token);
            ID = currentID;
            currentID++;
        }

        public Queuemation(string name)
        {
            Name = name;
            Status = QueuemationStatus.NOT_STARTED;
            cts = new CancellationTokenSource();
            controller = new QueuemationController(cts.Token);
            ID = currentID;
            currentID++;
        }

        public void Cancel()
        {
            SetCanceled();
            UpdateChildrenControllers();
        }

        private void SetCanceled()
        {
            cts.Cancel();
            Status = QueuemationStatus.CANCELED;
        }

        public void SetSpeed(float speed)
        {
            controller.LocalPlaybackSpeed = speed;
            UpdateChildrenControllers();
        }

        public void SetPaused(bool paused)
        {
            controller.Paused = paused;
            UpdateChildrenControllers();
        }

        private void UpdateChildrenControllers()
        {
            IEnumerable<IPlayable> children = GetChildren();

            if (children != null)
            {    
                foreach (IPlayable child in children)
                {
                    child.GetQueuemation().UpdateController(controller);
                }
            }
            
            TriggerUpdate();
        }

        protected void UpdateController(QueuemationController parent)
        {
            if (parent.CT.IsCancellationRequested) SetCanceled();
            controller = new QueuemationController(parent, controller);
            UpdateChildrenControllers();
        }

        public void Monitior()
        {
            MonitoredQueuemation = this;
            QueueUpdates?.Invoke(this, EventArgs.Empty);
        }

        protected void TriggerUpdate()
        {
            QueueUpdates?.Invoke(this, EventArgs.Empty);
        }

        protected async UniTask PlayInternal()
        {
            switch (Status)
            {
                case QueuemationStatus.IN_PROGRESS:
                    Debug.LogWarning($"Could not play queuemation \"{Name}\" since it is already in progress.");
                    break;
                case QueuemationStatus.COMPLETED:
                    Debug.LogWarning($"Could not play queuemation \"{Name}\" since it is has already completed.");
                    break;
                case QueuemationStatus.CANCELED:
                    Debug.LogWarning($"Could not play queuemation \"{Name}\" since it has been canceled.");
                    break;
                default:
                    Status = QueuemationStatus.IN_PROGRESS;
                    TriggerUpdate();
                    await Run();
                    Status = QueuemationStatus.COMPLETED;
                    TriggerUpdate();
                    break;
            }
        }
    }
}
