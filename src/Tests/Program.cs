using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Tests // Change to whatever namespace you have
{

    class Program
    {
        
        static void Main(string[] args)
        {
            
            string[] data = { "debitcard", "elvis", "silent", "badcredit", "lives", "freedom", "listen", "levis", "money","dog","god","cat","act"};
            Hashtable results = ComputeAnagrams(data);
            DisplayResults(results, data);
        }


        /// <summary>
        /// Displays the results of the Anagram algorithm to the console window
        /// </summary>
        /// <param name="processedData"> </param>
        /// <param name="inputData"></param>
         static void DisplayResults(Hashtable processedData, string [] inputData)
        {
            Console.WriteLine("-----------Input---------");
            Console.WriteLine(Environment.NewLine);

            for (int i = 0; i < inputData.Length; i++)            
                Console.WriteLine(inputData[i]);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("-----------Anagram groups after computation-----------");
            Console.WriteLine(Environment.NewLine);

            foreach (DictionaryEntry item in processedData)           
                Console.WriteLine(item.Value);
   
            Console.ReadLine();
        }

        
        /// <summary>
        /// Calculates Anagram pairs from a set of strings
        /// </summary>
        /// <param name="data"> string Array</param>
        /// <returns>A Hashtable of Anagram pairs</returns>
        static Hashtable ComputeAnagrams(string[] data)
        {
            int dataLength = data.Length, keyC=0;
            List<string> possibleAnagrams;


            Hashtable hashElements = new Hashtable();

            // For each item check all other items to see if it is an Anagram
            for (int i = 0; i < data.Length; i++)
            {
                // Crete list to store possible Anagrams
                possibleAnagrams = new List<string>(); 

                for (int j = 0; j < data.Length; j++)
                {                 
                    // Don't check itself
                    if (i != j) 
                    {
                        // Only add possible Anagrams when  following conditions are met
                        if (data[i].Length == data[j].Length && 
                            !hashElements.ContainsValue(data[i]) && !hashElements.ContainsValue(data[j]))
                            possibleAnagrams.Add(data[j]);                     
                    }
                }

                // it did not find  possible Anagrams skip to next element in data
                if (possibleAnagrams.Count == 0) continue;

                // if Found  possible Anagrams proceed with computation
                else
                {
                    string currentItem = string.Concat(data[i].OrderBy(c => c));
                    string possiblePair;                 
                    int length = currentItem.Length;
                   
                    for (int k = 0; k < possibleAnagrams.Count; k++)
                    {
                       possiblePair = string.Concat(possibleAnagrams[k].OrderBy(c => c));
                   
                        // Only consider Anagram pair of both strings have the same chars                    
                        if(currentItem == possiblePair)
                        {
                            // Only add Anagram pair if they currently 
                            // don't exists the  collection

                            if (!hashElements.ContainsValue(data[i]))
                            {
                                if (i == 0) keyC = 0;
                                else keyC++;
                                hashElements.Add(keyC, data[i]);
                            }

                            if (!hashElements.ContainsValue(possibleAnagrams[k]))
                            {
                                keyC++;
                                hashElements.Add(keyC, possibleAnagrams[k]);
                            }
                        }
                    }
                }
            }


            return hashElements; 
        }
    

    }
}
