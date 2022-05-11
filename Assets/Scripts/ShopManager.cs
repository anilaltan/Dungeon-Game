using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private int PowerupId;
    private UIManager _uimanager;

    private void Start()
    {
        _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            Player player = collision.GetComponent<Player>();
            int coin = _uimanager.GetCoin();
            if (player != null)
            {
                //enable full health
                if(coin >= 100)
                {
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
                    _uimanager.DecreaseCoin();
                    Destroy(this.gameObject);
                }
            }
            
        }
    }
}
