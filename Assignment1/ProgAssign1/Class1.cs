using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgAssign1
{
    class Class1
    {
        int i;
        Class1() {
            i = 1;
        }

        public static void Main(String[] args)
        {
            //DirWalker dw = new DirWalker();
            //dw.walk("C:/Users/Vishnu/Workspace/MCDA/5510/A00465519_MCDA5510/Assignment1/ProgAssign1/Sample Data/Sample Data/");

            string[] allFiles = walk("C:\\Users\\Vishnu\\Workspace\\MCDA\\5510\\A00465519_MCDA5510\\Assignment1\\ProgAssign1\\Sample Data\\Sample Data\\");
            int i = 0;
            foreach (string filepath in allFiles)
            {
                Console.WriteLine("File:" + filepath);
                i++;
            }
            Console.WriteLine("count of files:" + allFiles.Length);
        }
        public static string[] walk(String dirPath)
        {
            string[] subDirectories = Directory.GetDirectories(dirPath);
            string[] filesInDirectory = Directory.GetFiles(dirPath);
            string[] allFiles = {};
            if (subDirectories == null)
            {
                return filesInDirectory;
            }
            List<string> list = new List<string>();
            list.AddRange(filesInDirectory);
            foreach (string dirpath in subDirectories)
            {
                if (Directory.Exists(dirpath))
                {   
                    list.AddRange(walk(dirpath));
                }
            }
            //foreach (string filepath in filesInDirectory)
            //{
            //    Console.WriteLine("File:" + filepath);
            //}
            allFiles = list.ToArray();
            return allFiles;
        }
    }
}
