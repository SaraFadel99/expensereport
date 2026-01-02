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
        public void PrintReport(List<Expense> expenses)
        {
            int total = 0;
            int mealExpenses = 0;

            Console.WriteLine("Expenses " + DateTime.Now);

            foreach (Expense expense in expenses)
            {
                (int expense, bool isAmeal) expenseInfo = calcExpensePerItem( expense);
                mealExpenses += expenseInfo.isAmeal ? expenseInfo.expense : 0;
                total += expenseInfo.expense;
            }

            foreach (Expense expense in expenses)
            {
                printExpenseEntry(expense);

            }

            Console.WriteLine("Meal expenses: " + mealExpenses);
            Console.WriteLine("Total expenses: " + total);
        }

        private static void printExpenseEntry(Expense expense)
        {
            String expenseName = "";
            expenseName = GetExpenseName(expense, expenseName);

            string mealOverExpensesMarker = validateExpensesMarker(expense);

            Console.WriteLine(expenseName + "\t" + expense.amount + "\t" + mealOverExpensesMarker);
        }

        private static (int amount, bool isAmeal) calcExpensePerItem(Expense expense)
        {
            bool isAmeal=false;
            if (expense.type == ExpenseType.DINNER || expense.type == ExpenseType.BREAKFAST)
            {
                isAmeal = true;
               // mealExpenses += expense.amount;
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