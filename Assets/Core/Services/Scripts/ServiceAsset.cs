using UnityEngine;

namespace LTH.Core.Services
{
    public abstract class ServiceAsset : ScriptableObject, IService
    {
        [SerializeField] private int _priority;

        public ServiceState State { get; private set; } = ServiceState.Uninitialized;
        public int Priority => _priority;

        public virtual void Initialize()
        {
            if (State != ServiceState.Uninitialized)
                return;

            State = ServiceState.Initialized;
            OnInit();
        }

        public virtual void Shutdown()
        {
            if (State != ServiceState.Initialized)
                return;

            OnShutdown();
            State = ServiceState.Shutdown;
        }

        protected abstract void OnInit();
        protected abstract void OnShutdown();
    }

    public enum ServiceState { Uninitialized, Initialized, Shutdown }

    public interface IService
    {
        ServiceState State { get; }
        int Priority { get; }
        void Initialize();
        void Shutdown();
    }
}
