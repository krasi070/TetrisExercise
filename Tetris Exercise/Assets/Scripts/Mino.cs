using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public Animator anim;
    public bool destroyed = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Transform mino = FindObjectOfType<Grid>().GetTransformAtPosition(new Vector2(transform.position.x, transform.position.y));
        if (mino != null && mino.parent != transform.parent)
        {
            FindObjectOfType<Grid>().GameOver();
        }
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

        if (destroyed && transform.localScale.x == 0 && transform.localScale.y == 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator Shrink()
    {
        for (float f = 1f; f >= 0; f -= 0.25f)
        {
            transform.localScale = new Vector3(f, f, 1);
            yield return new WaitForSeconds(0.10f);
        }

        FindObjectOfType<Grid>().deletedMino = true;
    }
}