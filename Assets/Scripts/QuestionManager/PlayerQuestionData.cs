using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerQuestionData
{
    public uint ID;
    public IPlayer PlayerReference;
    public List<GameQuestionScriptable> ChosenQuestions;
}
