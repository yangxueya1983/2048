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
        Tile testTile = CreateTile();
        TileCell randomCell = tileGrid.GetRandomEmptyCell();
        if (randomCell == null)
        {
            // all cells are occupied
        }
        else
        {
            testTile.transform.position = randomCell.transform.position;    
        }
        
    }

    private void Update()
    {
        //var testTile = tiles[0]; 
        //testTile.transform.position = tileGrid.cells[0].transform.position;
        //Debug.Log("Update" + testTile.transform.position.ToString());
    }

    public Tile CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, tileGrid.transform);
        //tile.SetState(tileStates[0]);
        //tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
        
        Debug.Log("CreateTile");
        return tile;
    }
    

}
