using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeController : MonoBehaviour
{
    [SerializeField] private float _fadeDurationMs = 200f;
    [SerializeField] private Image _fadePanel;

    private CancellationTokenSource _fadeSource;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Cancel()
    {
        _fadeSource?.Cancel();
    }

    private async UniTask Fade(CancellationTokenSource source, Color from, Color to)
    {
        float t = 0f;
        var now = Time.realtimeSinceStartup;

        while (t < 1.0f)
        {
            if (source.IsCancellationRequested)
                return;

            await UniTask.Yield();
            t += (Time.realtimeSinceStartup - now) / _fadeDurationMs;
            _fadePanel.color = Color.Lerp(from, to, t);
        }
    }

    public async UniTask FadeOut()
    {
        Cancel();
        var to = Color.black;
        var from = Color.black;
        from.a = 0f;
        _fadePanel.color = from;
        _fadePanel.gameObject.SetActive(true);

        _fadeSource = new();
        await Fade(_fadeSource, from, to);
    }

    public async UniTask FadeIn()
    {
        Cancel();
        var to = Color.black;
        to.a = 0f;
        var from = Color.black;
        _fadePanel.color = from;

        _fadeSource = new();
        await Fade(_fadeSource, from, to);

        _fadePanel.gameObject.SetActive(false);
    }

    private void OnDisable() {
        _fadeSource?.Dispose();
    }
}
