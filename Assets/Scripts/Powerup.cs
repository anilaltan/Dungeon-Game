using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int PowerupId;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                //enable full health
                if (PowerupId == 0)
                {
                    player.FullHealthdPowerupOn();
                }
                //enable increase strength
                else if (PowerupId == 1)
                {
                    player.StrengthPowerupOn();
                }
                //enable shield
                else if (PowerupId == 2)
                {
                    player.ShieldPowerupOn();
                }
            }
            Destroy(this.gameObject);
        }


    }
}
