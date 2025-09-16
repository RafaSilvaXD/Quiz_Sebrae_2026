using System; 
using System.Linq;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine; 

public class BridgeController : MonoBehaviour
{
    private int _currentStep;
    [SerializeField] private int _bridgeIndex;
    [SerializeField] private float _startY;
    private BridgeStepController[] _steps;
    [SerializeField] private float _time;

    public int BridgeIndex { get => _bridgeIndex;}
    public int CurrentStep { get => _currentStep;}

    private void Awake()
    {
        _steps = GetComponentsInChildren<BridgeStepController>(); 
    }
    private async void Start()
    {
        await ResetSteps();
    }
    public async UniTask ResetSteps(bool dropLast = false)
    { 
        if (dropLast)
        {
            _steps[Mathf.Clamp(_currentStep-1,0, _steps.Length)].Drop();
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
        }
        _currentStep = 0;
        await UniTask.WhenAll(_steps.Select(x => MoveStep(x, 1, false)));
        foreach (var step in _steps)
        {
            step.Restore();
        } 
    }
    public async void NextStep()
    { 
        await MoveStep(_currentStep, true); 
        _currentStep++;
    } 
    private async UniTask MoveStep(int index, bool up)
    {
        var selectedSteps = _steps.Where(x => x.Index == index).ToArray(); 
        for(float elapsedTime = 0; elapsedTime < 1; elapsedTime += Time.deltaTime/_time)
        { 
            for (int indexStep = 0; indexStep < selectedSteps.Length; indexStep++) 
            {
                selectedSteps[indexStep].Step.localPosition = Vector3.Lerp(
                   new Vector3(selectedSteps[indexStep].Step.localPosition.x, _startY + ((up) ? -10 : 0), selectedSteps[indexStep].Step.localPosition.z),
                   new Vector3(selectedSteps[indexStep].Step.localPosition.x, _startY + ((up) ? 0 : -10), selectedSteps[indexStep].Step.localPosition.z),
                   elapsedTime);
            } 
            await UniTask.NextFrame();
        }
    }
    private async UniTask MoveStep(BridgeStepController target,float time, bool up)
    { 
        for (float elapsedTime = 0; elapsedTime < 1; elapsedTime += Time.deltaTime / time)
        {
            target.Step.localPosition = Vector3.Lerp(
                    new Vector3(target.Step.localPosition.x, _startY + ((up) ? -10 : 0), target.Step.localPosition.z),
                    new Vector3(target.Step.localPosition.x, _startY + ((up) ? 0 : -10), target.Step.localPosition.z),
                    elapsedTime);
            await UniTask.NextFrame();
        }
    }
} 
