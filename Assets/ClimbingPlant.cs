using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingPlant : APlants
{
    public enum state
    {
        Activate,
        Desactivate,
    }

    public state currentState = state.Desactivate;
    [SerializeField] private float ActivationDuration;
    [SerializeField] private bool isClimbing;
    [SerializeField] private CharacterController _player;
    [SerializeField] private float ClimbingSpeed = 10f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) && isClimbing)
        {
            _player.Move(Vector3.up * Time.deltaTime * ClimbingSpeed);
        }

        switch (currentState)
        {
            case state.Activate:
                gameObject.GetComponent<Renderer>().material.color = Color.green;
                break;
            case state.Desactivate:
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var col = other.gameObject.GetComponent<IPlantActivator>();

        if (col != null) Activate();
    }

    private void OnTriggerStay(Collider other)
    {
        var player = other.gameObject.GetComponent<CharacterController>();

        if (player != null && currentState == state.Activate)
        {
            _player = player;
            if (currentState == state.Activate)
            {
                SetParent(player);
            }
        }
        else
            DeParent(_player);
    }

    void Activate()
    {
        StartCoroutine(ActivateState(ActivationDuration));
    }

    private IEnumerator ActivateState(float a)
    {
        isClimbing = true;
        transform.localScale = new Vector3(1, 8, 1);
        currentState = state.Activate;
        yield return new WaitForSeconds(a);
        if (_player != null) DeParent(_player);
        currentState = state.Desactivate;
        transform.localScale = new Vector3(1, 1, 1);
        isClimbing = false;
    }

    void SetParent(CharacterController controller)
    {
        controller.gameObject.transform.SetParent(this.transform);
    }

    void DeParent(CharacterController controller)
    {
        controller.gameObject.transform.parent = null;
    }
}