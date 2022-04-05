using System.Collections.Generic;

// hold off until we understand how the game plays and what we need for GameBoard

namespace sudoku.Models
{
    public class GameBoard
    {
        // Top element is current state of the board
        public Stack<int[]> State { get; set; }

        // Top elements are (row, col) of cell which has been modified compared to previous state
        public Stack<int> RowIndices { get; set; }
        public Stack<int> ColumnIndices { get; set; }

        // Top element indicates candidate digits (those with False) for (row, col)
        public Stack<bool[]> UsedDigits { get; set; }

        // Top element is the value that was set on (row, col)
        public Stack<int> LastDigits { get; set; }

        public Command Command { get; set; }

        public char[][] Board;
    }

    // Indicates operation to perform next
    // - expand - finds next empty cell and puts new state on stacks
    // - move - finds next candidate number at current pos and applies it to current state
    // - collapse - pops current state from stack as it did not yield a solution
    public enum Command
    {
        Expand,
        Move,
        Collapse,
        Fail,
        Complete
    }
}
