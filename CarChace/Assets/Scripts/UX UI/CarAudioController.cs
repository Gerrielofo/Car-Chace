using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudioController : MonoBehaviour
{
    public EngineNote[] engineNotes;
    public float rpm;
    public float masterVolume;

    static readonly float[] workingVolumes = new float[1];

    private void Update()
    {
        float totalVolume = 0f;
        for (int i = 0; i < engineNotes.Length; ++i)
        {
            totalVolume += workingVolumes[i] = engineNotes[i].SetPitchAndGetVolumeForRPM(rpm);
        }

        if (totalVolume > 0f)
        {
            for (int i = 0; i < engineNotes.Length; ++i)
            {
                engineNotes[i].SetVolume(masterVolume * workingVolumes[i] / totalVolume);
            }
        }
    }

}

[System.Serializable]
public class EngineNote
{
    public AudioSource source;
    public float minRPM;
    public float peakRPM;
    public float maxRPM;
    public float pitchReferenceRPM;

    public float SetPitchAndGetVolumeForRPM(float rpm)
    {
        source.pitch = rpm / pitchReferenceRPM;

        if (rpm < minRPM || rpm > maxRPM)
        {
            return 0f;
        }

        if (rpm < peakRPM)
        {
            return Mathf.InverseLerp(minRPM, peakRPM, rpm);
        }
        else
        {
            return Mathf.InverseLerp(maxRPM, peakRPM, rpm);
        }
    }

    public void SetVolume(float volume)
    {
        source.mute = (source.volume = volume) == 0;
    }

}
