using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : Bullet
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        damage = 15f;
        path = "Prefab/PeaBulletHit";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
