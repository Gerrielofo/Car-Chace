using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    public AudioSource engineAudioSource;
    public AudioClip engineSound;
    public AudioClip idleSound;

    private Rigidbody carRigidbody;
    [SerializeField] private float maxPitch = 2.0f; // Maximum pitch for engine sound
    [SerializeField] private float minPitch = 0.5f; // Minimum pitch for engine sound

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        engineAudioSource.Play();
    }

    void Update()
    {
        float speed = carRigidbody.velocity.magnitude;
        float pitch = Mathf.Lerp(minPitch, maxPitch, speed / 22); // Adjust pitch based on speed
        if (speed > 1 && !engineAudioSource.isPlaying)
        {
            engineAudioSource.clip = engineSound;
            engineAudioSource.Play();
        }
        else if (speed < 1)
        {
            engineAudioSource.clip = idleSound;
            if (!engineAudioSource.isPlaying)
                engineAudioSource.Play();
        }
        engineAudioSource.pitch = pitch;
    }
}
