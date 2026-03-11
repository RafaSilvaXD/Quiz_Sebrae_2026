using System; 
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisingController : MonoBehaviour
{
    [SerializeField] private Sprite[] _advertisings;
    [SerializeField] private float _timePerAD;
    [SerializeField] private Image _output;

    private void Start()
    {
        ShowAD().Forget();
    }
    private async UniTaskVoid ShowAD()
    {
        int count = 0;
        _output.sprite = _advertisings[count];
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_timePerAD));
            count++;
            if(count >= _advertisings.Length)
            {
                count = 0;
            }
            _output.sprite = _advertisings[count];
        }
    }
}
