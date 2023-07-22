using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileGrid : MonoBehaviour
{
    public TileCell[] cells { get; private set; }

    public int width = 4;
    public int height = 4;
    
    private void Awake()
    {
        cells = GetComponentsInChildren<TileCell>();
    }
    

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            TileCell tmpCell = cells[i];
            int y = i / width;
            int x = i % width;
            tmpCell.coordinates = new Vector2Int(x, y);
        }  
    }

    public TileCell GetCell(int x, int y)
    {
        if (x>=0 && x<width && y>=0 && y<height)
        {
            int index = y * width + x;
            if (index < cells.Length)
            {
                return cells[index];
            }
        }
        
        return null;
    }

    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    { 
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;
        
        Debug.Log(cell.coordinates.ToString() + direction.ToString() + coordinates.ToString());
        
        return GetCell(coordinates.x, coordinates.y);
    }
    
    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingIndex = index;

        while (cells[index].occupied)
        {
            index++;

            if (index >= cells.Length) {
                index = 0;
            }

            // all cells are occupied
            if (index == startingIndex)
                return null;
        }
    
        return cells[index];
    }
}
