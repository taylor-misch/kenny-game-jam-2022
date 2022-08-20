using UnityEngine;

[CreateAssetMenu(fileName = "Buildable", menuName = "MenuHexItem/Create Menu Hex Item")]
public class MenuHexItem : ScriptableObject
{
    [SerializeField] BuildMaterial buildMaterial;
    [SerializeField] GameObject menuItemPrefab;
    bool isClickable;

    public BuildMaterial BuildMaterial
    {
        get => buildMaterial;
        set => buildMaterial = value;
    }

    public GameObject MenuItemPrefab
    {
        get => menuItemPrefab;
        set => menuItemPrefab = value;
    }

    public bool IsClickable
    {
        get => isClickable;
        set => isClickable = value;
    }
}
