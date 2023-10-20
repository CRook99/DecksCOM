using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private List<Tile> neighbours;
    
    public Tile()
    {
        neighbours = new List<Tile>();

    }
}
