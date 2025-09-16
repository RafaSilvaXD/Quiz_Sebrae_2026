using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mirror;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets; 

public class QuizController : MonoBehaviour
{  
    [SerializeField] private RectTransform _quizContainer;
    private int _index;

    public int Index => _index;  
    
    public void SetIndex(int index)
    {
        _index = index;
    }
    public void Hidden()
    {
        GetComponent<CanvasGroup>().alpha = 0; 
    }
    public async UniTask SetupQuestion(string question)
    {
        var selectedQuestion = JsonConvert.DeserializeObject<Question>(question);
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if(selectedQuestion.Type == 0)
        {
            var quiz = await Addressables.InstantiateAsync("TextQuiz", parent: _quizContainer.transform);
            quiz.GetComponent<TextQuizUIController>().SetupQuestion(selectedQuestion, async ctx =>
            {
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                await UniTask.Delay(TimeSpan.FromSeconds(2)); 
                Destroy(_quizContainer.GetChild(0).gameObject);
                GetComponent<CanvasGroup>().alpha = 0; 
                if (ctx)
                {
                    FindAnyObjectByType<EnviromentNetworkBehaviour>().NextStep();
                }
                else
                {
                    FindAnyObjectByType<EnviromentNetworkBehaviour>().ResetSteps();
                } 
                FindAnyObjectByType<GameplayManagerNetworking>().Cmd_NextPlayer(ctx);
            });
        }
    }
}
public class Question
{
    public int Id { get; set; }
    public int Type { get; set; }
    public string Text { get; set; }
    public Answer[] Answer { get; set; }
}
public class Answer
{
    public bool Correct { get; set; } 
    public string Text { get; set; } 
}
