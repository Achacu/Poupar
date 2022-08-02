using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionModifier : MonoBehaviour
{
    [SerializeField] private Interactable interaction;
    [SerializeField] private EventSender triggerSender;
    [SerializeField] private EventSender newSender;
    // Start is called before the first frame update
    public void OnEnable()
    {
        triggerSender.OnActivate += ModifyInteraction;
    }
    public void OnDisable()
    {
        triggerSender.OnActivate -= ModifyInteraction;
    }

    private void ModifyInteraction(EventSender obj)
    {
        interaction.eventSender = newSender;
    }

}
