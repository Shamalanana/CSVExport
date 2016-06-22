/* Read a CSV file.
 * Gather Data from user
 * Extract important Col data from import.csv
 * Duplicate Col data
 * Insert User Col data
 * Insert Static Col data
 * Write to output.csv
 * 
 * Copyright (c) 2015 Benjamin Woody
 * 
 *     This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace csvexport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Open an output file for writing.
            var outfile = new StreamWriter(File.OpenWrite(@"output.csv"));
            // Gather user input information
            Console.Write("Please Enter Customer Code: ");
            string cus = Console.ReadLine();
            Console.Write("Please Enter Effective Start Date (YYYYMMDD): ");
            string start = Console.ReadLine();
            Console.Write("Please Enter EFfective End Date (YYYYMMDD): ");
            string end = Console.ReadLine();
            Console.Write("How many SCAC codes do you need? ");
            // Convert this value to an integer for use later.
            int scacCount = Convert.ToInt16(Console.ReadLine());
            // Create an array to store SCACs in.
            string[] scac = new string[scacCount];
            // Populate SCAC array.
            for (int i = 0; i < scacCount; i++)
            {
                Console.Write("Please Enter SCAC Code #{0}: ", i+1);
                scac[i] = Console.ReadLine();
            }

            // Repeat this loop for every SCAC we have.
            for (int s = 0; s < scacCount; s++)
            {
                // Read the file in each time we change SCACs.
                // This done to minimize memory usage.
                // TODO: Rewrite to handle NFO
                var reader = new StreamReader(File.OpenRead(@"import.csv"));
                while (!reader.EndOfStream)
                {
                    // Create a varaiable and load a line of data
                    var line = reader.ReadLine();
                    // split the line into values
                    var values = line.Split(',');
                    // We know we need to repeat 6 times for each.
                    // This value will change to a variable 
                    // once we put in the code to determine weight classes.
                    int counter = 6;
                    // Create a line of output for each weight.
                    for (int c = 0; c < counter; c++)
                    {
                        StringBuilder outline = new StringBuilder();
                        outline.Append(values[0]);
                        outline.Append(",");
                        outline.Append(cus);
                        outline.Append(",*,");
                        outline.Append(scac[s]);
                        outline.Append(",*,*,*,*,*,*,");
                        outline.Append(values[13]);
                        outline.Append(",");
                        outline.Append(values[14]);
                        outline.Append(",*,");
                        if (values[17].Contains("press"))
                            outline.Append("AM,");
                        else if (values[17].Contains("dard"))
                            outline.Append("PM,");
                        else
                        {
                            outline.Append(values[17]);
                            outline.Append(",");
                        }
                        outline.Append(values[22]);
                        outline.Append(",");
                        if (c == 0)
                            outline.Append("0,44,");
                        if (c == 1)
                            outline.Append("45,99,");
                        if (c == 2)
                            outline.Append("100,299,");
                        if (c == 3)
                            outline.Append("300,499,");
                        if (c == 4)
                            outline.Append("500,999,");
                        if (c == 5)
                            outline.Append("1000,9999,");
                        outline.Append(values[23 + c]);
                        outline.Append(",KG,");
                        outline.Append(start);
                        outline.Append(",");
                        outline.Append(end);
                        outline.AppendLine(",USD,KG,JWOODY,$");
                        outfile.Write(outline);
                    }
                }
            }
            // Close the file
            outfile.Close();
        }
    }
}
