using UnityEngine;

namespace LTH.Core.Services
{
    public abstract class AService<T> : MonoBehaviour, IService where T : AService<T>
    {
        protected virtual void Awake()
        {
            Register((T)this);
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

        private static void Register(T instance)
        {
            ServiceLocator.Instance.RegisterService(instance);
        }

        private static void Unregister()
        {
            ServiceLocator.Instance.UnregisterService<T>();
        }
    }

    public interface IService {}
}