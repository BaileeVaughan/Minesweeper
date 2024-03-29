﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour
    {
        public int x, y;
        public bool isMine = false;
        public bool isRevealed = false;
        [Header("References")]
        public Sprite[] emptySprites;
        public Sprite[] mineSprites;
        private SpriteRenderer rend;

        private void Awake()
        {
            rend = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            isMine = Random.value < 0.5;
        }

        public void Reveal(int adjacentMines, int mineState = 0)
        {
            isRevealed = true;

            if (isMine)
            {
                rend.sprite = mineSprites[mineState];
            }
            else
            {
                rend.sprite = emptySprites[adjacentMines];
            }
        }
    } 
}
