using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent uEvent;
    public GameObject TiggerObject;
    
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == TiggerObject)
        {
            uEvent.Invoke();
        }
    }
}
