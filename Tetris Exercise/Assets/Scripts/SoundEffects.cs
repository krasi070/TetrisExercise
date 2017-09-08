using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip moveSound;
    public AudioClip dropSound;
    public AudioClip rotateSound;
    public AudioClip noHoldSound;
    public AudioClip lineSound;
    public AudioClip tetrisSound;
    public AudioClip holdSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
	
	void Update()
    {
		
	}

    public void PlayMoveAudio()
    {
        audioSource.PlayOneShot(moveSound);
    }

    public void PlayDropAudio()
    {
        audioSource.PlayOneShot(dropSound);
    }

    public void PlayRotateAudio()
    {
        audioSource.PlayOneShot(rotateSound);
    }

    public void PlayNoHoldAudio()
    {
        audioSource.PlayOneShot(noHoldSound);
    }

    public void PlayHoldAudio()
    {
        audioSource.PlayOneShot(holdSound);
    }

    public void PlayClearedLineSound()
    {
        audioSource.PlayOneShot(lineSound);
    }

    public void PlayTetrisSound()
    {
        audioSource.PlayOneShot(tetrisSound);
    }
}