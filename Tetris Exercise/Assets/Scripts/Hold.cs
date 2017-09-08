using UnityEngine;
using UnityEngine.UI;

public class Hold : MonoBehaviour
{
    public void ChangeSprite()
    {
        int level = FindObjectOfType<Grid>().level % 10 == 0 ? 10 : FindObjectOfType<Grid>().level % 10;
        string spriteName = "Tetromino" + FindObjectOfType<Grid>().heldTetromino;
        transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        transform.GetChild(0).gameObject.GetComponent<Image>().sprite = (Sprite)Instantiate(Resources.Load("Tetrominos/Tetrominos" + level + "/" + spriteName, typeof(Sprite)));
    }
}