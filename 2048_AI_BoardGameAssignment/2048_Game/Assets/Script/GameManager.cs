using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject YouWonText;
    public GameObject GameOverText;
    public Text GameOverScoreText;
    public GameObject GameOverPanel;
    public GameObject UpArrow;
    public GameObject RightArrow;
    public GameObject DownArrow;
    public GameObject LeftArrow;

    private Tile[,] AllTiles = new Tile[4, 4];
    private List<Tile> EmptyTiles = new List<Tile>();
    private List<Tile[]> columns = new List<Tile[]>();
    private List<Tile[]> rows = new List<Tile[]>();

    // Start is called before the first frame update
    void Start()
    {
        Tile[] AllTilesOneDim = GameObject.FindObjectsOfType<Tile>();
        foreach(Tile t in AllTilesOneDim)
        {
            t.Number = 0;
            AllTiles[t.indRow, t.indCol] = t;
            EmptyTiles.Add (t);
        }

        columns.Add(new Tile[] { AllTiles[0, 0], AllTiles[1, 0], AllTiles[2, 0], AllTiles[3, 0] });
        columns.Add(new Tile[] { AllTiles[0, 1], AllTiles[1, 1], AllTiles[2, 1], AllTiles[3, 1] });
        columns.Add(new Tile[] { AllTiles[0, 2], AllTiles[1, 2], AllTiles[2, 2], AllTiles[3, 2] });
        columns.Add(new Tile[] { AllTiles[0, 3], AllTiles[1, 3], AllTiles[2, 3], AllTiles[3, 3] });

        rows.Add(new Tile[] { AllTiles[0, 0], AllTiles[0, 1], AllTiles[0, 2], AllTiles[0, 3] });
        rows.Add(new Tile[] { AllTiles[1, 0], AllTiles[1, 1], AllTiles[1, 2], AllTiles[1, 3] });
        rows.Add(new Tile[] { AllTiles[2, 0], AllTiles[2, 1], AllTiles[2, 2], AllTiles[2, 3] });
        rows.Add(new Tile[] { AllTiles[3, 0], AllTiles[3, 1], AllTiles[3, 2], AllTiles[3, 3] });

        Generate();
        Generate();

    }

    public void NewGameButtonHandler()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void YouWon()
    {
        GameOverText.SetActive(false);
        YouWonText.SetActive(true);
        GameOverScoreText.text = ScoreTracker.Instance.Score.ToString();
        GameOverPanel.SetActive(true);
    }

    private void GameOver()
    {
        GameOverScoreText.text = ScoreTracker.Instance.Score.ToString();
        GameOverPanel.SetActive(true);
    }

    bool CanMove()
    {
        if(EmptyTiles.Count > 0)
            return true;
        else
        {   //Check colums
            for (int i = 0; i < columns.Count; i++)
            {
                for (int j = 0; j < rows.Count - 1; j++)
                {
                    if (AllTiles[j, i].Number == AllTiles[j + 1, i].Number)
                    {
                        return true;
                    }
                }
            }

            //Check rows
            for (int i = 0; i< columns.Count; i++)
            {
                for(int j = 0; j < rows.Count - 1; j++)
                {
                    if(AllTiles[i,j].Number == AllTiles[i, j + 1].Number)
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }

    bool MakeOneMoveDownIndex(Tile[] LineOfTiles)
    {
        for (int i = 0; i<LineOfTiles.Length-1; i++)
        {
            //MOVE BLOCK
            if(LineOfTiles[i].Number == 0 && LineOfTiles[i+1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i + 1].Number;
                LineOfTiles[i + 1].Number = 0;
                return true;
            }

            //MERGE BLOCK
            if(LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i + 1].Number && 
                LineOfTiles[i].mergedThisTurn == false && LineOfTiles[i+1].mergedThisTurn == false)
            {
                LineOfTiles[i].Number *= 2;
                LineOfTiles[i + 1].Number = 0;
                LineOfTiles[i].mergedThisTurn = true;
                ScoreTracker.Instance.Score += LineOfTiles[i].Number;
                if(LineOfTiles[i].Number == 2048)
                {
                    YouWon();
                }
                return true;
            }
        }
        return false;
    }

    bool MakeOneMoveUpIndex(Tile[] LineOfTiles)
    {
        for (int i = LineOfTiles.Length-1; i > 0; i--)
        {
            //MOVE BLOCK
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i - 1].Number != 0)
            {
                LineOfTiles[i].Number = LineOfTiles[i - 1].Number;
                LineOfTiles[i - 1].Number = 0;
                return true;
            }

            //MERGE BLOCK
            if (LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i - 1].Number &&
                LineOfTiles[i].mergedThisTurn == false && LineOfTiles[i - 1].mergedThisTurn == false)
            {
                LineOfTiles[i].Number *= 2;
                LineOfTiles[i - 1].Number = 0;
                LineOfTiles[i].mergedThisTurn = true;
                ScoreTracker.Instance.Score += LineOfTiles[i].Number;
                if (LineOfTiles[i].Number == 2048)
                {
                    YouWon();
                }
                return true;
            }
        }
        return false;
    }

    void Generate()
    {
        if (EmptyTiles.Count > 0)
        {
            int indexForNewNumber = Random.Range(0, EmptyTiles.Count);
            int randomNum = Random.Range(0, 10);

            if (randomNum == 4)
            EmptyTiles[indexForNewNumber].Number = 4;
            else
                EmptyTiles[indexForNewNumber].Number = 2;
            
            EmptyTiles.RemoveAt(indexForNewNumber);
        }
    }

    private void ResetMergedFlags()
    {
        foreach (Tile t in AllTiles)
        {
            t.mergedThisTurn = false;
        }
    }

    private void UpdateEmptyTiles()
    {
        EmptyTiles.Clear();
        foreach(Tile t in AllTiles)
        {
            if(t.Number == 0)
            {
                EmptyTiles.Add(t);
            }
        }
    }

    private void removeArrows()
    {
        DownArrow.SetActive(false);
        LeftArrow.SetActive(false);
        UpArrow.SetActive(false);
        RightArrow.SetActive(false);
    }
    public void Move(MoveDirection md)
    {
        Debug.Log(md.ToString() + " move");
        bool moveMade = false;
        ResetMergedFlags();
        removeArrows();

        for (int i = 0; i< rows.Count; i++)
        {
            switch(md)
            {
                case MoveDirection.Down:
                    while (MakeOneMoveUpIndex(columns[i])) 
                    {
                        moveMade = true;
                        DownArrow.SetActive(true);
                    }
                    break;

                case MoveDirection.Left:
                    while (MakeOneMoveDownIndex(rows[i]))
                    {
                        moveMade = true;
                        LeftArrow.SetActive(true);
                    }
                    break;

                case MoveDirection.Right:
                    while (MakeOneMoveUpIndex(rows[i]))
                    {
                        moveMade = true;
                        RightArrow.SetActive(true);
                    }
                    break;

                case MoveDirection.Up:
                    while (MakeOneMoveDownIndex(columns[i]))
                    {
                        moveMade = true;
                        UpArrow.SetActive(true);
                    }
                    break;                
            }
        }
        if (moveMade == true)
        {
            UpdateEmptyTiles();
            Generate();

            if(!CanMove())
            {
                GameOver();
            }
        }
        
    }
}
