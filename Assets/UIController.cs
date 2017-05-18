using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public enum UIType { ParticleSystem, Canvas };
    public UIType type;

    public float lifeTime = 5f;

    private Vector3 rotationoffset;

    void Start()
    {
        rotationoffset = transform.eulerAngles;
        TurnOn();
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }



	public void TurnOn()
    {
        if(type == UIType.ParticleSystem)
        {
            transform.eulerAngles += rotationoffset;
            GetComponent<ParticleSystem>().Play();
        }
    }
}
