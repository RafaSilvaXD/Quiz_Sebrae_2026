using UnityEngine;

public class GameplayManagerLocal : MonoBehaviour
{

    [SerializeField] private QuestionManager _questionManager;
    [SerializeField] private TimerController _timer;

    public void AddPlayerToSession(IPlayer playerReference, uint  playerId = 0)
    {
        _questionManager.ManagerPlayerQuestion(playerReference, playerId);
        _timer.StartTime();
    }
}
