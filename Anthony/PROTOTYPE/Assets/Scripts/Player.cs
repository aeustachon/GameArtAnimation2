using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 8;
        public int health = 8;
    }

    public PlayerStats playerStats = new PlayerStats();

    public int fallBoundary = -20;

    void Update()
    {
        if (transform.position.y <= fallBoundary)
            damagePlayer(99999);
    }

	public void damagePlayer(int damage)
    {
        playerStats.health -= damage;
        if (playerStats.health <= 0)
        {
            Destroy(this);
        } 
    }

    public void healPlayer(int healAmount)
    {
        playerStats.health += healAmount;
        if (playerStats.health > playerStats.maxHealth)
            playerStats.health = playerStats.maxHealth;
    }
    
     

}
