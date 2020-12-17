using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
    public GameObject GridButtonsContainer;
    public Transform GridLines;
    public Sprite[] PlayerSprites;
    public TextMeshProUGUI PlayerInfo;
    public UIController _uiController;

    [Header("Private Fields")]

    [SerializeField] private int _currentPlayer;
    [SerializeField] private int[,] _grid;


    private int _gridSize;

    private void Awake()
    {
        ShowPlayerTurn(-100, erase: true);        
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _gridSize = 3;
        _grid = new int[_gridSize, _gridSize];
        InitializeGrid();        
        SetCurrentPlayer(Mathf.RoundToInt(Random.value));
        HideAllLines();
    }

    private void ShowPlayerTurn(int playerNo, bool erase=false)
    {
        if (erase)
        {
            PlayerInfo.text = string.Empty;
        }
        else
        {
            PlayerInfo.text = string.Format("Player {0}'s Turn", playerNo+1);
        }
    }

    public void SetCurrentPlayer(int i)
    {
        if (i == 0 || i == 1)
        {
            _currentPlayer = i;
            Debug.Log("Current Player=" + _currentPlayer);
            ShowPlayerTurn(_currentPlayer);
        }
    }

    public int GetCurrentPlayer()
    {
        return _currentPlayer;
    }

    private void InitializeGrid()
    {
        Debug.Log("Length of _grid = " + _grid.Length);
        for(int index =0;index< _grid.Length;index++)
        {
            //int i = Mathf.FloorToInt(index / Mathf.Sqrt(_grid.Length));
            int i = Mathf.FloorToInt(index / 3);
            int j = (int)(index % 3);
            _grid[i, j] = -1;
            GridButtonsContainer.transform.GetChild(index).GetComponent<Image>().enabled = false;   
        }
    }

    public async void GridButtonClicked(string coords)
    {
        int row = int.Parse( coords.Split()[0]);
        int col = int.Parse(coords.Split()[1]);

        if (_grid[row, col] == -1)
        {
            _grid[row, col] = GetCurrentPlayer();            
            UpdateButtonGraphic(row, col, GetCurrentPlayer());
            if (CheckGridForWin(row, col, out int lineShow))
            {
                // Game ends. Current Player wins
                await Task.Delay(1000);
                HideAllLines(showLine: lineShow);
                await Task.Delay(2000);
                _uiController.GameOver(GetCurrentPlayer());
            }
            else
            {
                SetCurrentPlayer(1 - GetCurrentPlayer());
            }
        }        
    }

    private void UpdateButtonGraphic(int row, int col, int player)
    {
        //int index = (row * (int)Mathf.Sqrt(_grid.Length)) + col;
        int index = (row * 3) + col;
        Debug.Log(string.Format("Clicked on : Row={0}, Column={1}, Index={2}", row, col, index));
        GridButtonsContainer.transform.GetChild(index).GetComponent<Image>().enabled = true;
        GridButtonsContainer.transform.GetChild(index).GetComponent<Image>().sprite = PlayerSprites[player];
    }

    private bool CheckGridForWin(int row, int col, out int lineShow)
    {
        bool diagonalCheck = false;
        lineShow = -1;
        #region Diagonal checks
        //if((row==col) && (row==0 || row == (int)Mathf.Sqrt(_grid.Length)-1)) // top left or bottom right corners
        if ((row == col) && (row == 0 || row == 2 || row==1)) // top left or bottom right corners or center
        {
            diagonalCheck = true;
        }
        //else if(Mathf.Abs(row-col) == (int)Mathf.Sqrt(_grid.Length) - 1) // top right or bottom left corner
        else if (Mathf.Abs(row - col) == 2) // top right or bottom left corner
        {
            diagonalCheck = true;
        }
        #endregion

        #region Checking Rows and Columns
        if((_grid[row,0] == _grid[row,1]) && (_grid[row, 1] == _grid[row, 2]) && (_grid[row,0]==GetCurrentPlayer()))
        {
            lineShow = 3 + row;
            return true;
        }
        else if((_grid[0, col] == _grid[1, col]) && (_grid[1, col] == _grid[2, col]) && (_grid[0, col] == GetCurrentPlayer()))
        {
            lineShow = col;
            return true;
        }
        #endregion

        #region Checking Diagonals
        else if (diagonalCheck)
        {
            //checking the first diagonal
            if(_grid[0,0]== _grid[1,1] && _grid[1,1]== _grid[2,2] && _grid[0,0]==GetCurrentPlayer())
            {
                lineShow = 6;
                return true;
            }
            // checking second diagonal
            else if(_grid[0,2]== _grid[1,1] && _grid[1,1]== _grid[2,0] && _grid[1,1]==GetCurrentPlayer())
            {
                lineShow = 7;
                return true;
            }
        }
        #endregion

        return false;
    }

    private void HideAllLines(int showLine=-1)
    {
        for(int i=0; i<8;i++)
        {
            GridLines.Find(string.Format("Line{0}", i)).gameObject.SetActive(false);
        }
        if(showLine>-1)
        {
            GridLines.Find(string.Format("Line{0}", showLine)).gameObject.SetActive(true);
        }
    }
}

