using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour 
{

	public Collider2D attackTrigger;
	public float attackCD = 0.3f;

	private Animator animator;
	private bool attacking = false;
	private float attackTimer = 0;

	void Start()
	{
		animator = gameObject.GetComponent<Animator> ();
		attackTrigger.enabled = false;
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space) && !attacking) 
		{
			attacking = true;
			attackTimer = attackCD;
			attackTrigger.enabled = true;
		}
			
		if (attacking) 
		{
			if (attackTimer > 0) 
			{
				attackTimer -= Time.deltaTime;
			}
			else 
			{
				attacking = false;
				attackTrigger.enabled = false;
			}
		}

		animator.SetBool ("Attacking", attacking);
	}

	public void attack()
	{
		attacking = true;
		attackTimer = attackCD;
		attackTrigger.enabled = true;
	}

}
