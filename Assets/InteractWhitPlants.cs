using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class InteractWhitPlants : MonoBehaviour,IPlantActivator,IDamageDeler
{
    [SerializeField] private float damage = 10;
    
    public float getDamage()
    {
        return damage;
    }
}
