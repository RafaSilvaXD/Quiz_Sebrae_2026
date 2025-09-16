using UnityEngine;

public class StartPointController : MonoBehaviour
{
    private bool _ready;
    [SerializeField] private int _index;
    [SerializeField] private ParticleSystem _particle;

    public int Index { get => _index; set => _index = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (_ready)
            return;
        if (other.TryGetComponent(out NetworkingPlayerController player))
        {
            if (player.Number == _index)
            {
                _particle.Stop();
                _ready = true;
                FindAnyObjectByType<GameplayManagerNetworking>().cmd_Ready();
            }
        }
    }
}
