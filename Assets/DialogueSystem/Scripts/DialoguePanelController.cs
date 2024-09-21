using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanelController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private float _canvasFadeTime = 0.8f;

    [SerializeField] private TMP_Text _targetLabel;
    [SerializeField] private int _characterDelayMs = 10;

    [SerializeField] private Image _targetImage;
    [SerializeField] private int _blinkDelayMs = 500;

    public event Action OnTypingCompleted;

    private bool _isVisible, _isBlinking, _isTyping;

    private void Start()
    {
        OnTypingCompleted += TypingCompleted;
        _targetLabel.text = "";
        _targetImage.enabled = false;
        _canvas.alpha = 0f;
        _isVisible = _isBlinking = _isTyping = false;
    }

    public void Type()
    {
        _isTyping = false;
        ExecuteTyping().Forget();
    }

    public void SetText(string text)
    {
        _targetLabel.text = text;
        _isBlinking = false;
        Type();
    }

    public void Show()
    {
        if (_isVisible)
        {
            if (!_isTyping)
                Type();
            return;
        }
        
        _isVisible = true;
        StartShowAndType().Forget();
    }

    public void Hide()
    {
        if (!_isVisible)
            return;
        
        _isVisible = false;
        ExecuteFade(1f, 0f).Forget();
    }

    private async UniTaskVoid StartShowAndType()
    {
        await ExecuteFade(0f, 1f);
        Type();
    }

    private async UniTask ExecuteFade(float from, float to)
    {
        float t = 0f;
        var now = Time.realtimeSinceStartup;

        while (t < 1f)
        {
            await UniTask.Yield();
            t += (Time.realtimeSinceStartup - now) / _canvasFadeTime;
            now = Time.realtimeSinceStartup;
            _canvas.alpha = Mathf.Lerp(from, to, t);
        }
    }

    private async UniTaskVoid ExecuteTyping()
    {
        _isTyping = true;

        for (int i = 0; i < _targetLabel.text.Length; ++i)
        {
            _targetLabel.maxVisibleCharacters = i;
            await UniTask.Delay(_characterDelayMs);
            
            if (!_isTyping)
            {
                OnTypingCompleted?.Invoke();
                return;
            }
        }

        _isTyping = false;

        OnTypingCompleted?.Invoke();
    }

    private void TypingCompleted()
    {
        _targetImage.enabled = true;
        StartBlinking().Forget();
    }

    private async UniTaskVoid StartBlinking()
    {
        if (_isBlinking)
            return;

        _isBlinking = true;

        while (gameObject.activeInHierarchy && _isBlinking)
        {
            await UniTask.Delay(_blinkDelayMs);
            _targetImage.enabled = !_targetImage.enabled;
        }

        _isBlinking = false;
    }
}
