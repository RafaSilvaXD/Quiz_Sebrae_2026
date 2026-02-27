using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DefeatPanelController : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField] private Button _endButton;


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        _endButton.onClick.AddListener(EndGame);
    }

    public void EndGame()
    {
        Application.Quit(); 
    }
    public void ShowInspector()
    {
        Show().Forget();
    }
    public async UniTaskVoid Show()
    {
        for (float elapsedTime = 0; elapsedTime < 1; elapsedTime += Time.deltaTime)
        {
            _canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime);
            await UniTask.NextFrame();
        }
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }
}
