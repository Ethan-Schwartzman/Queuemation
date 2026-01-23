using System;

namespace QueuemationPackage
{    
    public abstract class QueueContainer : Queuemation
    {
        public QueueContainer() : base() { }
        public QueueContainer(string name) : base(name) { }

        protected abstract void HandleAdd(IPlayable queuemation);

        /// <summary>
        /// Adds a function to the queuemation.
        /// </summary>
        /// <param name="action">The function to be executed</param>
        public void Add(Action action)
        {
            IPlayable queuemation = new QueuedTask(BasicUtils.RunAction(action));
            HandleAdd(queuemation);
        }

        /// <summary>
        /// Adds a function to the queuemation.
        /// </summary>
        /// <param name="name">The name of the queuemation</param>
        /// <param name="action">The function to be executed</param>
        public void Add(string name, Action action)
        {
            IPlayable queuemation = new QueuedTask(name, BasicUtils.RunAction(action));
            HandleAdd(queuemation);
        }

        /// <summary>
        /// Adds a function with parameter dt (delta time) to be executed every frame until it returns true. 
        /// </summary>
        /// <param name="update">A function with float dt that returns true when completed</param>
        public void Add(Func<float, bool> update)
        {
            IPlayable queuemation = new QueuedTask(BasicAnimations.Update(update));
            HandleAdd(queuemation);
        }

        /// <summary>
        /// Adds a function with parameter dt (delta time) to be executed every frame until it returns true. 
        /// </summary>
        /// <param name="name">The name of the queuemation</param>
        /// <param name="update">A function with float dt that returns true when completed</param>
        public void Add(string name, Func<float, bool> update)
        {
            IPlayable queuemation = new QueuedTask(name, BasicAnimations.Update(update));
            HandleAdd(queuemation);
        }

        /// <summary>
        /// Adds a function with parameter dt (delta time) to be executed every frame until it returns true. 
        /// </summary>
        /// <param name="pre">The function to be executed at the start of the queuemation</param>
        /// <param name="update">A function to be executed every frame with float dt that returns true when completed</param>
        /// <param name="post">The function to be executed at the end of the queuemation</param>
        public void Add(Action pre, Func<float, bool> update, Action post)
        {
            IPlayable queuemation = new QueuedTask(BasicAnimations.Update(pre, update, post));
            HandleAdd(queuemation);
        }

        /// <summary>
        /// Adds a function with parameter dt (delta time) to be executed every frame until it returns true. 
        /// </summary>
        /// <param name="name">The name of the queuemation</param>
        /// <param name="pre">The function to be executed at the start of the queuemation</param>
        /// <param name="update">A function to be executed every frame with float dt that returns true when completed</param>
        /// <param name="post">The function to be executed at the end of the queuemation</param>
        public void Add(string name, Action pre, Func<float, bool> update, Action post)
        {
            IPlayable queuemation = new QueuedTask(name, BasicAnimations.Update(pre, update, post));
            HandleAdd(queuemation);
        }

        /// <summary>
        /// Adds a function with parameter t which increases from 0 to 1 thoughout the duration of the queuemation. 
        /// This function will be executed every frame for the duration.
        /// </summary>
        /// <param name="tween">The function to be executed, with parameter t increasing from 0 to 1</param>
        /// <param name="duration">The duration of the queuemation</param>
        public void Add(Action<float> tween, float duration)
        {
            IPlayable queuemation = new QueuedTask(BasicAnimations.Interpolate(tween, duration));
            HandleAdd(queuemation);
        }

        /// <summary>
        /// Adds a function with parameter t which increases from 0 to 1 thoughout the duration of the queuemation. 
        /// This function will be executed every frame for the duration.
        /// </summary>
        /// <param name="name">The name of the queuemation</param>
        /// <param name="tween">The function to be executed, with parameter t increasing from 0 to 1</param>
        /// <param name="duration">The duration of the queuemation</param>
        public void Add(string name, Action<float> tween, float duration)
        {
            IPlayable queuemation = new QueuedTask(name, BasicAnimations.Interpolate(tween, duration));
            HandleAdd(queuemation);
        }

        /// <summary>
        /// Adds a function with parameter t which increases from 0 to 1 thoughout the duration of the queuemation. 
        /// This function will be executed every frame for the duration.
        /// </summary>
        /// <param name="pre">The function to be executed at the start of the queuemation</param>
        /// <param name="tween">The function to be executed every frame, with parameter t increasing from 0 to 1</param>
        /// <param name="post">The function to be executed at the end of the queuemation</param>
        /// <param name="duration">The duration of the queuemation</param>
        public void Add(Action pre, Action<float> tween, Action post, float duration)
        {
            IPlayable queuemation = new QueuedTask(BasicAnimations.Interpolate(pre, tween, post, duration));
            HandleAdd(queuemation);
        }

        /// <summary>
        /// Adds a function with parameter t which increases from 0 to 1 thoughout the duration of the queuemation. 
        /// This function will be executed every frame for the duration.
        /// </summary>
        /// <param name="name">The name of the queuemation</param>
        /// <param name="pre">The function to be executed at the start of the queuemation</param>
        /// <param name="tween">The function to be executed every frame, with parameter t increasing from 0 to 1</param>
        /// <param name="post">The function to be executed at the end of the queuemation</param>
        /// <param name="duration">The duration of the queuemation</param>
        public void Add(string name, Action pre, Action<float> tween, Action post, float duration)
        {
            IPlayable queuemation = new QueuedTask(name, BasicAnimations.Interpolate(pre, tween, post, duration));
            HandleAdd(queuemation);
        }
    }
}
