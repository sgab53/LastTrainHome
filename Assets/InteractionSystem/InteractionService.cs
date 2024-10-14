using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LTH.Core.Services;
using UnityEngine;

namespace LTH.InteractionSystem
{
    public class InteractionService : MonoBehaviour, IService
    {
        private readonly Dictionary<GameObject, Interactable> _interactables = new();

        private Interactable _target, _forcedTarget;

        private void Awake()
        {
            ServiceLocator.Instance.RegisterService(this);
        }

        private void OnDestroy()
        {
            _interactables.Clear();
            ServiceLocator.Instance.UnregisterService<InteractionService>();
        }

        public void AddInteractable(GameObject key, Interactable value) => _interactables.Add(key, value);
        public void RemoveInteractable(GameObject key) => _interactables.Remove(key);

        private void SwapTarget(Interactable interactable)
        {
            if (_forcedTarget)
            {
                interactable = _forcedTarget;
            }

            if (_target == interactable)
                return;

            _target?.Deselect();
            interactable.Select();
            _target = interactable;
        }

        public void SwapTargetIfValid(GameObject key)
        {
            if (!_target || _target.gameObject != key.gameObject)
                SwapTarget(_interactables.GetValueOrDefault(key));
        }

        public void UnsetTarget()
        {
            _target?.Deselect();
            _target = null;
        }

        public void InteractWithSelectedTarget()
        {
            if (_forcedTarget)
            {
                _forcedTarget.Interact();
                return;
            }

            _target?.Interact();
        }

        public void ForceTarget(Interactable interactable)
        {
            _forcedTarget = interactable;
        }

        public void UnsetForcedTarget()
        {
            _forcedTarget = null;
        }
    }
}