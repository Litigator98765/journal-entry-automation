using System;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Linq;

namespace Journal_Entry_Automation
{

    struct Header
    {
        public int seqNum;
        public const string unit = "STATE";
        public string journalID;
        public string journalDate;
        public string desc;
        public string ledgerGroup;
        public string ledger;
        public const string source = "SPJ";
        public const string user = "CURRENT_USER";
        public DateTime date;
        public const string reversal = "N";
        public const string autoGenLines = "N";
        public const string adjustEntry = "N";
    };

    struct Line
    {
        int journalLine;
        public string unit;
        public string ledger;
        public string account;
        public string fund;
        public string ALI;
        public string dept;
        public string prog;
        public string grtPrj;
        public string proj;
        public string servLoc;
        public string reporting;
        public string agency;
        public string ISTVXRef;
        public string budRef;
        public string amount;
        public string desc;
    };

    struct Entry
    {
        public Header Head;
        public Line Line;
    };


    class journalEntries
    {


        const string outFileSting = "C:\\Users\\10213984\\OneDrive - State of Ohio\\Documents\\TEST.txt";
        [STAThread]
        static void Main(string[] args)
        {

            Dictionary<string, string> ledgerGroups = new Dictionary<string, string>();
            buildDictionary(ledgerGroups);
            try
            {
                StreamWriter sw = new StreamWriter("C:\\Users\\10213984\\OneDrive - State of Ohio\\Documents\\CCOAKS_TEST_OUT.txt");
                StreamReader sr = new StreamReader("C:\\Users\\10213984\\OneDrive - State of Ohio\\Documents\\CCOAKS_TEST.txt");

                //Discards first Journal Entires Line
                sr.ReadLine();
                using (sr)
                {

                    Entry Entry;

                    //Read first line and trim all information except for Journal ID
                    string journalID = sr.ReadLine();
                    journalID = journalID[12..];
                    Entry.Head.journalID = journalID;

                    //Read second line and trim all information except for Journal Date/Desc
                    string journalDateAndDesc = sr.ReadLine();
                    string journalDate = journalDateAndDesc.Substring(14, 10);
                    string journalDesc = journalDateAndDesc[26..];
                    Entry.Head.journalDate = journalDate;
                    Entry.Head.desc = journalDesc;

                    //Discard header titles
                    sr.ReadLine();

                    string line = sr.ReadLine();
                    string[] lineVals = line.Split();

                    //Get all line values
                    //TODO: there has to be a better way to do 
                    Entry.Line.unit = lineVals[0];
                    Entry.Head.ledger = lineVals[1];
                    Entry.Head.ledgerGroup = ledgerGroups[lineVals[1]];
                    Entry.Line.ledger = lineVals[1];
                    Entry.Line.account = lineVals[2];
                    Entry.Line.fund = lineVals[3];
                    Entry.Line.ALI = lineVals[4];
                    Entry.Line.dept = lineVals[5];
                    Entry.Line.prog = lineVals[6];
                    Entry.Line.grtPrj = lineVals[7];
                    Entry.Line.proj = lineVals[8];
                    Entry.Line.servLoc = lineVals[9];
                    Entry.Line.reporting = lineVals[10];
                    Entry.Line.agency = lineVals[11];
                    Entry.Line.ISTVXRef = lineVals[12];
                    Entry.Line.budRef = lineVals[13];
                    //Round amount to two decimal places
                    float amountFloat = int.Parse(lineVals[14]);
                    Math.Round(amountFloat, 2);
                    Entry.Line.amount = amountFloat.ToString();
                    Entry.Line.desc = lineVals[15];

                    //Entry.Head = Head;
                    //Entry.Line = LineContent;

                    //printFile(sw, Entry);
                }

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

        static void buildDictionary(Dictionary<string, string> dictionary)
        {
            dictionary.Add("CC_APR_ENC", "CC_APPROP");
            dictionary.Add("CC_ALT_ENC", "CC_ALLOT");
            dictionary.Add("CC_PLN_ENC", "CC_PLAN");
            dictionary.Add("CC_DET_ENC", "CC_DETAIL");
            dictionary.Add("CC_ACT_ENC", "CC_AGY_CTL");
            dictionary.Add("CC_CSH_EXP", "CC_CASH");
            dictionary.Add("CC_GR1_ENC", "CC_GRNT1");
        }

        static void printFile(StreamWriter sw, Entry Entry)
        {
            sw.WriteLine("<JRNL_HDR_IMP>");
            sw.WriteLine("  <SEQNO>NEED THIS NUM</SEQNO>");
            sw.WriteLine("  " + Entry.Head.journalID);
            sw.WriteLine("  " + Entry.Head.journalDate);
        }


    }
}
