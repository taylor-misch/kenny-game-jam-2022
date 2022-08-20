using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Buildable", menuName = "Select Object/Create Selectable")]
public class SelectObjectBase : ScriptableObject
{
    [SerializeField] TileBase tileBase;

    public TileBase TileBase
    {
        get { return tileBase; }
    }
}