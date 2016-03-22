using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;
using System.Net;
using Microsoft;
using System.Text;
using SemanticLibrary;
using System.IO;
using System.Windows.Forms;



namespace kw
{
	class Program
	{

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Demotest()); 
            
            
        
        }

		static void Main1(string[] args)
		{
			//Note: you will have to supply your own text files
            string gettys = File.ReadAllText(@"C:\Users\LaKissMe\Desktop\First.docx");
           // string gu = File.ReadAllText(@"C:\Users\LaKissMe\Desktop\Second.txt");

			KeywordAnalyzer ka = new KeywordAnalyzer();

			var g = ka.Analyze(gettys, "hello");

		//	var s = ka.Analyze(gu);

			Console.WriteLine("first");
			foreach (var key in g.Keywords)
			{
				Console.WriteLine( key.Word, key.Rank);

              //  Console.WriteLine( key.Word, key.Rank);
			}

			//Console.WriteLine("second");
			/*foreach (var key in s.Keywords)
			{
				Console.WriteLine("   key: {0}, rank: {1}", key.Word, key.Rank);
			}*/

			Console.WriteLine("first");
			var gty = (from n in g.Keywords select n).Take(50);
			foreach (var key in gty)
			{


                Console.WriteLine("   {0}", key.Word);
               // Console.WriteLine("Hitlergruß");

                //Translating here..
               /* TranslatorContainer tc = InitializeTranslatorContainer();
                var sourceLanguage1 = DetectSourceLanguage(tc, key.Word);
                var targetLanguage1 = PickRandomLanguage(tc);
                var translationResult1 = TranslateString(tc, key.Word, sourceLanguage1, targetLanguage1);
                Console.WriteLine(" Translated to English : " + translationResult1.Text);
                */

			}

			//Console.WriteLine("second");
			//var gus = (from n in s.Keywords select n).Take(50);
			//foreach (var key in gus)
			//{
			//	Console.WriteLine("   {0}", key.Word);
			//}
			Console.ReadLine();
		}

        private static DetectedLanguage DetectSourceLanguage(TranslatorContainer tc, string inputString)
        {
            // calling Detect gives us a DataServiceQuery which we can use to call the service
            var translateQuery = tc.Detect(inputString);

            // since this is a console application, we do not want to do an asynchronous 
            // call to the service. Otherwise, the program thread would likely terminate
            // before the result came back, causing our app to appear broken.
            var detectedLanguages = translateQuery.Execute().ToList();

            // since the result of the query is a list, there might be multiple
            // detected languages. In practice, however, I have only seen one.
            // Some input strings, 'hi' for example, are obviously valid in 
            // English but produce other results, suggesting that the service
            // only returns the first result.
            if (detectedLanguages.Count() > 1)
            {
                Console.WriteLine("Possible source languages:");

                foreach (var language in detectedLanguages)
                {
                    Console.WriteLine("\t" + language.Code);
                }

                Console.WriteLine();
            }

            // only continue if the Microsoft Translator identified the source language
            // if there are multiple, let's go with the first.
            if (detectedLanguages.Count() > 0)
            {
                return detectedLanguages.First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Asks the service represented by the TranslatorContainer for a list
        /// of all supported languages and then picks one at random.
        /// </summary>
        /// <param name="tc">The TranslatorContainer to use.</param>
        /// <returns>A randomly selected Language.</returns>
        private static Language PickRandomLanguage(TranslatorContainer tc)
        {
            // Used to generate a random index
            var random = new Random();

            // Generate the query
            var languagesForTranslationQuery = tc.GetLanguagesForTranslation();

            // Call the query to get the results as an Array
            var availableLanguages = languagesForTranslationQuery.Execute().ToArray();

            // Generate a random index between 0 and the total number of items in the array
            var randomIndex = random.Next(availableLanguages.Count());

            // Select the randomIndex'th value from the array
            var selectedLanguage = availableLanguages[10];
            //  selectedLanguage = "en";

            return selectedLanguage;
        }

        /// <summary>
        /// Uses the TranslatorContainer to translate inputString from sourceLanguage
        /// to targetLanguage.
        /// </summary>
        /// <param name="tc">The TranslatorContainer to use</param>
        /// <param name="inputString">The string to translate</param>
        /// <param name="sourceLanguage">The Language Code to use in interpreting the input string.</param>
        /// <param name="targetLanguage">The Language Code to translate the input string to.</param>
        /// <returns>The translated string, or null if no translation results were found.</returns>
        private static Translation TranslateString(TranslatorContainer tc, string inputString, DetectedLanguage sourceLanguage, Language targetLanguage)
        {
            // Generate the query
            var translationQuery = tc.Translate(inputString, targetLanguage.Code, sourceLanguage.Code);

            // Call the query and get the results as a List
            var translationResults = translationQuery.Execute().ToList();

            // Verify there was a result
            if (translationResults.Count() <= 0)
            {
                return null;
            }

            // In case there were multiple results, pick the first one
            var translationResult = translationResults.First();

            return translationResult;
        }

        /// <summary>
        /// Concatenates the input arguments into a single string
        /// </summary>
        /// <param name="args">The args array passed into Main</param>
        /// <returns>the concatonated result string</returns>
        private static TranslatorContainer InitializeTranslatorContainer()
        {
            // this is the service root uri for the Microsoft Translator service 
            var serviceRootUri = new Uri("https://api.datamarket.azure.com/Bing/MicrosoftTranslator/");

            // this is the Account Key I generated for this app
            var accountKey = "XM5qyTSXAh7JxjRC0rxr5fAT9pkMoSRJUQXI589aj/g";

            // Replace the account key above with your own and then delete 
            // the following line of code. You can get your own account key
            // for from here: https://datamarket.azure.com/account/keys
            //throw new Exception("Invalid Account Key");

            // the TranslatorContainer gives us access to the Microsoft Translator services
            var tc = new TranslatorContainer(serviceRootUri);

            // Give the TranslatorContainer access to your subscription
            tc.Credentials = new NetworkCredential(accountKey, accountKey);
            return tc;
        }
	}
}
