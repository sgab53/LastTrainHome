using System;
using System.Collections.Generic;
using UnityEngine;

namespace LTH.Core.Services
{
    public sealed class ServiceLocator : Singleton<ServiceLocator>
    {
        private readonly Dictionary<Type, IService> _services = new();

        public event Action Destroyed;

        protected override void Awake()
        {
            base.Awake();
            _services.Clear();
        }

        public void RegisterService<T>(T service) where T : IService
        {
            var type = typeof(T);

            if (!_services.TryAdd(type, service))
                Debug.LogError($"Service {type.Name} is already registered.");
        }

        public void UnregisterService<T>() where T : IService
        {
            var type = typeof(T);

            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
                return;
            }

            Debug.LogError($"Service {type.Name} is not registered.");
        }

        public T GetService<T>() where T : IService
        {
            var type = typeof(T);

            _services.TryGetValue(type, out var service);

            return (T)service;
        }

        protected override void OnDestroy()
        {
            Destroyed?.Invoke();
            base.OnDestroy();
        }
    }
}
