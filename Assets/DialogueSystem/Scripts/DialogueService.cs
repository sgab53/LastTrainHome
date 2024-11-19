using LTH.Core.Services;
using Action = System.Action;

namespace LTH.DialogueSystem
{
    public sealed partial class DialogueService : AService<DialogueService>
    {
        private readonly DialogueSequencer _sequencer = new();

        public void BindUI(DialogueUI dialogue)
        {
            _sequencer.DialogueInitialized += dialogue.ShowDialoguePrompt;
            //_sequencer.DialogueStarted += dialogue.;
            _sequencer.DialogueEnded += dialogue.HideDialoguePrompt;
            _sequencer.NextLineChanged += dialogue.UpdateDialogueEntry;

            dialogue.RegisterNextLineCallback(NextLine);
        }

        public void RegisterTriggerCallback(Action callback)
        {
            _sequencer.DialogueEnded += callback;
        }

        public void UnregisterTriggerCallback(Action callback)
        {
            _sequencer.DialogueEnded -= callback;
        }

        public void StartDialogue(DialogueEntry entry)
        {
            _sequencer.StartDialogue(entry);
        }

        private void NextLine()
        {
            _sequencer.NextLine();
        }
    }
}