using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCan : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (audioSource) {
            if (collision.gameObject.name == "Ball") {
                audioSource.Play();
            }
        }
    }
}
