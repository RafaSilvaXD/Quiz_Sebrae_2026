using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private float _distance;
    public void OpenedDoor()
    {
        GetComponent<AudioSource>().Play();
        StartCoroutine(Coroutine_Open());
    }

    void OnEnable()
    {
        Debug.Log(EventManager.Instance);
        EventManager.Instance.OnOpenDoor += OpenedDoor;
    }

    void OnDisable()
    {
        EventManager.Instance.OnOpenDoor -= OpenedDoor;
    }
    
    private IEnumerator Coroutine_Open()
    {
        float elapsedTime = 0;
        Vector3 startPosition = transform.position;
        while (true)
        {
            elapsedTime += Time.deltaTime/_time;
            transform.position = Vector3.Lerp(startPosition, startPosition - _direction * _distance, elapsedTime);
            if (elapsedTime >= 1)
            {
                break;
            }
            yield return null;
        }
    }
}
