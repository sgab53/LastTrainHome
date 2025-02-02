using Cinemachine;
using LTH.Core.Services;
using UnityEngine;

namespace LTH.CameraSystem
{
    public class CameraDirectorService : MonoBehaviour, IService
    {
        private static CinemachineBrain _brain;

        public static Camera Camera => _brain.OutputCamera;

        private void Reset()
        {
            _brain ??= (CinemachineBrain)Camera.main!.GetComponent(typeof(CinemachineBrain));
        }

        private void Awake()
        {
            ServiceLocator.Instance.RegisterService(this);
            _brain = (CinemachineBrain)Camera.main!.GetComponent(typeof(CinemachineBrain));
        }

        private void OnDestroy()
        {
            ServiceLocator.Instance.UnregisterService<CameraDirectorService>();
        }
    }
}