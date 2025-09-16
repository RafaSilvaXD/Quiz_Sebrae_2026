using UnityEngine;

public class BridgeStepController:MonoBehaviour
{
    private Transform _step;
    private GlassController _glass;
    [SerializeField] private int _index;

    private void Awake()
    {
        _glass = GetComponentInChildren<GlassController>();
    } 
    public void Drop()
    {
        _glass.Drop();
    }
    public void Restore()
    {
        _glass.Restore();
    }
    public int Index { get => _index;}
    public Transform Step { get 
        {
            if(_step == null)
            {
                _step = transform.GetChild(0);
            }
            return _step;
        } }
}
