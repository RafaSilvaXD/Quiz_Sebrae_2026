using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerController : MonoBehaviour, IPlayer
{
   [SerializeField] private int _number;
    public int Number => _number;

    private uint _id =0;

    [SerializeField] private List<GameQuestionScriptable> _questions;

    public void ConnectPlayerInGame()
    {
        throw new System.NotImplementedException();
    }

    public void ReceiveQuestions(List<GameQuestionScriptable> chosenQuestions)
    {
        _questions = chosenQuestions;
    }
}
