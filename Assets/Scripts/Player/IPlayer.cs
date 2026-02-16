using System.Collections.Generic;
using UnityEngine;

public interface IPlayer  
{
   public int Number { get; } 

   public void ConnectPlayerInGame();
   public void ReceiveQuestions(List<GameQuestionScriptable> chosenQuestions);
}
