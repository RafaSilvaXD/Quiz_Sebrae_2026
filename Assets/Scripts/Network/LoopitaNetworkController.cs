using Cysharp.Threading.Tasks;
using Mirror;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoopitaNetworkController : NetworkBehaviour,ILoopitaController
{
    private Vector3 _startPos;
    private CancellationTokenSource _cancellationFloatTokenSour;
    [SerializeField] private AssetReference[] _audiosStartRef; 
    [SerializeField] private AssetReference[] _audiosPlayerRef;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Transform _botHead;
    [SerializeField] private float _moveAmplitude;
    [SerializeField] private float _speed;

    private void Start()
    {
        Move().Forget();
    }
    
    [Client] 
    public void Rpc_MovePointB()
    {
        var point = GameObject.Find("LoopitaB");
        transform.LookAt(point.transform, Vector3.up);
        transform.position = point.transform.position;
        _audioSource?.PlayOneShot(_audiosStartRef[1].LoadAssetAsync<AudioClip>().WaitForCompletion());

    }
    [ClientRpc]
    public async void Rpc_PlayStartAudio()
    {
        _audioSource?.PlayOneShot(_audiosStartRef[0].LoadAssetAsync<AudioClip>().WaitForCompletion());
        await UniTask.WaitUntil(() => !_audioSource.isPlaying);
        await UniTask.Delay(TimeSpan.FromSeconds(0.4f));
        EventManager.Instance.OnOpenDoor?.Invoke();
    }
    [ClientRpc]
    public void Rpc_TalkPlayer(int index)
    {
        var clip = Addressables.LoadAssetAsync<AudioClip>(_audiosPlayerRef[index - 1]).WaitForCompletion();
        _audioSource?.PlayOneShot(clip);
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
        Rpc_PlayStartAudio();
    }
}
