using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ZodiacService.DataAccess
{
    public class ZodiacOperations
    {
        private const string FilePath = "./Resources/signs.txt";

        public static List<Tuple<Zodiac, Zodiac, string>> GetAllZodiacs()
        {
            
            var zodiacs = new List<Tuple<Zodiac, Zodiac, string>>();

            try
            {
                var sr = new StreamReader(FilePath);
                var line = sr.ReadLine()?.Split(" ");
                while (line != null)
                {
                    zodiacs.Add(new Tuple<Zodiac, Zodiac, string>(
                        new Zodiac() { Date = line[0] },
                        new Zodiac() { Date = line[1] },
                        line[2]));

                    line = sr.ReadLine()?.Split(" ");
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }

            return zodiacs;
        }
    }
}
