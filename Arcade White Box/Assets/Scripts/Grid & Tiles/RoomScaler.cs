using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScaler : MonoBehaviour
{

    private List<Tile> roomTiles;

    private void Awake()
    {
        roomTiles = new List<Tile>();
    }

    private void OnCollisionStay(Collision c)
    {

    }

    public List<Tile> GetRoomTiles()
    {
        return roomTiles;
    }
}
