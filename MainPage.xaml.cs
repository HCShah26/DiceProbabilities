using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI.Composition;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DiceProbabilities
{
    /// <summary>
    /// This class calculated the probability of rolling a number of dice to get different sums 
    /// </summary>
    /// <param name="numberOfDice">Number of dice to roll</param>
    /// <returns>A dictionary where the key is sum and value is its probability</returns>
    class DiceProbabilities
    {
        public static Dictionary<int, Double> CalculateProbabilitiesForNumberOfDice(int numberOfDice)
        {
            Dictionary<int, int> diceRollSumTally = new Dictionary<int, int>(); //Consider using an array instead of Dictionary???
            int minSumOfAllDiceRolled = numberOfDice; //Min sum of all dice rolled 1 = the total number of dice
            int maxSumOfAllDiceRolled = numberOfDice * 6; //Max sum of all dice rolled 6 = the total number of dice x 6 (Max dice value)
            
            Debug.WriteLine("Running the first loop");
            //This For Loop is setting the values for each possible rolled dice value to 0
            for (int index = minSumOfAllDiceRolled; index <= maxSumOfAllDiceRolled; index++)
            {
                diceRollSumTally[index] = 0;
                Debug.WriteLine($"Inside the loop rc[{index}] = {diceRollSumTally[index]}");
            }

            Debug.WriteLine("Evaluating the next loop");
            // Set an array for each dice with initial value set at 1
            int[] eachDiceValue = new int[numberOfDice];
            Array.Fill(eachDiceValue, 1);
            //Replaced the code below with the Array.Fill command
            //for (int i = 0; i < numberOfDice; i++)
            //{
            //    d[i] = 1;
            //    Debug.WriteLine($"Setting Dice[{i}] value - {d[i]}");
            //}

            bool allDiceRollsDone = false;
            while (!allDiceRollsDone)
            {
                //The code below is calculating the total for roll of all dice. 
                //Once the sum of all the dice is calculated, it's tallied in the diceRollSumTally 
                int diceRollSum = 0; //Reset the diceRollSum to 0
                foreach (int dice in eachDiceValue)
                {
                    diceRollSum += dice;
                    Debug.Write($" Current dice {dice} + ");
                }
                diceRollSumTally[diceRollSum] += 1;
                Debug.WriteLine("Sum = {diceRollSum}");

                //The code below is generating new value for each to 
                //calculate the new rolled dice values. 
                int index = 0;
                bool newDiceCombination = false;
                while (!newDiceCombination)
                {
                    //Incrementing the value of the rolled dice for a specific dice 
                    eachDiceValue[index] += 1;
                    Debug.WriteLine($"eachDiceValue[{index}] = {eachDiceValue[index]}");

                    //Checking if the rolled dice value has got to max value of the dice (6)
                    if (eachDiceValue[index] <= 6)
                    {
                        //Rolled dice value has not reached the max value
                        //Set the newDiceCombination flag to true to exit the inner loop
                        newDiceCombination = true;
                    }
                    else
                    {
                        //Rolled dice value has reached the maximum value.
                        //
                        //Check if all the dice have reached the maximum value
                        if (index == numberOfDice - 1)
                        {
                            //All the dice have reached the maximum value
                            //Set both allDiceRollsDone and newDiceCombination to true to exit both while conditional loops
                            allDiceRollsDone = true;
                            newDiceCombination = true;
                        }
                        else
                        {
                            //Set current dice rolled value to 1
                            eachDiceValue[index] = 1;
                            Debug.WriteLine($"Set the current dice {index} rolled value to {eachDiceValue[index]}");
                        }
                    }
                    index++; //Increent the index by 1 to move to the next dice
                }
            }

            //Calculate the probability of dice roll sum tally 
            Dictionary<int, Double> diceRollProbability = new Dictionary<int, double>();
            Double totalCombinations = Math.Pow(6.0, (Double)numberOfDice); //Calculates the total combination of dice roll possibility (6 ^ number of dice)
            for (int i = minSumOfAllDiceRolled; i <= maxSumOfAllDiceRolled; i++)
            {
                diceRollProbability[i] = (Double)diceRollSumTally[i] / totalCombinations;
            }
            return diceRollProbability;
        }
    }


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            int numberOfDice = 3; // Change this to test different numbers of dice

            Dictionary<int, double> probabilities = DiceProbabilities.CalculateProbabilitiesForNumberOfDice(numberOfDice);

            Debug.WriteLine($"Probabilities for rolling {numberOfDice} dice:");
            foreach (var kvp in probabilities)
            {
                Debug.WriteLine($"Sum {kvp.Key}: {kvp.Value:P2}"); // Format as percentage
            }
        }
    }
}
