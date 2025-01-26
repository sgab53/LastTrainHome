using LTH.Core.Services;
using Action = System.Action;

namespace LTH.DialogueSystem
{
    public sealed partial class DialogueService : AService<DialogueService>
    {
        private static readonly DialogueSequencer Sequencer = new();

        public static void BindUI(DialogueUI gui)
        {
            Sequencer.BeforeDialogueStarted += gui.ShowBeforeDialoguePrompt;
            //_sequencer.DialogueStarted += gui.;
            Sequencer.DialogueEnded += gui.HideDialoguePrompt;
            Sequencer.NextLineChanged += gui.UpdateDialogueEntry;

            gui.RegisterNextLineCallback(NextLine);
        }

        public static void RegisterTriggerCallback(Action callback)
        {
            Sequencer.DialogueEnded += callback;
        }

        public static void UnregisterTriggerCallback(Action callback)
        {
            Sequencer.DialogueEnded -= callback;
        }

        public static void StartDialogue(DialogueEntry entry)
        {
            Sequencer.LoadDialogue(entry);
        }

        private static void NextLine()
        {
            Sequencer.Next();
        }

        public static void Break()
        {
            Sequencer.ForceDialogueEnd();
        }
    }
}