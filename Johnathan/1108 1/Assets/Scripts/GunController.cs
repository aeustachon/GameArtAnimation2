using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	public float attackCD = 0.3f;
	public GameObject rightBullet;
	public GameObject leftBullet;
	private Transform firePoint;
	private Animator animator;
	private bool attacking = false;
	private float attackTimer = 0;
	private bool facingRight = true;

	void Start()
	{
		animator = gameObject.GetComponent<Animator> ();
		firePoint = transform.FindChild ("firePoint");
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space) && !attacking) 
		{
			attacking = true;
			attackTimer = attackCD;
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
			}
		}

		animator.SetBool ("Attacking", attacking);
	}

	public void attack()
	{
		attacking = true;
		attackTimer = attackCD;
		if (firePoint.transform.localPosition.x > 0) {
			Instantiate (rightBullet, firePoint.position, Quaternion.identity);
		}
		if (firePoint.transform.localPosition.x < 0) {
			Instantiate (leftBullet, firePoint.position, Quaternion.identity);
		}
	}
}
