using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mirror;
using Newtonsoft.Json;
using UnityEngine;

public class GameplayManagerNetworking : NetworkBehaviour
{
    [SyncVar] private int _currentPlayer = 0;
    private bool _defeat;
    private int _playersReady;
    private NetworkController _networkController;
    private Question[] _questions;
    private Dictionary<int, int> _playerScore = new();
    

    public int CurrentPlayer { get => _currentPlayer;}

    private void Awake()
    {
        _networkController = FindAnyObjectByType<NetworkController>();
    } 
    public override void OnStartServer()
    {
        for(int index = 0; index < NetworkServer.connections.Count; index++)
        {
            _playerScore.Add(index + 1, 0);
        }
        StartGame().Forget(); 
        _currentPlayer = UnityEngine.Random.Range(1, NetworkServer.connections.Count+1);
        base.OnStartServer();
    }
    [Server] 
    public async UniTaskVoid StartGame()
    {
        _questions = await _networkController.GetQuestions();
        FindAnyObjectByType<LoopitaNetworkController>().PlayStartAudio();
        await UniTask.Delay(TimeSpan.FromSeconds(55));
        FindAnyObjectByType<LoopitaNetworkController>().Rpc_MovePointB(); 
    }

    [Server] 
    public async void ShowQuiz()
    {
        var question = _questions.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        if (question != null)
        {
            _questions = _questions.Where(x => x.Id != question.Id).ToArray();
            FindAnyObjectByType<EnviromentNetworkBehaviour>().NextPlayer(JsonConvert.SerializeObject(question), _currentPlayer);
        }
        else
        {
            _questions = await _networkController.GetQuestions();
            ShowQuiz();
        }
    }
    [Command(requiresAuthority = false)]
    public void Cmd_NextPlayer(bool positive)
    {
        if (_defeat)
            return;
        if (positive)
        {
            _playerScore[_currentPlayer]++;
        }
        else
        {
            _playerScore[_currentPlayer] = 0;
        }
        if (_playerScore[_currentPlayer] >= 5)
        {
            return;
        }
        NextPlayer();
    }
    [Server]
    public async void NextPlayer()
    {
        if(_currentPlayer == NetworkServer.connections.Count)
        {
            _currentPlayer = 0;
        }
        _currentPlayer++;
        FindAnyObjectByType<LoopitaNetworkController>().Rpc_TalkPlayer(_currentPlayer);
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        ShowQuiz();
    }
    [ClientRpc]
    public async void Rpc_Victory()
    {
        /*var paths = FindObjectsByType<BridgeController>(FindObjectsSortMode.None).Where(x => x.BridgeIndex != _currentPlayer);
        foreach (var path in paths) 
        {
            await path.ResetSteps();
        }
        FindAnyObjectByType<VictoryPanelController>().Show().Forget();
        FindAnyObjectByType<DefeatPanelController>().Show().Forget();*/
    }
    [Command(requiresAuthority =false)]
    public void Cmd_Defeat()
    {
        Rpc_Defeat();
    }
    [ClientRpc]
    public async void Rpc_Defeat()
    {
        _defeat = true;
        var paths = FindObjectsByType<BridgeController>(FindObjectsSortMode.None);
        foreach (var path in paths)
        {
            await path.ResetSteps();
        }
    }
    [Command(requiresAuthority =false)]
    public void cmd_Ready()
    {
        _playersReady++;
        if(_playersReady == NetworkServer.connections.Count)
        {
            //FindAnyObjectByType<TimerController>().StartTime().Forget();
            NextPlayer();
        }
    }
}
