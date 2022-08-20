using System;
using UnityEngine;

public class BoardHex
{
    Vector3Int position;
    BuildMaterial buildMaterial;

    public BoardHex(Vector3Int position, BuildMaterial buildMaterial)
    {
        this.position = position;
        this.buildMaterial = buildMaterial;
    }

    public Vector3Int Position
    {
        get => position;
        set => position = value;
    }

    public BuildMaterial BuildMaterial
    {
        get => buildMaterial;
        set => buildMaterial = value;
    }
}