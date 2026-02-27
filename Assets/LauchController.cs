using System;
using UnityEngine;

public class LaunchController : MonoBehaviour
{
    private Action _victoryAction;
    private bool _firstTime = true;
    [SerializeField] private RocketController _rocket;

    public void DefineAction(Action victoryAction)
    {
        _victoryAction = victoryAction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_firstTime)
            return;
         other.TryGetComponent(out IPlayer player);
        if(player == null)
            return;

        _firstTime = false;
       _victoryAction?.Invoke();
        _rocket.Engine();
    }
}
