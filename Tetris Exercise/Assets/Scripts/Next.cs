using UnityEngine;
using UnityEngine.UI;

public class Next : MonoBehaviour
{
    public void ChangeSprites()
    {
        int level = FindObjectOfType<Grid>().level % 10 == 0 ? 10 : FindObjectOfType<Grid>().level % 10;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            string tetrominoName = string.Join("", FindObjectOfType<Grid>().nextTetrominos[i].Split('_'));
            transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            transform.GetChild(i).gameObject.GetComponent<Image>().sprite = (Sprite)Instantiate(Resources.Load("Tetrominos/Tetrominos" + level + "/" + tetrominoName, typeof(Sprite)));
        }
    }
}