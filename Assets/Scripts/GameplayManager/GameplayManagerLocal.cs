using UnityEngine;

public class GameplayManagerLocal : MonoBehaviour
{

    [SerializeField] private QuestionManager _questionManager;
    [SerializeField] private TimerController _timer;
    
    [SerializeField] private FallPlace _fallPlace;
    [SerializeField] private LaunchController _launchController;
    [SerializeField] private Transform _restartPosition;
    [SerializeField] private DefeatPanelController _defeatPanel;
    [SerializeField] private VictoryPanelController _victoryPanel;

    private IPlayer _playerReference;
    private bool _gamingIsWorking;

    void Start()
    {
        _fallPlace.DefineAction(PlayerFallAction);
        _launchController.DefineAction(PlayerVictory);
    }

    private void PlayerFallAction(IPlayer player)
    {
        if (_playerReference == player)
        {
            FinishGame(_restartPosition.position);
        }
        else
        {
            player.TeleportTo(_restartPosition.position);
        }
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
        FinishGame(_restartPosition.position);
    }

    private void PlayerVictory()
    {
        _victoryPanel.Show().Forget();
        _timer.StopTimer();
    }

    private void FinishGame(Vector3 teleportPosition)
    {
        EventManager.Instance.OnFinishGame?.Invoke();
        _playerReference.TeleportTo(teleportPosition);
        _defeatPanel.Show().Forget();
        _timer.ResetTimer();
        _playerReference = null;
    }
}
