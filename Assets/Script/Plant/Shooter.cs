using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Plant
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (start)
        {
            if (Check())
                isAttack = true;
            else
                isAttack = false;
        }
    }
    protected bool Check()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = new Vector2(1, 0);
        //一定要设置距离
        RaycastHit2D hit = Physics2D.Raycast(position, direction, Globals.Shooter - transform.position.x, LayerMask.GetMask("Zombie"));
        if (hit.collider)
        {
            if (hit.collider.tag == "Zombie")
                return true;
        }
        return false;
    }
}
