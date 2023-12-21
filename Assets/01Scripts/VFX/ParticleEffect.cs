using UnityEngine;

public class ParticleEffect : PoolableMono
{
    [SerializeField] private ParticleSystem _particle;

    public void PlayParticle()
    {
        _particle.Simulate(0);
        _particle.Play();
    }

    public void StopParticle()
    {
        _particle.Stop();
    }
    public override void ResetPooingItem()
    {
        
    }
}
