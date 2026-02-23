using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EnviromentNetworkBehaviour : NetworkBehaviour
{ 
    [SerializeField] private AssetReference[] _audiosStartRef; 

    [SerializeField] private AudioSource _audioSource;
       
    [ClientRpc]
    public void Rpc_Open()
    {
        EventManager.Instance.OnOpenDoor?.Invoke();
    }
    public void NextPlayer(string message, int index)
    {
        Rpc_NextPlayer(message,index);
    }
    [ClientRpc]
    public async void Rpc_NextPlayer(string message, int index)
    {
        /*var selected = FindObjectsByType<QuizController>(FindObjectsSortMode.None).ToArray();
        foreach (var quiz in selected) 
        {
            if(quiz.Index == index)
            {
                await quiz.SetupQuestion(message);
            }
            else
            {
                quiz.Hidden();
            }
        }*/
    }

     
     
    public override void OnStartClient()
    {  
        var localPlayer = GameObject.Find("LocalPlayer");
        if (localPlayer != null)
        {
            localPlayer.SetActive(false);
        }
        base.OnStartClient();
    } 
    public void NextStep()
    {
        cmd_NextStep();
    }
    [Command(requiresAuthority = false)]
    public void cmd_NextStep()
    {
        var gameplayManager = FindFirstObjectByType<GameplayManagerNetworking>();
        Rpc_NextStep(gameplayManager.CurrentPlayer);
    }
    [ClientRpc]
    private void Rpc_NextStep(int index)
    {

        /*FindObjectsByType<BridgeController>(FindObjectsSortMode.None)
            .First(x => x.BridgeIndex == index).NextStep();*/
    }
    public void ResetSteps()
    {
        cmd_ResetSteps();
    }
    [Command(requiresAuthority =false)]
    public void cmd_ResetSteps()
    {
        var gameplayManager = FindFirstObjectByType<GameplayManagerNetworking>();
        Rpc_ResetSteps(gameplayManager.CurrentPlayer);
    }
    [ClientRpc]
    private async void Rpc_ResetSteps(int index)
    {
        /*await FindObjectsByType<BridgeController>(FindObjectsSortMode.None)
           .First(x => x.BridgeIndex == index).ResetSteps(true);*/
    }
}
