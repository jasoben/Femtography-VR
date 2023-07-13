using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[ExecuteInEditMode]
public class SpotLightParticles : MonoBehaviour
{
    ParticleSystem particleSystem;

    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> outside = new List<ParticleSystem.Particle>();

    [SerializeField] Color opaqueColor;
    [SerializeField] Color transparentColor;
    Color lerpedColor;

    [SerializeField] Collider triggerCollider;

    [SerializeField] private float lightFalloffDistance;

    [SerializeField] GameObject testObject;

    // Start is called before the first frame update
    void OnEnable()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        int numInside = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        int numOutside = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);

        for (int i = 0; i < numInside; i++)
        {
            ParticleSystem.Particle particle = inside[i];
            particle.startColor = opaqueColor;
            inside[i] = particle;
        }
        
        for (int i = 0; i < numOutside; i++)
        {
            ParticleSystem.Particle particle = outside[i];

            Vector3 particleClosestPointOnCollider = triggerCollider.ClosestPoint(
                particle.position);

            float distanceToTriggerCollider = Vector3.Distance(
                particle.position, particleClosestPointOnCollider);

            float percentOfFallOffDistance = distanceToTriggerCollider / lightFalloffDistance;

            percentOfFallOffDistance = Mathf.Clamp01(percentOfFallOffDistance);

            lerpedColor = Color.Lerp(opaqueColor, transparentColor,
                percentOfFallOffDistance);
            particle.startColor = lerpedColor;
            outside[i] = particle;
        }
        particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
