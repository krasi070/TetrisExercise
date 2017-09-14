using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Grid : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 25;
    public static int invisibleRows = 5;
    public static int maxPossible = 22;
    public static int spawnDifference = 1;
    public static bool holdingOn = true;
    public static bool ghostOn = true;
    public static Transform[,] grid = new Transform[gridHeight, gridWidth];
    public List<string> nextTetrominos = new List<string>();
    public int currScore = 0;
    public int level = 1;

    public Text scoreText;
    public Text linesText;
    public Text levelText;
    public Text highSoreText;

    public string heldTetromino = null;
    public bool deletedMino = false;

    private const int maxLevel = 30;
    private const float startRow = 20f;
    private const float startCol = 5f;
    private const int scoreOneRow = 40;
    private const int scoreTwoRows = 100;
    private const int scoreThreeRows = 300;
    private const int scoreFourRows = 1200;

    private static float[] levelSpeeds = new float[maxLevel];

    private int numberOfLinesClearedThisTurn = 0;
    private int lines = 0;

    private AudioSource audioSource;

    private List<int> fullRows = new List<int>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InitLevelSpeeds();
        GenerateFirstTetrominos();
        SpawnNextTetromino();
	}

    private void Update()
    {
        UpdateScore();
        UpdateLevel();
        UpdateUI();
        if (deletedMino)
        {
            deletedMino = false;
            for (int i = 0; i < fullRows.Count; i++)
            {
                MoveAllRowsDown(fullRows[i] + i);
            }

            SpawnNextTetromino();
        }
    }

    public void UpdateUI()
    {
        scoreText.text = currScore.ToString().PadLeft(7, '0');
        linesText.text = lines.ToString().PadLeft(3, '0');
        levelText.text = level.ToString().PadLeft(2, '0');
        highSoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString().PadLeft(7, '0');
    }

    public void UpdateScore()
    {
        if (numberOfLinesClearedThisTurn > 0)
        {
            switch (numberOfLinesClearedThisTurn)
            {
                case 1:
                    currScore += level * scoreOneRow;
                    FindObjectOfType<SoundEffects>().PlayClearedLineSound();
                    break;
                case 2:
                    currScore += level * scoreTwoRows;
                    FindObjectOfType<SoundEffects>().PlayClearedLineSound();
                    break;
                case 3:
                    currScore += level * scoreThreeRows;
                    FindObjectOfType<SoundEffects>().PlayClearedLineSound();
                    break;
                case 4:
                    currScore += level * scoreFourRows;
                    FindObjectOfType<SoundEffects>().PlayTetrisSound();
                    break;
            }
        }

        numberOfLinesClearedThisTurn = 0;
    }

    public void UpdateLevel()
    {
        int newlevel = Mathf.Min(lines / 10 + 1, maxLevel);
        if (newlevel != level)
        {
            level = newlevel;
            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {
                    if (grid[i, j] != null)
                    {
                        string type = grid[i, j].parent.gameObject.GetComponent<Tetromino>().minoGroup;
                        int minoType = level % 10 == 0 ? 10 : level % 10;
                        grid[i, j].gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Minos/Minos" + minoType + "/Mino" + type, typeof(Sprite));

                    }
                }
            }

            FindObjectOfType<Next>().ChangeSprites();
            if (!string.IsNullOrEmpty(heldTetromino))
            {
                UpdateHeldTetromino();
            }
        }
    }

    public void UpdateHeldTetromino()
    {
        FindObjectOfType<Hold>().ChangeSprite();
    }

    public bool IsAboveGrid(Tetromino tetromino)
    {
        foreach (Transform mino in tetromino.transform)
        {
            int y = (int)Mathf.Round(mino.position.y);
            if (y > maxPossible)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsRowFullAt(int row)
    {
        for (int col = 0; col < gridWidth; col++)
        {
            if (grid[row, col] == null)
            {
                return false;
            }
        }

        numberOfLinesClearedThisTurn++;
        lines++;

        return true;
    }

    public void DeleteMinoAt(int row)
    {
        for (int col = 0; col < gridWidth; col++)
        {
            grid[row, col].gameObject.GetComponent<Mino>().destroyed = true;
            StartCoroutine(grid[row, col].gameObject.GetComponent<Mino>().Shrink());       
            grid[row, col] = null;
        }
    }

    public void MoveRowDown(int row)
    {
        for (int col = 0; col < gridWidth; col++)
        {
            if (grid[row, col] != null)
            {
                grid[row + 1, col] = grid[row, col];
                grid[row, col] = null;
                grid[row + 1, col].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int row)
    {
        for (int i = row; i >= 0; i--)
        {
            MoveRowDown(i);
        }
    }

    public void DeleteRow()
    {
        fullRows = new List<int>();
        for (int row = gridHeight - 1; row >= 0; row--)
        {
            if (IsRowFullAt(row))
            {
                DeleteMinoAt(row);
                fullRows.Add(row);
            }
        }

        if (fullRows.Count == 0)
        {
            SpawnNextTetromino();
        }
    }

    public void UpdateGrid(Tetromino tetromino, bool held = false)
    {
        for (int row = 0; row < gridHeight; row++)
        {
            for (int col = 0; col < gridWidth; col++)
            {
                if (grid[row, col] != null && grid[row, col].parent == tetromino.transform)
                {
                    grid[row, col] = null;
                }
            }
        }

        if (!held)
        {
            foreach (Transform mino in tetromino.transform)
            {
                if (mino.position.y <= gridHeight)
                {
                    int row = (int)Mathf.Round(gridHeight - mino.position.y);
                    int col = (int)Mathf.Round(mino.position.x);
                    grid[row, col] = mino;
                }
            }
        }
        else
        {
            FindObjectOfType<SoundEffects>().PlayHoldAudio();
            if (!string.IsNullOrEmpty(heldTetromino))
            {
                SpawnNextTetromino(heldTetromino, false);
                heldTetromino = tetromino.name;
            }
            else
            {
                heldTetromino = tetromino.name;
                SpawnNextTetromino(false);
            }

            foreach (Transform mino in tetromino.transform)
            {
                Destroy(mino.gameObject);
            }

            Destroy(tetromino.gameObject);
            UpdateHeldTetromino();
        }
    }

    public Transform GetTransformAtPosition(Vector2 position)
    {
        int row = (int)(gridHeight - Mathf.Round(position.y));
        int col = (int)Mathf.Round(position.x);
        if (position.y > gridHeight - 1 || row < 0 || col < 0 || col >= gridWidth | row >= gridHeight)
        {
            return null;
        }
        else
        {
            return grid[row, col];
        }
    }

    public Vector2 GetHardDropPosition(Tetromino tetromino)
    {
        int offset = 0;
        for (int i = 0; i < tetromino.transform.position.y; i++)
        {
            bool possible = true;
            foreach (Transform mino in tetromino.transform)
            {
                Transform currMino = GetTransformAtPosition(new Vector2(mino.position.x, mino.position.y - i));
                if ((currMino != null && currMino.parent != tetromino.transform) || mino.position.y - i <= 0)
                {
                    possible = false;
                    break;
                }
            }

            if (possible)
            {
                offset = i;
            }
            else
            {
                break;
            }
        }

        return new Vector2(tetromino.transform.position.x, tetromino.transform.position.y - offset);
    }

    public void SpawnNextTetromino(bool allowHolding = true)
    {
        float startY = startRow;
        float startX = startCol;
        GameObject tetromino = (GameObject)Resources.Load("Prefabs/" + nextTetrominos[0], typeof(GameObject));
        while (CheckSpawnPoint(tetromino, startY, startX))
        {
            startY += spawnDifference;
        }

        int minoType = level % 10 == 0 ? 10 : level % 10;
        foreach (Transform mino in tetromino.transform)
        {
            mino.gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Minos/Minos" + minoType + "/Mino" + tetromino.GetComponent<Tetromino>().minoGroup, typeof(Sprite));
        }

        GameObject nextTetromino = Instantiate(tetromino, new Vector2(startX, startY), Quaternion.identity);
        nextTetromino.GetComponent<Tetromino>().fallSpeed = levelSpeeds[level - 1];
        nextTetromino.GetComponent<Tetromino>().allowHolding = allowHolding;
        nextTetromino.GetComponent<Tetromino>().holdingOn = holdingOn;
        if (ghostOn)
        {
            GameObject ghost = (GameObject)Resources.Load("Prefabs/Ghost" + nextTetrominos[0], typeof(GameObject));
            foreach (Transform mino in ghost.transform)
            {
                mino.gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Minos/Minos" + minoType + "/GhostMino" + tetromino.GetComponent<Tetromino>().ghostMinoGroup, typeof(Sprite));
            }

            GameObject ghostTetromino = Instantiate(ghost, new Vector3(startX, startY, 1f), Quaternion.identity);
            ghostTetromino.GetComponent<GhostTetromino>().tetromino = nextTetromino;
        }
        
        nextTetrominos.RemoveAt(0);
        nextTetrominos.Add(GetRandomTetromino());
        FindObjectOfType<Next>().ChangeSprites();
    }

    public void SpawnNextTetromino(string tetrominoName, bool allowHolding = true)
    {
        float startY = startRow;
        float startX = startCol;
        string path = "Prefabs/Tetromino_" + tetrominoName;
        GameObject tetromino = (GameObject)Resources.Load(path, typeof(GameObject));
        while (CheckSpawnPoint(tetromino, startY, startX))
        {
            startY += spawnDifference;
        }

        int minoType = level % 10 == 0 ? 10 : level % 10;
        foreach (Transform mino in tetromino.transform)
        {
            mino.gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Minos/Minos" + minoType + "/Mino" + tetromino.GetComponent<Tetromino>().minoGroup, typeof(Sprite));
        }

        GameObject nextTetromino = Instantiate(tetromino, new Vector2(startX, startY), Quaternion.identity);
        nextTetromino.GetComponent<Tetromino>().fallSpeed = levelSpeeds[level - 1];
        nextTetromino.GetComponent<Tetromino>().allowHolding = allowHolding;
        nextTetromino.GetComponent<Tetromino>().holdingOn = holdingOn;
        if (ghostOn)
        {
            GameObject ghost = (GameObject)Resources.Load("Prefabs/GhostTetromino_" + tetrominoName, typeof(GameObject));
            foreach (Transform mino in ghost.transform)
            {
                mino.gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Minos/Minos" + minoType + "/GhostMino" + tetromino.GetComponent<Tetromino>().ghostMinoGroup, typeof(Sprite));
            }

            GameObject ghostTetromino = Instantiate(ghost, new Vector3(startX, startY, 1f), Quaternion.identity);
            ghostTetromino.GetComponent<GhostTetromino>().tetromino = nextTetromino;
        }
    }

    public bool CheckIsInsidePlayArea(Vector2 position)
    {
        bool leftX = Mathf.Round(position.x) >= transform.position.x;
        bool rightX = Mathf.Round(position.x) < transform.position.x + gridWidth;
        bool downY = Mathf.Round(position.y) > transform.position.y - (gridHeight - invisibleRows);

        return leftX && rightX && downY; 
    }

    public void GameOver()
    {
        if (currScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currScore);
        }

        SceneManager.LoadScene("GameOver");
    }

    bool CheckSpawnPoint(GameObject tetromino, float y, float x)
    {
        foreach (Transform mino in tetromino.transform)
        {
            int row = (int)y + (int)Mathf.Round(mino.localPosition.y);
            int col = (int)x + (int)Mathf.Round(mino.localPosition.x);
            if (GetTransformAtPosition(new Vector2(col, row)) != null)
            {
                return true;
            }
        }


        return false;
    }

    void GenerateFirstTetrominos()
    {
        for (int i = 0; i < 3; i++)
        {
            nextTetrominos.Add(GetRandomTetromino());
        }
    }

    void InitLevelSpeeds()
    {
        levelSpeeds[0] = 0.8f;
        levelSpeeds[1] = 0.72f;
        levelSpeeds[2] = 0.63f;
        levelSpeeds[3] = 0.55f;
        levelSpeeds[4] = 0.47f;
        levelSpeeds[5] = 0.38f;
        levelSpeeds[6] = 0.3f;
        levelSpeeds[7] = 0.22f;
        levelSpeeds[8] = 0.13f;
        levelSpeeds[9] = 0.1f;
        levelSpeeds[10] = 0.08f;
        levelSpeeds[11] = 0.08f;
        levelSpeeds[12] = 0.08f;
        levelSpeeds[13] = 0.07f;
        levelSpeeds[14] = 0.07f;
        levelSpeeds[15] = 0.07f;
        levelSpeeds[16] = 0.05f;
        levelSpeeds[17] = 0.05f;
        levelSpeeds[18] = 0.05f;
        levelSpeeds[19] = 0.03f;
        levelSpeeds[20] = 0.03f;
        levelSpeeds[21] = 0.03f;
        levelSpeeds[22] = 0.03f;
        levelSpeeds[23] = 0.03f;
        levelSpeeds[24] = 0.03f;
        levelSpeeds[25] = 0.03f;
        levelSpeeds[26] = 0.03f;
        levelSpeeds[27] = 0.03f;
        levelSpeeds[28] = 0.03f;
        levelSpeeds[29] = 0.02f;
    }

    string GetRandomTetromino()
    {
        int randomTetromino = Random.Range(1, 8);
        string tetrominoName = "Tetromino_T";
        switch (randomTetromino)
        {
            case 1:
                tetrominoName = "Tetromino_T";
                break;
            case 2:
                tetrominoName = "Tetromino_O";
                break;
            case 3:
                tetrominoName = "Tetromino_I";
                break;
            case 4:
                tetrominoName = "Tetromino_S";
                break;
            case 5:
                tetrominoName = "Tetromino_Z";
                break;
            case 6:
                tetrominoName = "Tetromino_L";
                break;
            case 7:
                tetrominoName = "Tetromino_J";
                break;
        }

        return tetrominoName;
    }
}