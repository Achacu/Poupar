using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private EventSender sender;
    [SerializeField] private FMODUnity.EventReference sound;
    public void OnEnable()
    {
        sender.OnActivate += PlaySound;
    }
    public void OnDisable()
    {
        sender.OnActivate -= PlaySound;
    }

    private void PlaySound(EventSender obj)
    {
        FMODUnity.RuntimeManager.PlayOneShot(sound, transform.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
