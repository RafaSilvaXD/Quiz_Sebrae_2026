using UnityEngine;
using System;
using System.Collections;

public class StartPointController : MonoBehaviour
{
    private bool _ready;
    [SerializeField] private int _index;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private BridgeController _bridgePath;
    void Start()
    {
        StartedPointController(-1);
    }
    IEnumerator EnableCoroutine()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);
        Debug.Log(EventManager.Instance);
        EventManager.Instance.OnStartPointController += StartedPointController;
    }
    void OnEnable()
    {
        StartCoroutine(EnableCoroutine());
    }

    void OnDisable()
    {
        EventManager.Instance.OnStartPointController -= StartedPointController;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (_ready)
            return;
        if (other.TryGetComponent(out NetworkingPlayerController player))
        {
            if (player.Number == _index)
            {
                _particle.Stop();
                _ready = true;
                FindAnyObjectByType<GameplayManagerNetworking>().cmd_Ready();
            }
        }
    }*/

    private void StartedPointController(int playerIndex)
    {
        _particle.gameObject.SetActive(_index == playerIndex);
    }

    
    void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out IPlayer player);
        if(player == null)
            return;
        
        if (player.Number == _index)
        {
            EventManager.Instance.OnConnectPlayerInGame?.Invoke(player, 0);
            player.ConnectPlayerInPath(_bridgePath);
            gameObject.SetActive(false);
        }
    }
}
