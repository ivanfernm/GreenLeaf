using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class Life : MonoBehaviour
{
   [SerializeField] private float maxLife;
   [SerializeField] private float currentLife;
   
   [SerializeField] private TextMeshProUGUI textMeshPro;
   
   Cinemachine.CinemachineImpulseSource impulseSource;

   private void Start()
   {
      currentLife = maxLife;
      textMeshPro.text = maxLife.ToString();
   }

   public void TakeDamage(float damage)
   {
      currentLife -= damage;
      textMeshPro.text = currentLife.ToString();
      if (currentLife <= 0)
      {
         Die();
      }
      impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
      impulseSource.GenerateImpulse(Camera.main.transform.forward);
   }
   void Die()
   {
      gameObject.SetActive(false);
   }

   private void OnTriggerEnter(Collider other)
   {
      var col = other.gameObject.GetComponent<IDamageDeler>();

      if (col != null)
      {
         TakeDamage(col.getDamage());
      }
   }
}
