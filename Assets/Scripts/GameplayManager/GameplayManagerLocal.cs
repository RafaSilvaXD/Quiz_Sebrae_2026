using UnityEngine;

public class GameplayManagerLocal : MonoBehaviour
{

    [SerializeField] private QuestionManager _questionManager;
    [SerializeField] private TimerController _timer;
    
    [SerializeField] private FallPlace _fallPlace;
    [SerializeField] private Transform _restartPosition;
    [SerializeField] private DefeatPanelController _defeatPanel;
    private IPlayer _playerReference;

    void Start()
    {
        _fallPlace.DefineAction(PlayerFallAction);
    }

    private void PlayerFallAction()
    {
        _playerReference.TeleportTo(_restartPosition.position);
        _playerReference.ReinitializeQuestions();
    }

    public void AddPlayerToSession(IPlayer playerReference, uint  playerId = 0)
    {
        _playerReference = playerReference;
        _questionManager.ManagerPlayerQuestion(playerReference, playerId);
        _timer.StartTime(FinishTimer);
    }

    public void GetNewQuestions(IPlayer playerReference, uint  playerId = 0)
    {
         _questionManager.SendNewQuestions(playerReference, playerId);
    }

    private void FinishTimer()
    {
        EventManager.Instance.OnFinishGame?.Invoke();
        _playerReference.TeleportTo(_restartPosition.position);
        _defeatPanel.Show();
    }
}
