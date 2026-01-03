using expensereport_csharp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tests
{
    public class Tests
    {
        private StringWriter _consoleOutput;
        private TextWriter _originalConsoleOut;

        [SetUp]
        public void Setup()
        {
            // Redirect Console.Out to capture output
             _originalConsoleOut = Console.Out;
            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);
        }

        [TearDown]
        public void TearDown()
        {
            // Runs AFTER each test automatically
            Console.SetOut(_originalConsoleOut);
            _consoleOutput?.Dispose();
        }

        [Test]
        public void ExpenseReportTest1()
        {
            var report = new ExpenseReport();

            List<Expense> expenses = new List<Expense>
            {
              new Expense { type = ExpenseType.BREAKFAST, amount = 2 },
              new Expense { type = ExpenseType.DINNER, amount = 3},
              new Expense { type = ExpenseType.CAR_RENTAL, amount = 2 },

            };
            DateTime now = DateTime.Now;
            report.PrintReport(expenses, now);
            string lines = _consoleOutput.ToString();

            string actualOutput = _consoleOutput.ToString().Replace("\r\n", "\n").TrimEnd('\n');

            int getMealsAmount = expenses.Where(m => m.type != ExpenseType.CAR_RENTAL).Sum(mealAmount => mealAmount.amount);
            int total = expenses.Sum(e => e.amount);
            string expectedOutput = $"Expenses {now.ToString()}\n" +
                                 $"Breakfast\t{expenses[0].amount}\t \n" +
                                 $"Dinner\t{expenses[1].amount}\t \n" +
                                 $"Car Rental\t{expenses[2].amount}\t " +
                                 $"Meal expenses:\t{getMealsAmount}\n" +
                                 $"Total expenses:\t{total}";

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [Test]
        public void ExpenseReportTestExpenseMarker()
        {
            var report = new ExpenseReport();

            List<Expense> expenses = new List<Expense>
            {
              new Expense { type = ExpenseType.BREAKFAST, amount = 2 },
              new Expense { type = ExpenseType.DINNER, amount = 3},
              new Expense { type = ExpenseType.CAR_RENTAL, amount = 2 },
              new Expense { type = ExpenseType.BREAKFAST, amount = 7000 }
            };
            DateTime now = DateTime.Now;
            report.PrintReport(expenses, now);
            string lines = _consoleOutput.ToString();

            string actualOutput = _consoleOutput.ToString().Replace("\r\n", "\n").TrimEnd('\n');
            int getMealsAmount = expenses.Where(m => m.type != ExpenseType.CAR_RENTAL).Sum(mealAmount => mealAmount.amount);
            int total = expenses.Sum(e => e.amount);
            string expectedOutput = $"Expenses {now.ToString()}\n" +
                                 $"Breakfast\t{expenses[0].amount}\t \n" +
                                 $"Dinner\t{expenses[1].amount}\t \n" +
                                 $"Car Rental\t{expenses[2].amount}\t \n" +
                                 $"Breakfast" + "\t" + expenses[3].amount + "\tX"+
                                 $"Meal expenses:\t{getMealsAmount}\n" +
                                 $"Total expenses:\t{total}";
        }

        [Test]
        public void ExpenseReportEmptyExpenseList()
        {
            var report = new ExpenseReport();

            List<Expense> expenses = new List<Expense>{  };
            DateTime now = DateTime.Now;
            report.PrintReport(expenses,now);
            string actualOutput = _consoleOutput.ToString().Replace("\r\n", "\n").TrimEnd('\n');
            string expectedOutput = $"Expenses {now.ToString()}\n" +
                                    $"Meal expenses:\t{0}\n" +
                                    $"Total expenses:\t{0}";
            Assert.AreEqual(expectedOutput, actualOutput);
        }

    }
}