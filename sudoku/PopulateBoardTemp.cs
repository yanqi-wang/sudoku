using sudoku.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sudoku
{

    public class PopulateBoardTemp
    {
        public class GridCell {
            public GridCell(int x, int y)
            {
                Row = x;
                Column = y;
                BlockRow = Row / 3;
                BlockCol = Column / 3;
            }
            public int BlockRow { get; }
            public int BlockCol { get; }
            public int Row { get; }
            public int Column { get; }
        }
        public Command CommandExpand(Random rng,
            Stack<int[]> stateStack,
            Stack<int> rowIndexStack,
            Stack<int> colIndexStack,
            Stack<bool[]> usedDigitsStack,
            Stack<int> lastDigitStack,
            char[][] board)
        {
            int[] currentState = new int[9 * 9];

            if (stateStack.Count > 0)
            {
                Array.Copy(stateStack.Peek(), currentState, currentState.Length);
            }

            int bestRow = -1;
            int bestCol = -1;
            bool[] bestUsedDigits = null;
            int bestCandidatesCount = -1;
            int bestRandomValue = -1;
            bool containsUnsolvableCells = false;

            // loop through each cell on the board
            for (int index = 0; index < currentState.Length; index++)
            {
                // if the cell is not yet assigned to a value
                if (currentState[index] == 0)
                {
                    // calculate the position of this cell
                    // calculate the position of the quadrant this cell is in
                    var cellPos = new GridCell(index / 9, index % 9);
                    bool[] isDigitUsed = new bool[9];

                    for (int i = 0; i < 9; i++)
                    {
                        int rowDigit = currentState[9 * i + cellPos.Column];
                        if (rowDigit > 0)
                            isDigitUsed[rowDigit - 1] = true;

                        int colDigit = currentState[9 * cellPos.Row + i];
                        if (colDigit > 0)
                            isDigitUsed[colDigit - 1] = true;

                        int blockDigit = currentState[(cellPos.BlockRow * 3 + i / 3) * 9 + (cellPos.BlockCol * 3 + i % 3)];
                        if (blockDigit > 0)
                            isDigitUsed[blockDigit - 1] = true;
                    } // for (i = 0..8)

                    int candidatesCount = isDigitUsed.Where(used => !used).Count();

                    if (candidatesCount == 0)
                    {
                        containsUnsolvableCells = true;
                        break;
                    }

                    int randomValue = rng.Next();

                    if (bestCandidatesCount < 0 ||
                        candidatesCount < bestCandidatesCount ||
                        (candidatesCount == bestCandidatesCount && randomValue < bestRandomValue))
                    {
                        bestRow = cellPos.Row;
                        bestCol = cellPos.Column;
                        bestUsedDigits = isDigitUsed;
                        bestCandidatesCount = candidatesCount;
                        bestRandomValue = randomValue;
                    }
                } // for (index 0...81)
            }

            if (!containsUnsolvableCells)
            {
                stateStack.Push(currentState);
                rowIndexStack.Push(bestRow);
                colIndexStack.Push(bestCol);
                usedDigitsStack.Push(bestUsedDigits);
                lastDigitStack.Push(0); // No digit was tried at this position
            }

            // Always try to move after expand
            return Command.Move;
        }

        public Command CommandCollapse(Stack<int[]> stateStack,
            Stack<int> rowIndexStack,
            Stack<int> colIndexStack,
            Stack<bool[]> usedDigitsStack,
            Stack<int> lastDigitStack)
        {
            stateStack.Pop();
            rowIndexStack.Pop();
            colIndexStack.Pop();
            usedDigitsStack.Pop();
            lastDigitStack.Pop();

            // Always try to move after collapse
            return Command.Move;
        }

        public Command CommandMove(Stack<int[]> stateStack,
            Stack<int> rowIndexStack,
            Stack<int> colIndexStack,
            Stack<bool[]> usedDigitsStack,
            Stack<int> lastDigitStack,
            char[][] board)
        {
            int rowToMove = rowIndexStack.Peek();
            int colToMove = colIndexStack.Peek();
            int digitToMove = lastDigitStack.Pop();

            int rowToWrite = rowToMove + rowToMove / 3 + 1;
            int colToWrite = colToMove + colToMove / 3 + 1;

            bool[] usedDigits = usedDigitsStack.Peek();
            int[] currentState = stateStack.Peek();
            int currentStateIndex = 9 * rowToMove + colToMove;

            int movedToDigit = digitToMove + 1;
            while (movedToDigit <= 9 && usedDigits[movedToDigit - 1])
                movedToDigit += 1;

            if (digitToMove > 0)
            {
                usedDigits[digitToMove - 1] = false;
                currentState[currentStateIndex] = 0;
                board[rowToWrite][colToWrite] = '.';
            }

            if (movedToDigit <= 9)
            {
                lastDigitStack.Push(movedToDigit);
                usedDigits[movedToDigit - 1] = true;
                currentState[currentStateIndex] = movedToDigit;
                board[rowToWrite][colToWrite] = (char) ('0' + movedToDigit);

                // Next possible digit was found at current position
                // Next step will be to expand the state
                return Command.Expand;
            }
            else
            {
                // No viable candidate was found at current position - pop it in the next iteration
                lastDigitStack.Push(0);
                return Command.Collapse;
            }
        }
    }
}
