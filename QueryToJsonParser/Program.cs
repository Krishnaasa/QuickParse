using QueryToJsonParserLibrary;
using System;

namespace QueryToJsonParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n\n-------------------------------");
            Console.WriteLine("Please paste your query payload");
            string xmlQuery = Console.ReadLine();
            Console.WriteLine("Click Enter");

            CoreParserLogic parseInoutQuery = new CoreParserLogic();
            Profile completeProfile = parseInoutQuery.GetJson(xmlQuery);
            if (completeProfile != null)
            {
                BeautifyJson writeToJsonFile = new BeautifyJson(completeProfile);

                Console.WriteLine("\n\n------------------------------");
                Console.WriteLine("Prettifying JSON structure");
                string convertToFormattedJsonString = writeToJsonFile.BeautifyJsonToString();

                string filePath = writeToJsonFile.FileWriteHelper(convertToFormattedJsonString);
                Console.WriteLine("------------------------------");
                Console.WriteLine("Successfully created file at " + filePath);
                Console.WriteLine("------------------------------");
                Console.WriteLine("\nPlease Check " );
                Console.ReadKey();
            }
            else
                Console.WriteLine("Sorry couldnt parse content payload");
        }
    }
}
