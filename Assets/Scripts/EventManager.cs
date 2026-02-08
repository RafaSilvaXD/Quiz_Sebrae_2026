using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
        public static EventManager Instance { get; private set; }

    public Action OnOpenDoor;
    public Action<int> OnStartPointController;

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
