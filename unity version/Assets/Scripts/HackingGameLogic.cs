using System;
using System.Collections.Generic;
using System.Numerics;

public class HackingGameLogic
{
    public bool row = true;
    private int[] lastIndex = {0, 0};
    private int chances;
    
    private List<List<string>> board;
    private List<string> correctSequence;
    private List<string> listOfPossible = new List<string>() {"1C", "BD", "55", "E9", "7A"};
    private List<string> playerSequence = new List<string>();

    public List<string> GETPlayerSequence()
    {
        return playerSequence;
    }
    public List<string> GETCorrectSequence()
    {
        return correctSequence;
    }

    public List<List<string>> GETBoard()
    {
        return board;
    }

    public UnityEngine.Vector2 GETRowCol()
    {
        UnityEngine.Vector2 vector2 = new UnityEngine.Vector2(lastIndex[0], lastIndex[1]);
        return vector2;
    }
    
    public bool GameEnd;
    public bool Win;
    public HackingGameLogic(int boardSize, int sequenceSize, int chances)
    {
        correctSequence = CorrectSequence(sequenceSize);
        board = FillWithRandom(boardSize);
        this.chances = chances;
        row = true;
        lastIndex[0] = 0;
        lastIndex[1] = 0;
    }
    
    private List<List<string>> FillWithRandom(int boardSize)
    {
        List<List<string>> board = new List<List<string>>();
        // Get random generator with seed
        Random r = new Random((int)DateTime.UtcNow.Ticks);
            
        // Run through board and fill field with random value from list
        for (int i = 0; i < boardSize; i++)
        {
            board.Add(new List<string>());
            for (int j = 0; j < boardSize; j++)
            {
                int index = r.Next(listOfPossible.Count);
                board[i].Add(listOfPossible[index]);
            }
        }

        // Replace random fields with correct sequence
        for (int i = 0; i < correctSequence.Count; i++)
        {
            int index = r.Next(0, boardSize - 1);
            if (row)
            {
                while (index == lastIndex[1])
                {
                    index = r.Next(0, boardSize - 1);
                }

                lastIndex[1] = index;
            }
            else
            {
                while (index == lastIndex[0])
                {
                    index = r.Next(0, boardSize - 1);
                }
                    
                lastIndex[0] = index;
            }
            board[lastIndex[0]][lastIndex[1]] = correctSequence[i];
            row = !row;
        }
            
        return board;
    }
    
    // Generate correct sequence
        private List<string> CorrectSequence(int sequenceSize)
        {
            List<string> sequence = new List<string>();
            // Get random generator
            Random r = new Random();

            // Add random strings from list to new list
            for (int i = 0; i < sequenceSize; i++)
            {
                int index = r.Next(listOfPossible.Count);
                sequence.Add(listOfPossible[index]);
            }
            return sequence;
        }

        // Check if game ended
        public bool CheckEnd()
        {
            Win = CheckWin();
            // Check if player run out of chances or win
            if (playerSequence.Count >= chances || Win)
                return true;

            return false;
        }

        // Check if player win
        private bool CheckWin()
        {
            // Check if player sequence is long enough
            if (playerSequence.Count >= correctSequence.Count)
            {
                // Convert arrays to strings
                string playerSequence = string.Join("", this.playerSequence);
                string correctSequence = string.Join("", this.correctSequence);
                // Check if player sequence contains correct sequence
                if (playerSequence.Contains(correctSequence))
                    return true;
            }
            return false;
        }
        
        // Handle player input
        public bool PlayerInput(int index)
        {
            // Convert input to int
            //int index = (int)char.GetNumericValue(rowcol) - 1;
            
            // Check if input is valid
            if (index > board.Count - 1 || index < 0)
                return false;

            if (row)
            {
                // Check if field is valid
                if (board[lastIndex[0]][index] == "[]")
                    return false;
                lastIndex[1] = index;
            }
            else
            {
                // Check if field is valid
                if (board[index][lastIndex[1]] == "[]")
                    return false;
                lastIndex[0] = index;
            }

            // Add board field to player sequence array
            playerSequence.Add(board[lastIndex[0]][lastIndex[1]]);
            // Remove field - replace with []
            board[lastIndex[0]][lastIndex[1]] = "[]";
            row = !row;
            GameEnd = CheckEnd();
            return true;
        }
}