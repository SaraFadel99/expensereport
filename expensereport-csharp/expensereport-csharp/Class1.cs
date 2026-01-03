using System;
using System.Collections.Generic;

namespace expensereport_csharp
{
    public enum ExpenseType
    {
        DINNER, BREAKFAST, CAR_RENTAL
    }

    public class Expense
    {
        public ExpenseType type;
        public int amount;
    }

    public class ExpenseReport
    {
        public void PrintReport(List<Expense> expenses, DateTime? dateTime=null)
        {
            printReportHeader();
          //  printExpenseEntries(expenses);
            Console.WriteLine(generateExpenseEntries(expenses));
            Console.WriteLine(generateTotalExpenses(expenses));
        }

        private static void printReportHeader()
        {
            Console.WriteLine("Expenses " + DateTime.Now);
        }

        private static string generateExpenseEntries(List<Expense> expenses)
        {
            string allEntriesExpenses = "";
            foreach (Expense expense in expenses)
            {
                allEntriesExpenses += generateExpenseEntry(expense) + "\n";
            }
            return allEntriesExpenses.TrimEnd('\n');
           // Console.WriteLine(allEntriesExpenses.TrimEnd('\n'));
        }

        private static string generateTotalExpenses(List<Expense> expenses)
        {
            (int mealExpenses, int total) totalExpenses = calculateExpenseTotals(expenses);

            return printTotalExpenses(totalExpenses.total, totalExpenses.mealExpenses);
        }

        private static (int mealExpenses, int total) calculateExpenseTotals(List<Expense> expenses)
        {
            int mealExpenses= 0; int total=0;
            foreach (Expense expense in expenses)
            {
                (int expense, bool isAmeal) expenseInfo = calcExpensePerItem(expense);
                mealExpenses += expenseInfo.isAmeal ? expenseInfo.expense : 0;
                total += expenseInfo.expense;
            }
            return (mealExpenses,total);
        }

        private static string printTotalExpenses(int total, int mealExpenses)
        {
            //string totalExpenses = "";
            string totalExpenses = "Meal expenses:\t" + mealExpenses + "\n";
            totalExpenses += "Total expenses:\t" + total;
            return totalExpenses;
            //Console.WriteLine("Meal expenses:\t" + mealExpenses);
          //  Console.WriteLine("Total expenses:\t" + total);
        }

        private static string generateExpenseEntry(Expense expense)
        {
            String expenseName = "";
            expenseName = GetExpenseName(expense, expenseName);

            string mealOverExpensesMarker = validateExpensesMarker(expense);
            string expenseEntryData = expenseName + "\t" + expense.amount + "\t" + mealOverExpensesMarker;
            return expenseEntryData; 
        
           // Console.WriteLine(expenseName + "\t" + expense.amount + "\t" + mealOverExpensesMarker);
        }

        private static (int amount, bool isAmeal) calcExpensePerItem(Expense expense)
        {
            bool isAmeal=false;
            if (expense.type == ExpenseType.DINNER || expense.type == ExpenseType.BREAKFAST)
            {
                isAmeal = true;
            }

            return (expense.amount, isAmeal);
        }

        private static string validateExpensesMarker(Expense expense)
        {
            string checkMarker = expense.type == ExpenseType.DINNER && expense.amount > 5000 ||
                                  expense.type == ExpenseType.BREAKFAST && expense.amount > 1000? "X" : " ";
            return checkMarker;
        }

        private static string GetExpenseName(Expense expense, string expenseName)
        {
            switch (expense.type)
            {
                case ExpenseType.DINNER:
                    expenseName = "Dinner";
                    break;
                case ExpenseType.BREAKFAST:
                    expenseName = "Breakfast";
                    break;
                case ExpenseType.CAR_RENTAL:
                    expenseName = "Car Rental";
                    break;
            }

            return expenseName;
        }
    }
}