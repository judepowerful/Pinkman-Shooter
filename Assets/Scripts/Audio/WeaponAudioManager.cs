using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponAudioManager : MonoBehaviour
{
    [Header("音效 Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip emptyClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayShoot()
    {
        PlayClip(shootClip);
    }

    public void PlayReload()
    {
        PlayClip(reloadClip);
    }

    public void PlayEmpty()
    {
        PlayClip(emptyClip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // 可选：你还可以加随机 pitch、音量等处理
}
