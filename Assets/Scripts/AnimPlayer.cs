using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlayer : MonoBehaviour
{
    [SerializeField] private Animation anim;
    [SerializeField] private EventSender sender;
    // Start is called before the first frame update
    void OnEnable()
    {
        sender.OnActivate += PlayAnimation;
    }
    void OnDisable()
    {
        sender.OnActivate -= PlayAnimation;
    }

    private void PlayAnimation(EventSender obj)
    {
        anim.Play();
    }
}
