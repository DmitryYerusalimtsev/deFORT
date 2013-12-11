
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public const string UnityTag = "Unit";

    public float Speed;
    public float Damage;
    public float Distance;
    public float ReloadTime;

    public int PointsForKill;

    public GameObject DefatultTarget;
    public Line BattleLine;

    private Animator animation;
    private bool facingRight = true;
    private float reloadTimer;
    private GameObject target;

    private Scores scores;

    void Start()
    {
        if (!tag.Equals(UnityTag)) tag = UnityTag;
        animation = GetComponentInChildren<Animator>();
        scores = FindObjectOfType<Scores>();
    }

    void Update()
    {
        if (reloadTimer > 0) reloadTimer -= Time.deltaTime;

        if (target == null)
        {
            StopMoving();
            animation.Play("UnitStay");
            return;
        }

        if (target.tag.Equals(Unit.UnityTag))
        {
            // Distance is close enough
            if (IsReachedTargetOnDistance(target, Distance))
            {
                TargetIsClose();
            }
            // Distance is too far
            else
            {
                Move();
            }
        }

        if (target.tag.Equals("Obstacle"))
        {

            if (IsReachedTargetOnDistance(target, Distance))
            {
                TargetIsClose();
            }
            // Distance is too far
            else
            {
                Move();
            }
        }

        // For creator scene
        if (target.tag.Equals("Respawn"))
        {

            if (IsReachedTargetOnDistance(target, 0.01f))
            {
                PointReached();
            }
            // Distance is too far
            else
            {
                Move();
            }
        }

        // Turn right
        if (transform.position.x > target.transform.position.x && facingRight)
        {
            Flip();
        }


        // Turn left
        if (transform.position.x < target.transform.position.x && !facingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        if (IsDead)
        {
            DestroyUnit();
        }

        // We are busy with some targer
        /*
        if (target != null)
        {
            if (target.tag.Equals(Unit.UnityTag) && IsReachedTargetOnDistance(target, Distance)) return;
            // For Creator Scene
            if (target.tag.Equals("Respawn") && IsReachedTargetOnDistance(target, 0.01f)) return;
            // Attack castle even if enemies here
            // if (target.tag.Equals("Obstacle") && IsReachedTargetOnDistance(target, Distance)) return;
        }
        */

        // Search something to do
        var lineEnemies = SearchEnemiesOn(Line);
        if (!lineEnemies.Any())
        {
            target = DefatultTarget;
            return;
        }

        target = GetClosestTargetFrom(lineEnemies);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Move()
    {
        animation.Play("UnitWalk");
        rigidbody2D.velocity = new Vector2(transform.localScale.x * Speed, rigidbody2D.velocity.y);
    }

    private void StopMoving()
    {
        rigidbody2D.velocity = Vector2.zero;
    }

	private void TargetIsClose()
	{
		StopMoving();

	    if (reloadTimer <= 0) Attack(target);
	}

    private void PointReached()
    {
        DestroyUnit();
    }

    private void Attack(GameObject enemy)
    {
        animation.Play("UnitAttack");
        
        enemy.GetComponent<Health>().OnDamage(Damage);
        reloadTimer = ReloadTime;
    }

    private bool IsReachedTargetOnDistance(GameObject t, float distance)
    {
        return Mathf.Abs(t.transform.position.x - transform.position.x) <= distance;
    }

    private IList<Unit> SearchEnemiesOn(int line)
    {
        return BattleLine.Units.Where(enemy => enemy.Line == line && enemy.Side != Side && enemy != this).ToList();
    }

    private GameObject GetClosestTargetFrom(IEnumerable<Unit> targets)
    {
        GameObject result = null;
        float lastDistance = float.MaxValue;

        foreach (var enemy in targets)
        {
            var distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (lastDistance <= distance) continue;
            lastDistance = distance;
            result = enemy.gameObject;
        }

        return result;
    }

    public void DestroyUnit()
    {

        BattleLine.RemoveUnit(this);
        Destroy(gameObject);
        if (Side == Scores.LeftSideNumber)
        {
            --scores.LeftUnitCount;
            scores.RightScorePoints += PointsForKill;
        }
        else
        {
            --scores.RightUnitCount;
            scores.LeftScorePoints += PointsForKill;
        }
    }

    public int Line
    {
        get
        {
            return BattleLine.Id;
        }
    }

    public bool IsDead
    {
        get { return GetComponent<Health>().Dead; }
    }

    public float Health
    {
        get { return GetComponent<Health>().CurrentHealth; }
        set { GetComponent<Health>().CurrentHealth = value; }
    }

    public float MaxHealth
    {
        get { return GetComponent<Health>().MaxHealth; }
        set { GetComponent<Health>().MaxHealth = value; }
    }

    public int Side { get; set; }
}
