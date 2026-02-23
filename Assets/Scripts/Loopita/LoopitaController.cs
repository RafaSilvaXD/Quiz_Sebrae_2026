using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoopitaController : MonoBehaviour, ILoopitaController
{
    private Vector3 _startPos;
    private CancellationTokenSource _cancellationFloatTokenSour;
    [SerializeField] private AudioClip[] _audiosStartRef;
    [SerializeField] private AudioClip[] _audiosPlayerRef;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Transform _botHead;
    [SerializeField] private float _moveAmplitude;
    [SerializeField] private float _speed;

    private void Start()
    {
        Move().Forget();
        PlayStartAudio();
    }
    
    public async UniTaskVoid Move()
    {
        if (_cancellationFloatTokenSour != null)
        {
            _cancellationFloatTokenSour.Cancel();
        }
        _startPos = transform.GetChild(0).position;
        _cancellationFloatTokenSour = new CancellationTokenSource();
        try
        {
            while (true)
            {
                _botHead.position = _startPos + new Vector3(0,
                                                             Mathf.Lerp(-_moveAmplitude, _moveAmplitude, Mathf.PingPong(Time.time * _speed, 1)),
                                                            0);
                await UniTask.NextFrame(cancellationToken: _cancellationFloatTokenSour.Token);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        finally
        {
            _cancellationFloatTokenSour.Dispose();
            _cancellationFloatTokenSour = null;
        }
    }
    public async UniTask Move(Vector3 final,float time)
    { 
        for (float elapsedTime = 0; 1 > elapsedTime; elapsedTime += Time.deltaTime/time)
        {
            transform.position = (final - transform.position).normalized * Time.deltaTime; ;
            await UniTask.NextFrame();
        }
    }
    public void PlayStartAudio()
    {
        PlayAsyncAudio(0, OpenInitiaDoor);
    }

    private async void PlayAsyncAudio(int audioIndex, Action finishAudio = null)
    {
        _audioSource?.PlayOneShot(_audiosStartRef[audioIndex]);
        await UniTask.WaitUntil(() => !_audioSource.isPlaying);

        await UniTask.Delay(TimeSpan.FromSeconds(0.4f));
        finishAudio.Invoke();
    }

    private void OpenInitiaDoor()
    {
        EventManager.Instance.OnOpenDoor?.Invoke();
        MovePointB();
        PlayAsyncAudio(1, ShowPlayerPosition);        
    }

    private void ShowPlayerPosition()
    {
        EventManager.Instance.OnStartPointController?.Invoke(1);
    }

    private void MovePointB()
    {
         var point = GameObject.Find("LoopitaB");
        transform.LookAt(point.transform, Vector3.up);
        transform.position = point.transform.position;
        _startPos = transform.GetChild(0).position;
        PlayAsyncAudio(1, ShowPlayerPosition);
    }
}
