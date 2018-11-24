using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrid : MonoBehaviour
{
    public GridGenerator grid;

    private List<Vector2> disabledTiles = new List<Vector2>();
    private Vector2[] surroundingTiles;
    private Vector2 currentTile;

    private List<Tile> tiles = new List<Tile>();

    private bool coroutineFinished = true;

	void Start ()
    {
        tiles = grid.tiles;
        currentTile = GetCoordinate(gameObject);
        surroundingTiles = new Vector2[8];
        GetSurroundingTiles(currentTile);
        ToggleObjects();
	}

    void FixedUpdate()
    {
        Vector2 coord = GetCoordinate(gameObject);

        if (coord != currentTile
            && coord != new Vector2(-1, -1)
            && coroutineFinished)
        {
            currentTile = coord;
            GetSurroundingTiles(currentTile);
            ToggleObjects();
        }
    }

    private void GetSurroundingTiles(Vector2 currentCoord)
    {
        if (disabledTiles.Count > 0)
        {
            disabledTiles.Clear();
        }
        foreach (Tile tile in tiles)
        {
            disabledTiles.Add(tile.coordinate);
        }
        // 6 7 0
        // 5 P 1
        // 4 3 2
        surroundingTiles[0] = new Vector2(currentCoord.x + 1, currentCoord.y + 1);
        surroundingTiles[1] = new Vector2(currentCoord.x + 1, currentCoord.y);
        surroundingTiles[2] = new Vector2(currentCoord.x + 1, currentCoord.y - 1);
        surroundingTiles[3] = new Vector2(currentCoord.x, currentCoord.y - 1);
        surroundingTiles[4] = new Vector2(currentCoord.x - 1, currentCoord.y - 1);
        surroundingTiles[5] = new Vector2(currentCoord.x - 1, currentCoord.y);
        surroundingTiles[6] = new Vector2(currentCoord.x - 1, currentCoord.y + 1);
        surroundingTiles[7] = new Vector2(currentCoord.x, currentCoord.y + 1);

        int numberRemoved = 0;
        for (int i = 0; i < disabledTiles.Count; i++)
        {
            if (disabledTiles[i] == currentTile)
            {
                disabledTiles.RemoveAt(i);
                numberRemoved++;
            }
            foreach(Vector2 tile in surroundingTiles)
            {
                if (disabledTiles[i] == tile)
                {
                    disabledTiles.RemoveAt(i);
                    numberRemoved++;
                }
            }
            if (numberRemoved == 9)
            {
                break;
            }
        }
    }

    private Vector2 GetCoordinate(GameObject obj)
    {
        foreach (Tile t in tiles)
        {
            if (obj.transform.position.x >= t.worldPosition.x
                && obj.transform.position.z >= t.worldPosition.z
                && obj.transform.position.x < t.worldPosition.x + grid.tileSize
                && obj.transform.position.z < t.worldPosition.z + grid.tileSize)
            {
                return t.coordinate;
            }
        }
        return new Vector2(-1, -1);
    }

    private void ToggleObjects()
    {
        Debug.Log("loading new tile: " + currentTile);

        StartCoroutine(grid.ToggleObjectsAtTile(currentTile, true));
        foreach (Vector2 tile in surroundingTiles)
        {
            StartCoroutine(grid.ToggleObjectsAtTile(tile, true));
        }
        foreach (Vector2 tile in disabledTiles)
        {
            StartCoroutine(grid.ToggleObjectsAtTile(tile, false));
        }
    }
}
