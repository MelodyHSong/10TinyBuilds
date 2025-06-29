// Ignore Spelling: Sudoku

using UnityEngine;
using UnityEngine.UI;
using TMPro; // 1. Add this line for TextMeshPro

public class GridGenerator : MonoBehaviour
{
    // Prefab for a single Sudoku cell (an InputField)
    public GameObject cellPrefab;

    // 2. Change the type from InputField to TMP_InputField
    public TMP_InputField[,] gridCells = new TMP_InputField[9, 9];

    void Awake()
    {
        GenerateGrid();
    }

    /// <summary>
    /// Instantiates the 9x9 grid of InputField cells.
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

                // 3. Change the component you're getting to TMP_InputField
                TMP_InputField cellInput = cellObject.GetComponent<TMP_InputField>();
                gridCells[row, col] = cellInput;

                int r = row;
                int c = col;
                cellInput.onValueChanged.AddListener((value) => FindObjectOfType<SudokuGameManager>().OnCellValueChanged(r, c, value));
            }
        }
    }

    /// <summary>
    /// Updates the text of the InputFields based on the board data.
    /// </summary>
    /// <param name="board">The 9x9 integer array of the current Sudoku board.</param>
    /// <param name="initialPuzzle">The 9x9 integer array of the initial puzzle to identify locked cells.</param>
    public void UpdateGridUI(int[,] board, int[,] initialPuzzle)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                // 4. Change the type here as well
                TMP_InputField cellInput = gridCells[row, col];
                int number = board[row, col];

                // Added a safety check to prevent the error and give a clearer message
                if (cellInput == null)
                {
                    Debug.LogError($"Cell at ({row},{col}) is missing its TMP_InputField component or was not assigned correctly!");
                    continue; // Skip this broken cell
                }

                // Reset cell style first
                cellInput.readOnly = false;
                cellInput.GetComponent<Image>().color = Color.white;

                if (number != 0)
                {
                    cellInput.text = number.ToString();
                    // Make the initial numbers read-only and give them a distinct color
                    if (initialPuzzle[row, col] != 0)
                    {
                        cellInput.readOnly = true;
                        cellInput.GetComponent<Image>().color = new Color(0.85f, 0.85f, 0.9f); // A light grey/blue
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
