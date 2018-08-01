using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using FizzWare.NBuilder;
using NUnit.Framework;

namespace DWHDashboard.ProfileManagement.Core.Infrastructure.Tests
{
    public static class TestHelpers
    {
        static Random random = new Random();

        public static void CreateTestData<T>(DwhDashboardContext context, IEnumerable<T> entities) where T : class 
        {
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }

        public static  List<TableauWorkbook> CreateWorkbooks(int count,int viewcount)
        {
           var tableauWorkbooks= Builder<TableauWorkbook>.CreateListOfSize(count).Build().ToList();

            int n = 0;

            foreach (var b in tableauWorkbooks)
            {
                n++;
                b.TableauId = $"{n}";
                b.Voided = false;
                
                if (viewcount > 0)
                {
                    var vList = Builder<TableauView>.CreateListOfSize(viewcount).Build().ToList();
                    int vn = 0;
                    foreach (var v in vList)
                    {
                        vn++;
                        v.TableauId = $"{n}-{vn}";
                        v.Voided = false;
                        if (vn == 1)
                        {
                            v.Name = $"{v.Name} dashboard";
                        }
                        else
                        {
                            v.Name = $"{v.Name} Live";
                        }
                        
                    }
                    b.AddTabViews(vList);
                }

            }
            return tableauWorkbooks;
        }

        public static List<Organization> CreateOrgs(int count)
        {
            var orgs = Builder<Organization>.CreateListOfSize(count).Build().ToList();
            return orgs;
        }

        public static List<User> CreateUsers(int count,UserType userType,Guid orgId,Guid id)
        {
            var users = Builder<User>.CreateListOfSize(count)
                .All().With(x=>x.UserType=userType)
                .With(x=>x.OrganizationId=orgId)
                .With(x => x.UserConfirmed = UserConfirmation.Confirmed)
                .With(x => x.EmailConfirmed =true)
                .With(x => x.ImpersonatorId = id)
                .Build()               
                .ToList();

            foreach (var user in users)
            {
                user.UserName = RandomString(7);
                user.Id = Guid.NewGuid().ToString();
            }
            return users;
        }

        public static List<Organization> CreateOrgs(List<TableauWorkbook> workbooks)
        {
            var orgsList=new List<Organization>();
            var org = Builder<Organization>.CreateNew().Build();
            var views = workbooks.SelectMany(x => x.TabViews);
           var viewids = views.Select(x => x.Id).ToList();
            org.UpdateViews(viewids,new List<Guid>());
            orgsList.Add(org);
            return orgsList;
        }

        private static string RandomString(int Size)
        {
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public static List<TableauWorkbook> GetTableauWorkbooks()
        {
            string path = TestContext.CurrentContext.TestDirectory;
            var files = Directory.GetFiles(path, $"TableauWorkbook.csv", SearchOption.AllDirectories);
            var csvFile = files.First(x => x.Contains("TableauWorkbook"));

            List<TableauWorkbook> records;

            using (TextReader reader = File.OpenText(csvFile))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.Delimiter = "|";
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                records = csv.GetRecords<TableauWorkbook>().ToList();
            }
            return records;
        }

        public static List<TableauView> GetTableauViews()
        {
            string path = TestContext.CurrentContext.TestDirectory;
            var files = Directory.GetFiles(path, $"TableauView.csv", SearchOption.AllDirectories);
            var csvFile = files.First(x => x.Contains("TableauView"));

            List<TableauView> records;

            using (TextReader reader = File.OpenText(csvFile))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.Delimiter = "|";
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                records = csv.GetRecords<TableauView>().ToList();
            }
            return records;
        }
        public static List<ViewConfig> GetViewConfigs()
        {
            string path = TestContext.CurrentContext.TestDirectory;
            var files = Directory.GetFiles(path, $"ViewConfig.csv", SearchOption.AllDirectories);
            var csvFile = files.First(x => x.Contains("ViewConfig"));

            List<ViewConfig> records;

            using (TextReader reader = File.OpenText(csvFile))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.Delimiter = "|";
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                records = csv.GetRecords<ViewConfig>().ToList();
            }
            return records;
        }
    }
}