using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torchwood : Plant
{
    private GameObject FireBulletPrefab;
    protected override void Start()
    {
        base.Start();
        FireBulletPrefab = Resources.Load("Prefab/FireBullet") as GameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //我觉得还可以优化
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            //本人觉得这里还可以改进
            if(bullet.isTorchwoodCreate)//如果是火炬树桩生成的子弹则不会被销毁
            {
                return;
            }
            bullet.DestroyBullet();
            //计算触发点的位置，对于火炬树桩来说，距离火炬触发器最近的点
            CreateBullet(other.bounds.ClosestPoint(transform.position));
        }
    }

    private void CreateBullet(Vector3 borPos)
    {
        GameObject fireBullet = Instantiate(FireBulletPrefab,borPos,Quaternion.identity);
        fireBullet.transform.parent = transform.parent;
        fireBullet.transform.position = borPos;
        fireBullet.GetComponent<Bullet>().isTorchwoodCreate = true;
    }
}
