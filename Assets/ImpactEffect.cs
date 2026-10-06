using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        Destroy(gameObject, audioSource.clip.length);
    }
}
