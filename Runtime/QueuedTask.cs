using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TaskGen = System.Func<QueuemationPackage.QueuemationController, Cysharp.Threading.Tasks.UniTask>;

namespace QueuemationPackage
{
    public class QueuedTask : Queuemation, IPlayable
    {
        private TaskGen taskGen;

        public QueuedTask(TaskGen taskGen) : base()
        {
            this.taskGen = taskGen;
        }

        public QueuedTask(string name, TaskGen taskGen) : base(name)
        {
            this.taskGen = taskGen;
        }

        protected override IEnumerable<IPlayable> GetChildren()
        {
            return null;
        }

        public async UniTask Play()
        {
            await PlayInternal();
        }

        protected override async UniTask Run()
        {
            await taskGen(controller);
        }

        public Queuemation GetQueuemation()
        {
            return this;
        }
    }
}
