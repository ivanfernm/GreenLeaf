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

    public state currentState;

    private void Update()
    {
        switch (currentState)
        {
            case state.Activate:
                gameObject.GetComponent<Renderer>().material.color = Color.green;
                break;
            case state.Deactivate:
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
        }
    }

    private IEnumerator ActivateState(float a)
    {
        currentState = state.Activate;
        yield return new WaitForSeconds(a);
        currentState = state.Deactivate;
    }

    private void Start()
    {
        currentState = state.Deactivate;
    }

    public void Activate()
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
    }
}