using UnityEngine;
using System.Collections;

public class ShotgunController : MonoBehaviour {

	public float attackCD = 1.0f;
	public int damage;
	public GameObject rightBullet;
	public GameObject upperRightBullet;
	public GameObject lowerRightBullet;

	public GameObject leftBullet;
	public GameObject upperLeftBullet;
	public GameObject lowerLeftBullet;

	private bool attacking = false;
	private float attackTimer = 0;

	private Transform firePoint;

    //All audio stuff
    public AudioClip audioShotgunShoot;
    private AudioSource audioSourcePlayer;

    void Start()
	{
		firePoint = transform.FindChild("firePoint");
        audioSourcePlayer = GetComponent<AudioSource>();      //Audio Stuff
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
	}

	public void attack()
	{
		if (attacking == false) {
            audioSourcePlayer.PlayOneShot(audioShotgunShoot, 1F);       //Shoot Sound
            attacking = true;
			attackTimer = attackCD;
			if (firePoint.transform.position.x > gameObject.transform.position.x) {
				Instantiate (rightBullet, firePoint.position, Quaternion.identity);
				Instantiate (upperRightBullet, firePoint.position, Quaternion.identity);
				Instantiate (lowerRightBullet, firePoint.position, Quaternion.identity);
			}
			if (firePoint.transform.position.x < gameObject.transform.position.x) {
				Instantiate (leftBullet, firePoint.position, Quaternion.identity);
				Instantiate (upperLeftBullet, firePoint.position, Quaternion.identity);
				Instantiate (lowerLeftBullet, firePoint.position, Quaternion.identity);
			}
		}
	}
}
