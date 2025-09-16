using System;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets; 
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyTotemController : MonoBehaviour
{
    private bool _serverFound;
    private IEnumerator _coroutineWaitHost; 
    [SerializeField] private float _searchServerTime; 
    private void Start()
    {
        SearchServer(); 
    }
    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            StartServer(1);
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            StartServer(2);
        } 
    }
    private void SearchServer()
    {
        _coroutineWaitHost = Coroutine_WaitServer();
        StartCoroutine(_coroutineWaitHost);
    } 
    public void ServerFound(ServerResponse response)
    {
        NetworkManager.singleton.networkAddress = response.EndPoint.Address.ToString();
        NetworkManager.singleton.StartClient();
        StopCoroutine(_coroutineWaitHost);
    }
    public void StartServer(int amount)
    {
        NetworkManager.singleton.maxConnections = amount;
        NetworkManager.singleton.StartHost();
        var networkDiscovery = FindAnyObjectByType<NetworkDiscovery>();
        networkDiscovery.AdvertiseServer();
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    private IEnumerator Coroutine_WaitServer()
    {
        var networkDiscovery = FindAnyObjectByType<NetworkDiscovery>();
        networkDiscovery.StartDiscovery(); 
        yield return new WaitForSeconds(_searchServerTime); 
        NetworkManager.singleton.networkAddress = GetLocalIPAddress();
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private Func<bool> WaitHostOrDelayStart()
    {
#if UNITY_EDITOR
        if (ParrelSync.ClonesManager.GetArgument() != "client")
        {
            return () => _serverFound;
        }
#endif
        return () =>
        {
            return true;
        };
    }

    private string GetLocalIPAddress()
    {
        string localIP = "";
        foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
