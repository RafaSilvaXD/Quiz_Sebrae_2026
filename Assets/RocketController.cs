using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class RocketController : MonoBehaviour
{
    private CancellationTokenSource _cancellationRocketTokenSource;
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private Transform _door;
    [SerializeField] private float _speed;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private AssetReference _audioEngineRef;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        OpenDoor();
    } 
    private void OnDisable()
    {
        if(_cancellationRocketTokenSource != null)
        {
            _cancellationRocketTokenSource.Cancel();
        }
    } 
    public async void OpenDoor()
    {
        await AsyncOpenDoor();
    }
    public async void CloseDoor()
    {
        await AsyncCloseDoor();
    }
    public async UniTask AsyncOpenDoor()
    {
        var startRotation = _door.rotation;
        for(float elapsedTime = 0; elapsedTime < 1;elapsedTime+= Time.deltaTime / 2)
        {
            _door.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(0, 0, 0), elapsedTime);
            await UniTask.NextFrame();
        }
        _door.rotation = Quaternion.Euler(0, 0, 0);
    }
    public async UniTask AsyncCloseDoor()
    {
        var startRotation = _door.rotation;
        for (float elapsedTime = 0; elapsedTime < 1; elapsedTime += Time.deltaTime / 2)
        {
            _door.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(-115, 0, 0), elapsedTime);
            await UniTask.NextFrame();
        }
        _door.rotation = Quaternion.Euler(-115, 0, 0);
    }
    public async void Engine()
    {
        _audioSource.PlayOneShot(Addressables.LoadAssetAsync<AudioClip>(_audioEngineRef).WaitForCompletion());
        CloseDoor();
        await UniTask.WhenAll(Smoke(0), Fire(2));
        Move().Forget();
        _smoke.Stop();
    }
    private async UniTaskVoid Move()
    {
        if(_cancellationRocketTokenSource != null)
        {
            _cancellationRocketTokenSource.Cancel();
        }
        try
        {

            float elapsedTime = 0;
            while (true)
            {
                elapsedTime += Time.deltaTime / 10;
                transform.position += Vector3.up * Time.deltaTime * _speed * _curve.Evaluate(Mathf.Clamp(elapsedTime, 0, 1));
                await UniTask.NextFrame();
            }
        }
        catch (Exception ex) 
        {

        } 
    }
    private async UniTask Smoke(float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay)); 
        _smoke.Play();
    }
    private async UniTask Fire(float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        _fire.Play();
    }
}
