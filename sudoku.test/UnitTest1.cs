using NUnit.Framework;
using sudoku.Models;
using System;
using System.Collections.Generic;

namespace sudoku.test
{
    public class CommandExpandShould
    {
        [Test]
        public void ShouldReturnMove()
        {
            // Arrange
            var populateBoardTemp = new PopulateBoardTemp();
            string line = "+---+---+---+";
            string middle = "|...|...|...|";
            var boardSetup = new char[][]
            {
                line.ToCharArray(),
                middle.ToCharArray(),
                middle.ToCharArray(),
                middle.ToCharArray(),
                line.ToCharArray(),
                middle.ToCharArray(),
                middle.ToCharArray(),
                middle.ToCharArray(),
                line.ToCharArray(),
                middle.ToCharArray(),
                middle.ToCharArray(),
                middle.ToCharArray(),
                line.ToCharArray()
            };

            // Act
            var actual = populateBoardTemp.CommandExpand(
                new Random(),
                new Stack<int[]>(),
                new Stack<int>(),
                new Stack<int>(),
                new Stack<bool[]>(),
                new Stack<int>(),
                boardSetup);

            // Assert
            Assert.AreEqual(actual, Command.Move);
        }
    }
}