using System;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace LTH.DialogueSystem
{
    public sealed partial class DialogueService
    {
        private sealed class DialogueSequencer
        {
            public event Action BeforeDialogueStarted;
            public event Action DialogueStarted;
            public event Action DialogueEnded;
            public event Action<TableReference, TableEntryReference> NextLineChanged;

            private DialogueEntry _currentDialogueEntry;
            private int _length;
            private int _currentLine;

            private DialogueState _state = DialogueState.Unset;

            public void LoadDialogue(DialogueEntry dialogue)
            {
                if (_state == DialogueState.Busy)
                    return;

                _state = DialogueState.Busy;
                PrepareDialogue(dialogue);
            }

            public bool Next()
            {
                if (_state != DialogueState.Ready)
                    return false;

                _state = DialogueState.Busy;

                if (++_currentLine >= _length)
                {
                    DialogueEnded?.Invoke();
                    _state = DialogueState.Ended;
                    return false;
                }

                UpdateLine();
                _state = DialogueState.Ready;
                return true;
            }

            public void ForceDialogueEnd()
            {
                DialogueEnded?.Invoke();
                _state = DialogueState.Ready;
            }

            private void UpdateLine()
            {
                var current = _currentDialogueEntry[_currentLine];
                NextLineChanged?.Invoke(current.TableReference, current.TableEntryReference);
            }

            private void PrepareDialogue(DialogueEntry dialogue)
            {
                BeforeDialogueStarted?.Invoke();

                if (dialogue.Length == 0)
                    Debug.LogError("Dialogue Entry is empty.");

                _currentDialogueEntry = dialogue;
                _length = dialogue.Length;
                _currentLine = 0;

                DialogueStarted?.Invoke();
                UpdateLine();
                _state = DialogueState.Ready;
            }

            private enum DialogueState : byte
            {
                Unset, Busy, Ready, Ended
            }
        }
    }
}