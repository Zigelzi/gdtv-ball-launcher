using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Environment
{
    public class DestroyEffectAfterFinished : MonoBehaviour
    {
        List<ParticleSystem> _childParticles = new List<ParticleSystem>();
        List<ParticleSystem> _aliveParticles = new List<ParticleSystem>();
        bool _hasCollided = false;

        void Awake()
        {
            GetChildParticles();
            StartCoroutine(DestroyFinishedFXWithChildren(0));
        }

        void GetChildParticles()
        {
            ParticleSystem[] particles = transform.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem particle in particles)
            {
                _childParticles.Add(particle);
                _aliveParticles.Add(particle);
            }
        }

        IEnumerator DestroyFinishedFXWithChildren(float waitTime)
        {
            while (_aliveParticles.Count > 0)
            {
                foreach (ParticleSystem vfx in _childParticles)
                {
                    DisableFinishedParticle(vfx);
                }
                yield return new WaitForSeconds(waitTime);
            }
            Destroy(gameObject);
        }

        void DisableFinishedParticle(ParticleSystem vfx)
        {
            if (!vfx.IsAlive())
            {
                vfx.gameObject.SetActive(false);
                _aliveParticles.Remove(vfx);
            }
        }
    }

}