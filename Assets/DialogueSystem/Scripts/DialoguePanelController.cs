using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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
    
    private bool _isTyping;

    private CancellationTokenSource _typeSource, _fadeSource, _blinkSource;

    private void Start()
    {
        OnTypingCompleted += TypingCompleted;
        _targetLabel.text = "";
        _targetImage.enabled = false;
        _isTyping = false;
        _canvas.alpha = 0f;
    }

    public void Type()
    {
        CancelSource(_blinkSource);
        CancelSource(_typeSource);

        _typeSource = new();
        ExecuteTyping(_typeSource).Forget();
    }

    public void SetText(string text)
    {
        _targetLabel.text = text;
        Type();
    }

    public void Show()
    {
        if (_canvas.alpha > 0f)
            return;

        if (_fadeSource != null)
        {
            _fadeSource.Cancel();
            _canvas.alpha = 1f;
            return;
        }

        _fadeSource = new();
        ExecuteFade(_fadeSource, 0f, 1f).Forget();
    }

    public void Hide()
    {
        if (_canvas.alpha < 1f)
            return;

        if (_fadeSource != null)
        {
            _fadeSource.Cancel();
            _canvas.alpha = 0f;
            return;
        }

        _fadeSource = new();
        ExecuteFade(_fadeSource, 1f, 0f).Forget();
    }

    private async UniTask ExecuteFade(CancellationTokenSource source, float from, float to)
    {
        float t = 0f;
        var now = Time.realtimeSinceStartup;

        while (t < 1f)
        {
            if (source.IsCancellationRequested)
                return;

            await UniTask.Yield();
            t += (Time.realtimeSinceStartup - now) / _canvasFadeTime;
            now = Time.realtimeSinceStartup;
            _canvas.alpha = Mathf.Lerp(from, to, t);
        }
    }

    private async UniTaskVoid ExecuteTyping(CancellationTokenSource source)
    {
        _isTyping = true;
        for (int i = 1; i < _targetLabel.text.Length; ++i)
        {
            _targetLabel.maxVisibleCharacters = i;

            if (source.IsCancellationRequested)
            {
                _isTyping = false;
                return;
            }

            await UniTask.Delay(_characterDelayMs);
        }

        _isTyping = false;
        OnTypingCompleted?.Invoke();
    }

    private void TypingCompleted()
    {
        _targetLabel.maxVisibleCharacters = _targetLabel.text.Length;

        CancelSource(_blinkSource);

        _blinkSource = new();
        StartBlinking(_blinkSource).Forget();
    }

    private async UniTaskVoid StartBlinking(CancellationTokenSource source)
    {
        _targetImage.enabled = true;

        while (gameObject.activeInHierarchy)
        {
            if (source.IsCancellationRequested)
            {
                _targetImage.enabled = false;
                return;
            }

            await UniTask.Delay(_blinkDelayMs);
            _targetImage.enabled = !_targetImage.enabled;
        }

        _targetImage.enabled = false;
    }

    private void CancelSource(CancellationTokenSource source)
    {
        if (source != null)
        {
            source?.Cancel();
        }
    }

    public void Cancel()
    {
        CancelSource(_fadeSource);
        CancelSource(_typeSource);
    }

    public void Dispose()
    {
        _fadeSource?.Dispose();
        _typeSource?.Dispose();
        _blinkSource?.Dispose();
    }

    public async UniTask<bool> Complete()
    {
        var busy = _isTyping;

        if (busy)
        {
            CancelSource(_fadeSource);
            CancelSource(_typeSource);

            await UniTask.WaitUntil(() => !_isTyping);

            _targetLabel.maxVisibleCharacters = _targetLabel.text.Length;
            _canvas.alpha = 1f;

            _blinkSource = new();
            StartBlinking(_blinkSource).Forget();
        }

        return busy;
    }

    private void OnDisable()
    {
        Cancel();
        Dispose();
    }

    private void OnDrawGizmos() {
        Gizmos.color = _isTyping ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, 1f);
    }
}
