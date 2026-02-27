using System;
using System.Collections;
using System.Collections.Generic;
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

     public void StartTime(Action FinishAction)
    {
        _onFinishedTimeCallback = FinishAction;
        TimerWork();
    }

    private async UniTaskVoid TimerWork()
    {
        float elapsedTime = 0;
        while (elapsedTime < _totalTimeSeconds)
        {
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
}
