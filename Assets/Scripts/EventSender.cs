using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventSender : MonoBehaviour
{
    public Action<EventSender> OnActivate = delegate { };
    public Action<EventSender> OnDeactivate = delegate { };

    [ContextMenuItem("TriggerEventTrue", "TriggerEventTrue")]
    [SerializeField] private bool singleUse = false;

    private bool hasBeenUsed = false;
    public void TriggerEventTrue() => TriggerEvent(true);
    public void TriggerEvent(bool on)
    {
        if (singleUse && hasBeenUsed) return;

        if (on) OnActivate.Invoke(this);
        else OnDeactivate.Invoke(this);
        hasBeenUsed = true;
    }
}
