using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace LTH.DialogueSystem
{
    public sealed partial class DialogueService
    {
        private sealed class DialogueSequencer
        {
            public event Action DialogueInitialized;
            // public event Action DialogueStarted;
            public event Action DialogueEnded;
            public event Action<TableReference, TableEntryReference> NextLineChanged;

            private DialogueEntry _dialogueEntry;
            private int _length;
            private int _currentLine;

            private CancellationTokenSource _dialoguePreparationSource;
            private DialogueState _state = DialogueState.Unset;

            public void StartDialogue(DialogueEntry dialogue)
            {
                if (_state == DialogueState.Busy)
                    return;

                CancelDialoguePreparation();
                _dialoguePreparationSource = new CancellationTokenSource();

                _state = DialogueState.Busy;
                PrepareDialogue(dialogue);
            }

            public bool NextLine()
            {
                if (_state != DialogueState.Ready)
                    return false;

                _state = DialogueState.Busy;

                if (++_currentLine >= _length)
                {
                    DialogueEnded?.Invoke();
                    _state = DialogueState.Ready;
                    return false;
                }

                UpdateLine();
                _state = DialogueState.Ready;

                return true;
            }

            private void UpdateLine()
            {
                var current = _dialogueEntry[_currentLine];
                NextLineChanged?.Invoke(current.TableReference, current.TableEntryReference);
            }

            private void CancelDialoguePreparation()
            {
                if (_dialoguePreparationSource == null)
                    return;

                _dialoguePreparationSource.Cancel();
                _dialoguePreparationSource.Dispose();
            }

            private void PrepareDialogue(DialogueEntry dialogue)
            {
                DialogueInitialized?.Invoke();

                CancelDialoguePreparation();

                if (dialogue.Length == 0)
                    Debug.LogError("Dialogue Entry is empty.");

                _dialogueEntry = dialogue;
                _length = dialogue.Length;
                _currentLine = 0;

                // DialogueStarted?.Invoke();
                UpdateLine();
                _state = DialogueState.Ready;
            }

            private enum DialogueState
            {
                Unset, Busy, Ready,
            }
        }
    }
}