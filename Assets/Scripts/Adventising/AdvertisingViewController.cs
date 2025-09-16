using System; 
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AdvertisingViewController : MonoBehaviour
{
    private Vector3 _startPos;
    private CancellationTokenSource _cancellationTimeToken;
    [SerializeField] private float _speed;
    [SerializeField] private float _moveAmplitude;

    private void Awake()
    {
        _startPos = transform.position;
    }
    private void Start()
    {
        Move().Forget();
    }
    private void OnDisable()
    {
        if(_cancellationTimeToken != null)
        {
            _cancellationTimeToken.Cancel();
        }
    }

    private async UniTaskVoid Move()
    {
        if (_cancellationTimeToken != null)
        {
            _cancellationTimeToken.Cancel();
        }
        _cancellationTimeToken = new CancellationTokenSource();
        try
        { 
            while (true)
            {
                transform.position = _startPos + new Vector3(0,
                                                             Mathf.Lerp(-_moveAmplitude, _moveAmplitude, Mathf.PingPong(Time.time * _speed, 1)),
                                                            0);
                await UniTask.NextFrame(cancellationToken: _cancellationTimeToken.Token);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        finally
        {
            _cancellationTimeToken.Dispose();
            _cancellationTimeToken= null;
        }
    }
}
