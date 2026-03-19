using System;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets; 
using Mirror;
using Mirror.Discovery;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyTotemController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _lobbyCanvas;
    private bool _serverFound;
    private IEnumerator _coroutineWaitHost; 
    [SerializeField] private float _searchServerTime;
    [SerializeField] private GameObject _loopitaSingleGame;
    [SerializeField] private TextMeshProUGUI _gameVersion;
    [SerializeField] private const string DEFAULT_VERSION_TEXT = "Game Version: ";

    void Start()
    {
        _gameVersion.text = DEFAULT_VERSION_TEXT + PlayerSettings.bundleVersion;
        ShowCanvas(true);
    }
    public void StartGame(int amountPlayer)
    {
        if(amountPlayer == 1)
        {
            StartLocalGame();
        }
        else
        {
            StartNetworkGame(amountPlayer);
        }
    } 

    private void StartLocalGame()
    {
        Debug.Log("Start Local Game");
        EventManager.Instance.OnConnectPlayerInSeasson?.Invoke();
        ShowCanvas(false);
        Instantiate(_loopitaSingleGame);
    }

    private void StartNetworkGame(int amount)
    {
        Debug.Log("This will implement soon");
    }

    private void SearchServer()
    {
        _coroutineWaitHost = Coroutine_WaitServer();
        StartCoroutine(_coroutineWaitHost);
    } 
    public void ServerFound(ServerResponse response)
    {
        Debug.Log("Start Client test");
        NetworkManager.singleton.networkAddress = response.EndPoint.Address.ToString();
        NetworkManager.singleton.StartClient();
        StopCoroutine(_coroutineWaitHost);
    }

    public void StartServer(int amount)
    {
        Debug.Log("Start Server test");
        NetworkManager.singleton.maxConnections = amount;
        NetworkManager.singleton.StartHost();
        var networkDiscovery = FindAnyObjectByType<NetworkDiscovery>();
        networkDiscovery.AdvertiseServer();
        ShowCanvas(false);
    }
    private IEnumerator Coroutine_WaitServer()
    {
        var networkDiscovery = FindAnyObjectByType<NetworkDiscovery>();
        networkDiscovery.StartDiscovery(); 
        yield return new WaitForSeconds(_searchServerTime); 
        NetworkManager.singleton.networkAddress = GetLocalIPAddress();
        ShowCanvas(true);
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

    private void ShowCanvas(bool status)
    {
        var value = status ? 1 : 0;
        _lobbyCanvas.alpha = value;
        GetComponent<CanvasGroup>().blocksRaycasts = status;
    }
}
