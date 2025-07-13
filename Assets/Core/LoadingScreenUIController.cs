using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace LTH.Core.UI
{
    public class LoadingScreenUIController : MonoBehaviour
    {
        public event Action ShowEvent;
        public event Action HideEvent;

        [SerializeField] private UIDocument _document;

        private VisualElement _loadingBar;
        private Label _loadingPercentageLabel;
        private VisualElement _loadingBarFill;

        private const string HiddenUSSClass = "hidden";

        private bool _ready = true;

        private void Awake()
        {
            var root = _document.rootVisualElement;

            _loadingBar = root.Q<VisualElement>("LoadingBar");
            _loadingPercentageLabel = root.Q<Label>("LoadingPercentageLabel");
            _loadingBarFill = root.Q<VisualElement>("LoadingBar_Fill");

            _ready = true;
        }

        public void Show()
        {
            if (!_ready)
                return;

            _ready = false;
            _loadingBar.RegisterCallbackOnce<TransitionEndEvent>(OnShow);
            _loadingBar.RemoveFromClassList(HiddenUSSClass);
        }

        public void Hide()
        {
            if (!_ready)
                return;

            _ready = false;
            _loadingBar.RegisterCallbackOnce<TransitionEndEvent>(OnHide);
            _loadingBar.AddToClassList(HiddenUSSClass);
        }

        public void UpdateLoading(float progress)
        {
            if (!_ready)
                return;

            _loadingBarFill.style.width = Length.Percent(progress * 100f);
            _loadingPercentageLabel.text = progress.ToString("00.00%");
        }

        private void OnShow(TransitionEndEvent _)
        {
            _ready = true;
            ShowEvent?.Invoke();
        }

        private void OnHide(TransitionEndEvent _)
        {
            _ready = false;
            HideEvent?.Invoke();
        }

        private void OnDestroy()
        {
            ShowEvent = null;
            HideEvent = null;
        }
    }
}
