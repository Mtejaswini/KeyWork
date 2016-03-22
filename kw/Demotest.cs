using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft;
using SemanticLibrary;
using System.IO;



namespace kw
{
    public partial class Demotest : Form
    {
        public Demotest()
        {
            InitializeComponent();
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
        int i = 0;
        int j = 0;
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
        String finalLines = "";
        String readFile(String fName)
        {
            finalLines = "";
            string line;
           
            string path = Directory.GetCurrentDirectory();

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(path + "\\LangFiles\\"+fName);
            while ((line = file.ReadLine()) != null)
            {
                finalLines = finalLines + line;
            }

            file.Close();

           // MessageBox.Show(finalLines);

            return finalLines;

                
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string gettys = "";
            String fileName;
            if (Lang.SelectedItem.Equals("Deutsch"))
            {

                fileName = "GermanStopwords.txt";

                String readContents = readFile(fileName);

                TestStemmer(new GermanStemmer(), richTextBox1.Text);

                gettys = richTextBox3.Text;

            }

            else if (Lang.SelectedItem.Equals("English"))
            {
                fileName = "EnglishStopwords.txt";

                String readContents = readFile(fileName);

                TestStemmer(new EnglishStemmer(), richTextBox1.Text);

                gettys = richTextBox3.Text;
            }
            else if (Lang.SelectedItem.Equals("Russian"))
            {

                fileName = "RussianStopwords.txt";

                String readContents = readFile(fileName);

                TestStemmer(new RussianStemmer(), richTextBox1.Text);

                gettys = richTextBox3.Text;

            }
           
            // string gu = File.ReadAllText(@"C:\Users\LaKissMe\Desktop\Second.txt");

          //  TranslatorContainer tc = InitializeTranslatorContainer();
         //   var sourceLanguage1 = DetectSourceLanguage(tc, gettys);
          //  var targetLanguage1 = PickRandomLanguage(tc);
           // var translationResult1 = TranslateString(tc, gettys, sourceLanguage1, targetLanguage1);

           // richTextBox1.Text = translationResult1.Text;

            KeywordAnalyzer ka = new KeywordAnalyzer();
          //  KeywordAnalyzer kager = new KeywordAnalyzer();

           // translationResult1 = gettys;
          //  var g = ka.Analyze(translationResult1.Text);
            var g = ka.Analyze(gettys, finalLines);
           // var ger = kager.Analyze(gettys, finalLines);
            //	var s = ka.Analyze(gu);

           // Console.WriteLine("first");



            int count = 0;
          //  int countger = 0;


            /*foreach (var keyger in ger.Keywords)
            {
                countger = countger + 1;

            }

            dataGridView2.Rows.Clear();
            dataGridView2.Rows.Add(countger);*/


              foreach (var key in g.Keywords)
            {
                  count = count +1;

              }
           //   MessageBox.Show(count.ToString());

              dataGridView1.Rows.Clear();
              dataGridView1.Rows.Add(count);
              

             /*foreach (var keyger in ger.Keywords)
              {


                  //   MessageBox.Show(key.Word);
                  //   MessageBox.Show(key.Rank.ToString());


                  dataGridView2.Rows[j].Cells[0].Value = keyger.Word;
                  dataGridView2.Rows[j].Cells[1].Value = keyger.Rank.ToString();

                  //Console.WriteLine("   key: {0}, rank: {1}", key.Word, key.Rank);

                  j = j + 1;



              }*/

              i = 0;
            foreach (var key in g.Keywords)
            {


                     //   MessageBox.Show(key.Word);
                     //   MessageBox.Show(key.Rank.ToString());

                
                      dataGridView1.Rows[i].Cells[0].Value = key.Word;
                      dataGridView1.Rows[i].Cells[1].Value = key.Rank.ToString();
                

                //Console.WriteLine("   key: {0}, rank: {1}", key.Word, key.Rank);

                i= i + 1;
            


            }

            //Console.WriteLine("second");*/
           /* foreach (var key in s.Keywords)
            {
                Console.WriteLine("   key: {0}, rank: {1}", key.Word, key.Rank);
            }*/

           // Console.WriteLine("first");   
            String rich = "";

            var gty = (from n in g.Keywords select n).Take(50);
            foreach (var key in gty)
            {
                rich = rich  + key.Word + "\n";
                //Console.WriteLine("   {0}", key.Word);
                // Console.WriteLine("Hitlergruß");

                //Translating here..
              

            }

            richTextBox2.Text = rich;

            //Console.WriteLine("second");
            //var gus = (from n in s.Keywords select n).Take(50);
            //foreach (var key in gus)
            //{
            //	Console.WriteLine("   {0}", key.Word);
            //}



        }

        public void TestStemmer(IStemmer stemmer, params string[] words)
        {
            // Console.WriteLine("Stemmer: " + stemmer);
            richTextBox3.Text = "";
            foreach (string word in words)
            {
              //  richTextBox3.Text = stemmer.Stem(word);

                //textBox1.Text =  richTextBox1.Text ;

                char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

                string text = word;

                string[] wordnew = text.Split(delimiterChars);



                foreach (string s in wordnew)
                {
                    richTextBox3.Text = richTextBox3.Text + " " + stemmer.Stem(s);
                }

             
               // richTextBox3.Text = stemmer.Stem(word);
            }
        }
    }
}
