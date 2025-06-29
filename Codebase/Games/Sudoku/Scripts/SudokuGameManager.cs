//============================================================================
//	Author: ✰ @MelodyHSong ✰
//	Date: 2025-06-28
//	Project: 10TinyBuilds-Sudoku
//  Description: SudokuGameManager.cs
//============================================================================

// This script manages the game logic for a Sudoku game,
// including generating puzzles, checking solutions, and handling player input.

// Ignore Spelling: Sudoku

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class SudokuGameManager : MonoBehaviour
{
    public GridGenerator gridGenerator;
    public TMP_Text scoreText;
    public GameObject winScreen;
    public GameObject solveConfirmPanel;

    private int[,] board = new int[9, 9];
    private int[,] solvedBoard = new int[9, 9];
    private int[,] initialPuzzle = new int[9, 9];
    private bool[,] mistakesMade = new bool[9, 9];
    private int score = 0;
    private bool gameIsOver = false;
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty difficulty = Difficulty.Medium;

    void Start()
    
    {
        if (PlayerPrefs.GetInt("load_game", 0) == 1)
        {
            LoadSavedGame();
        }
        else
        {
            this.difficulty = (Difficulty)PlayerPrefs.GetInt("difficulty", 1);
            GenerateNewPuzzle();
        }
    }

    void OnApplicationQuit()
    {
        if (!gameIsOver)
        {
            SaveCurrentGame();
        }
    }

    private void SaveCurrentGame()
    {
        GameData data = new GameData
        {
            board = this.board,
            solvedBoard = this.solvedBoard,
            initialPuzzle = this.initialPuzzle,
            mistakesMade = this.mistakesMade,
            score = this.score,
            difficulty = (int)this.difficulty
        };
        SaveLoadManager.SaveGame(data);
    }

    private void LoadSavedGame()
    {
        GameData data = SaveLoadManager.LoadGame();
        if (data != null)
        {
            this.board = data.board;
            this.solvedBoard = data.solvedBoard;
            this.initialPuzzle = data.initialPuzzle;
            this.mistakesMade = data.mistakesMade;
            this.score = data.score;
            this.difficulty = (Difficulty)data.difficulty;

            gridGenerator.UpdateGridUI(this.board, this.initialPuzzle);

            // ✰ Time to restore the grid to exactly how we left it, colors and all! ✰
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    // ✰ We only care about the numbers the player entered. ✰
                    if (this.initialPuzzle[r, c] != 0) continue;

                    if (this.board[r, c] != 0)
                    {
                        // ✰ If it's a correct guess, let's make it green and lock it in. ✰
                        if (this.board[r, c] == this.solvedBoard[r, c])
                        {
                            gridGenerator.gridCells[r, c].readOnly = true;
                            gridGenerator.gridCells[r, c].GetComponent<Image>().color = new Color(0.8f, 1.0f, 0.8f);
                        }
                        // ✰ Oops, a wrong number! Let's color it red. ✰
                        else
                        {
                            gridGenerator.gridCells[r, c].GetComponent<Image>().color = new Color(1.0f, 0.8f, 0.8f); // Light red
                        }
                    }
                }
            }
            UpdateScoreUI();
        }
        else
        {
            this.difficulty = (Difficulty)PlayerPrefs.GetInt("difficulty", 1);
            GenerateNewPuzzle();
        }
    }
    /// <summary>
    /// ✰ This is where the magic happens! Let's create a brand new, solvable puzzle. ✰
    /// </summary>
    public void GenerateNewPuzzle()
    {
        gameIsOver = false;
        if (winScreen != null) winScreen.SetActive(false);
        if (solveConfirmPanel != null) solveConfirmPanel.SetActive(false);

        board = new int[9, 9];
        Solve(board, true); // ✰ Shuffling the numbers 1-9 before solving gives us a new random grid every time. ✰
        System.Array.Copy(board, solvedBoard, board.Length);

#if UNITY_EDITOR
        DebugPrintSolution(solvedBoard);
#endif

        RemoveNumbers();
        System.Array.Copy(board, initialPuzzle, board.Length);
        gridGenerator.UpdateGridUI(board, initialPuzzle);

        score = 0;
        mistakesMade = new bool[9, 9];
        UpdateScoreUI();
    }



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
            
            if (CountSolutions(boardCopy, 0) != 1) board[row, col] = temp;
            else removedCount++;
        }
    }


    private bool IsValid(int[,] currentBoard, int row, int col, int num)
    {
        for (int x = 0; x < 9; x++) if (currentBoard[row, x] == num) return false;
        for (int y = 0; y < 9; y++) if (currentBoard[y, col] == num) return false;
        int startRow = row - row % 3, startCol = col - col % 3;
        for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++) if (currentBoard[i + startRow, j + startCol] == num) return false;
        return true;
    }
    
    /// <summary>
    /// ✰ Okay, the player is giving up. Let's show the solution and handle the score. ✰
    /// </summary>
    
    public void SolveSudoku()
    {
        if (solveConfirmPanel != null) solveConfirmPanel.SetActive(false);
        gameIsOver = true;

        System.Array.Copy(solvedBoard, board, solvedBoard.Length);
        gridGenerator.UpdateGridUI(board, solvedBoard);

        SaveLoadManager.SaveHighScore(score, "Gave Up");
        SaveLoadManager.DeleteSavedGame();

        score = 0;
        if (scoreText != null) scoreText.text = "Score: Gave Up";
    }

    public void ShowSolveConfirmPanel() { if (solveConfirmPanel != null) solveConfirmPanel.SetActive(true); }
    public void HideSolveConfirmPanel() { if (solveConfirmPanel != null) solveConfirmPanel.SetActive(false); }
    
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
                            
                            else currentBoard[row, col] = 0; // ✰ This isn't the right path, let's go back and try something else. (This is backtracking!) ✰
                            
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }

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

                    currentBoard[row, col] = 0; // ✰ Backtrack! ✰
                    return count;
                }
            }
        }
        return count + 1;
    }

    /// <summary>
    /// ✰ This function wakes up every time the player types a number in a cell. ✰

    /// </summary>
    public void OnCellValueChanged(int row, int col, string value)
    {
        if (gridGenerator.gridCells[row, col].readOnly) return;

        if (string.IsNullOrEmpty(value))
        {
            board[row, col] = 0;

            gridGenerator.gridCells[row, col].GetComponent<Image>().color = Color.white;

            return;
        }

        if (int.TryParse(value, out int number))
        {
            if (number == solvedBoard[row, col])
            {
                board[row, col] = number;

                if (mistakesMade[row, col]) score += 6;
                else score += 15;
                if (AudioManager.instance != null) AudioManager.instance.PlayCorrectSound();


                gridGenerator.gridCells[row, col].readOnly = true;
                gridGenerator.gridCells[row, col].GetComponent<Image>().color = new Color(0.8f, 1.0f, 0.8f);

                CheckForWinCondition();
            }
            else
            {

                board[row, col] = number; // ✰ Let's keep the wrong number on the board so the player can see their mistake. ✰
                score -= 10;
                mistakesMade[row, col] = true;
                if (AudioManager.instance != null) AudioManager.instance.PlayWrongSound();

                // ✰ Time to paint this cell red to show it's incorrect. ✰
                gridGenerator.gridCells[row, col].GetComponent<Image>().color = new Color(1.0f, 0.8f, 0.8f); // Light red

            }
            UpdateScoreUI();
        }
    }

    /// <summary>
    /// ✰ Did we win? Let's check the whole board to see if it's full. ✰
    /// </summary>
    private void CheckForWinCondition()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {

                if (board[row, col] == 0) return; // ✰ If we find even one empty cell, the game isn't over yet. ✰
            }
        }

        // ✰ We checked every cell and they're all full! We have a winner! ✰
        gameIsOver = true;
        if (winScreen != null) winScreen.SetActive(true);
        SaveLoadManager.SaveHighScore(score, "Completed");
        SaveLoadManager.DeleteSavedGame();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    public void BackToMainMenu()
    {
        if (!gameIsOver) SaveCurrentGame();
        SceneManager.LoadScene("MainMenu");
    }

    private void DebugPrintSolution(int[,] solution)
    {
        // ... same as before
    }
}
