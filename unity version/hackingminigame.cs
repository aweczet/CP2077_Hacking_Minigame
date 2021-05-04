using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace hacking_minigame
{
    class Program
    {
        static void Main(string[] args)
        {
            // board size, correct sequence to find, number of tries (buffer) 
            Game hacking = new Game(5, 4, 6);
            // main game loop
            while (!hacking.GameEnd)
            {
                hacking.PrintBoard();
                char value = Convert.ToChar(Console.ReadLine());
                hacking.PlayerInput(value);
                Console.Clear();
            }

            if (hacking.Win)
            {
                Console.WriteLine("Congrats");
            }
            else
            {
                Console.WriteLine("Nope");
            }
            
            hacking.PrintBoard();
        }
    }

    class Game
    {
        private List<List<string>> _board;
        private List<string> _correctSequence;
        private List<string> _listOfPossible = new() {"1C", "BD", "55", "E9", "7A"};
        private List<string> _playerSequence = new();
        
        private bool _row = true;
        private int[] _lastIndex = {0, 0};
        private int _chances;
        
        public bool GameEnd;
        public bool Win;

        // Initialize game
        public Game(int boardSize, int sequenceSize, int chances)
        {
            _correctSequence = CorrectSequence(sequenceSize);
            _board = FillWithRandom(boardSize);
            _chances = chances;
            _row = true;
            _lastIndex[0] = 0;
            _lastIndex[1] = 0;
        }
        
        // Fill board with random data and include correct sequence
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
                    int index = r.Next(_listOfPossible.Count);
                    board[i].Add(_listOfPossible[index]);
                }
            }

            // Replace random fields with correct sequence
            for (int i = 0; i < _correctSequence.Count; i++)
            {
                int index = r.Next(0, boardSize - 1);
                if (_row)
                {
                    while (index == _lastIndex[1])
                    {
                        index = r.Next(0, boardSize - 1);
                    }

                    _lastIndex[1] = index;
                }
                else
                {
                    while (index == _lastIndex[0])
                    {
                        index = r.Next(0, boardSize - 1);
                    }
                    
                    _lastIndex[0] = index;
                }
                board[_lastIndex[0]][_lastIndex[1]] = _correctSequence[i];
                _row = !_row;
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
                int index = r.Next(_listOfPossible.Count);
                sequence.Add(_listOfPossible[index]);
            }
            return sequence;
        }

        // Check if game ended
        private bool CheckEnd()
        {
            Win = CheckWin();
            // Check if player run out of chances or win
            if (_playerSequence.Count >= _chances || Win)
                return true;

            return false;
        }

        // Check if player win
        private bool CheckWin()
        {
            // Check if player sequence is long enough
            if (_playerSequence.Count >= _correctSequence.Count)
            {
                // Convert arrays to strings
                string playerSequence = string.Join("", _playerSequence);
                string correctSequence = string.Join("", _correctSequence);
                // Check if player sequence contains correct sequence
                if (playerSequence.Contains(correctSequence))
                    return true;
            }
            return false;
        }
        
        // Handle player input
        public void PlayerInput(char rowcol)
        {
            // Convert input to int
            int index = (int)char.GetNumericValue(rowcol) - 1;
            
            // Check if input is valid
            if (index > _board.Count - 1 || index < 0)
                return;

            if (_row)
            {
                // Check if field is valid
                if (_board[_lastIndex[0]][index] == "[]")
                    return;
                _lastIndex[1] = index;
            }
            else
            {
                // Check if field is valid
                if (_board[index][_lastIndex[1]] == "[]")
                    return;
                _lastIndex[0] = index;
            }

            // Add board field to player sequence array
            _playerSequence.Add(_board[_lastIndex[0]][_lastIndex[1]]);
            // Remove field - replace with []
            _board[_lastIndex[0]][_lastIndex[1]] = "[]";
            _row = !_row;
            GameEnd = CheckEnd();
        }
        
        // Print current board
        public void PrintBoard()
        {
            // Print correct sequence
            Console.Write("Sequence to find:\n");
            foreach (string s in _correctSequence)
            {
                Console.Write(s + " ");
            }
            // Print current player sequence
            Console.Write("\nPlayer sequence:\n");
            foreach (string s in _playerSequence)
            {
                Console.Write(s + " ");
            }
            Console.WriteLine("\n");
            // Print board
            foreach (List<string> list in _board)
            {
                foreach (string s in list)
                {
                    Console.Write(s + " ");
                }
                Console.WriteLine();
            }
        }
    }
}