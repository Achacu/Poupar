using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    void OnValidate()
    {
        if (!eventSender) eventSender = GetComponent<EventSender>();
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
    public void Awake()
    {
        OnValidate();
    }

    public EventSender eventSender;
    public void Interact()
    {
        eventSender.TriggerEvent(true);
    }
}
