using System; 
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TextQuizUIController : MonoBehaviour
{
    private Action<bool> _callback;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AssetReference _audioPositiveRef;
    [SerializeField] private AssetReference _audioNegativeRef;

    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            _callback?.Invoke(true);
        }
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            _callback?.Invoke(false);
        }
    }

    public void SetupQuestion(Question question,Action<bool> callback)
    {
        _callback = callback;
        _text.SetText(question.Text);
        question.Answer = question.Answer.OrderBy(x=>Guid.NewGuid()).ToArray();
        int count = 0;
        foreach (var button in _buttons)
        {
            button.onClick.RemoveAllListeners();
            var answer = question.Answer[count];
            count++; 
            button.onClick.AddListener(()=> _callback?.Invoke(answer.Correct));
            button.onClick.AddListener(() =>
            {
                button.targetGraphic.color = (answer.Correct) ? Color.green : Color.red;
                _audioSource.PlayOneShot((answer.Correct) ? _audioPositiveRef.LoadAssetAsync<AudioClip>().WaitForCompletion() : _audioNegativeRef.LoadAssetAsync<AudioClip>().WaitForCompletion());

            });
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = answer.Text;
        }
    } 
}
