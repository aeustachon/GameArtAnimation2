using UnityEngine;
using System.Collections;

public class BeeController : MonoBehaviour {

	public int health = 1;
	public int speed = 10;

	private Transform target;

	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update () {
		goToTarget ();
	}

	void OnTriggerEnter2D(Collider2D otherObject)
	{
		if (otherObject.gameObject.tag == "Player" || otherObject.gameObject.tag == "Bullet") {
			Destroy (gameObject);
		}
	}

	void goToTarget()
	{
		if (target.transform.position.x > transform.position.x)
		{
			if (speed < 0)
			{
				speed = -speed;
			}
		}
		else if (target.transform.position.x < transform.position.x)
		{
			if (speed > 0)
			{
				speed = -speed;
			}
		}

		transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
	}

	public void Damage(int dmg)
	{
		print ("Bee Damaged...");
		health = health - dmg;
		if (health <= 0) {
			Destroy (gameObject);
		}
	}
}
