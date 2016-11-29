using UnityEngine;
using System.Collections;

public class WolfController : MonoBehaviour
{
	public Color hurtColor = Color.red;
	public Color normalColor = Color.white;
	public GameObject theDrop;
    public float speed;
    public float patrolWidth = 3.50f;
	public int health = 100;

	private float wallLeft;
	private float wallRight;
	private bool playerInAggroRange;
	private bool facingRight;
	private bool isDead = false;
	private int randomNumber;

	private Vector3 walkAmount;
	private Transform myTrans;
	private Transform target;
	private Animator animator;
	private Rigidbody2D rb;
	private BoxCollider2D bc2d;
	private CircleCollider2D c2d;
	private GameObject childGo;
    private Renderer render;


    void Start ()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        c2d = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<Renderer>();
        wallLeft = transform.position.x - patrolWidth / 2;
        wallRight = transform.position.x + patrolWidth / 2;
        myTrans = this.transform;
        playerInAggroRange = false;
        facingRight = true;
		target = GameObject.FindGameObjectWithTag ("Player").transform;
    }

    void Update()
    {
        if (playerInAggroRange) {
            animator.SetInteger("State", 1);
            MoveToPlayer();
        }

        if (!playerInAggroRange)
        {
            walkAmount.x = speed * Time.deltaTime;
            if (facingRight && transform.position.x >= wallRight)
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y += 180;
                myTrans.eulerAngles = currRot;
                facingRight = false;
            }

            else if (!facingRight && transform.position.x <= wallLeft)
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y += 180;
                myTrans.eulerAngles = currRot;
                facingRight = true;
            }

            transform.Translate(walkAmount);
            animator.SetInteger("State", 1);
        }

        HasBeenHit();
        
	}

    void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.gameObject.tag == "Player")
        {
            playerInAggroRange = true;
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        //rotate to look at player
        if (target.transform.position.x > transform.position.x)
        {
            if (facingRight)
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            else
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y += 180;
                myTrans.eulerAngles = currRot;
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
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
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
                facingRight = false;
            }
            else
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));

            }
        }
    }

    void HasBeenHit()
    {
        if(isDead == true)
        {
            animator.SetBool("IsDead", isDead);
        }
    }


    public void StopWalk()
    {
        animator.SetInteger("State", 1);
        speed = 0;
    }

	public void Damage(int dmg)
	{
		health = health - dmg;
        StartCoroutine(Flasher());
		if (health <= 0) {
			isDead = true;
			StopWalk ();
			rb.isKinematic = true;
			dropHealth ();
			Destroy (bc2d);
			Destroy (c2d);
			Destroy (gameObject, 1);
		}
	}

    public void dropHealth()
    {
		if (theDrop.tag == "Key")
			Instantiate (theDrop, transform.position, transform.rotation);
		else {
			randomNumber = Random.Range (1, 101);
			if (randomNumber < 35)
				Instantiate (theDrop, transform.position, transform.rotation);
		}
    }

    IEnumerator Flasher()
    {
        for (int i = 0; i < 5; i++)
        {
            render.material.color = hurtColor;
            yield return new WaitForSeconds(.1f);
            render.material.color = normalColor;
            yield return new WaitForSeconds(.05f);
        }
    }
}
