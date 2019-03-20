using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace MySQL
{
    class Program
    {
        public static SQLiteConnection GetSQLiteConnection(string dbPath)
        {
            return new SQLiteConnection($"Data Source = {dbPath}; Version = 3; ");
        }

        public class Company
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public string Address { get; set; }
            public double Salary { get; set; }

            public override string ToString()
            {
                return $"{ID} {Name} {Age} {Address} {Salary}";
            }
        }

        // requires SQLITE nugget!!!!!!!!
        static void Main(string[] args)
        {

            List<Company> companies = new List<Company>();

            using (SQLiteConnection con = GetSQLiteConnection(@"e:\sqlite\sqlite-tools-win32-x86-3250300\db1.db"))
            {
                con.Open();

                // Read results into list of ResultBuffer
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT * From COMPANY ", con))
                {

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            int id = (int)reader.GetValue(0);
                            string name = (string)reader.GetValue(1);

                            int _id = (int)reader.GetValue(reader.GetOrdinal("ID"));
                            string _name = (string)reader.GetValue(reader.GetOrdinal("NAME"));
                            int _age = (int)reader.GetValue(reader.GetOrdinal("AGE"));
                            string _add = (string)reader.GetValue(reader.GetOrdinal("ADDRESS"));
                            double _salary = (double)reader.GetValue(reader.GetOrdinal("SALARY"));

                            companies.Add(new Company { ID = _id, Name = _name, Age = _age, Address = _add, Salary = _salary});

                            Console.WriteLine($"{_id} {_name}");
                            //resultBuffer.Key = (string) reader.GetValue(0);
                            //resultBuffer.Buffer = (byte[]) reader.GetValue(1);

                            //resultBuffers.Add(resultBuffer);
                        }
                    }

                }

                Console.WriteLine();
            }

            
            companies.ForEach(c => Console.WriteLine(c));

            var names = from c in companies
                        select new { NAME = c.Name, AGE = c.Age};

            foreach (var c in names)
            {
                Console.WriteLine(c);
                Console.WriteLine(c.GetType());
            }

            var cats = companies
                .Select(c => new {c.Name, c.Age})
                .Distinct()
                .OrderByDescending(c => c.Age);

            foreach (var c in cats)
            {
                Console.WriteLine(c);
                Console.WriteLine(c.GetType());
            }


            Console.WriteLine();
        }
    }
}
