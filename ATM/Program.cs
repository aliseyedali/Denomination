using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Program
{
    // The count of possible combinations by recursion
    static int RecursiveCalculateCombinations(int amount, List<int> denominations)
    {
        if (amount == 0)
            return 1;

        if (amount < 0 || denominations.Count == 0)
            return 0;

        // Calculate combinations with and witout the first denomination 
        return RecursiveCalculateCombinations(amount - denominations[0], denominations) + RecursiveCalculateCombinations(amount, denominations.Skip(1).ToList());
    }

    // All combinations by recursion
    static List<List<int>> RecursiveGetAllCombinations(int amount, List<int> denominations)
    {
        if (amount == 0)
            return new List<List<int>> { new List<int>() };

        if (amount < 0 || denominations.Count == 0)
            return new List<List<int>>();

        var includeFirstDenomination = RecursiveGetAllCombinations(amount - denominations[0], denominations);
        var excludeFirstDenomination = RecursiveGetAllCombinations(amount, denominations.Skip(1).ToList());

        includeFirstDenomination = includeFirstDenomination.Select(p => new List<int> { denominations[0] }.Concat(p).ToList()).ToList();

        return includeFirstDenomination.Concat(excludeFirstDenomination).ToList();
    }

    // The count of possible combinations by Dynamic Programming
    static int DPCalculateCombinations(int amount, List<int> denominations)
    {
        int[] dp = new int[amount + 1];
        dp[0] = 1;

        foreach (var denomination in denominations)
        {
            for (int i = denomination; i < dp.Length; i++)
            {
                dp[i] += dp[i - denomination];
            }
        }

        return dp[amount];
    }

    // All combinations by Dynamic Programming
    static List<List<int>> DPGetAllCombinations(int amount, List<int> denominations)
    {
        int[] dp = new int[amount + 1];
        dp[0] = 1;

        foreach (var denomination in denominations)
        {
            for (int i = denomination; i < dp.Length; i++)
            {
                dp[i] += dp[i - denomination];
            }
        }

        List<List<int>> result = new List<List<int>>();
        GetCombinations(result, new List<int>(), amount, denominations, 0, dp);
        return result;
    }

    static void GetCombinations(List<List<int>> result, List<int> currentCombination, int remainingAmount, List<int> denominations, int index, int[] dp)
    {
        if (remainingAmount == 0)
        {
            result.Add(new List<int>(currentCombination));
            return;
        }

        for (int i = index; i < denominations.Count; i++)
        {
            int denomination = denominations[i];
            if (remainingAmount >= denomination && dp[remainingAmount - denomination] > 0)
            {
                currentCombination.Add(denomination);
                GetCombinations(result, currentCombination, remainingAmount - denomination, denominations, i, dp);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }
    }

    static void DisplayCombinations(int payout, List<List<int>> combinations)
    {
        Console.WriteLine($"For {payout} EUR the count of possible combinations is {combinations.Count}, the available payout denominations would be: ");
        int index = 1;
        foreach (var combination in combinations)
        {
            var distinctCombo = combination.Distinct();
            Console.WriteLine($"{index++}) " + string.Join(" + ", distinctCombo.Select(d => $"{combination.Count(x => x == d)} x {d} EUR")));
        }
    }

    static void Main()
    {
        // Denominations
        var denominations = new List<int> { 10, 50, 100 };

        // Payout amounts
        var payouts = new List<int> { 30, 50, 60, 80, 140, 230, 370, 610, 980 };

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.WriteLine($"Calculation is starting...");
        Console.WriteLine();

        foreach (var amount in payouts)
        {
            //var countCombinations = RecursiveCalculateCombinations(amount, denominations);
            var countCombinations = DPCalculateCombinations(amount, denominations);
            Console.WriteLine($"For {amount} EUR the count of possible combinations is {countCombinations}");
            Console.WriteLine();

            /*//var allCombinations = RecursiveGetAllCombinations(amount, denominations);
            var allCombinations = DPGetAllCombinations(amount, denominations);
            DisplayCombinations(amount, allCombinations);
            Console.WriteLine();*/
        }

        Console.WriteLine($"Calculation is done.");
        Console.WriteLine();

        stopwatch.Stop();

        Console.WriteLine($"Elapsed Time:{stopwatch.Elapsed}");

        Console.WriteLine();
    }
}
