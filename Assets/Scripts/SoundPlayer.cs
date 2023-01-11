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
        activeSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaySound(null);   
    }
    private FMOD.Studio.EventInstance activeSoundInstance;
    public event Action<FMOD.Studio.EventInstance, SoundPlayer> OnPlaySound = delegate { };
    private void PlaySound(EventSender sender)
    {
        for(int i = 0; i < eventSounds.Length; i++)
        {
            if(eventSounds[i].sender == sender)
            {
                PlaySound(eventSounds[i].sound);
                    //FMODUnity.RuntimeManager.PlayOneShot(eventSounds[i].sound, transform.position);
                break;
            }
        }
    }    

    public void PlaySound(EventReference sound)
    {
        activeSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        activeSoundInstance.release();

        if (sound.IsNull) return;

        activeSoundInstance = FMODUnity.RuntimeManager.CreateInstance(sound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(activeSoundInstance, transform);
        activeSoundInstance.start();

        OnPlaySound.Invoke(activeSoundInstance, this);
    }


    private float GetMax(float[] array)
    {
        float max = 0f;
        for(int i = 0; i < array.Length; i++)
        {
            if (array[i] > max) max = array[i];
        }
        return max;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
