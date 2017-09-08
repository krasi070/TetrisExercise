using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMino : MonoBehaviour
{
    void Start()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        if (transform.position.y <= Grid.gridHeight - Grid.invisibleRows)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}