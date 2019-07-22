using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace dogs {

    public class randomComparer : IComparer {
        Random rnd = new Random();
        public int Compare(Object x, Object y) {
            return rnd.Next();
        }
    }

    class Program {
        private static void printCard(Dog card) {
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"   Name         : {card.Name}");
            Console.WriteLine($"1. Exercise     : {card.Exercise}/5");
            Console.WriteLine($"2. Intelligence : {card.Intelligence}/100");
            Console.WriteLine($"3. Friendlyness : {card.Friendlyness}/10");
            Console.WriteLine($"4. Drool        : {card.Drool}/10 (lowest better)");
            Console.WriteLine("------------------------------------------------");
        }
        static void Main(string[] args) {
            while (true) {
                Console.WriteLine("-- Dogs Top Trumps --");
                Console.WriteLine("Please enter what you'd like to do");
                Console.WriteLine("1. Play");
                Console.WriteLine("2. Exit");
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.D1) {
                    initializeDecks();
                } else if (input.Key == ConsoleKey.D2) {
                    Console.WriteLine("\nOkay, bye!");
                    break;
                }
            }
        }

        class Dog {
            public string Name = "MISSINGNO";
            public int Exercise = -1;
            public int Intelligence = -1; 
            public int Friendlyness = -1; 
            public int Drool = 11; 
        }
        private static void initializeDecks() {
            Random rnd = new Random();

            // Read dog names.
            Console.WriteLine("\nReading dogs.txt");
            var dogNames = File.ReadAllLines("dogs.txt");

            // Randomize dog names.
            IComparer comparer = new randomComparer(); // why is c# so complicated
            Console.WriteLine("Randomizing...");
            Array.Sort(dogNames,comparer);

            // Create dogs
            List<Dog> dogs = new List<Dog>();            
            foreach (var name in dogNames) {
                Dog dog = new Dog();
                dog.Name = name;
                dog.Exercise = rnd.Next(1,6);
                dog.Intelligence = rnd.Next(1,101);
                dog.Friendlyness = rnd.Next(1,11);
                dog.Drool = rnd.Next(1,11);
                dogs.Add(dog);
            }

            // Deck size input
            int cards = 30;
            while (true) {
                Console.WriteLine("How many dogs in deck? (4-30, even numbers only, please!)");
                try {
                    int result = Int16.Parse(Console.ReadLine());
                    if (result < 4 || result > 30) {
                        Console.WriteLine("Between 4-30 please.");
                    } else {
                        if (result % 2 == 0) {
                            Console.WriteLine(result);
                            cards = result;
                            break;
                        } else {
                            Console.WriteLine("Even number please.");
                        }
                    }
                } catch (FormatException) {
                    Console.WriteLine("That doesn't look like a number to me.");
                }
            }
            Console.WriteLine(cards);
            if (dogs.Count > cards) {
                dogs.RemoveRange(cards - 1,dogs.Count - cards);
            }

            // Deck seperation
            List<Dog> computerDogs = new List<Dog>();
            List<Dog> humanDogs = new List<Dog>();
            bool turn = false; // if false, add to computer's deck, if true add to humans deck
            foreach (var dog in dogs) {
                if (turn) {
                    humanDogs.Add(dog);
                } else {
                    computerDogs.Add(dog);
                }
                turn = !turn;
            }
            game(computerDogs,humanDogs);            
        }
        private static void game(List<Dog> computerDogs, List<Dog> humanDogs) {
            bool humanTurn = true;
            while (true) {
                if (computerDogs.Count <= 0) {
                    Console.WriteLine("Congratulations! You win!");
                    System.Threading.Thread.Sleep(2000);
                    break;
                }
                if (humanDogs.Count <= 0) {
                    Console.WriteLine("Ooops! You lost.");
                    System.Threading.Thread.Sleep(2000);
                    break;
                }
                Console.WriteLine("\n\n\n\n");
                Console.WriteLine($"CPU: {computerDogs.Count} | YOU: {humanDogs.Count}");
                Dog computerCard = computerDogs[0];
                computerDogs.RemoveAt(0);
                Dog humanCard = humanDogs[0];
                humanDogs.RemoveAt(0);

                int humanScore;
                int cpuScore;
                if (humanTurn) {
                    Console.WriteLine("It's your turn!");
                    Console.WriteLine("Your current card:");
                    printCard(humanCard);
                    while (true) {
                        Console.WriteLine("What stat would you like to play? [1-4]");
                        ConsoleKeyInfo input = Console.ReadKey();
                        if (input.Key == ConsoleKey.D1) { // Exercise
                            humanScore = humanCard.Exercise;
                            cpuScore = computerCard.Exercise;
                            break;
                        } else if (input.Key == ConsoleKey.D2) { // Intelligence
                            humanScore = humanCard.Intelligence;
                            cpuScore = computerCard.Intelligence;
                            break;
                        } else if (input.Key == ConsoleKey.D3) { // Friendlyness
                            humanScore = humanCard.Friendlyness;
                            cpuScore = computerCard.Friendlyness;
                            break;
                        } else if (input.Key == ConsoleKey.D4) { // Drool
                            humanScore = 10 - humanCard.Drool;
                            cpuScore = 10 - computerCard.Drool;
                            break;
                        } else {
                            Console.WriteLine("Invalid key choice!");
                        }
                    }
                } else {
                    Console.WriteLine("It's the computers turn.");
                    Console.WriteLine("Your current card:");
                    printCard(humanCard);
                    Console.WriteLine("Computers current card:");
                    printCard(computerCard);
                    Dictionary<string,float> results = new Dictionary<string,float>();
                    results.Add("exercise",computerCard.Exercise / 5);
                    results.Add("intelligence",computerCard.Intelligence / 100);
                    results.Add("friendlyness",computerCard.Friendlyness / 10);
                    results.Add("drool",1 - (computerCard.Drool / 10));

                    // pick highest of above
                    double max = results.Max(kvp => kvp.Value);
                    string chosen = results.Where(kvp => kvp.Value == max).Select(kvp => kvp.Key).First();
                    Console.WriteLine($"Computer chose {chosen}");
                    if (chosen == "exercise") { // Exercise
                        humanScore = humanCard.Exercise;
                        cpuScore = computerCard.Exercise;
                    } else if (chosen == "intelligence") { // Intelligence
                        humanScore = humanCard.Intelligence;
                        cpuScore = computerCard.Intelligence;
                    } else if (chosen == "friendlyness") { // Friendlyness
                        humanScore = humanCard.Friendlyness;
                        cpuScore = computerCard.Friendlyness;
                    } else { // Drool
                        humanScore = 10 - humanCard.Drool;
                        cpuScore = 10 - computerCard.Drool;
                    }
                }
                Console.WriteLine("\n\n\n\n");
                Console.WriteLine("Your card was: ");
                printCard(humanCard);
                Console.WriteLine("The computers card was: ");
                printCard(computerCard);
                System.Threading.Thread.Sleep(2500);
                Console.WriteLine("\n\n\n\n");
                if (humanScore >= cpuScore) {
                    
                        Console.WriteLine("You won that round!");
                        Console.WriteLine("You obtained the card:");
                        printCard(computerCard);
                        humanDogs.Add(humanCard);
                        humanDogs.Add(computerCard);
                        humanTurn = true;
                } else {
                    Console.WriteLine("You lost that round.");
                        Console.WriteLine("You lost the card:");
                        printCard(humanCard);
                        computerDogs.Add(computerCard);
                        computerDogs.Add(humanCard);
                        humanTurn = false;
                }
                System.Threading.Thread.Sleep(2500);
            }
        }
    }
}
