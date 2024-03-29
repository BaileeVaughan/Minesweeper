﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper
{
    public class Grid : MonoBehaviour
    {
        public GameObject tilePrefab;
        public int width = 10, height = 10;
        public float spacing = .155f;
        private Tile[,] tiles;

        Tile SpawnTile(Vector3 pos)
        {
            GameObject clone = Instantiate(tilePrefab);
            clone.transform.position = pos;
            Tile currentTile = clone.GetComponent<Tile>();
            return currentTile;
        }

        private void Start()
        {
            GenerateTiles();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectATile();
            }
        }

        void GenerateTiles()
        {
            tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2 halfsize = new Vector2(width * 0.5f, height * 0.5f);

                    Vector2 pos = new Vector2(x - halfsize.x, y - halfsize.y);

                    Vector2 offset = new Vector2(0.5f, 0.5f);
                    pos += offset;
                    pos *= spacing;

                    Tile tile = SpawnTile(pos);
                    tile.transform.SetParent(transform);
                    tile.x = x;
                    tile.y = y;
                    tiles[x, y] = tile;
                }
            }
        }

        public int GetAdjacentMineCount(Tile tile)
        {
            int count = 0;
            for (int x = -1; x < 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int desiredX = tile.x + x;
                    int desiredY = tile.y + y;
                    if (desiredX < 0 || desiredX >= width || desiredY < 0 || desiredY >= height)
                    {
                        continue;
                    }

                    Tile curentTile = tiles[desiredX, desiredY];

                    if (curentTile.isMine)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        void SelectATile()
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);

            if (hit.collider != null)
            {
                Tile hitTile = hit.collider.GetComponent<Tile>();

                if (hitTile != null)
                {
                    int adjacentMines = GetAdjacentMineCount(hitTile);

                    hitTile.Reveal(adjacentMines);
                }
            }
        }

        void FFuncover(int x, int y, bool[,] visited)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                if (visited[x, y])
                {
                    return;
                }
                Tile tile = tiles[x, y];
                int adjacentMines = GetAdjacentMineCount(tile);
                tile.Reveal(adjacentMines);

                if (adjacentMines == 0)
                {
                    visited[x, y] = true;

                    FFuncover(x - 1, y, visited);
                    FFuncover(x + 1, y, visited);
                    FFuncover(x, y - 1, visited);
                    FFuncover(x, y + 1, visited);
                }
            }
        }

        void Uncov1erMines()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = tiles[x, y];
                    if (tile.isMine)
                    {
                        int adjacentMines = GetAdjacentMineCount(tile);
                    }
                }
            }               
        }
    }
}
