using LTH.Core.Services;
using LTH.InteractionSystem;
using UnityEngine;

namespace LTH.DialogueSystem
{
    public sealed class DialogueTrigger : Interactable
    {
        [SerializeField] private DialogueEntry _dialogue;
        [SerializeField] private DialogueTriggerActivation _activation;

        private DialogueService _dialogueService;

        public override void Interact()
        {
            base.Interact();
            _dialogueService.StartDialogue(_dialogue);
            this.enabled = false;
        }

        private void Start()
        {
            if (!_dialogueService)
                _dialogueService = ServiceLocator.Instance.GetService<DialogueService>();

            _dialogueService.RegisterTriggerCallback(OnDialogueEnded);
        }

        protected override void OnDestroy()
        {
            _dialogueService.UnregisterTriggerCallback(OnDialogueEnded);
            base.OnDestroy();
            _dialogueService = null;
        }

        private void OnDialogueEnded()
        {
            switch (_activation)
            {
                case DialogueTriggerActivation.Once:
                    break;
                case DialogueTriggerActivation.Repeat:
                    this.enabled = true;
                    break;
                default:
                    break;
            }
        }

        private enum DialogueTriggerActivation
        {
            Once, Repeat
        }
    }
}