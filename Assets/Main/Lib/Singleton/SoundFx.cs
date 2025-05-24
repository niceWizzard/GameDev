using UnityEngine;

namespace Main.Lib.Singleton
{
    public class SoundFx : PrefabSingleton<SoundFx>
    {
        [SerializeField] private AudioSource normalDamageSfx;

        public static void PlayDamagedSound(Vector2 position = default, bool randomPitch = false)
        {
            Play(Instance.normalDamageSfx, position, randomPitch);
        }

        private static void Play(AudioSource audioSource, Vector2 position = default, bool randomPitch = false)
        {
            var copy = Instantiate(audioSource, position, Quaternion.identity);
            if (randomPitch)
                copy.pitch = Random.Range(0.9f, 1.1f);
            copy.Play();
            Destroy(copy.gameObject, copy.clip.length);
        }
    }
}
