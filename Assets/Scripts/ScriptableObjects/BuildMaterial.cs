using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Buildable", menuName = "BuildMaterial/Create Build Material")]
public class BuildMaterial : ScriptableObject
{
    [SerializeField] int buildMaterialIndex;
    [SerializeField] String materialName;
    [SerializeField] List<BuildMaterial> downgradeOptions = new List<BuildMaterial>();
    [SerializeField] List<BuildMaterial> buildRecipe = new List<BuildMaterial>();
    [SerializeField] bool isBaseMaterial;
    [SerializeField] Sprite sprite;
    [SerializeField] TileBase tileBase;

    public int BuildMaterialIndex
    {
        get => buildMaterialIndex;
        set => buildMaterialIndex = value;
    }

    public string MaterialName
    {
        get => materialName;
        set => materialName = value;
    }

    public List<BuildMaterial> DowngradeOptions
    {
        get => downgradeOptions;
        set => downgradeOptions = value;
    }

    public List<BuildMaterial> BuildRecipe
    {
        get => buildRecipe;
        set => buildRecipe = value;
    }

    public bool IsBaseMaterial
    {
        get => isBaseMaterial;
        set => isBaseMaterial = value;
    }

    public Sprite Sprite
    {
        get => sprite;
        set => sprite = value;
    }

    public TileBase TileBase
    {
        get => tileBase;
        set => tileBase = value;
    }
    
}