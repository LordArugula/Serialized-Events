﻿using UnityEngine;

namespace Arugula.SerializedEvents
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        private void Awake()
        {
            if (player == null)
            {
                player = FindObjectOfType<Player>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.TakeDamage(10);
            }
        }
    }
}
