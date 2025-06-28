// This script manages the Sudoku game logic, including puzzle generation, solving, and player interactions.
//Ignore Spelling: Sudoku

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro; // Required for TextMeshPro UI elements

public class SudokuGameManager : MonoBehaviour
{
    // A reference to the GridGenerator script to access the UI cells
    public GridGenerator gridGenerator;
    // A reference to the UI Text element to display the score
    public TMP_Text scoreText;
    // A reference to the Win Screen panel
    public GameObject winScreen;
    // A reference to the confirmation prompt panel for the solve button
    public GameObject solveConfirmPanel;

    // The main 9x9 Sudoku board data
    private int[,] board = new int[9, 9];
    // Stores the fully solved version of the board
    private int[,] solvedBoard = new int[9, 9];
    // Stores the initial puzzle state to differentiate original numbers
    private int[,] initialPuzzle = new int[9, 9];
    // Tracks if a mistake has been made in a given cell
    private bool[,] mistakesMade = new bool[9, 9];
    // Stores the player's current score
    private int score = 0;

    // Enum to set difficulty level
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty difficulty = Difficulty.Medium;

    void Start()
    {
        GenerateNewPuzzle();
    }

    /// <summary>
    /// Generates a new random, solvable Sudoku puzzle.
    /// </summary>
    public void GenerateNewPuzzle()
    {
        // Hide overlay screens if they're active
        if (winScreen != null) winScreen.SetActive(false);
        if (solveConfirmPanel != null) solveConfirmPanel.SetActive(false);

        // 1. Create a completely empty board
        board = new int[9, 9];

        // 2. Solve it completely to get a valid, full Sudoku grid
        Solve(board, true); // Use shuffle to get a random solved grid
        System.Array.Copy(board, solvedBoard, board.Length);

        // Add a debug log for the solution, only runs in the editor
#if UNITY_EDITOR
        DebugPrintSolution(solvedBoard);
#endif

        // 3. Remove numbers to create the puzzle based on difficulty
        RemoveNumbers();

        // 4. Store the generated puzzle and update the UI
        System.Array.Copy(board, initialPuzzle, board.Length);
        gridGenerator.UpdateGridUI(board, initialPuzzle);

        // Reset score and mistakes for the new game, now that generation is complete.
        score = 0;
        mistakesMade = new bool[9, 9];
        UpdateScoreUI();
    }

    /// <summary>
    /// Removes numbers from the solved grid to create the puzzle.
    /// The number of removals depends on the difficulty setting.
    /// </summary>
    private void RemoveNumbers()
    {
        int cellsToRemove;
        switch (difficulty)
        {
            case Difficulty.Easy: cellsToRemove = 40; break;
            case Difficulty.Medium: cellsToRemove = 50; break;
            case Difficulty.Hard: cellsToRemove = 55; break;
            default: cellsToRemove = 50; break;
        }

        List<int> positions = Enumerable.Range(0, 81).ToList();
        System.Random rand = new System.Random();
        positions = positions.OrderBy(x => rand.Next()).ToList();

        int removedCount = 0;
        foreach (int pos in positions)
        {
            if (removedCount >= cellsToRemove) break;

            int row = pos / 9;
            int col = pos % 9;

            if (board[row, col] == 0) continue;

            int temp = board[row, col];
            board[row, col] = 0;

            int[,] boardCopy = new int[9, 9];
            System.Array.Copy(board, boardCopy, board.Length);

            if (CountSolutions(boardCopy, 0) != 1)
            {
                board[row, col] = temp;
            }
            else
            {
                removedCount++;
            }
        }
    }

    /// <summary>
    /// Checks if a number placement is valid according to Sudoku rules.
    /// </summary>
    private bool IsValid(int[,] currentBoard, int row, int col, int num)
    {
        for (int x = 0; x < 9; x++) if (currentBoard[row, x] == num) return false;
        for (int y = 0; y < 9; y++) if (currentBoard[y, col] == num) return false;
        int startRow = row - row % 3, startCol = col - col % 3;
        for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++) if (currentBoard[i + startRow, j + startCol] == num) return false;
        return true;
    }

    /// <summary>
    /// Instantly solves the puzzle on the board and forfeits the score.
    /// </summary>
    public void SolveSudoku()
    {
        // Hide the confirmation panel since we are proceeding.
        if (solveConfirmPanel != null) solveConfirmPanel.SetActive(false);

        // Copy the solved board state to the current board.
        System.Array.Copy(solvedBoard, board, solvedBoard.Length);
        Debug.Log("Player gave up. Showing solution.");

        // Update the grid visuals to show the complete solution.
        // We pass the solvedBoard as the 'initial puzzle' to make all cells read-only.
        gridGenerator.UpdateGridUI(board, solvedBoard);

        // Reset the score and update the UI to show a forfeit message.
        score = 0;
        if (scoreText != null)
        {
            scoreText.text = "Score: Gave Up";
        }
    }

    /// <summary>
    /// Shows the 'Are you sure?' prompt.
    /// </summary>
    public void ShowSolveConfirmPanel()
    {
        if (solveConfirmPanel != null) solveConfirmPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the 'Are you sure?' prompt.
    /// </summary>
    public void HideSolveConfirmPanel()
    {
        if (solveConfirmPanel != null) solveConfirmPanel.SetActive(false);
    }

    /// <summary>
    /// The recursive backtracking solver for puzzle generation.
    /// </summary>
    private bool Solve(int[,] currentBoard, bool shuffleNumbers = false)
    {
        List<int> numbers = Enumerable.Range(1, 9).ToList();
        if (shuffleNumbers)
        {
            System.Random rand = new System.Random();
            numbers = numbers.OrderBy(x => rand.Next()).ToList();
        }

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (currentBoard[row, col] == 0)
                {
                    foreach (int num in numbers)
                    {
                        if (IsValid(currentBoard, row, col, num))
                        {
                            currentBoard[row, col] = num;
                            if (Solve(currentBoard, shuffleNumbers)) return true;
                            currentBoard[row, col] = 0; // Backtrack
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Counts the number of possible solutions from a given board state.
    /// </summary>
    private int CountSolutions(int[,] currentBoard, int count)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (currentBoard[row, col] == 0)
                {
                    for (int num = 1; num <= 9 && count < 2; num++)
                    {
                        if (IsValid(currentBoard, row, col, num))
                        {
                            currentBoard[row, col] = num;
                            count = CountSolutions(currentBoard, count);
                        }
                    }
                    currentBoard[row, col] = 0; // Backtrack
                    return count;
                }
            }
        }
        return count + 1;
    }

    /// <summary>
    /// Handles player input and updates score.
    /// </summary>
    public void OnCellValueChanged(int row, int col, string value)
    {
        if (gridGenerator.gridCells[row, col].readOnly) return;

        if (string.IsNullOrEmpty(value))
        {
            board[row, col] = 0;
            return;
        }

        if (int.TryParse(value, out int number))
        {
            if (number == solvedBoard[row, col])
            {
                board[row, col] = number;

                if (mistakesMade[row, col]) score += 6;
                else score += 15;

                gridGenerator.gridCells[row, col].readOnly = true;
                gridGenerator.gridCells[row, col].GetComponent<Image>().color = new Color(0.8f, 1.0f, 0.8f);

                CheckForWinCondition();
            }
            else
            {
                score -= 10;
                mistakesMade[row, col] = true;

                gridGenerator.gridCells[row, col].text = "";
                board[row, col] = 0;
            }
            UpdateScoreUI();
        }
    }

    /// <summary>
    /// Checks if all cells are filled correctly.
    /// </summary>
    private void CheckForWinCondition()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] == 0) return;
            }
        }

        Debug.Log("Congratulations! You solved the puzzle!");
        if (winScreen != null) winScreen.SetActive(true);
    }

    /// <summary>
    /// Updates the score display text.
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    /// <summary>
    /// Prints the entire solved grid to the Unity console for debugging.
    /// </summary>
    private void DebugPrintSolution(int[,] solution)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("--- Sudoku Solution (Debug) ---");
        for (int row = 0; row < 9; row++)
        {
            sb.Append("  ");
            for (int col = 0; col < 9; col++)
            {
                sb.Append(solution[row, col]);
                sb.Append(" ");
            }
            sb.AppendLine();
        }
        sb.AppendLine("-------------------------------");
        Debug.Log(sb.ToString());
    }
}
