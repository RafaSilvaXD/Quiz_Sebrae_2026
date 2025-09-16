using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets; 
using UnityEngine.XR;

public class GlassController : MonoBehaviour
{ 
    private bool _checked;
    [SerializeField] private AssetReference _refGlassBroken; 
    [SerializeField] private AssetReference _audioStart;
    [SerializeField] private AssetReference _audioBroken; 
    [SerializeField] private AudioSource _source;
    [SerializeField] private BoxCollider _box;

    private void OnTriggerEnter()
    {
        if (_checked)
            return;
        _checked = true;  
    } 
    public async void Drop()
    {
        _source?.PlayOneShot(Addressables.LoadAssetAsync<AudioClip>(_audioStart).WaitForCompletion());
        Vib();
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        var broken = Addressables.InstantiateAsync(_refGlassBroken, transform.GetChild(0).transform.position,Quaternion.identity).WaitForCompletion();
        foreach (var glass in broken.GetComponentsInChildren<Rigidbody>())
        {
            glass.isKinematic = false;
            glass.AddForce(new Vector3(UnityEngine.Random.Range(0, 1f), -1, UnityEngine.Random.Range(0, 1f)).normalized, ForceMode.Impulse);
            Destroy(glass.gameObject, 3);
        }
        transform.GetChild(0).gameObject.SetActive(false);
        _box.isTrigger = true;
        _source?.PlayOneShot(_audioBroken.LoadAssetAsync<AudioClip>().WaitForCompletion());
    }
    public void Restore()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        _box.isTrigger = false;
    }

    private void Vib()
    {
        uint channel = 0;
        float amplitude = 1f;
        float duration = 0.5f;
        if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetHapticCapabilities(out var capabilitiesL))
        {
            if (capabilitiesL.supportsImpulse)
            { 
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(channel, amplitude, duration);
            }
        }
        if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetHapticCapabilities(out var capabilitiesR))
        {
            if (capabilitiesR.supportsImpulse)
            { 
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }
} 