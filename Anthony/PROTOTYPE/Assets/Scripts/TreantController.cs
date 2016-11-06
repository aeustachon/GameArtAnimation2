using UnityEngine;
using System.Collections;

public class TreantController : MonoBehaviour
{
	public Collider2D attackTrigger;
	public Transform target;

	Transform transform;
	Animator animator;
	bool playerInAggroRange;
	bool facingLeft;

	void Start () 
	{
		facingLeft = false;
		playerInAggroRange = false;
		attackTrigger.enabled = false;
		animator = GetComponent<Animator> ();
		transform = GetComponent<Transform> ();
	}
		
	void Update () 
	{
		if (facingLeft == true) {
			
		}
		if (playerInAggroRange) {
			attack();
		} else if (!playerInAggroRange) {
			idle ();
		}
	}

	void OnTriggerEnter2D(Collider2D otherObject)
	{
		if (otherObject.gameObject.tag == "PlayerAttack") 
		{
			Destroy (gameObject);
		}
		if (otherObject.gameObject.tag == "Player")
		{
			playerInAggroRange = true;
			lookAtTarget ();
			
		}
	}

	void attack()
	{
		attackTrigger.enabled = true;
		animator.SetBool ("Attacking", true);
	}

	void idle()
	{
		animator.SetBool ("Attacking", false);
	}

	void setFacingDirection(Vector2 v2)
	{
		transform.localScale = v2;
	}

	void lookAtTarget()
	{
		if (target.transform.position.x > transform.position.x)
		{
			if (facingLeft)
			{
				transform.localScale = new Vector2 (-transform.localScale.x, transform.localScale.y);
				facingLeft = false;
			}
		}
		else if (target.transform.position.x < transform.position.x)
		{
			if (!facingLeft)
			{
				transform.localScale = new Vector2 (-transform.localScale.x, transform.localScale.y);
				facingLeft = true;
			}
		}
	}
}
