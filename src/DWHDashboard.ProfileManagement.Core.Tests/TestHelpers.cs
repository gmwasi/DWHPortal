using CsvHelper;
using DWHDashboard.ProfileManagement.Core.Model;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DWHDashboard.ProfileManagement.Core.Tests
{
    public static class TestHelpers
    {
        public static string GetKeysFile()
        {
            string name = "tableau.key";
            string theFile = $@"C:\{name}";

            if (File.Exists(theFile))
                return theFile;

            string path = TestContext.CurrentContext.TestDirectory;
            var files = Directory.GetFiles(path, $"{name}", SearchOption.AllDirectories);
            return files.FirstOrDefault(x => x.Contains(name));
        }

        public static void CreateTestData<T>(DbContext context, IEnumerable<T> entities) where T : class
        {
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }

        public static Impersonator GetImpersonator()
        {
            var impersonator = Builder<Impersonator>.CreateNew()
                .With(x => x.UserName = "kenyahmis")
                .With(x => x.Password = "S3tpassw0rd")
                .Build();

            var user = Builder<User>.CreateNew().Build();

            impersonator.AddUser(user);

            return impersonator;
        }

        public static List<ViewConfig> GetConfigs()
        {
            string path = TestContext.CurrentContext.TestDirectory;
            var files = Directory.GetFiles(path, $"ViewConfig.csv", SearchOption.AllDirectories);
            var csvFile = files.First(x => x.Contains("ViewConfig"));

            List<ViewConfig> records;

            using (TextReader reader = File.OpenText(csvFile))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.Delimiter = "|";
                records = csv.GetRecords<ViewConfig>().ToList();
            }

            return records;
        }
    }
}