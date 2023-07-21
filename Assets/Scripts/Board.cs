using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using Update = Unity.VisualScripting.Update;

public class Board : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private Tile tilePrefab;
    public TileState[] tileStatesArr;

    private List<Tile> tiles;

    private Vector2 startP;
    private Vector2 endP;

    void Awake()
    {
        //Debug.Log("Board awake");
        tiles = new List<Tile>(16);
    }

    void Start()
    {
        Tile t0 = SpawnTile();
        Tile t1 = SpawnTile();
        
        //Debug.Log(t0);
        //Debug.Log(t1);
        
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

    
    
    //drag interface
    public void OnBeginDrag(PointerEventData eventData)
    {
        startP = eventData.position;
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        endP = eventData.position;

        float chaX = endP.x - startP.x;
        float chaY = endP.y - startP.y;

        if (Mathf.Abs(chaX) > Mathf.Abs(chaY))//h
        {
            if (chaX > 0)//right
            {
                Debug.Log("right");
            }
            else//left
            {
                Debug.Log("left");
            }
        }
        else//v
        {
            
            if (chaY > 0)//down
            {
                Debug.Log("up");
            }
            else//up
            {
                Debug.Log("down");
            }
        }

    }
        
}
