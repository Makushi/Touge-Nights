using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] songs = null;
    [SerializeField] private AudioSource audioSource = null;

    [SerializeField] private float[] volumeSettings = null;
    [SerializeField] private float[] pitchSettings = null;

    [SerializeField] private float volumeAutomationTime = 0.5f;
    [SerializeField] private float pitchAutomationTime = 1.0f;

    private float volumeT = 0;
    private float pitchT = 0;

    private float volumeAutomationValue = 0.0f;
    private float pitchAutomationValue = 0.0f;

    private enum PITCH_AUTOMATION { OFF, ON };
    private enum VOLUME_AUTOMATION { OFF, ON };

    private VOLUME_AUTOMATION volumeAutomationState = VOLUME_AUTOMATION.OFF;
    private PITCH_AUTOMATION pitchAutomationState = PITCH_AUTOMATION.OFF;

    public static AudioManager Instance = null;

    public enum VOLUME { GAMEPLAY, PAUSE };
    public enum PITCH { LOW, NORMAL, HIGH };

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (volumeAutomationState == VOLUME_AUTOMATION.ON)
        {
            AutomateVolume();
        }

        if (pitchAutomationState == PITCH_AUTOMATION.ON)
        {
            AutomatePtich();
        }
    }

    public void PlayRandomSong()
    {
        audioSource.Stop();
        audioSource.clip = songs[Random.Range(0, songs.Length)];
        audioSource.Play();
    }

    public void PlayMenuSong()
    {
        audioSource.Stop();
        audioSource.clip = songs[0];
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetPauseMenuVolume() 
    {
        audioSource.volume = volumeSettings[(int)VOLUME.PAUSE];
    }

    public void SetGameplayVolume()
    {
        audioSource.volume = volumeSettings[(int)VOLUME.GAMEPLAY];
    }

    public void SetVolume(int volume)
    {
        audioSource.volume = volumeSettings[volume];
    }

    public void SetPitch(int pitch)
    {
        audioSource.pitch = pitchSettings[pitch];
    }

    public void StartVolumeAutomation(int volume)
    {
        volumeAutomationValue = volumeSettings[volume];
        volumeAutomationState = VOLUME_AUTOMATION.ON;
    }

    public void StartPitchAutomation(int pitch)
    {
        pitchAutomationValue = pitchSettings[pitch];
        pitchAutomationState = PITCH_AUTOMATION.ON;
    }

    private void AutomateVolume()
    {
        if (volumeT <= volumeAutomationTime)
        {
            volumeT += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(audioSource.volume, volumeAutomationValue, volumeT / volumeAutomationTime);
        }
        else
        {
            volumeAutomationTime = 0.0f;
            volumeAutomationState = VOLUME_AUTOMATION.OFF;
        }
    }

    private void AutomatePtich()
    {
        if (pitchT <= pitchAutomationTime)
        {
            pitchT += Time.deltaTime;
            audioSource.pitch = Mathf.Lerp(audioSource.volume, pitchAutomationValue, pitchT / pitchAutomationTime);
        }
        else
        {
            pitchAutomationTime = 0.0f;
            pitchAutomationState = PITCH_AUTOMATION.OFF;
        }
    }
}
