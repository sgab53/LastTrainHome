using LTH.Core.Services;
using Unity.Cinemachine;
using UnityEngine;

namespace LTH.CameraSystem
{
    public sealed class CameraDirectorService : AService<CameraDirectorService>
    {
        private static CinemachineBrain _brain;

        private void Reset()
        {
            if (!_brain && Camera.main)
                _brain = (CinemachineBrain)Camera.main!.GetComponent(typeof(CinemachineBrain));
        }

        private void OnValidate()
        {
            Reset();
        }

        // SEARCHME: use this object to setup cameras on instantiation
    }
}