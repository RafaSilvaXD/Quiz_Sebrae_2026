using UnityEngine;

public class LaunchController : MonoBehaviour
{
    private bool _firstTime = true;
    [SerializeField] private RocketController _rocket;
    private void OnTriggerEnter(Collider other)
    {
        if (!_firstTime)
            return;
        _firstTime = false;
        FindAnyObjectByType<GameplayManagerNetworking>().Rpc_Victory();
        _rocket.Engine();
    }
}
