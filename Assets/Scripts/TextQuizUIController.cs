using System; 
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextQuizUIController : MonoBehaviour
{
    private Action<bool> _callback;
    [SerializeField] private Button[] _buttonAnswers;
    [SerializeField] private TextMeshProUGUI[] _textAnswers;
    [SerializeField] private TextMeshProUGUI _textQuestion;
    [SerializeField] private int _correctAnswerIndex;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioCorrectAnswer;
    [SerializeField] private AudioClip _audioWrongAnswer;

    [SerializeField] private Color _defaultButtonCollor;
    [SerializeField] private Color _correctButtonCollor;
    [SerializeField] private Color _wrongButtonCollor;

    [SerializeField] private int _blinkCount;
    [SerializeField] private float _blinkFrequency;

    public void SetupQuestionUI(GameQuestionScriptable question, Action<bool> callback)
    {
        _callback = callback;
        _textQuestion.text = question.Question;

        for(int i = 0; i < _textAnswers.Length; i++)
        {
            _textAnswers[i].text = question.Answers[i];
        }
        _correctAnswerIndex = question.CorrectAnswer;
    }

    public void ChosenAnswer(int indexQuestion)
    {
        ChosenAnswerEffect(indexQuestion, indexQuestion == _correctAnswerIndex);
    }

    public async UniTask ChosenAnswerEffect(int indexQuestion, bool isCorrect)
    {
        var colorAnswer = isCorrect ? _correctButtonCollor : _wrongButtonCollor;
        var audioAnswer = isCorrect ? _audioCorrectAnswer : _audioWrongAnswer;
        _audioSource.clip = audioAnswer;
        _audioSource.Play();
        AnswersButtonInteractable(false);

        for(int i =0; i < _blinkCount; i++)
        {
            if(i % 2 == 0)
            {
                _buttonAnswers[indexQuestion].targetGraphic.color = colorAnswer;
            }
            else
            {
                 _buttonAnswers[indexQuestion].targetGraphic.color = _defaultButtonCollor;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(_blinkFrequency));
        }

        AnswersButtonInteractable(true);
        _callback.Invoke(indexQuestion == _correctAnswerIndex);
    }

    private void AnswersButtonInteractable(bool value)
    {
        foreach(var button in _buttonAnswers)
        {
            button.interactable = value;
        }
    }
}
