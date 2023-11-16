using UnityEngine;
using UnityEngine.Audio;

namespace Sounds
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        [SerializeField] private GameObject _soundPrefab;
        [SerializeField] private AudioMixer _audioMixer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        public void PlaySound(AudioClip clip, AudioMixerGroup audioMixerGroup = null, float volume = 1)
        {
            var newSound = Instantiate(_soundPrefab);
            var audio = newSound.GetComponent<AudioSource>();
            if (audioMixerGroup != null) audio.outputAudioMixerGroup = audioMixerGroup;
            audio.volume = volume;
            audio.clip = clip;
            audio.Play();
        }

        public void PlaySoundRandom(AudioClip[] clips, AudioMixerGroup audioMixerGroup = null, float volume = 1)
        {
            var newSound = Instantiate(_soundPrefab);
            var audio = newSound.GetComponent<AudioSource>();
            var audioClip = clips[Random.Range(0, clips.Length)];
            if (audioMixerGroup != null) audio.outputAudioMixerGroup = audioMixerGroup;
            audio.volume = volume;
            audio.clip = audioClip;
            audio.Play();
        }
    }
}

