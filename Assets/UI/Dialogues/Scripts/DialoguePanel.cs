using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace LTH.UI
{
    [UxmlElement]
    public sealed partial class DialoguePanel : VisualElement
    {
        private readonly Label _dialogueLabel;

        private readonly LocalizedString _localizedContent = new();
        private readonly StringBuilder _stringBuilder = new();
        private CancellationTokenSource _typingSource;

        private const string HiddenUssClassName = "hidden";

        private string _currentText;
        private int _typeDelay;

        [UxmlAttribute]
        public int CharsPerSecond
        {
            get => _typeDelay * 1000;
            set => _typeDelay = (int)(value * 0.001f);
        }

        private bool _dialogueReady = false;

        public DialoguePanel()
        {
            this.LoadVisualTreeAsset();

            _dialogueLabel = this.Q<Label>("DialogueText");
            _localizedContent.StringChanged += UpdateText;

            Hide();
        }

        public void UpdateDialogue(TableReference table, TableEntryReference entry)
        {
            _localizedContent.SetReference(table, entry);
        }

        public void SetTypeSpeed(int charsPerSecond)
        {
            if (charsPerSecond <= 0)
                charsPerSecond = 5; //SEARCHME: get default speed from settings

            _typeDelay = 1000 / charsPerSecond;
        }

        public void Show()
        {
            RegisterCallback<TransitionEndEvent>(OnTransitionEnd);
            RemoveFromClassList(HiddenUssClassName);
        }

        public void Hide()
        {
            AddToClassList(HiddenUssClassName);
            _dialogueReady = false;
        }

        private void CancelTyping()
        {
            if (_typingSource == null)
                return;

            _typingSource.Cancel();
            _typingSource.Dispose();
            _typingSource = null;
        }

        private void UpdateText(string text)
        {
            if (text == null)
                return;

            CancelTyping();
            _typingSource = new CancellationTokenSource();

            _dialogueLabel.text = "";
            _currentText = text;

            TypeText().Forget();
        }

        private void OnTransitionEnd(TransitionEndEvent transition)
        {
            _dialogueReady = true;
            UnregisterCallback<TransitionEndEvent>(OnTransitionEnd);
        }

        private async UniTaskVoid TypeText()
        {
            await UniTask.WaitUntil(() => _dialogueReady, cancellationToken: _typingSource.Token);

            var length = _currentText.Length;

            for (var i = 0; i < length; ++i)
            {
                if (char.IsWhiteSpace(_currentText[i]))
                    continue;

                await UniTask.Delay(_typeDelay, cancellationToken: _typingSource.Token);

                _stringBuilder.Append(_currentText[i]);
                _dialogueLabel.text = _stringBuilder.ToString();
            }

            CancelTyping();

            _stringBuilder.Clear();
        }
    }
}