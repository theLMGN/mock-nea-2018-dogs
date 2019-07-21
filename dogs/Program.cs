using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace dogs {

    public class randomComparer : IComparer {
        Random rnd = new Random();
        public int Compare(Object x, Object y) {
            return rnd.Next();
        }
    }

    class Program {
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
                Console.WriteLine($"CPU: {computerDogs.Count} | YOU: {humanDogs.Count}");
            }
        }
    }
}
