using UnityEngine;
using System.Collections;

public class RifleController : MonoBehaviour {

	//public float attackCD = 0.3f; ---Rifle has no cooldown
	public int damage;
	public GameObject rightBullet;
	public GameObject leftBullet;

	private bool attacking = false;
    public float fireRate = .1f;
    private float nextFire = .0f;

	private Transform firePoint;

	void Start()
	{
		firePoint = transform.FindChild("firePoint");
	}

	void Update()
	{


        if (attacking && Time.time > nextFire)
        {
            shoot();
        }
	}

	public void attack()
	{
	    attacking = true;
	}

    public void stopAttack()
    {
        attacking = false;
    }

    public void shoot()
    {
        if (firePoint.transform.position.x > gameObject.transform.position.x)
        {
            nextFire = Time.time + fireRate;
            GameObject clone = Instantiate(rightBullet, transform.position, transform.rotation) as GameObject;

        }
        if (firePoint.transform.position.x < gameObject.transform.position.x)
        {
            nextFire = Time.time + fireRate;
            GameObject clone = Instantiate(leftBullet, transform.position, transform.rotation) as GameObject;
        }
    }
}
