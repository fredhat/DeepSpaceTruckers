using UnityEngine;

public class AudioManager : Singleton<AudioManager> {
    [SerializeField] private AudioSource _musicSource, _soundSource;
    [SerializeField] private float _masterVolume = 1f;

    protected override void Awake() {
        base.Awake();
        ChangeMasterVolume(_masterVolume);
    }
    
    public void PlayMusic() {
        _musicSource.Play();
    }
    
    public void PlayMusic(AudioClip clip) {
        _musicSource.clip = clip;
        PlayMusic();
    }
    
    public void StopMusic() {
        _musicSource.Stop();
    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1f) {
        _soundSource.transform.position = pos;
        PlaySound(clip, vol);
    }

    public void PlaySound(AudioClip clip, float vol = 1f) {
        _soundSource.PlayOneShot(clip, vol);
    }

    public void ChangeMasterVolume(float val) {
        AudioListener.volume = val;
    }
}