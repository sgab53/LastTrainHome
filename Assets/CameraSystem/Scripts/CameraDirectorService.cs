using Cinemachine;
using LTH.Core.Services;
using UnityEngine;

namespace LTH.CameraSystem
{
    public sealed class CameraDirectorService : AService<CameraDirectorService>
    {
        private static CinemachineBrain _brain;

        public static Camera Camera => _brain.OutputCamera;

        private void Reset()
        {
            _brain ??= (CinemachineBrain)Camera.main!.GetComponent(typeof(CinemachineBrain));
        }
    }
}