using System.Threading;

namespace QueuemationPackage
{
    public struct QueuemationController
    {
        public float PlaybackSpeed
        {
            get => Paused ? 0 : LocalPlaybackSpeed * parentCumulativePlaybackSpeed;
            private set => LocalPlaybackSpeed = value;
        }
        public float LocalPlaybackSpeed { get; set; }
        public CancellationToken CT { get; }
        public bool Paused { get; set; }
        private float parentCumulativePlaybackSpeed;

        public QueuemationController(CancellationToken cancellationToken)
        {
            LocalPlaybackSpeed = 1.0f;
            parentCumulativePlaybackSpeed = 1.0f;
            Paused = false;
            CT = cancellationToken;
        }

        public QueuemationController(QueuemationController parent, QueuemationController child)
        {
            LocalPlaybackSpeed = child.LocalPlaybackSpeed;
            parentCumulativePlaybackSpeed = parent.PlaybackSpeed;
            Paused = child.Paused;
            CT = child.CT;
        }
    }
}
