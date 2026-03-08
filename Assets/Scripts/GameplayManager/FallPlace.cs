using System;
using UnityEngine;

public class FallPlace : MonoBehaviour
{
    private Action<IPlayer> _fallAction;

    public void DefineAction(Action<IPlayer> fallAction)
    {
        _fallAction = fallAction;
    }


    void OnTriggerEnter(Collider other)
    {
        _fallAction?.Invoke(other.GetComponent<IPlayer>());
    }
}
