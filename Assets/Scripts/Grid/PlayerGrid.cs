using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrid : MonoBehaviour
{
    public GridClass grid;

    private List<Vector2> disabledTiles;
    private Vector2[] surroundingTiles;
    private Vector2 currentTile;

    private bool coroutineFinished = true;

	void Start ()
    {
        currentTile = grid.GetCoordinatesOfObject(gameObject);
        surroundingTiles = new Vector2[8];
        GetSurroundingTiles(currentTile);
        ToggleObjects();
	}

    void FixedUpdate()
    {
        if (grid.GetCoordinatesOfObject(gameObject) != currentTile
            //&& grid.GetCoordinatesOfObject(gameObject) != Vector2.zero
            && coroutineFinished)
        {
            currentTile = grid.GetCoordinatesOfObject(gameObject);
            coroutineFinished = false;
            CheckPosition();

            ToggleObjects();
        }
    }

    private void GetSurroundingTiles(Vector2 currentCoord)
    {
        //Debug.Log("Current tile: " + currentTile);
        disabledTiles = grid.coordinates;

        int tileSize = grid.tileSize;
        // 6 7 0
        // 5 P 1
        // 4 3 2
        surroundingTiles[0] = new Vector2(currentCoord.x + tileSize, currentCoord.y + tileSize);
        surroundingTiles[1] = new Vector2(currentCoord.x + tileSize, currentCoord.y);
        surroundingTiles[2] = new Vector2(currentCoord.x + tileSize, currentCoord.y - tileSize);
        surroundingTiles[3] = new Vector2(currentCoord.x, currentCoord.y - tileSize);
        surroundingTiles[4] = new Vector2(currentCoord.x - tileSize, currentCoord.y - tileSize);
        surroundingTiles[5] = new Vector2(currentCoord.x - tileSize, currentCoord.y);
        surroundingTiles[6] = new Vector2(currentCoord.x - tileSize, currentCoord.y + tileSize);
        surroundingTiles[7] = new Vector2(currentCoord.x, currentCoord.y + tileSize);

        int numberRemoved = 0;
        for (int i = 0; i < disabledTiles.Count; i++)
        {
            if (disabledTiles[i] == currentTile)
            {
                //Debug.Log("Disabled tile: " + disabledTiles[i]);
                disabledTiles.RemoveAt(i);
                numberRemoved++;
            }
            foreach(Vector2 tile in surroundingTiles)
            {
                if (disabledTiles[i] == tile)
                {
                    //Debug.Log("Disabled tile: " + disabledTiles[i]);
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

    private void ToggleObjects()
    {
        Debug.Log("loading new tile: " + currentTile);
        StartCoroutine(grid.ToggleObjectsAtTile(currentTile, true));

        //foreach (Vector2 tile in surroundingTiles)
        //{
        //    StartCoroutine(grid.ToggleObjectsAtTile(tile, true));
        //}
        //foreach (Vector2 tile in disabledTiles)
        //{
        //    StartCoroutine(grid.ToggleObjectsAtTile(tile, false));
        //}
    }

    private void CheckPosition()
    {
        currentTile = grid.GetCoordinatesOfObject(gameObject);
        GetSurroundingTiles(currentTile);
        coroutineFinished = true;
    }
}
