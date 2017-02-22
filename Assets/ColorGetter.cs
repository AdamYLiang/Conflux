using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGetter : MonoBehaviour {

    public EmitterScript emitter;

    [SerializeField]
    Color laserColor;

    protected ParticleSystem pSystem;

    protected ParticleSystem.Particle[] particles;
    protected int totalParticles;

	// Use this for initialization
	void Start () {
        emitter = transform.parent.GetComponent<EmitterScript>();
        pSystem = GetComponent<ParticleSystem>();
        totalParticles = pSystem.particleCount;

	}
	
	// Update is called once per frame
	void Update () {

        
        laserColor  = transform.root.GetComponent<PuzzleManager>().GetLaserPigment(emitter.laserColor);

        //Array of all particles.
        totalParticles = pSystem.GetParticles(particles);

        for(int i = 0; i < totalParticles; i++)
        {
            particles[i].startColor = laserColor;
        }

        pSystem.SetParticles(particles, totalParticles);
        

	}
}
