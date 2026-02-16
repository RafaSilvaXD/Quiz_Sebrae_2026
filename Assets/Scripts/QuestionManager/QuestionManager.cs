using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private List<GameQuestionScriptable> _allQuestions = new List<GameQuestionScriptable>();
    private const string PATH = "GameQuestion";

    [SerializeField] private LocalQuestionManager _localQuestion;
    [SerializeField] private MultiplayerQuestionManager _multiplayerQuestion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ReadAllQuestion();
    }

    public void ManagerPlayerQuestion(IPlayer playerReference, uint  playerId = 0)
    {
        if (playerId == 0)
        {
            _localQuestion.gameObject.SetActive(true);
            _localQuestion.SortPlayerQuestion(playerId, playerReference, _allQuestions);
        }
        else
        {
            _multiplayerQuestion.gameObject.SetActive(true);
            _multiplayerQuestion.SortPlayerQuestion(playerId, _allQuestions);
        }
    }

    private void ReadAllQuestion()
    {
        var questionsOrigin = Resources.LoadAll<GameQuestionScriptable>(PATH);

        foreach(var quest in questionsOrigin)
        {
            GameQuestionScriptable newQuestion = new GameQuestionScriptable();
            newQuestion.name = quest.name;
            newQuestion.Question = quest.Question;
            newQuestion.CorrectAnswer = quest.CorrectAnswer;
            _allQuestions.Add(newQuestion);
        }
    }
}
