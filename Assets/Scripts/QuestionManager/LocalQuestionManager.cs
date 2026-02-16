using System.Collections.Generic;
using UnityEngine;

public class LocalQuestionManager : MonoBehaviour
{
    [SerializeField] private int maxChosenQuestions;
    [SerializeField] private PlayerQuestionData _playersQuestion = new PlayerQuestionData();

    public void SortPlayerQuestion(uint playerConnectionId, IPlayer playerReference, List<GameQuestionScriptable> allQuestions){
        
        List<GameQuestionScriptable> chosenQuestions = new List<GameQuestionScriptable>();

        for(int i = 0; i < maxChosenQuestions; i++)
        {
            if(allQuestions.Count <= 0)            
                return;
            
            var index = Random.Range(0, allQuestions.Count);
            GameQuestionScriptable choose = allQuestions[index];
            chosenQuestions.Add(choose);
            allQuestions.RemoveAt(index);
        }
        _playersQuestion.ID = playerConnectionId;
        _playersQuestion.PlayerReference = playerReference;
        _playersQuestion.ChosenQuestions = chosenQuestions;

        playerReference.ReceiveQuestions(_playersQuestion.ChosenQuestions);
    }
}
