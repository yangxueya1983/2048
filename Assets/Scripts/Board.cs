using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;
using Update = Unity.VisualScripting.Update;

public class Board : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private TileGrid tileGrid;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private GameManager gmManager;

    private int successNumber = 2048;
    public SoundPlayer soundPlayer;
    
    
    public TileState[] tileStatesArr;

    private List<Tile> tiles;

    private Vector2 startP;
    private Vector2 endP;
    
    private bool waiting;// if swipe too fast, the next action should wait

    void Awake()
    {
        //Debug.Log("Board awake");
        tiles = new List<Tile>(16);
    }

    void Start()
    {
        successNumber = tileStatesArr[^1].number;
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

        bool canBeFour = false;
        foreach (var tt in tiles)
        {
            if (tt.state.number > 4)
            {
                canBeFour = true;
                break;
            }
            
        }
        int randomIndex = (canBeFour && Random.Range(0, 3)==2) ? 1 : 0;

        tile.SetState(tileStatesArr[randomIndex]);
        tile.SpawnInCell(tileGrid.GetRandomEmptyCell());
        tiles.Add(tile);
        
        return tile;
    }

    private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        bool merged = false;

        for (int x = startX; x >= 0 && x < tileGrid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < tileGrid.height; y += incrementY)
            {
                TileCell cell = tileGrid.GetCell(x, y);
                if (cell != null && cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction, ref merged);
                }
            }
        }

        if (changed) {
            if (merged)
            {
                soundPlayer.PlayMerge();
            }
            else
            {
                soundPlayer.PlayMove();
            }
            
            StartCoroutine(WaitForChanges());
        }
    }
    public bool MoveTile(Tile tile, Vector2Int direction, ref bool merged)
    {
        TileCell targetCell = null;
        TileCell adjacentCell = tileGrid.GetAdjacentCell(tile.crtCell, direction);

        while (adjacentCell != null)
        {
            if (adjacentCell.occupied)
            {
                if (CanMerge(tile, adjacentCell))
                {
                    merged = true;
                    MergeTileToCell(tile, adjacentCell);
                    return true;
                }

                break;
            }

            targetCell = adjacentCell;
            adjacentCell = tileGrid.GetAdjacentCell(adjacentCell, direction);
        }

        if (targetCell != null)
        {
            tile.MoveToCell(targetCell);
            return true;
        }
        
        
        return false;
    }
    
    private void MergeTileToCell(Tile tile, TileCell tileCell)
    {
        tiles.Remove(tile);
        
        int indexOfState = System.Array.IndexOf(tileStatesArr, tile.state);
        if (indexOfState >= 0 && indexOfState < tileStatesArr.Length )
        {
            int index = Mathf.Clamp(indexOfState + 1, 0, tileStatesArr.Length - 1);
            TileState newState = tileStatesArr[index];
        
            tile.MergeToCell(tileCell, newState);

            if (newState.number == successNumber)
            {
                gmManager.ShowGameSuccess();
            }
            
            gmManager.IncreaseScore(newState.number);
        }
        else
        {
            Debug.Log("error in MergeTileToCell");
        }
    }

    public bool CanMerge(Tile fromTile, TileCell toCell)
    {
        return fromTile.state == toCell.tile.state && !toCell.tile.locked;
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
        if (waiting)
        {
            return;
        }
        endP = eventData.position;

        float chaX = endP.x - startP.x;
        float chaY = endP.y - startP.y;
        

        if (Mathf.Abs(chaX) > Mathf.Abs(chaY))//h
        {
            if (chaX > 0)//right
            {
                Debug.Log("right");
                Move(Vector2Int.right, tileGrid.width - 2, -1,0,1);
            }
            else//left
            {
                Debug.Log("left");
                Move(Vector2Int.left, 1, 1,0,1);
            }
        }
        else//v
        {
            
            if (chaY > 0)
            {
                Debug.Log("up");
                Move(Vector2Int.up, 0, 1,1,1);
            }
            else
            {
                Debug.Log("down");
                Move(Vector2Int.down, 0, 1, tileGrid.height - 2, -1);
            }
        }
    }
    
    
    private IEnumerator WaitForChanges()
    {
        waiting = true;
 
        yield return new WaitForSeconds(0.1f);

        waiting = false;

        foreach (var tile in tiles) {
            tile.locked = false;
        }

        if (tiles.Count != tileGrid.cells.Length) {
            SpawnTile();
        }

        if (CheckForGameOver()) {
            gmManager.ShowGameOver();
        }
    }

    private bool CheckForGameOver()
    {
        if (tiles.Count != tileGrid.width * tileGrid.height)
        {
            return false;
        }

        foreach (var tile in tiles)
        {
            TileCell up = tileGrid.GetAdjacentCell(tile.crtCell, Vector2Int.up);
            TileCell down = tileGrid.GetAdjacentCell(tile.crtCell, Vector2Int.down);
            TileCell left = tileGrid.GetAdjacentCell(tile.crtCell, Vector2Int.left);
            TileCell right = tileGrid.GetAdjacentCell(tile.crtCell, Vector2Int.right);


            if (up!=null && CanMerge(tile, up))
            {
                return false;
            }
            if (down!=null && CanMerge(tile, down))
            {
                return false;
            }
            if (left!=null && CanMerge(tile, left))
            {
                return false;
            }
            if (right!=null && CanMerge(tile, right))
            {
                return false;
            }
        }

        return true;
    }
        
}
