using UnityEngine;

namespace LTH.Core.Services
{
    public abstract class AService<T> : MonoBehaviour, IService where T : AService<T>
    {
        protected virtual void Awake()
        {
            ServiceLocator.Instance.RegisterService((T)this);
            ServiceLocator.Instance.Destroyed += DestroySelf;
        }

        protected virtual void OnDestroy()
        {
            ServiceLocator.Instance.Destroyed -= DestroySelf;
            ServiceLocator.Instance.UnregisterService<T>();
        }

        private void DestroySelf()
        {
            DestroyImmediate(gameObject);
        }

        public virtual void Register()
        {
            ServiceLocator.Instance.RegisterService((T)this);
        }

        public virtual void Unregister()
        {
            ServiceLocator.Instance.UnregisterService<T>();
        }
    }

    public interface IService {}
}