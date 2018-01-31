using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType { Empty, Passable, Impassable, Object, Wall }

    [SerializeField] private TileType tileType;
    [SerializeField] private MeshFilter trashMesh;
    [SerializeField] private GameObject trashParticles;
    [SerializeField] private Mesh[] trashLevels;

    private int id, trashLevel;
    private bool placedOn;
    private Vector2 tileCoordinates;

    void Start()
    {
        trashParticles.SetActive(false);

        trashLevel = 0;

        Vector3 newRotation = trashMesh.transform.rotation.eulerAngles;
        newRotation.y = Random.Range(0, 360);
        trashMesh.transform.rotation = Quaternion.Euler(newRotation);
    }

    public TileType GetTileType()
    {
        return tileType;
    }

    public Vector2 GetCoordinates()
    {
        return tileCoordinates;
    }

    public void SetID(int id)
    {
        this.id = id;
    }

    public int GetID()
    {
        return id;
    }

    public void SetTileType(TileType newType)
    {
        tileType = newType;
    }

    public void SetIfPlacedOn(bool placedOn)
    {
        this.placedOn = placedOn;
    }

    public void SetCoordinates(Vector2 tileCoordinates)
    {
        this.tileCoordinates = tileCoordinates;
    }

    public virtual TileSaveable GetTileSaveable()
    {
        TileSaveable newSave = new TileSaveable();
        newSave.tile_ID = this.id.ToString();
        newSave.posX = this.transform.position.x;
        newSave.posY = this.transform.position.y;
        newSave.posZ = this.transform.position.z;
        newSave.placedOn = this.placedOn;

        // TEXTURE ID NEEDS TO GO HERE!

        return newSave;
    }

    public void AddToTrash()
    {
        if(trashLevel < trashLevels.Length - 1)
        {
            trashLevel++;
            trashMesh.mesh = trashLevels[trashLevel];
            trashParticles.SetActive(false);
        }
        else if(trashLevel == trashLevels.Length - 1)
        {
            trashParticles.SetActive(true);
        }
    }

    public void CleanTrash()
    {
        trashLevel = 0;
        trashMesh.mesh = null;
        trashParticles.SetActive(false);
    }
}

[System.Serializable]
public class TileSaveable
{
    public int trashLvl;
    public string tile_ID;
    public float posX, posY, posZ;
    public int textureID;
    public bool placedOn;
}
