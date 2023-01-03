using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum State
{
    None,
    Down,
    Up,
    Over,
}
public class Squash : Plant
{
    private float damage;
    private float findZombieDistance;
    private Vector3 attackPosition;
    protected override void Start()
    {
        base.Start();
        damage = 300f;
        findZombieDistance = 80f;
        isAttack = false;
    }
    private State squashState = State.None;
    protected override void Update()
    {
        if (start)
        {
            Attack();

            //用枚举类型比较方便
            switch (squashState)
            {
                case State.Down:
                    break;
                case State.Up:
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(attackPosition.x, attackPosition.y + 100), Time.deltaTime * 200);
                    break;
                case State.Over:
                    transform.position = Vector2.MoveTowards(transform.position, attackPosition, Time.deltaTime * 200);
                    break;
                default:
                    break;
            }
        }
    }
    private int Check()
    {
        //先检测左边
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 directionl = new Vector2(-1, 0);
        RaycastHit2D hitleft = Physics2D.Raycast(position, directionl, findZombieDistance, LayerMask.GetMask("Zombie"));
        if (hitleft.collider)
        {
            if (hitleft.collider.tag == "Zombie")
            {
                attackPosition = hitleft.transform.position;
                return -1;
            }
        }
        Vector2 directionr = new Vector2(1, 0);
        //一定要设置距离
        RaycastHit2D hitright = Physics2D.Raycast(position, directionr, findZombieDistance, LayerMask.GetMask("Zombie"));
        if (hitright.collider)
        {
            {
                attackPosition = hitright.transform.position;
                return 1;
            }
        }
        return 0;
    }
    private void Attack()
    {
        if (isAttack)
            return;
        int direction = Check();
        if (direction == 0)
            return;
        SoundManager.instance.PlaySound(Globals.S_Squash);
        if (direction == -1)
            animator.SetTrigger("Left");
        else
            animator.SetTrigger("Right");
        isAttack = true;
    }

    public void SetAttackUp()
    {
        squashState = State.Up;
        animator.SetTrigger("AttackUp");
    }
    public void SetAttackOver()
    {
        squashState = State.Over;
        animator.SetTrigger("AttackOver");
    }
    public void DoReallyAttack()
    {
        boxCollider2D.enabled = true;
        Destroy(gameObject, 0.5f);
    }
    public void SetAttackDown()
    {
        squashState = State.Down;
        animator.SetTrigger("Attack");
    }
    //这里使得窝瓜在移动过程中不攻击
    public override void SetPlantStart()
    {
        base.SetPlantStart();
        boxCollider2D.enabled = false;
    }
    //这样窝瓜就不会扣血了
    public override float ChangeHealth(float num)
    {
        return health;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zombie"))
        {
            other.GetComponent<ZombieNormal>().ChangeHealth(-damage);
        }
    }
}
