using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public AudioClip clip;
    private AudioSource audio;

	private void Start() {
        audio = GetComponent<AudioSource>();
	}
}
