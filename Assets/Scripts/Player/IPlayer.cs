using System.Collections.Generic;
using UnityEngine;

public interface IPlayer  
{
   public int Number { get; }
   public uint ID {get;}

   public void ConnectedPlayerInSeasson();
   public void ReceiveQuestions(List<GameQuestionScriptable> chosenQuestions);

   public void ConnectPlayerInPath(BridgeController bridgePath);

   public void TeleportTo(Vector3 targetPosition);
   public void ReinitializeQuestions();
}
