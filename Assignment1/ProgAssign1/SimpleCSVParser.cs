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
using System.Formats.Asn1;
using System.Diagnostics;
using System.Security;
using Serilog.Sinks.SystemConsole.Themes;

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
            watch.Start();
            try
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.File(@"../../../logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

                Log.Information("--------------------------------------------------------Start--------------------------------------------------------");

                int skipCount = 0;
                List<Customer> outputRecords = new List<Customer>();
                string mainDirectoryPath = @"..\..\..\Sample Data\Sample Data\";

                string[] filesToParse = walk(mainDirectoryPath);

                using (var streamWriter = new StreamWriter(@"..\..\..\Output\output.csv"))
                using (var csvWriter = new CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture))
                {
                    foreach (string filePath in filesToParse)
                    {
                        if (filePath.EndsWith(".csv"))
                        {
                            string[] filePaths = filePath.Split(@"\");
                            using (var streamReader = new StreamReader(filePath))
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append(Directory.GetParent(Directory.GetParent(Directory.GetParent(filePath).FullName).FullName).Name);
                                sb.Append("/");
                                sb.Append(Directory.GetParent(Directory.GetParent(filePath).FullName).Name);
                                sb.Append("/");
                                sb.Append(Directory.GetParent(filePath).Name);
                                string date = sb.ToString();

                                try
                                {
                                    var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
                                    config.MissingFieldFound = null;
                                    var csvReader = new CsvReader(streamReader, config);
                                    csvReader.Context.RegisterClassMap<CustomerClassMap>();

                                    while (csvReader.Read())
                                    {
                                        var record = csvReader.GetRecord<Customer>();
                                        if (record.FirstName == "" || record.LastName == "" || record.Country == "" || record.Province == "" || record.City == "" || record.EmailAddress == "" || record.PhoneNumber == "" || record.PostalCode == "" || record.StreetNumber == "" || record.Street == "")
                                        {
                                            skipCount++;
                                        }
                                        else
                                        {
                                            record.Date = date;
                                            outputRecords.Add(record);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Log.Error("++++++++++++>" + e.Message);
                                }
                            }
                        }
                    }
                    csvWriter.WriteRecords(outputRecords);
                }

                Log.Information("==========Skipped records count = " + skipCount);
                Log.Information("==========Valid records count = " + outputRecords.Count);
                outputRecords.Clear();
                watch.Stop();
                Log.Information($"==========Execution Time = {watch.ElapsedMilliseconds} ms");
                Log.Information("--------------------------------------------------------END--------------------------------------------------------");
            }
            catch (UnauthorizedAccessException e)
            {
                Log.Error("\n++++++++++++"+"Unable to access system resource. Authorization unsucessful: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (ArgumentNullException e)
            {
                Log.Error("\n++++++++++++" + "Invalid arugument to function: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Log.Error("\n++++++++++++" + "Argument is out of range: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (ArgumentException e)
            {
                Log.Error("\n++++++++++++" + "Invalid arguments have been passed to a function: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (FileNotFoundException e)
            {
                Log.Error("\n++++++++++++" + "Unable to find the file specified: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (DirectoryNotFoundException e)
            {
                Log.Error("\n++++++++++++" + ""); Log.Error("\n++++++++++++" + "Unable to find the directory specified: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (PathTooLongException e)
            {
                Log.Error("\n++++++++++++" + "Resource path longer than permitted " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (IOException e)
            {
                Log.Error("\n++++++++++++" + "Unable to find the file specified: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (NotSupportedException e)
            {
                Log.Error("\n++++++++++++" + "The functionality is not supported  " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (SecurityException e)
            {
                Log.Error("\n++++++++++++" + "Security issues detected: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (Exception e)
            {
                Log.Error("\n++++++++++++" + "Exception: " + e.Message);
                Log.Information(e.StackTrace);
            }

        }

        public static string[] walk(String dirPath)
        {            
            try
            {
                string[] filesInDirectory = Directory.GetFiles(dirPath);
                string[] subDirectories = Directory.GetDirectories(dirPath);
                if (subDirectories != null)
                {
                    foreach (string subDirectory in subDirectories)
                    {
                        filesInDirectory = filesInDirectory.Concat(walk(subDirectory)).ToArray();
                    }
                }
                return filesInDirectory;
            }
            catch (UnauthorizedAccessException e)
            {
                Log.Error("\n++++++++++++" + "Unable to access system resource. Authorization unsucessful: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (ArgumentNullException e)
            {
                Log.Error("\n++++++++++++" + "Invalid arugument to function: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (ArgumentException e)
            {
                Log.Error("\n++++++++++++" + "Invalid arguments have been passed to a function: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (FileNotFoundException e)
            {
                Log.Error("\n++++++++++++" + "Unable to find the file specified: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (DirectoryNotFoundException e)
            {
                Log.Error("\n++++++++++++" + ""); Log.Error("\n++++++++++++" + "Unable to find the directory specified: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (PathTooLongException e)
            {
                Log.Error("\n++++++++++++" + "Resource path longer than permitted " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (IOException e)
            {
                Log.Error("\n++++++++++++" + "Unable to find the file specified: " + e.Message);
                Log.Information(e.StackTrace);
            }
            catch (Exception e)
            {
                Log.Error("\n++++++++++++" + "Exception: " + e.Message);
                Log.Information(e.StackTrace);
            }
            return new string[] { };
        }
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
            Map(m => m.Date).Name("Date").Optional();
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
        public string Date { get; set; }
    }
}
