using UnityEngine;

public class KeyParticle : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        _particleSystem.Play();
        float duration = _particleSystem.main.duration;

        Destroy(gameObject, duration);
    }
}