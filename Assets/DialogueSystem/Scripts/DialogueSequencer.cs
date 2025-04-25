using System;
using UnityEngine;

using TableRef = UnityEngine.Localization.Tables.TableReference;
using EntryRef = UnityEngine.Localization.Tables.TableEntryReference;

namespace LTH.DialogueSystem
{
    public sealed partial class DialogueService
    {
        private sealed class DialogueSequencer
        {
            public event Action DialogueStarted;
            public event Action DialogueReady;
            public event Action DialogueEnded;
            public event Action<TableRef, EntryRef> NextLineChanged;

            private DialogueEntry _currentDialogueEntry;
            private int _length;
            private int _currentLine;

            private DialogueState _state = DialogueState.Unset;

            public void LoadDialogue(DialogueEntry dialogue)
            {
                if (_state == DialogueState.Busy)
                    return;

                #if UNITY_EDITOR
                if (dialogue.Length == 0)
                {
                    Debug.LogError("Dialogue Entry is empty.");
                    _state = DialogueState.Unset;
                    return;
                }
                #endif

                _state = DialogueState.Busy;

                DialogueStarted?.Invoke();

                _currentDialogueEntry = dialogue;
                _length = dialogue.Length;
                _currentLine = 0;

                UpdateLine();
                _state = DialogueState.Ready;

                DialogueReady?.Invoke();
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
                var line = _currentDialogueEntry[_currentLine];
                NextLineChanged?.Invoke(line.TableReference, line.TableEntryReference);
            }

            private enum DialogueState : byte
            {
                Unset, Busy, Ready, Ended
            }
        }
    }
}