using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTetromino : MonoBehaviour
{
    public GameObject tetromino;

	void Start()
    {
        transform.position = FindObjectOfType<Grid>().GetHardDropPosition(tetromino.GetComponent<Tetromino>());
    }
	
	void Update()
    {
        if (!tetromino.GetComponent<Tetromino>().enabled)
        {
            Destroy(gameObject);
        }
	}
}