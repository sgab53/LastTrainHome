using LTH.InteractionSystem;
using UnityEngine;

namespace LTH.DialogueSystem
{
    public sealed class DialogueTrigger : Interactable
    {
        [SerializeField] private DialogueEntry _dialogue;
        [SerializeField] private DialogueTriggerActivation _activation;

        public override void Interact()
        {
            base.Interact();
            DialogueService.StartDialogue(_dialogue);
            this.enabled = false;
        }

        private void Start()
        {
            DialogueService.RegisterTriggerCallback(OnDialogueEnded);
        }

        protected override void OnDestroy()
        {
            DialogueService.UnregisterTriggerCallback(OnDialogueEnded);
            base.OnDestroy();
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
                    return;
            }
        }

        private enum DialogueTriggerActivation
        {
            Once, Repeat
        }
    }
}