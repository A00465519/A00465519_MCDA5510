using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace ProgAssign1
{
    public class SimpleCSVParser
    {


        //public static void Main(String[] args)
        //{
        //    SimpleCSVParser parser = new SimpleCSVParser();
        //    parser.parse(@"/Users/Vishnu/Workspace/MCDA/5510/A00465519_MCDA5510/Assignment1/ProgAssign1/sampleFile.csv");
        //}
        //C:\Users\Vishnu\Workspace\MCDA\5510\A00465519_MCDA5510\Assignment1\ProgAssign1\Sample Data\Sample Data

        public void parse(String fileName)
        {
            try { 
            using (TextFieldParser parser = new TextFieldParser(fileName))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        //Process row
                        string[] fields = parser.ReadFields();
                        foreach (string field in fields)
                        {
                            Console.WriteLine(field);
                        }
                    }
            }
        
        } catch(IOException ioe){
                Console.WriteLine(ioe.StackTrace);
         }

    }


    }
}
