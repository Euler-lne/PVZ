using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal : MonoBehaviour
{
    public Vector3 direction = new Vector3(-1, 0, 0);
    public float speed = 1;
    private bool isWalk;
    public float damage;
    float damageInterval = 1f;
    private float damageTimer;
    private Animator animator;
    public float health = 100;
    float lostHeadHealth = 30;
    private float currentHealth;
    private GameObject head;
    private bool lostHead;
    private bool isDie;

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        isWalk = true;
        damageTimer = 0;
        currentHealth = health;
        head = transform.Find("Head").gameObject;
        isDie = false;
        lostHead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
            return;
        Move();
    }
    private void Move()
    {
        if (isWalk)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDie)
            return;
        if (other.tag == "Plant")
        {
            //碰到植物停止移动
            isWalk = false;
            animator.SetBool("Walk", false);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDie)
            return;
        if (other.tag == "Plant")
        {
            //持续接触植物，造成伤害
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0;
                //对所有植物都造成伤害
                Plant plant = other.GetComponent<Plant>();
                float newHealth = plant.ChangeHealth(-damage);
                SoundManager.instance.PlaySound(Globals.S_ZombieEat);
                //血量为0，转换为行走状态
                if (newHealth <= 0)
                {
                    isWalk = true;
                    animator.SetBool("Walk", true);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (isDie)
            return;
        if (other.tag == "Plant")
        {
            //离开植物，或者植物被僵尸消灭了，继续移动
            isWalk = true;
            animator.SetBool("Walk", true);
        }
    }

    public void ChangeHealth(float num)
    {
        if (Mathf.Approximately(num, -Globals.JalapenoDamge))
        {
            currentHealth = 0;
            animator.SetTrigger("Borm");
            isDie = true;
            GetComponent<BoxCollider2D>().enabled = false;
            return;
        }
        if (Mathf.Approximately(num, -Globals.CarDamage))
        {
            currentHealth = 0;
            animator.SetTrigger("Back");
            isDie = true;
            GetComponent<BoxCollider2D>().enabled = false;
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth + num, 0, health);
        //血量低于lostHeadHealth时，进入LostHead状态
        if (currentHealth < lostHeadHealth && !lostHead)
        {
            lostHead = true;
            animator.SetBool("LostHead", true);
            head.SetActive(true);
        }
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");
            isDie = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void DieAniOver()
    {
        animator.enabled = false;
        GameManager.instance.ZombieDead(gameObject);
        GameObject.Destroy(gameObject);
    }
}
