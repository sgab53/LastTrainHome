using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LTH.Core.Services
{
    public sealed class ServiceRegistry : MonoBehaviour
    {
        [SerializeField] private AssetReferenceT<ServiceAsset>[] _startupAssets;

        private readonly List<ServiceAsset> _loadedServices = new();
        private readonly Dictionary<Type, ServiceAsset> _runtimeServices = new();

        public async UniTask<T> LoadServiceAsync<T>() where T : ServiceAsset
        {
            var serviceAsset = GetService<T>();
            if (serviceAsset)
                return serviceAsset;

            var handle = Addressables.LoadAssetAsync<T>("Service");
            await handle.Task;

            serviceAsset = handle.Result;
            handle.Release();
            serviceAsset.Initialize();
            _runtimeServices[typeof(T)] = serviceAsset;

            return serviceAsset;
        }

        public T LoadService<T>() where T : ServiceAsset
        {
            var serviceAsset = GetService<T>();
            if (serviceAsset)
                return serviceAsset;

            serviceAsset = Addressables.LoadAssetAsync<T>("Service").WaitForCompletion();
            serviceAsset.Initialize();
            _runtimeServices[typeof(T)] = serviceAsset;

            return serviceAsset;
        }

        public T GetService<T>() where T : ServiceAsset
        {
            foreach (var service in _loadedServices)
            {
                if (service is T existing)
                    return existing;
            }

            if (_runtimeServices.TryGetValue(typeof(T), out var cached))
                return (T)cached;

            return null;
        }

        public void UnloadService<T>()
        {
            var staticService = _loadedServices.Find(s => s is T);
            if (staticService != null)
            {
                staticService.Shutdown();
                _loadedServices.Remove(staticService);
                Addressables.Release(staticService);
                return;
            }

            var type = typeof(T);
            if (_runtimeServices.TryGetValue(type, out var runtimeService))
            {
                runtimeService.Shutdown();
                Addressables.Release(runtimeService);
                _runtimeServices.Remove(type);
            }
        }

        public bool IsServiceLoaded<T>() =>
            _loadedServices.Exists(s => s is T) ||
            _runtimeServices.ContainsKey(typeof(T));

        private void Awake()
        {
            Service.Register(this);
            LoadAndInitializeServicesAsync().Forget();
        }

        private async UniTask LoadAndInitializeServicesAsync()
        {
            var handle =
                Addressables.LoadAssetsAsync<ServiceAsset>(_startupAssets, null, Addressables.MergeMode.Union);
            await handle.Task;

            _loadedServices.AddRange(handle.Result);
            _loadedServices.Sort((a, b) => a.Priority.CompareTo(b.Priority));

            foreach (var service in _loadedServices)
            {
                service.Initialize();
            }

            handle.Release();
        }

        private void OnDestroy()
        {
            for (var i = _loadedServices.Count - 1; i >= 0; --i)
            {
                _loadedServices[i].Shutdown();
            }

            foreach (var service in _runtimeServices.Values)
            {
                service.Shutdown();
            }

            _runtimeServices.Clear();
        }
    }

    public static class Service
    {
        private static ServiceRegistry _registry;

        public static void Register(ServiceRegistry registry) => _registry = registry;
        public static bool IsLoaded<T>() => _registry.IsServiceLoaded<T>();
        public static T Get<T>() where T : ServiceAsset => _registry.GetService<T>();
        public static T Load<T>() where T : ServiceAsset => _registry.LoadService<T>();
        public static async UniTask<T> LoadAsync<T>() where T : ServiceAsset =>
            await _registry.LoadServiceAsync<T>();
    }
}