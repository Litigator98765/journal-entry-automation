using System.Globalization;

/*
 * Upload with "Journal Entries" as first line, followed by journal entries with one line in between each.
 * 
 * Supports single line journal entries, multiple  line journal entries should work but could have bugs.
 * 
 */

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
        public string date;
        public const string reversal = "N";
        public const string autoGenLines = "N";
        public const string adjustEntry = "N";
    };

    struct Line
    {
        public int journalLine;
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


    class JournalEntries
    {

        static void Main(string[] args)
        {
            int numEntries = 0;
            try
            {
                StreamWriter sw = new StreamWriter("C:\\Users\\10213984\\OneDrive - State of Ohio\\Documents\\Journals\\Output.txt");
                StreamReader sr = new StreamReader("C:\\Users\\10213984\\OneDrive - State of Ohio\\Documents\\Journals\\Input.txt");

                Entry Entry = new Entry();


                while (sr.Peek() != -1)
                {
                    processInformation(sr, sw, ref Entry);
                    numEntries++;
                }

                //Close the file
                sw.Close();
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine(numEntries + " entries succesfully completed.");
            }
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
            dictionary.Add("CC_ATR_ENC", "CC_AGY_TRK");

            dictionary.Add("CC_APR_EXP", "CC_APPROP");
            dictionary.Add("CC_ALT_EXP", "CC_ALLOT");
            dictionary.Add("CC_ATR_EXP", "CC_AGY_TRK");
            dictionary.Add("CC_PLN_EXP", "CC_PLAN");
            dictionary.Add("CC_DET_EXP", "CC_DETAIL");

            dictionary.Add("CC_DET_EMC", "CC_DETAIL");

        }

        static void processInformation(StreamReader sr, StreamWriter sw, ref Entry Entry)
        {
            parseHeader(sr, ref Entry);

            //Consume line headers
            sr.ReadLine();
            //Parse first line so header can be printed
            parseLine(sr, ref Entry);

            //Print header and first line
            printHeader(sw, Entry);
            printLine(sw, Entry);

            //parse and print any following lines
            int lineNum = 1;
            while (sr.Peek() == 'S')
            {
                lineNum++;
                parseLine(sr, ref Entry);
                Entry.Line.journalLine = lineNum;
                printLine(sw, Entry);
            }
            sw.WriteLine("</JRNL_HDR_IMP>");

        }

        static void printHeader(StreamWriter sw, Entry Entry)
        {
            sw.WriteLine("<JRNL_HDR_IMP>");
            sw.WriteLine("  <SEQNO>1715</SEQNO>");
            sw.WriteLine("  <BUSINESS_UNIT>STATE</BUSINESS_UNIT>");
            sw.WriteLine("  <JOURNAL_ID>" + Entry.Head.journalID + "</JOURNAL_ID>");
            sw.WriteLine("  <JOURNAL_DATE>" + Entry.Head.journalDate + "</JOURNAL_DATE>");
            sw.WriteLine("  <DESCR254>" + Entry.Head.desc + "</DESCR254>");
            sw.WriteLine("  <LEDGER_GROUP>" + Entry.Head.ledgerGroup + "</LEDGER_GROUP>");
            sw.WriteLine("  <LEDGER>" + Entry.Head.ledger + "</LEDGER>");
            sw.WriteLine("  <SOURCE>SPJ</SOURCE>");
            sw.WriteLine("  <OPRID>CURRENT_USER</OPRID>");
            Entry.Head.date = DateTime.Now.ToString("yyyyMMdd");
            sw.WriteLine("  <CUR_EFFDT>" + Entry.Head.date + "</CUR_EFFDT>");
            sw.WriteLine("  <REVERSAL_CD>N</REVERSAL_CD>");
            sw.WriteLine("  <AUTO_GEN_LINES>N</AUTO_GEN_LINES>");
            sw.WriteLine("  <ADJUSTING_ENTRY>N</ADJUSTING_ENTRY>");
        }

        static void printLine(StreamWriter sw, Entry Entry)
        {
            //REQUIRED LINE FIELDS
            sw.WriteLine("  <JRNL_LN_IMP>");
            sw.WriteLine("    <JOURNAL_LINE>" + Entry.Line.journalLine + "</JOURNAL_LINE>");
            sw.WriteLine("    <BUSINESS_UNIT>STATE</BUSINESS_UNIT>");
            sw.WriteLine("    <LEDGER>" + Entry.Line.ledger + "</LEDGER>");
            sw.WriteLine("    <ACCOUNT>" + Entry.Line.account + "</ACCOUNT>");
            sw.WriteLine("    <FUND_CODE>" + Entry.Line.fund + "</FUND_CODE>");
            sw.WriteLine("    <PRODUCT>" + Entry.Line.ALI + "</PRODUCT>");
            sw.WriteLine("    <DEPTID>" + Entry.Line.dept + "</DEPTID>");

            if (Entry.Line.prog != "") 
            { 
                sw.WriteLine("    <PROGRAM_CODE>" + Entry.Line.prog + "</PROGRAM_CODE>"); 
            }
            

            //OPTIONAL FIELDS
            if (Entry.Line.grtPrj != "")
            {
                sw.WriteLine("    <PROJECT_ID>" + Entry.Line.grtPrj + "</PROJECT_ID>");
            }

            if (Entry.Line.proj != "")
            {
                sw.WriteLine("    <CHARTFIELD1>" + Entry.Line.proj + "</CHARTFIELD1>");
            }

            if (Entry.Line.servLoc != "")
            {
                sw.WriteLine("    <CLASS_FLD>" + Entry.Line.servLoc + "</CLASS_FLD>");
            }

            if (Entry.Line.reporting != "")
            {
                sw.WriteLine("    <CHARTFIELD2>" + Entry.Line.reporting + "</CHARTFIELD2>");
            }

            if (Entry.Line.agency != "")
            {
                sw.WriteLine("    <CHARTFIELD3>" + Entry.Line.agency + "</CHARTFIELD3>");
            }

            if (Entry.Line.ISTVXRef != "")
            {
                sw.WriteLine("    <OPERATING_UNIT>" + Entry.Line.ISTVXRef + "</OPERATING_UNIT>");
            }

            if (Entry.Line.budRef != "")
            {
                sw.WriteLine("    <BUDGET_REF>" + Entry.Line.budRef + "</BUDGET_REF>");
            }

            sw.WriteLine("    <FOREIGN_AMOUNT>" + Entry.Line.amount + "</FOREIGN_AMOUNT>");
            sw.WriteLine("    <LINE_DESCR>" + Entry.Line.desc + "</LINE_DESCR>");
            sw.WriteLine("  </JRNL_LN_IMP>");
        }

        static void parseHeader(StreamReader sr, ref Entry Entry)
        {
            Dictionary<string, string> ledgerGroups = new Dictionary<string, string>();
            buildDictionary(ledgerGroups);

            //DISCARDS FIRST JOURNAL ENTRY LINE
            //CAN CAUSE PROBLEMS, TXT FILE MUST BEGIN WITH JOURNAL ENTRIES: AND HAVE ONE LINE BETWEEN ENTRIES
            sr.ReadLine();

            //Read first line and trim all information except for Journal ID
            string journalID = sr.ReadLine();
            journalID = journalID[11..];
            journalID = journalID.Trim();
            Entry.Head.journalID = journalID;

            //Read second line and trim all information except for Journal Date/Desc
            string journalDateAndDesc = sr.ReadLine();
            string[] dateAndDescArray = journalDateAndDesc.Split();
            string journalDate = dateAndDescArray[2];
            journalDate = DateTime.ParseExact(journalDate.Trim(), "M/d/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            Entry.Head.journalDate = journalDate;
            string journalDesc = journalDateAndDesc[23..];
            journalDesc = journalDesc.Trim();
            //URL Encoding to conform to excel
            journalDesc = Uri.EscapeDataString(journalDesc);
            Entry.Head.desc = journalDesc;

            Console.WriteLine(journalDesc);
        }

        static void parseLine(StreamReader sr, ref Entry Entry)
        {
            Dictionary<string, string> ledgerGroups = new Dictionary<string, string>();
            buildDictionary(ledgerGroups);

            string line = sr.ReadLine();
            string[] lineVals = line.Split();


            //Get all line values
            //TODO: there has to be a better way to do 
            Entry.Line.journalLine = 1;
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


            double amountFloat = float.Parse(lineVals[14]);
            amountFloat = Math.Round(amountFloat, 2);
            Entry.Line.amount = amountFloat.ToString();
            
            Entry.Line.desc = Uri.EscapeDataString(lineVals[15]);
        }

    }

}
