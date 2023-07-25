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

    public bool locked;
    
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

    public void MoveToCell(TileCell toCell)
    {
        //clear old
        if (crtCell != null)
        {
            crtCell.tile = null;
        }
        
        //modify
        crtCell = toCell;
        crtCell.tile = this;
        
        
        StartCoroutine(Animate(crtCell.transform.position, false));
    }

    public void MergeToCell(TileCell cell, TileState nextState)
    {
        //delete old
        if (crtCell != null) {
            crtCell.tile = null;
        }
        crtCell = null;
        
        //modify new
        cell.tile.SetState(nextState);
        cell.tile.locked = true;

        StartCoroutine(Animate(cell.transform.position, true));
    }
    
    private IEnumerator Animate(Vector3 to, bool merging)
    {
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;
        
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (merging) {
            Destroy(gameObject);
        }
    }
    
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

}


