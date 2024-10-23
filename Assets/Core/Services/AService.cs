using UnityEngine;

namespace LTH.Core.Services
{
    public abstract class AService<T> : MonoBehaviour, IService where T : AService<T>
    {
        protected virtual void Awake()
        {
            Register();
            ServiceLocator.Instance.Destroyed += DestroySelf;
        }

        protected virtual void OnDestroy()
        {
            ServiceLocator.Instance.Destroyed -= DestroySelf;
            Unregister();
        }

        private void DestroySelf()
        {
            DestroyImmediate(gameObject);
        }

        protected virtual void Register()
        {
            ServiceLocator.Instance.RegisterService((T)this);
        }

        protected virtual void Unregister()
        {
            ServiceLocator.Instance.UnregisterService<T>();
        }
    }

    public interface IService {}
}