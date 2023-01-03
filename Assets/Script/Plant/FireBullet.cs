using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    // Start is called before the first frame update
    protected override void Start()
    {
        //在这里赋值后unity中就不管用了
        base.Start();
        damage = 30f;
        path = "Prefab/Fire";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
