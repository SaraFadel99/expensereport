using expensereport_csharp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tests
{
    public class Tests
    {
        private StringWriter _consoleOutput;


        List<Expense> expenses = new List<Expense>
            {
              new Expense { type = ExpenseType.BREAKFAST, amount = 2 },
              new Expense { type = ExpenseType.DINNER, amount = 3},
              new Expense { type = ExpenseType.CAR_RENTAL, amount = 2 },

            };
        [SetUp]
        public void Setup()
        {
            // Redirect Console.Out to capture output
            TextWriter _originalConsoleOut = Console.Out;
            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);
        }

        // Test with multiple test cases using TestCaseSource
        //private static List<Expense> TestCases()
        //{
        //    var expected = "Meal expenses: " + 5;

        //    return new List<Expense>
        //    {
        //      new Expense { type = ExpenseType.BREAKFAST, amount = 2 },
        //      new Expense { type = ExpenseType.DINNER, amount = 3},
        //      new Expense { type = ExpenseType.CAR_RENTAL, amount = 2 },
         
        //    };


        //}

        [Test]
      //  [TestCaseSource(nameof(TestCases))]
        public void Test1()
        {
            Assert.Pass();
        }   

        [Test]
        public void ExpenseReportTest1()
        {
            var report = new ExpenseReport();
          
            report.PrintReport(expenses);
            string[] lines = _consoleOutput.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Assert.That(lines[0], Does.StartWith("Expenses "));
            Assert.That(lines[1], Is.EqualTo("Breakfast" + "\t" + 2 + "\t "));
            Assert.That(lines[2], Is.EqualTo("Dinner" + "\t" +  3 + "\t "));
            Assert.That(lines[3], Is.EqualTo("Car Rental" + "\t" + 2 + "\t "));
            Assert.That(lines[4], Is.EqualTo($"Meal expenses: {5}"));
            Assert.That(lines[5], Is.EqualTo($"Total expenses: {7}"));
        }
    }
}