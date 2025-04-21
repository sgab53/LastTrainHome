using System.Collections.Generic;
using LTH.Core.Services;
using UnityEngine;

namespace LTH.InteractionSystem
{
    [CreateAssetMenu(fileName = "InteractionService", menuName = "Services/Interaction Service")]
    public sealed class InteractionService : ServiceAsset
    {
        private readonly Dictionary<GameObject, Interactable> _interactables = new();
        private Interactable _target, _forcedTarget;

        public void AddInteractable(GameObject key, Interactable interactable)
        {
            _interactables.Add(key, interactable);
        }

        public void RemoveInteractable(GameObject key)
        {
            if (_target?.gameObject == key)
                _target = null;

            if (_forcedTarget?.gameObject == key)
                _forcedTarget = null;

            _interactables.Remove(key);
        }

        public void SwapTargetIfValid(GameObject key)
        {
            if (!_target || _target.gameObject != key)
                SwapTarget(_interactables.GetValueOrDefault(key));
        }

        public void UnsetTarget()
        {
            if (!_target)
                return;

            _target.Deselect();
            _target = null;
        }

        public void InteractWithSelectedTarget()
        {
            if (!_target)
                return;

            if (_forcedTarget)
            {
                _forcedTarget.Interact();
                return;
            }

            _target.Interact();
        }

        public void ForceTarget(Interactable interactable)
        {
            _forcedTarget = interactable;
        }

        public void UnsetForcedTarget()
        {
            _forcedTarget = null;
        }

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

        protected override void OnInit()
        {
            _interactables.Clear();
            _target = _forcedTarget = null;
        }

        protected override void OnShutdown()
        {
            _interactables.Clear();
        }
    }
}