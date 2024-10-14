using UnityEngine;

namespace LTH.Core
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        [Header("Singleton")]
        [SerializeField] private bool _persistent;

        private static T _sharedInstance;

        public static T Instance
        {
            get
            {
                if (!_sharedInstance)
                    FindAndSetSharedInstance();
                return _sharedInstance;
            }
        }

        private static void FindAndSetSharedInstance()
        {
            GameObject go;
            var type = typeof(T);
            var instances = (T[])FindObjectsOfType(type);
            var count = instances.Length;

            if (count >= 1)
            {
                _sharedInstance = instances[0];

                go = _sharedInstance.gameObject;

                Debug.Log($"Found {count} instances of {type.Name}.");

                for (var i = 1; i < count; ++i)
                {
                    Destroy(instances[i].gameObject);
                }
            }
            else
            {
                go = new GameObject(type.Name);
                _sharedInstance = (T)go.AddComponent(type);

                Debug.Log($"Creating a new instance of {type.Name}.");
            }

            if (_sharedInstance._persistent)
                DontDestroyOnLoad(go);
        }

        protected virtual void Awake()
        {
            if (_sharedInstance)
            {
                if (_sharedInstance != this)
                    Destroy(gameObject);

                return;
            }

            FindAndSetSharedInstance();
        }

        protected virtual void OnDestroy()
        {
            if (_sharedInstance == this)
                _sharedInstance = null;
        }
    }
}