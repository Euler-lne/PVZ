using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    protected float damage;
    public bool isTorchwoodCreate = false;//不可以在Start里面赋值
    public bool isZombie = false;
    protected string path;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        direction = new Vector3(1, 0, 0);
        speed = 50f;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        //超过一定长度要把子弹删除
        if (gameObject.transform.position.x >= 400)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Zombie")
        {
            isZombie = true;
            DestroyBullet();
            other.GetComponent<ZombieNormal>().ChangeHealth(-damage);
        }
    }
    //销毁子弹 public原因：火炬树桩用到
    public virtual void DestroyBullet()
    {
        GameObject.Destroy(gameObject);
        if (isZombie)
        {
            //让相机晃动，镜头摇晃时间和摇晃强度
            // Camera.main.transform.DOShakePosition(2, 3);
            SoundManager.instance.PlaySound(Globals.S_PeaHit);
            GameObject FirePrefab = Resources.Load(path) as GameObject;
            float offset = 45f;
            Vector3 position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
            GameObject FireObject = Instantiate(FirePrefab, position, Quaternion.identity);
        }
        //冰冻等僵尸特效音乐放在僵尸上面
    }
}
