using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrid : MonoBehaviour
{
    private Vector2[] surroundingTiles;
    private Vector2 currentTile;

    private bool coroutineFinished;

	void Start ()
    {
        currentTile = GridClass.grid.GetCoordinatesOfObject(gameObject);
        surroundingTiles = new Vector2[8];
        GetSurroundingTiles(currentTile);
	}

    void Update()
    {
        //The current tile will not be loaded until the coroutine is finished.
        //This will need addressing

        if ((transform.position.x >= currentTile.x + GridClass.grid.tileSize
            || transform.position.z >= currentTile.y + GridClass.grid.tileSize)
            && coroutineFinished)
        {
            coroutineFinished = false;
            StartCoroutine(CheckPosition());
        }
    }

    private void GetSurroundingTiles(Vector2 currentCoord)
    {
        int tileSize = GridClass.grid.tileSize;
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
    }

    IEnumerator CheckPosition()
    {
        yield return new WaitForSeconds(3.0f);
        currentTile = GridClass.grid.GetCoordinatesOfObject(gameObject);
        coroutineFinished = true;
    }
}
