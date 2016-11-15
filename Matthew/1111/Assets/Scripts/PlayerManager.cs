using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
	public string loadLevelOnVictory;
    public Color hurtColor = Color.red;
    public Color normalColor = Color.white;
	public GameObject weapon;
    public bool touchScreenMode;                                //if you want to use WASD, make touchScreenMode false. if you want to use buttons, true.
    
	private bool hasKey = false;
	private float speed;
	private bool cantBeHurt = false;
	private bool facingRight = true;
    private bool moveLeft = false;
    private bool moveRight = false;
    private bool jumping = false;
    private bool wasRunningBeforeJump = false;

	private Transform weaponTransform;
	private ActorController controller;
	private Jumping jumper;
	private Player player;
    private Renderer render;
    private Animator animator;
    public Text healthText;
    private Rigidbody2D rb;

    //All audio stuff
    public AudioClip audioPlayerJump;
    public AudioClip audioPlayerDamaged;
    public AudioClip audioDoorOpen;
    public AudioClip audioKeyPickup;
    public AudioClip audioHealthPickup;
    private AudioSource audioSourcePlayer;

    public void Awake()
    {
        controller = GetComponent<ActorController>();
        jumper = GetComponent<Jumping>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<Renderer>();
        player = GetComponent<Player>();
        healthText.text = "Health: " + player.playerStats.health.ToString();
		weaponTransform = weapon.transform;
        audioSourcePlayer = GetComponent<AudioSource>();      //Audio Stuff
    }

    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        // player movement
        // left player movement
        if (Mathf.Abs(horizontalInput) > 0.0f || moveRight || moveLeft)                 //check for keyboard movement, or check if eventTriggers were pressed.
            {      
                if (horizontalInput > 0.0f || moveRight == true)                        //if Right eventTrigger pressed, or if player trying to move right
                {
                    if(moveRight == true)
                        horizontalInput = 1;
                    if (controller.IsGrounded || wasRunningBeforeJump)
                    {
                        wasRunningBeforeJump = true;
                        controller.Move(horizontalInput);
                    }
                    else
                        controller.Move(horizontalInput * .5f);
                    facingRight = true;
                }

                if (horizontalInput < 0.0f || moveLeft == true)                         //if Left event trigger pressed, or if player trying to move left
                {
                    if(moveLeft == true)
                        horizontalInput = -1;
                    if (controller.IsGrounded || wasRunningBeforeJump)
                    {
                        wasRunningBeforeJump = true;
                        controller.Move(horizontalInput);
                    }
                    else
                        controller.Move(horizontalInput * .5f);
                    facingRight = false;
                }

                if (controller.IsGrounded)                                              //no running animation unless player grounded.
                    RunAnimation();
        }

		flipXAxis (weaponTransform);
		//flipXAxis (firePoint);

        //idle
        if (controller.Velocity == Vector2.zero && !touchScreenMode)                                       //if player isn't moving and touchScreenMode isnt active.
        {
            IdleAnimation();
        }
        if(!moveLeft && !moveRight && touchScreenMode && controller.IsGrounded)                             
        {
            rb.velocity = Vector2.zero;
            IdleAnimation();
        }

    
        // jump
        if (Input.GetKeyDown(KeyCode.UpArrow) || jumping)                                          
        {
            if (wasRunningBeforeJump) {
                audioSourcePlayer.PlayOneShot(audioPlayerJump, 1F);       //Jump Sound
                jumper.Jump();
            }
            else if (!wasRunningBeforeJump)
            {
                IdleAnimation();
                audioSourcePlayer.PlayOneShot(audioPlayerJump, 1F);       //Jump Sound
                jumper.Jump();
            }
        }

        // shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Fire();
        }
        
    }

    void OnTriggerEnter2D(Collider2D otherObject)
    {
		if ((otherObject.gameObject.tag == "AggressiveEnemy" || otherObject.gameObject.tag == "EnemyAttack") && !cantBeHurt)
        {
            audioSourcePlayer.PlayOneShot(audioPlayerDamaged, 1F);        //Damage Sound
            player.damagePlayer(1);     
            setHealthText();
            StartCoroutine(Flasher());
            if(player.playerStats.health <= 0)
            {
                Destroy(gameObject);
                loadScene("Fail");
            }

        }
        else if (otherObject.gameObject.tag == "Key")
        {
            audioSourcePlayer.PlayOneShot(audioKeyPickup, 1F);        //Key Sound
            hasKey = true;
            Destroy(otherObject.gameObject);
        }
        else if(otherObject.gameObject.tag == "Door")
        {
            if (hasKey)
            {
                audioSourcePlayer.PlayOneShot(audioDoorOpen, 1F);     //Door Sound
                loadScene(loadLevelOnVictory);
            }
        }
        else if(otherObject.gameObject.tag == "HealthPickUp")
        {
            audioSourcePlayer.PlayOneShot(audioHealthPickup, 1F);     //Health Sound
            player.healPlayer(1);
            setHealthText();
            Destroy(otherObject.gameObject);
        }

    }

    void OnTriggerStay2D(Collider2D otherObject)
    {
        if ((otherObject.gameObject.tag == "Enemy" || otherObject.gameObject.tag == "AggressiveEnemy") && !cantBeHurt)
        {
            audioSourcePlayer.PlayOneShot(audioPlayerDamaged, 1F);        //Damage Sound
            player.damagePlayer(1);
            setHealthText();
            StartCoroutine(Flasher());
            if (player.playerStats.health <= 0)
            {
                Destroy(gameObject);
                loadScene("Fail");
            }
        }
    }

    private void RunAnimation()
    {
        animator.SetInteger("State", 2);
        wasRunningBeforeJump = true;
    }

    private void IdleAnimation()
    {
        wasRunningBeforeJump = false;
        animator.SetInteger("State", 0);
    }

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void setHealthText()
    {
        healthText.text = "Health: " + player.playerStats.health.ToString();
    }

    IEnumerator Flasher()
    {
        for (int i = 0; i < 5; i++)
        {
            render.material.color = hurtColor;
            cantBeHurt = true;
            yield return new WaitForSeconds(.1f);
            render.material.color = normalColor;
            yield return new WaitForSeconds(.1f);
            cantBeHurt = false;
        }
    }

    public void moveCharLeft()
    {
		
        moveLeft = true;
    }

    public void stopMovingCharLeft()
    {
        moveLeft = false;
    }

    public void moveCharRight()
    {
        moveRight = true;
    }

    public void stopMovingCharRight()
    {
        moveRight = false;
    }

    public void startJumping()
    {
        jumping = true;
    }

    public void stopJumping()
    {
        jumping = false;
    }

    public void jump()
    {
        jumper.Jump();
    }

	private void flipXAxis(Transform t)
	{
		if (t.transform.localPosition.x < 0 && facingRight == true) {
			t.transform.localPosition = new Vector2 (-t.transform.localPosition.x, t.transform.localPosition.y);
			t.transform.localScale = new Vector3 (-t.transform.localScale.x, t.transform.localScale.y, 1);
		}
		if (t.transform.localPosition.x > 0 && facingRight == false) {
			t.transform.localPosition = new Vector2 (-t.transform.localPosition.x, t.transform.localPosition.y);
			t.transform.localScale = new Vector3 (-t.transform.localScale.x, t.transform.localScale.y, 1);
		}
	}

}
