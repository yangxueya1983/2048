using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Tile : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI numText;
    public TileState state;


    public TileCell crtCell;
    
    
    public void SetState(TileState aState)
    {
        state = aState;
        
        background.color = aState.backgroundColor;
        numText.color = aState.textColor;
        numText.text = aState.number.ToString();

    }


    public void SpawnInCell(TileCell cell)
    {
        if (crtCell != null)
        {
            crtCell.tile = null;
        }
        
        crtCell = cell;
        crtCell.tile = this;

        transform.position = cell.transform.position;

    }
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
