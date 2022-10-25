using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using Serilog;
using System.Text;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using static System.Net.Mime.MediaTypeNames;

namespace ProgAssign1
{
    class Class1
    {
        Class1()
        {
        }

        public static void Main(String[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.File(@"../../../logs/log.txt", rollingInterval: RollingInterval.Hour)
                .CreateLogger();
            Log.Information("--------------------------------------------------------Start--------------------------------------------------------");
            
            string mainDirectoryPath = @"..\..\..\Sample Data\Sample Data\";
            int skipCount = walk(mainDirectoryPath, 0);
            Log.Information("==========TOTAL SKIP COUNT = " + skipCount);
            watch.Stop();
            Log.Information($"Execution Time: {watch.ElapsedMilliseconds} ms");
            Log.Information("--------------------------------------------------------END--------------------------------------------------------");
        }

        public static int walk(String dirPath, int skipInit)
        {
            int skipCount = skipInit;
            string[] filesInDirectory = Directory.GetFiles(dirPath);
            var good = new List<Customer>();
            var bad = new List<string>();
            string[] subDirectories = Directory.GetDirectories(dirPath);
            
            foreach (string filePath in filesInDirectory)
            {
                if(filePath.EndsWith(""))
                {
                    string[] filePaths = filePath.Split(@"\");
                    using (var streamReader = new StreamReader(filePath))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(Directory.GetParent(filePath));
                        sb.Append(Directory.GetParent(Directory.GetParent(filePath).FullName));
                        sb.Append(Directory.GetParent(Directory.GetParent(Directory.GetParent(filePath).FullName).FullName));
                        string date = sb.ToString();
                        string s = String.Empty;
                        try
                        {
                            var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
                            bool isRecordBad = false;
                            config.MissingFieldFound = context =>
                            {
                                isRecordBad = true;
                                skipCount++;
                                Log.Error("++++++++++++>Skip Count is " + skipCount);
                            };
                            var csvReader = new CsvReader(streamReader, config);
                            csvReader.Context.RegisterClassMap<CustomerClassMap>();
                            while (csvReader.Read())
                            {
                                var record = csvReader.GetRecord<Customer>();
                                if (!isRecordBad)
                                {
                                    //good.Add(record);
                                }

                                isRecordBad = false;
                            }                           
                        }
                        catch (Exception e)
                        {
                            Log.Error("++++++++++++>" + e.Message);
                        }
                    }
                }
            }
            if (subDirectories != null)
            {
                foreach (string subDirectory in subDirectories)
                {
                    skipCount += walk(subDirectory, skipCount);
                }
            }
            return skipCount;
        }
        //public static string[] walk(String dirPath)
        //{
        //    string[] subDirectories = Directory.GetDirectories(dirPath);
        //    string[] filesInDirectory = Directory.GetFiles(dirPath);

        //    string[] allFiles = {};
        //    if (subDirectories == null)
        //    {
        //        return filesInDirectory;
        //    }
        //    List<string> list = new List<string>();
        //    list.AddRange(filesInDirectory);
        //    foreach (string dirpath in subDirectories)
        //    {
        //        if (Directory.Exists(dirpath))
        //        {   
        //            list.AddRange(walk(dirpath));
        //        }
        //    }
        //    allFiles = list.ToArray();
        //    return allFiles;
        //}
    }

    public class CustomerClassMap : ClassMap<Customer>
    {
        public CustomerClassMap()
        {

            Map(m => m.FirstName).Name("First Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.StreetNumber).Name("Street Number");
            Map(m => m.Street).Name("Street");
            Map(m => m.City).Name("City");
            Map(m => m.Province).Name("Province");
            Map(m => m.PostalCode).Name("Postal Code");
            Map(m => m.Country).Name("Country");
            Map(m => m.PhoneNumber).Name("Phone Number");
            Map(m => m.EmailAddress).Name("email Address");
        }

    }
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        //public DateTime Date { get; set; }
    }
}
