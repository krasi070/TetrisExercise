using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public string name;
    public string minoGroup;
    public string ghostMinoGroup;
    public float fallSpeed = 1f;
    public bool allowRotation = true;
    public bool limitRotation = false;
    public bool allowHolding = true;

    private int softDropNumber = 0;
    private float verticalInterval = 0.05f;
    private float horizontalInterval = 0.1f;
    private float verticalTime = 0;
    private float horizontalTime = 0;

    float fall = 0;

	void Start()
    {
        fall = Time.time;
        FindObjectOfType<Grid>().UpdateGrid(this);
    }
	
	void Update()
    {
        CheckInput();
	}

    void CheckInput()
    {
        if (Input.GetKeyUp(GameManager.Instance.Left) || Input.GetKeyUp(GameManager.Instance.Right) || Input.GetKeyUp(GameManager.Instance.SoftDrop))
        {
            verticalTime = 0;
            horizontalTime = 0;
        }

        if (Input.GetKey(GameManager.Instance.Left))
        {
            MoveLeft();
        }

        if (Input.GetKey(GameManager.Instance.Right))
        {
            MoveRight();
        }

        if (Input.GetKey(GameManager.Instance.SoftDrop) || Time.time - fall >= fallSpeed)
        {
            Drop();
        }

        if (Input.GetKeyDown(GameManager.Instance.HardDrop))
        {
            HardDrop();
        }

        if (Input.GetKeyDown(GameManager.Instance.RotateLeft))
        {
            Rotate(90);
        }

        if (Input.GetKeyDown(GameManager.Instance.RotateRight))
        {
            Rotate(-90);
        }

        if (Input.GetKeyDown(GameManager.Instance.Hold))
        {
            if (allowHolding)
            {
                HoldTetromino();
            }
            else
            {
                FindObjectOfType<SoundEffects>().PlayNoHoldAudio();
            }
        }
    }

    void MoveRight()
    {
        if (horizontalTime < horizontalInterval && !Input.GetKeyDown(GameManager.Instance.Right))
        {
            horizontalTime += Time.deltaTime;

            return;
        }

        horizontalTime = 0;
        transform.position += new Vector3(1, 0, 0);
        if (!CheckIsValidPosition())
        {
            transform.position -= new Vector3(1, 0, 0);
        }
        else
        {
            FindObjectOfType<Grid>().UpdateGrid(this);
            FindObjectOfType<SoundEffects>().PlayMoveAudio();
            FindObjectOfType<GhostTetromino>().transform.position = FindObjectOfType<Grid>().GetHardDropPosition(this);
        }
    }

    void MoveLeft()
    {
        if (horizontalTime < horizontalInterval && !Input.GetKeyDown(GameManager.Instance.Left))
        {
            horizontalTime += Time.deltaTime;

            return;
        }

        horizontalTime = 0;
        transform.position += new Vector3(-1, 0, 0);
        if (!CheckIsValidPosition())
        {
            transform.position -= new Vector3(-1, 0, 0);
        }
        else
        {
            FindObjectOfType<Grid>().UpdateGrid(this);
            FindObjectOfType<SoundEffects>().PlayMoveAudio();
            FindObjectOfType<GhostTetromino>().transform.position = FindObjectOfType<Grid>().GetHardDropPosition(this);
        }
    }

    void Drop()
    {
        if (verticalTime < verticalInterval && !Input.GetKeyDown(GameManager.Instance.SoftDrop))
        {
            verticalTime += Time.deltaTime;

            return;
        }

        verticalTime = 0;
        transform.position += new Vector3(0, -1, 0);
        if (!CheckIsValidPosition())
        {
            transform.position -= new Vector3(0, -1, 0);
            enabled = false;
            FindObjectOfType<SoundEffects>().PlayDropAudio();
            var grid = FindObjectOfType<Grid>();
            grid.DeleteRow();
            if (grid.IsAboveGrid(this))
            {
                grid.GameOver();
            }

            grid.currScore += softDropNumber;
        }
        else
        {
            FindObjectOfType<Grid>().UpdateGrid(this);
            if (Input.GetKey(KeyCode.DownArrow))
            {
                softDropNumber++;
            }
        }

        fall = Time.time;
    }

    void HardDrop()
    {
        int previousY = (int)transform.position.y;
        var grid = FindObjectOfType<Grid>();
        transform.position = grid.GetHardDropPosition(this);
        int currentY = (int)transform.position.y;
        FindObjectOfType<Grid>().UpdateGrid(this);
        enabled = false;
        FindObjectOfType<SoundEffects>().PlayDropAudio();
        grid.DeleteRow();
        if (grid.IsAboveGrid(this))
        {
            grid.GameOver();
        }

        grid.currScore += Mathf.Max(previousY - currentY + softDropNumber, 0);
        grid.UpdateGrid(this);
    }

    void Rotate(int value)
    {
        if (allowRotation)
        {
            if (limitRotation)
            {
                int rotation = 0;
                if (transform.eulerAngles.z >= 90)
                {
                    rotation = -90;
                }
                else
                {
                    rotation = 90;
                }

                transform.Rotate(new Vector3(0, 0, rotation));
                RotateIfPossible(rotation);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, value));
                RotateIfPossible(value);
            }
        }
        else
        {
            FindObjectOfType<SoundEffects>().PlayRotateAudio();
        }
    }

    void HoldTetromino()
    {
        Destroy(FindObjectOfType<GhostTetromino>().gameObject);
        FindObjectOfType<Grid>().UpdateGrid(this, true);
    }

    void RotateIfPossible(int rotation)
    {
        if (CheckIsValidPosition())
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).transform.Rotate(new Vector3(0, 0, -rotation));
            }

            FindObjectOfType<Grid>().UpdateGrid(this);
            FindObjectOfType<SoundEffects>().PlayRotateAudio();
            GameObject ghost = FindObjectOfType<GhostTetromino>().gameObject;
            ghost.transform.Rotate(new Vector3(0, 0, rotation));
            for (int i = 0; i < ghost.transform.childCount; i++)
            {
                ghost.transform.GetChild(i).transform.Rotate(new Vector3(0, 0, -rotation));
            }

            ghost.transform.position = FindObjectOfType<Grid>().GetHardDropPosition(this);
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -rotation));
        }
    }

    bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform)
        {
            var grid = FindObjectOfType<Grid>();
            if (!grid.CheckIsInsidePlayArea(mino.position))
            {
                return false;
            }

            if (grid.GetTransformAtPosition(mino.position) != null && grid.GetTransformAtPosition(mino.position).parent != transform)
            {
                return false;
            }
        }

        return true;
    }
}