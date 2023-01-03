using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNut : Plant
{
    private float woundedHealth;
    private float dyingHealth;
    private bool isWounded;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        woundedHealth = 0.5f * health;
        dyingHealth = 0.3f * health;
        isWounded = false;
    }


    // Update is called once per frame
    protected override void Update()
    {
        if (!start)
            return;
        if (currentHealth <= woundedHealth && !isWounded)
        {
            isWounded = true;
            animator.SetBool("Wounded", true);
        }
        if (currentHealth <= dyingHealth && isWounded)
        {
            animator.SetBool("Dying", true);
        }
    }
}
