using expensereport_csharp;
using System;
using System.Collections.Generic;

namespace expensereport_csharp
{
    public enum ExpenseType
    {
        DINNER, BREAKFAST, LUNCH,CAR_RENTAL
    }

    public class Expense
    {
        public ExpenseType type;
        public int amount;
    }

    public class ExpenseReport
    {

        private static readonly Dictionary<ExpenseType, int> ExpenseTypeMaxAmounts = new Dictionary<ExpenseType, int>
        {
            { ExpenseType.DINNER, 5000 },
            { ExpenseType.BREAKFAST, 1000 },
            { ExpenseType.LUNCH,2000},
            { ExpenseType.CAR_RENTAL, int.MaxValue }
         };

        public void PrintReport(List<Expense> expenses, DateTime? currentime=null)
        {
            //  DateTime time = currentime != null ? currentime : DateTime.Now;
            Console.WriteLine( generateReport(expenses));
        }

        private static string generateReport(List<Expense> expenses)
        {
            return generateReportHeader(DateTime.Now)+ "\n"+
           generateExpenseEntries(expenses)+
           generateTotalExpenses(expenses);
        }

        private static string generateReportHeader(DateTime time)
        {
            return "Expenses " +time ;
        }

        private static string generateExpenseEntries(List<Expense> expenses)
        {
            string allEntriesExpenses = "";
            foreach (Expense expense in expenses)
            {
                allEntriesExpenses += generateExpenseEntry(expense) + "\n";
            }

            return allEntriesExpenses.TrimEnd('\n');
        }

        private static string generateTotalExpenses(List<Expense> expenses)
        {
            (int mealExpenses, int total) totalExpenses = calculateExpenseTotals(expenses);

            return  genTotalsEntries(totalExpenses.total, totalExpenses.mealExpenses);
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

        private static string  genTotalsEntries(int total, int mealExpenses)
        {
            string totalExpenses = "Meal expenses:\t" + mealExpenses + "\n";
            totalExpenses += "Total expenses:\t" + total;
            return totalExpenses;
        
        }

        private static string generateExpenseEntry(Expense expense)
        {
            String expenseName = "";
            expenseName = GetExpenseName(expense, expenseName);

            string mealOverExpensesMarker = validateExpensesMarker(expense);
            string expenseEntryData = expenseName + "\t" + expense.amount + "\t" + mealOverExpensesMarker;
            return expenseEntryData; 
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
            string checkMarker = expense.type == ExpenseType.DINNER && expense.amount > ExpenseTypeMaxAmounts[ExpenseType.DINNER] ||
                                  expense.type == ExpenseType.BREAKFAST && expense.amount > ExpenseTypeMaxAmounts[ExpenseType.BREAKFAST]||
                                  expense.type == ExpenseType.LUNCH && expense.amount > ExpenseTypeMaxAmounts[ExpenseType.LUNCH] ? "X" : " ";
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
                case ExpenseType.LUNCH:
                    expenseName = "Lunch";
                    break;
                case ExpenseType.CAR_RENTAL:
                    expenseName = "Car Rental";
                    break;
            }

            return expenseName;
        }
    }
}