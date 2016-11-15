using UnityEngine;
using System.Collections;

public class RifleController : MonoBehaviour {

	//public float attackCD = 0.3f; ---Rifle has no cooldown
	public GameObject rightBullet;
	public GameObject leftBullet;
	public int damage;
	public float fireRate = .1f;

	private bool attacking = false;
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
            Instantiate(rightBullet, firePoint.transform.position, transform.rotation);

        }
        if (firePoint.transform.position.x < gameObject.transform.position.x)
        {
            nextFire = Time.time + fireRate;
            Instantiate(leftBullet, firePoint.transform.position, transform.rotation);
        }
    }
}
