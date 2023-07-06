using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    [SerializeField] private KeyCode slashKey = KeyCode.Mouse0;
    [SerializeField] private ParticleSystem slashEffect;
    [SerializeField] private GameObject slashCollider;

    private void Update()
    {
        if (Input.GetKeyDown(slashKey))
        {
            StartCoroutine(PlaySlashEffect());
        }
    }

    public IEnumerator PlaySlashEffect()
    {
        slashEffect.Play();
        slashCollider.SetActive(true);
        var a = slashEffect.main.duration;
        yield return new WaitForSeconds(a);
        //slashEffect.Stop();
        slashCollider.SetActive(false);
    }
}
