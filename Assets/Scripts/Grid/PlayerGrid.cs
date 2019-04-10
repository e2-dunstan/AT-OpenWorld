using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrid : MonoBehaviour
{
    public static PlayerGrid g;

    public GridGenerator grid;

    private List<Vector2> disabledTiles = new List<Vector2>();
    private Vector2[] surroundingTiles;
    private Vector2 currentCoordinate;
    [HideInInspector]
    public Tile currentTile;

    private List<Tile> tiles = new List<Tile>();

    private bool coroutineFinished = true;

	void Start ()
    {
        if (g == null)
        {
            g = this;
        }
        tiles = grid.tiles;
        currentCoordinate = GetCoordinate(gameObject);
        surroundingTiles = new Vector2[9];
        GetSurroundingTiles(currentCoordinate);
        ToggleObjects();
	}

    void FixedUpdate()
    {
        Vector2 coord = GetCoordinate(gameObject);

        if (coord != currentCoordinate
            && coord != new Vector2(-1, -1)
            && coroutineFinished)
        {
            currentCoordinate = coord;
            StartCoroutine(GetSurroundingTiles(currentCoordinate));
            ToggleObjects();
        }
    }

    IEnumerator GetSurroundingTiles(Vector2 currentCoord)
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
        //player tile
        surroundingTiles[8] = new Vector2(currentCoord.x, currentCoord.y);
        
        disabledTiles.Remove(surroundingTiles[0]);
        disabledTiles.Remove(surroundingTiles[1]);
        disabledTiles.Remove(surroundingTiles[2]);
        disabledTiles.Remove(surroundingTiles[3]);
        disabledTiles.Remove(surroundingTiles[4]);
        disabledTiles.Remove(surroundingTiles[5]);
        disabledTiles.Remove(surroundingTiles[6]);
        disabledTiles.Remove(surroundingTiles[7]);
        disabledTiles.Remove(surroundingTiles[8]);
        
        yield return null;
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
                if (obj = gameObject)
                {
                    currentTile = t;
                }
                return t.coordinate;
            }
        }
        return new Vector2(-1, -1);
    }

    private void ToggleObjects()
    {
        ObjectContainer objContainer = ObjectContainer.Load("Assets/Resources/sceneobjects.xml");

        //StartCoroutine(grid.ToggleObjectsAtTile(currentTile, true, objContainer));
        foreach (Vector2 tile in surroundingTiles)
        {
            StartCoroutine(grid.ToggleObjectsAtTile(tile, true, objContainer));
        }
        foreach (Vector2 tile in disabledTiles)
        {
            StartCoroutine(grid.ToggleObjectsAtTile(tile, false, objContainer));
        }
    }
}
