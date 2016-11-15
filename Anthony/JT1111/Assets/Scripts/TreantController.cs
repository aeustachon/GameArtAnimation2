using UnityEngine;
using System.Collections;

public class TreantController : MonoBehaviour
{
    public GameObject theDrop;
	public GameObject bee;
	public float attackCD = 0.3f;
	public int health = 50;

	private float attackTimer = 0;
	private bool attacking = false;
	private bool playerInAggroRange;
	private bool facingLeft;
    private bool dead;
	private int randomNumber;

	private Transform firePoint;
	private Transform target;
	private Transform transform;
	private Animator animator;
	private Renderer render;
    private Color hurtColor = Color.black;
    private Color normalColor = Color.white;

    void Start () 
	{
        render = GetComponent<Renderer>();
        dead = false;
		facingLeft = false;
		playerInAggroRange = false;
		animator = GetComponent<Animator> ();
		transform = GetComponent<Transform> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		firePoint = transform.FindChild("firePoint");
	}
		
	void Update () 
	{
        
		if (playerInAggroRange && !dead && !attacking) {
			attack ();
		} else if (!playerInAggroRange && attacking) {
			idle ();
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

	void OnTriggerEnter2D(Collider2D otherObject)
	{
		if (otherObject.gameObject.tag == "Player")
		{
			playerInAggroRange = true;
			attacking = true;
			lookAtTarget ();
		}
	}

    void OnTriggerStay2D(Collider2D otherObject)
    {
        if (otherObject.gameObject.tag == "Player")
        {
            playerInAggroRange = true;
            attacking = true;
            lookAtTarget();
        }
    }


    void OnTriggerExit2D(Collider2D otherObject)
	{
		playerInAggroRange = false;
	}

    void attack()
	{
		if (attacking == false) {
			attackTimer = attackCD;
			attacking = true;
			Instantiate (bee, firePoint.transform.position, transform.rotation);
			animator.SetInteger ("State", 1);
		}
	}

	void idle()
	{
		animator.SetInteger ("State", 0);
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

	public void Damage(int dmg)
	{
		health = health - dmg;
		StartCoroutine(Flasher());
		if (health <= 0) {
			dead = true;
			idle ();
			dropHealth ();
			Destroy (gameObject, 1);
		}
	}

    public void dropHealth()
    {
        randomNumber = Random.Range(1, 101);
        if (randomNumber < 35)
            Instantiate(theDrop, transform.position, transform.rotation);
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
