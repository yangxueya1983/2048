using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Update = Unity.VisualScripting.Update;

public class Board : MonoBehaviour
{
    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private Tile tilePrefab;
    public TileState[] tileStatesArr;

    private List<Tile> tiles;

    void Awake()
    {
        //Debug.Log("Board awake");
        tiles = new List<Tile>(16);
    }

    void Start()
    {
        Debug.Log("Board Start");
        SpawnTile();
        SpawnTile();
        
    }

    private void Update()
    {
        //var testTile = tiles[0]; 
        //testTile.transform.position = tileGrid.cells[0].transform.position;
        //Debug.Log("Update" + testTile.transform.position.ToString());
    }

    public Tile SpawnTile()
    {
        Tile tile = Instantiate(tilePrefab, tileGrid.transform);
        tile.SetState(tileStatesArr[0]);
        tile.SpawnInCell(tileGrid.GetRandomEmptyCell());
        tiles.Add(tile);
        
        return tile;
    }


    public void ClearBoard()
    {
        foreach (var cell in tileGrid.cells)
        {
            cell.tile = null;
        }

        foreach (var tile in tiles)
        {
            tile.crtCell = null;
            Destroy(tile.gameObject);
        }
        
        tiles.Clear();
    }

}
