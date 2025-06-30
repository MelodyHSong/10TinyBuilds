//============================================================================
//	Author: ✰ @MelodyHSong ✰
//	Date: 2025-06-29
//	Project: 10TinyBuilds-Sudoku
//  Description: GridGenerator.cs
//============================================================================

// This script generates a 9x9 grid of InputFields for a Sudoku game, allowing players to input numbers.

// Ignore Spelling: Sudoku, grey


using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public TMP_InputField[,] gridCells = new TMP_InputField[9, 9];

    void Awake()
    {
        GenerateGrid();
    }
    /// <summary>
    /// ✰ Instantiates the 9x9 grid of InputField cells. ✰
    /// </summary>
    private void GenerateGrid()
    {
        if (cellPrefab == null)
        {
            Debug.LogError("Cell Prefab is not assigned in the GridGenerator!");
            return;
        }

        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            Debug.LogError("GridGenerator requires a GridLayoutGroup component on the same GameObject.");
            return;
        }
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 9;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                GameObject cellObject = Instantiate(cellPrefab, transform);
                cellObject.name = $"Cell_{row}_{col}";

                TMP_InputField cellInput = cellObject.GetComponent<TMP_InputField>();
                gridCells[row, col] = cellInput;

                int r = row;
                int c = col;
                cellInput.onValueChanged.AddListener((value) => FindObjectOfType<SudokuGameManager>().OnCellValueChanged(r, c, value));
            }
        }
    }
    /// <summary>
    /// ✰ Updates the text of the InputFields based on the board data. ✰
    /// </summary>
    public void UpdateGridUI(int[,] board, int[,] initialPuzzle)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                TMP_InputField cellInput = gridCells[row, col];
                int number = board[row, col];

                if (cellInput == null)
                {
                    Debug.LogError($"Cell at ({row},{col}) is missing its TMP_InputField component or was not assigned correctly!");
                    continue;
                }

                cellInput.readOnly = false;
                cellInput.GetComponent<Image>().color = Color.white;

                if (number != 0)
                {
                    cellInput.text = number.ToString();
                    // ✰ We check against the initial puzzle to lock the original numbers ✰
                    if (initialPuzzle[row, col] != 0)
                    {
                        cellInput.readOnly = true;
                        cellInput.GetComponent<Image>().color = new Color(0.85f, 0.85f, 0.9f);
                    }
                }
                else
                {
                    cellInput.text = "";
                }
            }
        }
    }
}
