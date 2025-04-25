using System;
using LTH.Core.Services;
using UnityEngine;

using TableRef = UnityEngine.Localization.Tables.TableReference;
using EntryRef = UnityEngine.Localization.Tables.TableEntryReference;

namespace LTH.DialogueSystem
{
    [CreateAssetMenu(fileName = "DialogueService", menuName = "Services/Dialogue Service")]
    public sealed partial class DialogueService : ServiceAsset
    {
        private readonly DialogueSequencer _sequencer = new();

        public event Action DialogueStarted
        {
            add => _sequencer.DialogueStarted += value;
            remove => _sequencer.DialogueStarted -= value;
        }

        public event Action DialogueReady
        {
            add => _sequencer.DialogueReady += value;
            remove => _sequencer.DialogueReady -= value;
        }

        public event Action DialogueEnded
        {
            add => _sequencer.DialogueEnded += value;
            remove => _sequencer.DialogueEnded -= value;
        }

        public event Action<TableRef, EntryRef> NextLineChanged
        {
            add => _sequencer.NextLineChanged += value;
            remove => _sequencer.NextLineChanged -= value;
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
            _sequencer.LoadDialogue(entry);
        }

        public void NextLine()
        {
            _sequencer.Next();
        }

        public void Break()
        {
            _sequencer.ForceDialogueEnd();
        }

        protected override void OnInit() {}

        protected override void OnShutdown() {}
    }
}