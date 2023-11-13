using UnityEngine;

namespace Sounds
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        [SerializeField] private GameObject _soundPrefab;

        private void Start()
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

        public void PlaySound(AudioClip clip)
        {
            var newSound = Instantiate(_soundPrefab);
            var audio = newSound.GetComponent<AudioSource>();
            audio.clip = clip;
            audio.Play();
        }

        public void PlaySoundRandom(AudioClip[] clips)
        {
            var newSound = Instantiate(_soundPrefab);
            var audio = newSound.GetComponent<AudioSource>();
            var audioClip = clips[Random.Range(0, clips.Length)];
            audio.clip = audioClip;
            audio.Play();
        }
    }
}

