using UnityEngine;

[CreateAssetMenu(menuName = "Tile State")]
public class TileState : ScriptableObject
{
    [SerializeField] public Color backgroundColor;
    [SerializeField] public Color textColor;
    [SerializeField] public int number;
}