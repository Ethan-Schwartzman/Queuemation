namespace QueuemationPackage
{    
    public abstract class Waitable : QueueContainer
    {
        public Waitable() : base() { }
        public Waitable(string name) : base(name) { }

        public void AddWait(float duration)
        {
            IPlayable queuemation = new QueuedTask($"Wait for {duration}s", BasicAnimations.Wait(duration));
            HandleAdd(queuemation);
        }
    }
}
