using NetworkObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public Type tileType;
    public PlayerController playerController;

    private void Awake() {
        // Hay que buscar al PlayerController y asignárselo
    }

    public void setTypeFromTile() {
        playerController.SetType(tileType);
    }

}
