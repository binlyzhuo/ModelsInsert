using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace ModelsInsert
{
    class Program
    {
        static void Main(string[] args)
        {
            var items = new List<Person>();

            items.Add(new Person() { Name = "zz1", Sex = "F1" , ID = Guid.Parse("318eb6c0-a91a-48fd-b6ed-bb66fd2a7efc")});
            items.Add(new Person() { Name = "zz2", Sex = "F2", ID = Guid.Parse("0b136c6a-51d2-4ce9-b817-7f006b04a70a")});
            items.Add(new Person() { Name = "zz3", Sex = "F3",ID = Guid.Parse("bc9fcf37-fe04-4d0d-b565-ed978c416850")});
            items.Add(new Person() { Name = "zz4", Sex = "F4",ID = Guid.Parse("90932dca-c4c2-4083-8225-1e4f37d86762")});
            items.Add(new Person() { Name = "zz5", Sex = "F5",ID = Guid.Parse("f6f4244c-3a3e-4dc0-84d6-e814bab052d0")});
            var markers = GetMarkers(items);
            foreach (var m in markers)
            {
                Console.WriteLine($"user:{m.ID}-marker:{m.MarkerGuid}");
            }

            //long spend = GetTimeCount();
            //long spend2 = BulkInsert();

            //Console.WriteLine($"dapper:{spend}");
            //Console.WriteLine($"bulkinsert:{spend2}");

            //var nums = GetMarker();
            //foreach (var num in nums)
            //{
            //    Console.WriteLine(num);
            //}
            Console.ReadLine();
        }

        static long GetTimeCount()
        {
            List<Person> items = new List<Person>();

            int count = 5000;

            for (int i = 0; i < count; i++)
            {
                items.Add(new Person()
                {
                    Sex = "F",
                    Name = $"User{i}"
                });
            }
            string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["TESTDB"].ConnectionString;
            var conn = new SqlConnection(connstring);
            string insertSql = "insert into Person(Name,Sex)values(@name,@sex)";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int result = conn.Execute(insertSql, items);
            //Thread.Sleep(3000);
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        static long BulkInsert()
        {
            List<Person> items = new List<Person>();

            int count = 5;

            for (int i = 0; i < count; i++)
            {
                items.Add(new Person()
                {
                    Sex = "F",
                    Name = $"User{i}",
                    Address = "XG"
                });
            }

            SqlServerDbHelper dbHelper = new SqlServerDbHelper();
            string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["TESTDB"].ConnectionString;
            bool result = false;
            Stopwatch sw = new Stopwatch();

            Type t = typeof(Person);
            var fields2 = t.GetProperties().Select(m=>m.Name).ToList();

            List<string> fields = new List<string>();
            fields.Insert(0,"ID");
            fields.Insert(1,"Name");
            fields.Insert(2,"Sex");
            fields.Insert(3,"Address");
            sw.Start();
            dbHelper.Insert(connstring, items,fields2, "Person", out result);
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        static List<string> GetMarker()
        {
            List<int> numbers = new List<int>();
            numbers.Add(1);
            numbers.Add(2);
            numbers.Add(3);


            //numbers.Add(4);
            //numbers.Add(6);
            //numbers.Add(9);

            List<int> numbers2 = numbers.Select(f => f).ToList();
            //numbers.Reverse();
            //Collections.shuffle(strList);
            List<string> ms = new List<string>();

            for (int i = 0; i < numbers2.Count; i++)
            {
                if (i == numbers.Count - 1)
                {
                    ms.Add($"{numbers2[i]}-{numbers[0]}");
                }
                else
                {
                    ms.Add($"{numbers2[i]}-{numbers[i + 1]}");
                }
            }

            return ms;
        }

        static List<Person> GetMarkers(List<Person> users)
        {
            var users2 = users.Select(m => m).ToList();
            for (int i = 0; i < users.Count; i++)
            {
                if (i == users2.Count - 1)
                {
                    //ms.Add($"{numbers2[i]}-{numbers[0]}");
                    users[i].MarkerGuid = users2[0].ID;
                }
                else
                {
                    //ms.Add($"{numbers2[i]}-{numbers[i + 1]}");
                    users[i].MarkerGuid = users2[i+1].ID;
                }
            }
            return users;
        }
    }
}
