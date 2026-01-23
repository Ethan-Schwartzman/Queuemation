using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TaskGen = System.Func<QueuemationPackage.QueuemationController, Cysharp.Threading.Tasks.UniTask>;

namespace QueuemationPackage
{
    public static class BasicAnimations
    {
        private static async UniTask WaitAnimation(float duration, QueuemationController controller)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                controller.CT.ThrowIfCancellationRequested();
                elapsed += controller.Paused ? 0 : Time.deltaTime * controller.PlaybackSpeed;
                await UniTask.Yield(PlayerLoopTiming.Update, controller.CT);
            }
        }

        public static TaskGen Wait(float duration)
        {
            return c => WaitAnimation(duration, c);
        } 

        private static async UniTask InterpolateAnimation(Action preAction, Action<float> tweenAction, Action postAction, float duration, QueuemationController controller)
        {
            float elapsed = 0f;
            preAction?.Invoke();

            while (elapsed < duration)
            {
                controller.CT.ThrowIfCancellationRequested();
                elapsed += controller.Paused ? 0 : Time.deltaTime * controller.PlaybackSpeed;
                float t = Mathf.Clamp01(elapsed / duration);
                tweenAction?.Invoke(t);
                await UniTask.Yield(PlayerLoopTiming.Update, controller.CT);
            }

            postAction?.Invoke();
        }

        private static async UniTask UpdateAnimation(Action preAction, Func<float, bool> updateFunc, Action postAction, QueuemationController controller)
        {
            bool completed = false;
            preAction?.Invoke();

            while (!completed)
            {
                controller.CT.ThrowIfCancellationRequested();
                float dt = controller.Paused ? 0 : Time.deltaTime * controller.PlaybackSpeed;
                completed = updateFunc.Invoke(dt);
                await UniTask.Yield(PlayerLoopTiming.Update, controller.CT);
            }

            postAction?.Invoke();
        }

        public static TaskGen Interpolate(Action preAction, Action<float> tweenAction, Action postAction, float duration)
        {
            return c => InterpolateAnimation(preAction, tweenAction, postAction, duration, c);
        }

        public static TaskGen Interpolate(Action<float> tweenAction, float duration)
        {
            return c => InterpolateAnimation(null, tweenAction, null, duration, c);
        }

        public static TaskGen Update(Action preAction, Func<float, bool> updateFunc, Action postAction)
        {
            return c => UpdateAnimation(preAction, updateFunc, postAction, c);
        }

        public static TaskGen Update(Func<float, bool> updateFunc)
        {
            return c => UpdateAnimation(null, updateFunc, null, c);
        }
    }
}
