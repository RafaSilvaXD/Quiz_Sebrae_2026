using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BridgeStepController:MonoBehaviour
{
    [SerializeField] private GameObject _armature;
    [SerializeField] private Animator _animatorController;

    public Animator AnimatorController => _animatorController;
    [SerializeField] private int _stepIndex;
    [SerializeField] List<Color> _stepColors;
    [SerializeField] private StepPosition _stepPosition;

    void Start()
    {
        _armature.GetComponent<Renderer>().material.color = _stepColors[_stepIndex];
    }

    public void OpenStep()
    {
        _animatorController.SetBool("IsOpen", true);
    }

    public void CloseStep()
    {
        _animatorController.SetBool("IsOpen", false);
    }

    public void DefineBridgeStepIndex(uint bridgeIndex)
    {
        _stepPosition.DefineStepIndex(bridgeIndex);
    }

    public void ActiveBridgeStepPosition()
    {
        _stepPosition.gameObject.SetActive(true);
        _stepPosition.ShowStepVFX();
    }
}
