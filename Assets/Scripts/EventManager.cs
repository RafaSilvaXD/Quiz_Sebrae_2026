using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public Action OnShowQuestion;

    public Action OnOpenDoor;
    public Action<int> OnStartPointController;

    public Action<IPlayer, uint> OnConnectPlayerInGame;
    public Action<IPlayer, uint> OnGetQuestions;
    public Action OnConnectPlayerInSeasson;

    public Action<bool> OnQuestionAnswer;

    public Action<int> OnOpenStep;
    public Action<int> OnCloseStep;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
