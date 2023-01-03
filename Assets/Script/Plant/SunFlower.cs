using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlower : Plant
{
    int i = 0;
    bool if_random = false;
    bool if_bornsun = false;
    private int sunNum;
    public GameObject SunPrefab;
    int interval;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        interval = Random.Range(4, 7);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (if_random)
        {
            interval = Random.Range(4, 7);
            if_random = false;
        }
        if (if_bornsun)
        {
            BornSun();
            if_bornsun = false;
        }
    }
    public void FlowerIdle()
    {
        if (i == interval)
        {
            //克隆对象
            animator.SetBool("Read", true);
            i = 0;
            if_random = true;
        }
        else if (i < interval)
            i += 1;
    }
    public void FlowerLight()
    {
        if (i == 1)
        {
            //克隆对象
            animator.SetBool("Read", false);
            i = 0;
            if_bornsun = true;
        }
        else if (i < 1)
            i += 1;
        else
            i = 0;
    }
    private void BornSun()
    {
        GameObject NewSun = Instantiate(SunPrefab);
        Sun sun = NewSun.GetComponent<Sun>();//获得脚本
        sunNum = Random.Range(1, 9);
        float randomX;
        if (sunNum % 2 == 1)
            randomX = Random.Range(transform.position.x - 30, transform.position.x - 20);
        else
            randomX = Random.Range(transform.position.x + 20, transform.position.x + 30);
        float randomY = Random.Range(transform.position.y - 20, transform.position.y + 20);
        NewSun.transform.position = new Vector2(randomX, randomY);
        Vector2 direction = new Vector2(randomX - transform.position.x, randomY - transform.position.y).normalized;
        sun.SunMove(direction);
    }
}
