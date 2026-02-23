using System;
using UnityEngine;

public class FallPlace : MonoBehaviour
{
    private Action _fallAction;

    public void DefineAction(Action fallAction)
    {
        _fallAction = fallAction;
    }


    void OnTriggerEnter(Collider other)
    {
        _fallAction?.Invoke();
    }
}
