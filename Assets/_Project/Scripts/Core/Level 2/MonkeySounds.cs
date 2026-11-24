
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MonkeySounds : MonoBehaviour
{
    [Header("Monkey Noises")]
    public AudioClip[] monkeySounds;

    [Header("Random Timing")]
    public float minTimeBetweenSounds = 4f;
    public float maxTimeBetweenSounds = 12f;

    [Header("Sound Settings")]
    [Range(0f, 1f)]
    public float volume = 0.5f;

    [Range(0f, 0.5f)]
    public float pitchVariation = 0.15f;

    private AudioSource audioSource;
    private float soundTimer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;

        ScheduleNextSound();
    }

    void Update()
    {
        if (monkeySounds == null ||
            monkeySounds.Length == 0)
            return;

        soundTimer -= Time.deltaTime;

        if (soundTimer <= 0f)
        {
            PlayRandomSound();
            ScheduleNextSound();
        }
    }

    void ScheduleNextSound()
    {
        float minDelay = Mathf.Max(0.1f, minTimeBetweenSounds);
        float maxDelay = Mathf.Max(minDelay, maxTimeBetweenSounds);

        soundTimer = Random.Range(minDelay, maxDelay);
    }

    void PlayRandomSound()
    {
        if (audioSource.isPlaying)
            return;

        AudioClip clip =
            monkeySounds[Random.Range(0, monkeySounds.Length)];

        if (clip == null)
            return;

        // Slight pitch differences make the monkeys
        // sound less repetitive.
        audioSource.pitch = Random.Range(
            1f - pitchVariation,
            1f + pitchVariation
        );

        audioSource.PlayOneShot(clip, volume);
    }
}
