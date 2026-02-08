using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit.Interactors; 
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class NetworkingPlayerController : NetworkBehaviour, IPlayer
{
    [SyncVar] private int _number;
    private Rigidbody _rigidBody;
    [SerializeField] private TrackedPoseDriver _camera;
    [SerializeField] private float _speed;
    [SerializeField] private InputActionReference _move;
    private CancellationTokenSource _cancellationTextTokenSource;
    public int Number => _number;
    [SyncVar(hook = nameof(SetViewName))] private string _name;
    [SerializeField] private List<InputActionAsset> m_ActionAssets;  
    [SerializeField] private TextMeshPro _nameView;
    [SerializeField] private GameObject[] _hands;
    [SerializeField] private XRRayInteractor _raycast;
    [SerializeField] private AssetReference _panelRef;

    private void OnDisable()
    {
        if(_cancellationTextTokenSource != null)
        {
            _cancellationTextTokenSource.Cancel();
        }
    }
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!isOwned)
            return;
        Vector2 direction = _move.action.ReadValue<Vector2>();
        var fixedDirection = Camera.main.transform.TransformDirection(new Vector3(direction.x, 0, direction.y) * _speed);
        fixedDirection.y = _rigidBody.linearVelocity.y;
        _rigidBody.linearVelocity = fixedDirection;
    } 
    public override void OnStartClient()
    {   
        var camera = GetComponentInChildren<Camera>();  
        if (!isOwned)
        { 
            LookToMainCamera().Forget();
            GetComponentInChildren<AudioListener>().enabled = false;  
            foreach(var controle in GetComponentsInChildren<ControllerInputActionManager>())
            {
                controle.enabled = false;
            }
            camera.enabled = false;
            foreach (var component in _hands[0].GetComponentsInChildren<XRHandTrackingEvents>().Concat(_hands[1].GetComponentsInChildren<XRHandTrackingEvents>()))
            { 
                component.enabled = false;
            }
        }
        else
        {
            EventManager.Instance.OnStartPointController?.Invoke(_number);
            var input = new XRIDefaultInputActions();
            input.Enable();
            _camera.positionAction = input.XRIHead.Position;
            _camera.rotationAction = input.XRIHead.Rotation; 
            var localPlayer = GameObject.Find("LocalPlayer");
            localPlayer.SetActive(false);
            GetComponentInChildren<AudioListener>().enabled = true; 
            camera.enabled = true; 
            EnableInput();
        }
         
        camera.tag = "MainCamera";
        var panel = _panelRef.InstantiateAsync().WaitForCompletion();
        panel.GetComponent<QuizController>().SetIndex(_number);
        panel.GetComponent<LazyFollow>().target = transform;
        base.OnStartClient();
    }
    public void Teleport()
    {
        if(_raycast.TryGetCurrent3DRaycastHit(out var hit))
        {
            Teleport(hit.point);
        } 
    }
    public void Teleport(Vector3 pos)
    {
        transform.position = pos;

    }
    public void SetIndex(int newName)
    {
        _number = newName;
        _name = $"Player: {newName}"; 

    }
    private void SetViewName(string oldName, string newName)
    {
        _nameView.text = newName;
        gameObject.name = newName;
    }
    private async UniTaskVoid LookToMainCamera()
    {
        if(_cancellationTextTokenSource != null)
        {
            _cancellationTextTokenSource.Cancel();
        }
        _cancellationTextTokenSource = new CancellationTokenSource();
        try
        { 
            while (true)
            {
                _nameView.transform.parent.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Vector3.up);
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f),cancellationToken:_cancellationTextTokenSource.Token);
            }
        }
        catch(Exception ex)
        {

        }
        finally
        {
            _cancellationTextTokenSource.Dispose();
            _cancellationTextTokenSource = null;
        } 
    }
    public void EnableInput()
    {
        Debug.Log("Enable");
        if (m_ActionAssets == null)
            return;

        foreach (var actionAsset in m_ActionAssets)
        {
            if (actionAsset != null)
            {
                actionAsset.Enable();
            }
        }
    }
    public void DisableInput()
    {
        Debug.Log("Disable"); 
        if (m_ActionAssets == null)
            return;

        foreach (var actionAsset in m_ActionAssets)
        {
            if (actionAsset != null)
            {
                actionAsset.Disable();
            }
        }
    }
}
