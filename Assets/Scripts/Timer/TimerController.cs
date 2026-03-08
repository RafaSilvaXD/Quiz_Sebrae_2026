using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class TimerController : MonoBehaviour
{
    private Action _onFinishedTimeCallback;
    [SerializeField] private float _totalTimeSeconds;
    [SerializeField] private TextMeshProUGUI _timerView;
    private CancellationTokenSource  _cts;
     public void StartTime(Action FinishAction)
    {
        _cts = new CancellationTokenSource ();
        _onFinishedTimeCallback = FinishAction;
        TimerWork(_cts.Token).Forget();
    }

    private async UniTaskVoid TimerWork(CancellationToken  token)
    {
        float elapsedTime = 0;
        while (elapsedTime < _totalTimeSeconds)
        {
            token.ThrowIfCancellationRequested();
            elapsedTime += Time.deltaTime; 
            var timespan = TimeSpan.FromSeconds(_totalTimeSeconds - elapsedTime);
            _timerView.SetText($"{timespan.Minutes:D2}:{timespan.Seconds:D2}");
            await UniTask.NextFrame();
        }

        _timerView.SetText($"00:00");
        _onFinishedTimeCallback?.Invoke();
    }

    private void FinishTimer()
    {
        _timerView.SetText($"00:00");
        FindAnyObjectByType<GameplayManagerNetworking>().Cmd_Defeat();
        _onFinishedTimeCallback?.Invoke();
    }

    public void StopTimer()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    public void ResetTimer()
    {
        _timerView.SetText($"00:00");
        StopTimer();
    }
}
