using System;
using Cysharp.Threading.Tasks;
using TaskGen = System.Func<QueuemationPackage.QueuemationController, Cysharp.Threading.Tasks.UniTask>;

namespace QueuemationPackage
{
    public static class BasicUtils
    {
        public static TaskGen RunAction(Action action)
        {
            return (QueuemationController controller) =>
            {
                action?.Invoke();
                return UniTask.CompletedTask;
            };
        }
    }
}
