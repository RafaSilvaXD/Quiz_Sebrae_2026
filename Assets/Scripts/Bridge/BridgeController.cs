using System; 
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine; 

public class BridgeController : MonoBehaviour
{
    private int _currentStep;
    private uint _bridgeIndex;
    [SerializeField] private BridgeStepController[] _steps;

    public int CurrentStep { get => _currentStep;}

    private void Awake()
    {
        _steps = GetComponentsInChildren<BridgeStepController>(); 
    }

    private async void Start()
    {
        await ResetSteps();
        foreach(var step in _steps)
        {
            step.DefineBridgeStepIndex(_bridgeIndex);
        }
    }
    public async UniTask ResetSteps()
    {
        foreach(var step in _steps)
        {
            step.CloseStep();
            await UniTask.Delay(TimeSpan.FromSeconds(0.25)); 
        }
    }
    public async UniTask OpenAllStep()
    {
         foreach(var step in _steps)
        {
            step.OpenStep();
            await UniTask.Delay(TimeSpan.FromSeconds(0.25)); 
        }
    } 

    public void OpenedStep(int index)
    {
        OpenStep(index);
    }

    public async UniTask OpenStep(int index)
    {
        if(index >= _steps.Length)
            return;
         _steps[index].OpenStep();
        await UniTask.WaitUntil(() => _steps[index].AnimatorController.GetCurrentAnimatorStateInfo(0).IsName("Opening"));

        // Espera terminar (normalizedTime >= 1)
        await UniTask.WaitUntil(() =>
        {
            var state = _steps[index].AnimatorController.GetCurrentAnimatorStateInfo(0);
            return state.IsName("Opening") && state.normalizedTime >= 1f;
        });
        _steps[index].ActiveBridgeStepPosition();       
    } 

    public void ClosedStep(int index)
    {
         _steps[index].CloseStep();
    }

    public void DefineBridgeIndex(uint index)
    {
        _bridgeIndex = index;
    }
} 
