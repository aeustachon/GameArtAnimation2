using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour {

	public GameObject theDrop;
	public int health = 1;
	public int speed = 5;
	public int verticalSpeed = 2;
	public float patrolWidth = 3.50f;

	private bool facingRight;
	private bool playerInAggroRange;
	private float wallRight;
	private float wallLeft;
	private int randomNumber;

	private Vector3 walkAmount;
	private Transform myTrans;
	private Transform target;
	private Animator animator;

    //All audio stuff
    public AudioClip audioGhost;
    private AudioSource audioSourcePlayer;

    void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		animator = GetComponent<Animator>();
		wallLeft = transform.position.x - patrolWidth / 2;
		wallRight = transform.position.x + patrolWidth / 2;
		myTrans = this.transform;
		playerInAggroRange = false;
		facingRight = true;
        audioSourcePlayer = GetComponent<AudioSource>();      //Audio Stuff
    }

	void Update () {
		if (!playerInAggroRange) {
			walkAmount.x = speed * Time.deltaTime;
			if (facingRight && transform.position.x >= wallRight) {
				Vector3 currRot = myTrans.eulerAngles;
				currRot.y += 180;
				myTrans.eulerAngles = currRot;
				facingRight = false;
			} else if (!facingRight && transform.position.x <= wallLeft) {
				Vector3 currRot = myTrans.eulerAngles;
				currRot.y += 180;
				myTrans.eulerAngles = currRot;
				facingRight = true;
			}

			transform.Translate (walkAmount);
		} else if (playerInAggroRange) {
			speed = 5;
			if (target.transform.position.y - myTrans.transform.position.y < 0) {
				MoveToPlayer (-verticalSpeed);
			} else if (target.transform.position.y - myTrans.transform.position.y > 0) {
				MoveToPlayer (verticalSpeed);
			} else if (target.transform.position.y - myTrans.transform.position.y > 0) {
				MoveToPlayer (0);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D otherObject)
	{
		if (otherObject.gameObject.tag == "Player") {
            audioSourcePlayer.PlayOneShot(audioGhost, 1F);       //Ghost Sound
            playerInAggroRange = true;
		}
	}

	void MoveToPlayer(int y)
	{
		//rotate to look at player
		if (target.transform.position.x > transform.position.x)
		{
			if (facingRight)
			{
				move (y);
			}
			else
			{
				Vector3 currRot = myTrans.eulerAngles;
				currRot.y += 180;
				myTrans.eulerAngles = currRot;
				move (y);
				facingRight = true;
			}
		}
		else if (target.transform.position.x < transform.position.x)
		{
			if (facingRight)
			{
				Vector3 currRot = myTrans.eulerAngles;
				currRot.y += 180;
				myTrans.eulerAngles = currRot;
				move (y);
				facingRight = false;
			}
			else
			{
				move (y);
			}
		}
	}

	public void Damage(int dmg)
	{
		health = health - dmg;
		if (health <= 0) {
			dropHealth ();
			Destroy (gameObject);
		}
	}

	private void move(int y)
	{
		transform.Translate(new Vector3(speed * Time.deltaTime, y * Time.deltaTime, 0));
	}

	public void dropHealth()
	{
		randomNumber = Random.Range(1, 101);
		if (randomNumber < 35)
			Instantiate(theDrop, transform.position, transform.rotation);
	}
}
