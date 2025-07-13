using LTH.Core.Services;
using LTH.Input;
using LTH.InteractionSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LTH.DialogueSystem
{
    public sealed class DialogueInteraction : Interactable
    {
        [SerializeField] private AssetReferenceT<DialogueEntry> _dialogueReference;
        [SerializeField] private DialogueTriggerActivation _activation;

        [Header("Optional")]
        [SerializeField] private DialogueService _dialogueService;

        private DialogueEntry _entry;

        public override void Interact()
        {
            if (!enabled)
                return;

            base.Interact();
            _dialogueService.StartDialogue(_entry);
            this.enabled = false;
        }

        protected override void Awake()
        {
            base.Awake();

            _entry =
                Addressables.LoadAssetAsync<DialogueEntry>(_dialogueReference).WaitForCompletion();

            if (!_dialogueService)
                _dialogueService = Service.Load<DialogueService>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _dialogueService.DialogueEnded += OnDialogueEnded;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _dialogueService.DialogueEnded -= OnDialogueEnded;
        }

        private void OnDialogueEnded()
        {
            // SEARCHME: add input service to make possible
            // input enabling and disabling without directly
            // referencing InputActionAsset

            // var inputService = Service.Get<InputService>()
            // inputService.

            switch (_activation)
            {
                case DialogueTriggerActivation.Once:
                    _dialogueService.DialogueEnded -= OnDialogueEnded;
                    break;
                case DialogueTriggerActivation.Repeat:
                    this.enabled = true;
                    break;
                default:
                    return;
            }
        }

        private enum DialogueTriggerActivation : byte
        {
            Once, Repeat
        }
    }
}