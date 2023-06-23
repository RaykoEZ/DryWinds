using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioItem 
{
    public string Name;
    public AudioClip Clip;
    public bool Loop;
    public float Volume;
    public float Pitch;
    [HideInInspector]
    public AudioSource Source;
}
public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioItem> m_audioList = default;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(var a in m_audioList) 
        {
            a.Source = gameObject.AddComponent<AudioSource>();
            a.Source.clip = a.Clip;
            a.Source.loop = a.Loop;
            a.Source.volume = a.Volume;
            a.Source.pitch = a.Pitch;
        }
    }
    public bool Play(string audioName) 
    {
        var result = m_audioList.Find((audio) => audio.Name == audioName);
        result?.Source?.Play();
        if (result == null) Debug.LogWarning($"Audio: {audioName} not found, cannot play audio");
        return result != null;
    }
    public bool Stop(string audioName)
    {
        var result = m_audioList.Find((audio) => audio.Name == audioName);
        result?.Source?.Stop();
        if (result == null) Debug.LogWarning($"Audio: {audioName} not found, cannot stop audio");
        return result != null;
    }
}
