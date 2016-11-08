using UnityEngine;
using System.Collections;

public class healthPickUp : MonoBehaviour {

    public int healthGained;
    Player player;

	// Use this for initialization
	void Start () {
        player = GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            player.healPlayer(healthGained);

    }
}


