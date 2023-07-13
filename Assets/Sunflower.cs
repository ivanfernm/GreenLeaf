using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : APlants
{
    public enum state
    {
        Activate,
        Deactivate,
    }

    [SerializeField] private float ActivationDuration;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private SkinnedMeshRenderer _mesh;
    [SerializeField] private float ImpuseForce = 10f;
    public state currentState = state.Deactivate;

    private void Update()
    {
        switch (currentState)
        {
            case state.Activate:
                _mesh.SetBlendShapeWeight(0, 0);
                //gameObject.GetComponent<Renderer>().material.color = Color.green;
                break;
            case state.Deactivate:
                _mesh.SetBlendShapeWeight(0, 100);
                //gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
        }
    }

    private IEnumerator ActivateState(float a)
    {
        particleSystem.Play();
        currentState = state.Activate;
        yield return new WaitForSeconds(a);
        particleSystem.Stop();
        currentState = state.Deactivate;
    }

    private void Start()
    {
        //currentState = state.Deactivate;
        _mesh = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Activate()
    {
        StartCoroutine(ActivateState(ActivationDuration));
    }

    private void OnTriggerEnter(Collider other)
    {
        var col = other.gameObject.GetComponent<IPlantActivator>();

        if (col != null)
        {
            Activate();
        }

        var player = other.gameObject.GetComponent<CharacterController>();

        if (player != null && currentState == state.Activate)
        {
            Impulse(player, Vector3.up, 10);
        }
    }
    

    private void Impulse(CharacterController controller, Vector3 direction, float force)
    {
        var velocity = controller.gameObject.GetComponent<PlayerMovement>().playerVelocity;
        velocity = transform.up;
        controller.Move(velocity * force);
    }
}