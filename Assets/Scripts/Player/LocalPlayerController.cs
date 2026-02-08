using UnityEngine;

public class LocalPlayerController : MonoBehaviour, IPlayer
{
   [SerializeField] private int _number;
    public int Number => _number;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
