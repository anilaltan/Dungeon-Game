using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToLevel1 : MonoBehaviour
{
    public GameManager _gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _gameManager.LoadLevel();
        }

    }
}
