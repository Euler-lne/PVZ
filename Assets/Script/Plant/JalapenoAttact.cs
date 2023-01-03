using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JalapenoAttact : MonoBehaviour
{
    private float damage;
    private void Start() {
        damage = Globals.JalapenoDamge;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Zombie"))
        {
            other.GetComponent<ZombieNormal>().ChangeHealth(-damage);
        }
    }
    public void DestroySelf()
    {
        GameObject.Destroy(gameObject);
    }
}
