using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrafixJobOrders
{
    public partial class Form1 : Form
    {
        private string fileName = @"JobOrderRecords.csv";
        DataSet dataset = new DataSet();

        public Form1()
        {
            InitializeComponent();
        }

        private void createJobOrderButton_Click(object sender, EventArgs e)
        {
            try
            {
                var csv = new StringBuilder();
                var newLine = string.Format("" +
                    "{0},{1},{2},{3},\"{4}\",\"{5}\",{6},{7},{8},{9},{10}," +
                    "{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}," +
                    "{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}," +
                    "{31},{32},{33},{34},{35},{36},{37},{38},{39},{40}," +
                    "{41},{42},{43},{44},{45},{46},{47},{48},{49},{50}," +
                    "{51},{52},{53},{54},{55},{56},{57},{58},{59},{60}," +
                    "{61},{62},{63},{64},{65},{66},{67},{68},{69},{70}," +
                    "{71},{72},{73},{74},{75}",
                    customer.Text, projectTitle.Text, colorCombination.Text, pattern.Text, date.Text, dueDate.Text, quantity.Text, fabric.Text, endorsedBy.Text, jobEndorsedBy.Text, k6Male.Text,
                    k8Male.Text, k10Male.Text, k12Male.Text, k14Male.Text, k16Male.Text, k18Male.Text, k20Male.Text, tsMale.Text, xsMale.Text, sMale.Text,
                    mMale.Text, lMale.Text, xlMale.Text, xxlMale.Text, xxxlMale.Text, xxxxlMale.Text, xxxxxlMale.Text, totalMale.Text, k6Female.Text, k8Female.Text,
                    k10Female.Text, k12Female.Text, k14Female.Text, k16Female.Text, k18Female.Text, k20Female.Text, tsFemale.Text, xsFemale.Text, sFemale.Text, mFemale.Text,
                    lFemale.Text, xlFemale.Text, xxlFemale.Text, xxxlFemale.Text, xxxxlFemale.Text, xxxxxlFemale.Text, totalFemale.Text, fabricMat.Text, quantityMat.Text, meterMat.Text,
                    kiloMat.Text, bodyColor1.Text, bodyColor2.Text, bodyColor3.Text, bodyColor4.Text, bodyColor5.Text, bodyColorAmount1.Text, bodyColorAmount2.Text, bodyColorAmount3.Text, bodyColorAmount4.Text,
                    bodyColorAmount5.Text, neckCollarCuffs1.Text, neckCollarCuffs2.Text, neckCollarCuffsAmount1.Text, neckCollarCuffsAmount2.Text, garterZipper1.Text, garterZipperAmount1.Text, thread1.Text, thread2.Text, threadAmount1.Text,
                    threadAmount2.Text, estimatedBy.Text, materialsOrderedBy.Text, note.Text, randomString(8));
                csv.AppendLine(newLine);
                File.AppendAllText(fileName, csv.ToString());
                MessageBox.Show("Job order succesfully created.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving job order. Please close the file " + fileName + " if you opened it. Full Error: \n" + ex.ToString(), "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadFromFile();
        }

        private void loadFromFile()
        {
            string delimiter = ",";
            string tablename = "export";
            StreamReader sr = new StreamReader(fileName);
            dataset.Tables.Add(tablename);
            string allData = sr.ReadToEnd();
            string[] rows = allData.Split("\r".ToCharArray());
            //add first the column names
            string[] items = rows[0].Split(delimiter.ToCharArray());
            foreach(string r in items)
            {
                dataset.Tables[tablename].Columns.Add(r);
            }
            for (int x=1; x<rows.Length; x++)
            {
                items = rows[x].Split(delimiter.ToCharArray());
                dataset.Tables[tablename].Rows.Add(items);
                Application.DoEvents();
            }
            allJobOrdersDataGrid.DataSource = dataset.Tables[0].DefaultView;
        }
        
        private string randomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            var stringChars = new char[length];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            return finalString;
        }

        private void allJobOrdersDataGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            int rowindex = allJobOrdersDataGrid.CurrentCell.RowIndex;
            string orderCode = allJobOrdersDataGrid.Rows[rowindex].Cells[75].Value.ToString();
            if (MessageBox.Show("Are you sure you want to delete job order " + orderCode + "?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                updateCSV(orderCode);
            }
            else
            {
                e.Cancel = true;
            }
        }

        public void updateCSV(string Pstring)
        {
            string[] values = File.ReadAllText(fileName).Split(new char[] { ',' });
            StringBuilder ObjStringBuilder = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == Pstring)
                    continue;
                ObjStringBuilder.Append(values[i] + ",");
            }
            ObjStringBuilder.ToString().Remove(ObjStringBuilder.Length - 1);
            File.WriteAllText(fileName, ObjStringBuilder.ToString());
        }
    }
}
