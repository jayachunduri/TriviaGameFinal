using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaGame
{
    class Program
    {
        public static string name;
        public static int score = 0;

        static void Main(string[] args)
        {
            //The logic for your trivia game happens here
            List<Trivia> AllQuestions = GetTriviaList();

            bool playing = true;
            //variables to keep track of correct and wrong answers
            int correct = 0, wrong = 0, total = 0;

            //random number generator to pick a random question
            Random rng = new Random();

            Console.WriteLine("Enter your name");
            name = Console.ReadLine();
            //loop for the game
            while (playing)
            {
                //pick a random question
                int question = rng.Next(1, 501);

                Console.WriteLine("The question is: " +AllQuestions[question].Question);
                Console.WriteLine("Enter Answer :");
                string ansByUser = Console.ReadLine();

                //checking if the answer is correct or not
                if (AllQuestions[question].Answer.ToLower() == ansByUser.ToLower())
                {
                    Console.WriteLine("Congratulations! You got it");
                    correct++;
                    score += 100;
                }
                else //means wrong answer
                {
                    Console.WriteLine("Sorry you got it wrong");
                    Console.WriteLine("The correct answer is: " + AllQuestions[question].Answer);
                    wrong++;
                    score -= 10;
                }

                //checking if the user wants to play more
                Console.WriteLine("Do you want to play more\n Enter \"Yes\" to play more \nEnter \"No\" to finish");
                string choice = Console.ReadLine();
                if (choice.ToLower() == "no")
                {
                    playing = false;
                }

            }
            total = correct + wrong;
            //comes out of while loop after the game is over
            Console.WriteLine("Out of " + total + " questions, you got " + correct + " questions correct and " + wrong + " questions wrong");
            Console.WriteLine("Thanks for playing");

            AddToDB();
            InfoFromDB();
        }


        //This functions gets the full list of trivia questions from the Trivia.txt document
        static List<Trivia> GetTriviaList()
        {
            //Get Contents from the file.  Remove the special char "\r".  Split on each line.  Convert to a list.
            List<string> contents = File.ReadAllText("trivia.txt").Replace("\r", "").Split('\n').ToList();

            //Each item in list "contents" is now one line of the Trivia.txt document.
            
            //make a new list to return all trivia questions
            List<Trivia> returnList = new List<Trivia>();
            // TODO: go through each line in contents of the trivia file and make a trivia object.
            //       add it to our return list.
            // Example: Trivia newTrivia = new Trivia("what is my name?*question");
           
            string line;

            // Read the file and display it line by line.
            try
            {
            StreamReader file =
               new StreamReader("trivia.txt");

            while ((line = file.ReadLine()) != null)
                {
                    Trivia questions = new Trivia(line);
                    returnList.Add(questions);
                }

                file.Close();
            }
            catch (FileNotFoundException exception)
            {
                Console.WriteLine(exception);
            }

            //Return the full list of trivia questions
            return returnList;
        }

        //add to db
        static void AddToDB()
        {
            //connection to db
            JayaEntities db = new JayaEntities();
            HighScore currentScore = new HighScore();

            //add all info to current score
            currentScore.DateCreated = DateTime.Now;
            currentScore.Game = "Trivia";
            currentScore.Name = name;
            currentScore.Score = score;

            db.HighScores.Add(currentScore);

            db.SaveChanges();
        }

        //display info from high score
        static void InfoFromDB()
        {
            Console.WriteLine("Press Enter to see high scores");
            Console.ReadKey();
            Console.Clear();

            JayaEntities db = new JayaEntities();

            List<HighScore> highScoreList = db.HighScores.Where(x => x.Game == "Trivia").OrderByDescending(x => x.Score).Take(10).ToList();

            foreach (var item in highScoreList)
            {
                Console.WriteLine("{0}, {1}{2}", highScoreList.IndexOf(item) + 1, item.Name, item.Score);
            }
        }
    }
}
