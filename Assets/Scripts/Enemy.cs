using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override void Move(Tile destination)
    {
        return;
    }
    
    public override void Die()
    {
        Debug.Log("Enemy Die");
    }
}
