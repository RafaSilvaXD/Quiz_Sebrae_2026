using Cysharp.Threading.Tasks;
using Mirror;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

public interface ILoopitaController
{
    UniTaskVoid Move();
    UniTask Move(Vector3 final,float time);
    void PlayStartAudio();
}
