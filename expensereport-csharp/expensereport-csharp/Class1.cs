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
        private record ExpenseTypeConfig(string Name, int MaxAmount);

        private static readonly Dictionary<ExpenseType, ExpenseTypeConfig> ExpenseTypeConfigs =
                    new Dictionary<ExpenseType, ExpenseTypeConfig>
                {
                    { ExpenseType.DINNER, new ExpenseTypeConfig("Dinner", 5000) },
                    { ExpenseType.BREAKFAST, new ExpenseTypeConfig("Breakfast", 1000) },
                    { ExpenseType.LUNCH, new ExpenseTypeConfig("Lunch", 2000) },
                    { ExpenseType.CAR_RENTAL, new ExpenseTypeConfig("Car Rental", int.MaxValue) }
                };


        public void PrintReport(List<Expense> expenses, DateTime? currentime=null)
        {
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
            expenseName = GetExpenseName(expense);

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
            string checkMarker = expense.type == ExpenseType.DINNER && expense.amount > ExpenseTypeConfigs[ExpenseType.DINNER].MaxAmount ||
                                  expense.type == ExpenseType.BREAKFAST && expense.amount > ExpenseTypeConfigs[ExpenseType.BREAKFAST].MaxAmount ||
                                  expense.type == ExpenseType.LUNCH && expense.amount > ExpenseTypeConfigs[ExpenseType.LUNCH].MaxAmount ? "X" : " ";
            return checkMarker;
        }

        private static string GetExpenseName(Expense expense)
        {

            var checkTypeExist = ExpenseTypeConfigs.TryGetValue(expense.type, out var config);
            if (checkTypeExist)
            {
                return config.Name;
            }
            return "";
        }
    }
}