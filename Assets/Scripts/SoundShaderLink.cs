using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundShaderLink : MonoBehaviour
{
    [SerializeField] private SoundPlayer soundPlayer; //LEGACY
    [SerializeField] private List<SoundPlayer> soundPlayers =new List<SoundPlayer>(1);
    [SerializeField] private List<MeshRenderer> meshes;

    void OnValidate()
    {
        if (meshes.Count == 0 || meshes[0] == null)
        {
            meshes.AddRange(GetComponentsInChildren<MeshRenderer>());
            if (meshes[0] == null) meshes.RemoveAt(0);
        }
        if(soundPlayers.Count == 0) soundPlayers.Add(soundPlayer); //LEGACY
    }
    public void Awake()
    {
        OnValidate();
        for (int i = 0; i < meshes.Count; i++) for (int j = 0; j < meshes[i].materials.Length; j++)
        {
            meshes[i].material.SetFloat("_Sounding", 0);
            meshes[i].material.renderQueue = 3000;
            meshes[i].materials[j].SetFloat("_OverrideAlpha", -1);
            if(hasOwnDitherScale) meshes[i].materials[j].SetFloat("_DitherScale", ownDitherScale);
        }
        lastValues = new float[soundPlayers.Count];
    }


    // Start is called before the first frame update
    public void OnEnable()
    {
        foreach(SoundPlayer soundPlayer in soundPlayers)
            soundPlayer.OnPlaySound += StartSoundMetering;
    }
    public void OnDisable()
    {
        foreach (SoundPlayer soundPlayer in soundPlayers)
            soundPlayer.OnPlaySound -= StartSoundMetering;
    }

    private EventInstance instance;
    private FMOD.ChannelGroup channelGroup;
    private FMOD.DSP dsp;
    private FMOD.DSP_METERING_INFO dspMeterInfo;
    private void StartSoundMetering(EventInstance obj, SoundPlayer soundPlayer)
    {
        instance = obj;
        instance.getChannelGroup(out channelGroup);
        channelGroup.getDSP(0, out dsp);
        dsp.setMeteringEnabled(true, true);
        dsp.getMeteringInfo(new IntPtr(), out dspMeterInfo);

       // if(meteringCorot != null)
       // {
       //     StopCoroutine(meteringCorot);
       //     meteringCorot = null;
       // }
        meteringCorot = StartCoroutine(MeterSound(instance,soundPlayers.IndexOf(soundPlayer)));
    }
    private Coroutine meteringCorot;
    [SerializeField] private float maxValue = 0.1f;
    [SerializeField] private float minValue = 0.0001f;
    [SerializeField] private float maxDecrease = 0.1f;
    [SerializeField] private float maxIncrease = 1f;
    private float currentValue;
    private float[] lastValues;
    private IEnumerator MeterSound(EventInstance instance, int i)
    {
        print("started meter sound: " + gameObject.name);
        while (true)
        {
            instance.getChannelGroup(out channelGroup);
            channelGroup.getDSP(0, out dsp);
            dsp.setMeteringEnabled(true, true);
            dsp.getMeteringInfo(new IntPtr(), out dspMeterInfo);
            //print(dspMeterInfo.peaklevel[0] + dspMeterInfo.peaklevel[1]);
            currentValue = Mathf.Min(Mathf.Max(dspMeterInfo.peaklevel[0] + dspMeterInfo.peaklevel[1], minValue) / maxValue, 1);
            //print(currentValue);
            currentValue = Mathf.MoveTowards(lastValues[i], currentValue, (currentValue > lastValues[i]) ? maxIncrease : maxDecrease);

            lastValues[i] = currentValue;

            print(i+" currentValue: " + currentValue);
            yield return null;
        }
    }

    public void Update()
    {
        currentValue = 0;
        for (int i = 0; i < lastValues.Length; i++) currentValue += lastValues[i];
        print("currentValue: "+currentValue);

        for (int i = 0; i < meshes.Count; i++) meshes[i].material.SetFloat("_Sounding", currentValue);
    }

    [Header("Debugging")]
    [SerializeField] private bool hasOwnDitherScale;
    [SerializeField]private float ownDitherScale = 10;
}
