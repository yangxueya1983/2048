using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
{

    public Vector2Int coordinates;
    public Tile tile;
    public bool empty => tile == null;
    public bool occupied => tile != null;
    
    
    private void Awake()
    {
        //Debug.Log("TileCell awake");
    }
    
    void Start()
    {
        //Debug.Log("TileCell Start");
        
    }
}
