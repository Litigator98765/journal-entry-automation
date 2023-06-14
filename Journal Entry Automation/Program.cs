//Write a text file - Version-1
using System;
using System.IO;
namespace readwriteapp
{
    class Class1
    {
        String outFileSting = "C:\\Users\\10213984\\OneDrive - State of Ohio\\Documents\\TEST.txt";
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                //Console.Write("Enter the journal file name to be read");
                //String fileName = Console.ReadLine();
                //Console.Write("Enter name of the file to be generated");
                StreamWriter sw = new StreamWriter("C:\\Users\\10213984\\OneDrive - State of Ohio\\Documents\\TEST.txt");
                //Write a line of text
                sw.WriteLine("Hello World!!");
                //Write a second line of text
                sw.WriteLine("From the StreamWriter class");
                //Close the file
                sw.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }


        static void printFile(StreamWriter sw, StreamReader sr)
        {
            
        }


    }
}
