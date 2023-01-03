using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Shooter
{
    int i = 0;
    public GameObject bullet;
    public Transform bulletPos;//获取豌豆射出的位置
    int interval = 2;
    //要改变Interval的值需要在unity animation中改变
    public void CreatBullet()
    {
        if (!isAttack)
            return;
        if (i == 0)
        {
            //克隆对象
            SoundManager.instance.PlaySound(Globals.S_Shoot, 0.3f);
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
            i = interval;
        }
        else
        {
            i -= 1;
        }
    }
}
