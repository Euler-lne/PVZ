using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    protected bool start;
    public float health = 100;
    protected float currentHealth;
    protected Animator animator;
    protected BoxCollider2D boxCollider2D;
    protected bool isAttack;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    protected virtual void Update()
    {
    }
    protected virtual void Start()
    {
        currentHealth = health;
        start = false;
        isAttack = false;
        animator.speed = 0;
        boxCollider2D.enabled = false;
        GetComponent<SpriteRenderer>().sortingOrder++;//层级
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);//透明度
    }
    //开关函数
    public virtual void SetPlantStart()
    {
        start = true;
        animator.speed = 1;
        boxCollider2D.enabled = true;
        GetComponent<SpriteRenderer>().sortingOrder--;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    public virtual float ChangeHealth(float num)
    {
        currentHealth = Mathf.Clamp(currentHealth + num, 0, health);
        if (currentHealth <= 0)
        {
            GameObject.Destroy(gameObject);
        }
        return currentHealth;
    }
}
