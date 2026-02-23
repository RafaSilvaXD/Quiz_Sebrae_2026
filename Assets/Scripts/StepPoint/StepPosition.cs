using UnityEngine;

public class StepPosition : MonoBehaviour
{
    [SerializeField] private uint _index;
    [SerializeField] private ParticleSystem _particle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DefineStepIndex(uint bridgeIndex)
    {
        _index = bridgeIndex;
    }

    public void ShowStepVFX()
    {
        _particle.Play();
    }
    void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out IPlayer player);
        if(player == null)
            return;
        
        if (player.ID == _index)
        {
           gameObject.SetActive(false);
           EventManager.Instance.OnShowQuestion?.Invoke();
        }
    }
}
