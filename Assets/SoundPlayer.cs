using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [System.Serializable]
    public class EventSound
    {
        public EventSender sender;
        public FMODUnity.EventReference sound;

        public EventSound(EventSender sender, EventReference sound)
        {
            this.sender = sender;
            this.sound = sound;
        }
    }
    [SerializeField] private EventSound[] eventSounds = new EventSound[1];


    public void OnEnable()
    {
        foreach(EventSound es in eventSounds)
            if(es.sender) es.sender.OnActivate += PlaySound;
    }
    public void OnDisable()
    {
        foreach (EventSound es in eventSounds)
            if (es.sender) es.sender.OnActivate -= PlaySound;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaySound(null);   
    }
    private FMOD.Studio.EventInstance activeSoundInstance;
    private void PlaySound(EventSender sender)
    {
        for(int i = 0; i < eventSounds.Length; i++)
        {
            if(eventSounds[i].sender == sender)
            {
                activeSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                activeSoundInstance.release();

                activeSoundInstance = FMODUnity.RuntimeManager.CreateInstance(eventSounds[i].sound);
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(activeSoundInstance, transform);
                activeSoundInstance.start();
                    //FMODUnity.RuntimeManager.PlayOneShot(eventSounds[i].sound, transform.position);
                break;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
