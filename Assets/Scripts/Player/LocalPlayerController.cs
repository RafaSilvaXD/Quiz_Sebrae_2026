using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerController : MonoBehaviour, IPlayer
{
    [SerializeField] private Rigidbody rb;
   [SerializeField] private int _number;
    public int Number => _number;

    private uint _id;
    public uint ID => _id;

    [SerializeField] private List<GameQuestionScriptable> _questions;
    [SerializeField] private int _selectCurrentQuestion = 0;
    [SerializeField] private int _correctAnswers;
    [SerializeField] private int _wrongAnswers;
    [SerializeField] QuizController _quizController;

    [SerializeField] BridgeController _bridgePath;

    IEnumerator EnableCoroutine()
    {
        yield return new WaitUntil(() => EventManager.Instance != null);
        EventManager.Instance.OnConnectPlayerInSeasson += ConnectedPlayerInSeasson;
        EventManager.Instance.OnQuestionAnswer += QuestionAnswered;
        EventManager.Instance.OnShowQuestion += ShowedQuestion;
    }
    void OnEnable()
    {
        StartCoroutine(EnableCoroutine());
    }

    void OnDisable()
    {
        EventManager.Instance.OnConnectPlayerInSeasson -= ConnectedPlayerInSeasson;
        EventManager.Instance.OnQuestionAnswer -= QuestionAnswered;
        EventManager.Instance.OnShowQuestion -= ShowedQuestion;
    }


    public void ConnectedPlayerInSeasson()
    {
       _id = 0;
       _selectCurrentQuestion = 0;
    }

    public void ReceiveQuestions(List<GameQuestionScriptable> chosenQuestions)
    {
        _questions = chosenQuestions;
        _quizController.SetupQuestion(_questions[_selectCurrentQuestion]);
    }

    private void QuestionAnswered(bool wasCorrect)
    {
        _selectCurrentQuestion++;
        if (wasCorrect)
        {
            _bridgePath.OpenedStep(_correctAnswers);
            _correctAnswers++;
        }
        else
        {
            if(_correctAnswers > 0)
            {
                _bridgePath.ClosedStep(_wrongAnswers);
                _wrongAnswers++;
                if(_wrongAnswers < _correctAnswers)
                    ShowedQuestion();
            }
        }

        if(_correctAnswers == 0)
        {
            ShowedQuestion();
        }
    }

    public void ConnectPlayerInPath(BridgeController bridgePath)
    {
        _bridgePath = bridgePath;
        _bridgePath.DefineBridgeIndex(_id);
    }

    public void ShowedQuestion()
    {
        if(_selectCurrentQuestion == _questions.Count)
        {
            _selectCurrentQuestion = 0;
            EventManager.Instance.OnGetQuestions?.Invoke(this, _id);
        }
        else
        {
            _quizController.SetupQuestion(_questions[_selectCurrentQuestion]);            
        }
    }

    public void TeleportTo(Vector3 targetPosition)
    {
        rb.position = targetPosition;
        rb.linearVelocity = Vector3.zero;
    }

    public void ReinitializeQuestions()
    {
        _correctAnswers = 0;
        _wrongAnswers = 0;
        _selectCurrentQuestion = 0;
        EventManager.Instance.OnGetQuestions?.Invoke(this, _id);

    }
}
