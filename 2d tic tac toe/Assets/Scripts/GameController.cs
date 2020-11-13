using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject GridButtonsContainer;
    public Sprite[] PlayerSprites;

    [SerializeField] private int _currentPlayer;
    [SerializeField] private int[,] _grid;

    private int _gridSize;

    // Start is called before the first frame update
    void Start()
    {
        _gridSize = 3;
        _grid = new int[_gridSize, _gridSize];
        InitializeGrid();
        SetCurrentPlayer(Mathf.RoundToInt(Random.value));        
    }

    public void SetCurrentPlayer(int i)
    {
        if (i == 0 || i == 1)
        {
            _currentPlayer = i;
            Debug.Log("Current Player=" + _currentPlayer);
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
            int i = Mathf.FloorToInt(index / Mathf.Sqrt(_grid.Length));
            int j = (int)(index % Mathf.Sqrt(_grid.Length));
            _grid[i, j] = -1;
            GridButtonsContainer.transform.GetChild(index).GetComponent<Image>().enabled = false;   
        }
    }

    public void GridButtonClicked(string coords)
    {
        int row = int.Parse( coords.Split()[0]);
        int col = int.Parse(coords.Split()[1]);

        if (_grid[row, col] == -1)
        {
            _grid[row, col] = GetCurrentPlayer();            
            UpdateButtonGraphic(row, col, GetCurrentPlayer());
            SetCurrentPlayer(1 - GetCurrentPlayer());
        }        
    }

    private void UpdateButtonGraphic(int row, int col, int player)
    {
        int index = (row * (int)Mathf.Sqrt(_grid.Length)) + col;
        Debug.Log(string.Format("Clicked on : Row={0}, Column={1}, Index={2}", row, col, index));
        GridButtonsContainer.transform.GetChild(index).GetComponent<Image>().enabled = true;
        GridButtonsContainer.transform.GetChild(index).GetComponent<Image>().sprite = PlayerSprites[player];
    }
}
