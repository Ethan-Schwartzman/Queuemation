using Cysharp.Threading.Tasks;

namespace QueuemationPackage
{    
    public interface IPlayable : IQueuemationData
    {
        public UniTask Play();
        public abstract Queuemation GetQueuemation();
    }
}
