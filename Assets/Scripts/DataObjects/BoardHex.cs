using System;
using UnityEngine;

public class BoardHex
{
    int listPosition;
    Vector3Int position;
    BuildMaterial buildMaterial;

    public BoardHex(int listPosition, Vector3Int position, BuildMaterial buildMaterial)
    {
        this.listPosition = listPosition;
        this.position = position;
        this.buildMaterial = buildMaterial;
    }

    public int ListPosition
    {
        get => listPosition;
        set => listPosition = value;
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