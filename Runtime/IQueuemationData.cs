using System.Collections.Generic;

namespace QueuemationPackage
{  
    public enum QueuemationStatus
    {
        NOT_STARTED,
        IN_PROGRESS,
        PAUSED,
        COMPLETED,
        CANCELED,
    }


    public interface IQueuemationData
    {
        public string Name { get; }
        public QueuemationStatus Status { get; }
        public int ID { get; }
        public IEnumerable<IQueuemationData> Children { get; }
    }
}
