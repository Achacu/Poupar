using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private bool disableOnInteract = false;
    // Start is called before the first frame update
    void OnValidate()
    {
        if (!eventSender) eventSender = GetComponent<EventSender>();
        //print(gameObject.layer + " " + LayerMask.NameToLayer("Interactable"));
        if(gameObject.layer != LayerMask.NameToLayer("Interactable")) gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
    public void Awake()
    {
        OnValidate();
    }

    public EventSender eventSender;
    public void Interact()
    {
        eventSender.TriggerEvent(true);
        if (disableOnInteract) gameObject.SetActive(false);
    }
}
