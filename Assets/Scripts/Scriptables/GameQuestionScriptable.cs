using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameQuestion", menuName = "Scriptable Objects/GameQuestion")]
public class GameQuestionScriptable : ScriptableObject
{
    public string Question;
    public List<string> Answers = new List<string>();
    [Range(0,3)] public int CorrectAnswer;

    public bool CheckCorrectAnswer(int answer)
    {
        return CorrectAnswer == answer;
    }
}
