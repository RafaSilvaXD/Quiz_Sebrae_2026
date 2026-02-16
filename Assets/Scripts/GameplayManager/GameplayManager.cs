using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameplayManagerLocal _localManager;
    [SerializeField] private GameplayManagerNetworking _multiplayerManager;
    
    IEnumerator EnableCoroutine()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);
        Debug.Log(EventManager.Instance);
        EventManager.Instance.OnConnectPlayerInGame += ConnectedPlayerInGame;
    }
    void OnEnable()
    {
        StartCoroutine(EnableCoroutine());
    }

    void OnDisable()
    {
        EventManager.Instance.OnConnectPlayerInGame -= ConnectedPlayerInGame;
    }

        private void ConnectedPlayerInGame(IPlayer playerReference, uint  playerId = 0)
    {
        if (playerId == 0)
        {
            _localManager.gameObject.SetActive(true);
            _localManager.AddPlayerToSession(playerReference, playerId);
        }
        else
        {
            _multiplayerManager.gameObject.SetActive(true);
        }
    }
}
