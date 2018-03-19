﻿using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrafixJobOrders
{
    public partial class Form1 : Form
    {
        private string fileName = "Data/JobOrderRecords.csv";
        private string fileNameBackup = "Data/JobOrderRecordsBackup.csv";
        private string orderCodeSelected = "";
        private int[] columnsToShow = new int[] { 0, 1, 4, 5, 8, 9, 87, 88, 89, 90, 91, 92, 93 };
        DataSet dataset = new DataSet();

        public Form1()
        {
            InitializeComponent();
        }

        private void createJobOrderButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (orderCodeSelected.Length == 0)
                {
                    insertToCSV();
                }
                else
                {
                    editToCSV();
                }
                allTabs.SelectedTab = allOrdersTab;
                ClearTextBoxes(orderDetailsTab);
                ClearTextBoxes(projectReportTab);
                enableDisableisableTextfieldsInOrderDetails(false, orderDetailsTab);
                enableDisableisableTextfieldsInOrderDetails(false, projectReportTab);
                loadFromFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving job order. Please close the file " + fileName + " if you opened it. \n\nFull Error: \n" + ex.ToString(), "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void insertToCSV()
        {
            var csv = new StringBuilder();
            var newLine = string.Format("" +
               "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}," +
               "{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}," +
               "{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}," +
               "{31},{32},{33},{34},{35},{36},{37},{38},{39},{40}," +
               "{41},{42},{43},{44},{45},{46},{47},{48},{49},{50}," +
               "{51},{52},{53},{54},{55},{56},{57},{58},{59},{60}," +
               "{61},{62},{63},{64},{65},{66},{67},{68},{69},{70}," +
               "{71},{72},{73},{74},{75},{76},{77},{78},{79},{80}," +
               "{81},{82},{83},{84},{85},{86},{87},{88},{89},{90}," +
               "{91},{92},{93}",
               customer.Text, projectTitle.Text, colorCombination.Text, pattern.Text, date.Text, dueDate.Text, quantity.Text, fabric.Text, endorsedBy.Text, jobEndorsedBy.Text, k6Male.Text,
               k8Male.Text, k10Male.Text, k12Male.Text, k14Male.Text, k16Male.Text, k18Male.Text, k20Male.Text, tsMale.Text, xsMale.Text, sMale.Text,
               mMale.Text, lMale.Text, xlMale.Text, xxlMale.Text, xxxlMale.Text, xxxxlMale.Text, xxxxxlMale.Text, totalMale.Text, k6Female.Text, k8Female.Text,
               k10Female.Text, k12Female.Text, k14Female.Text, k16Female.Text, k18Female.Text, k20Female.Text, tsFemale.Text, xsFemale.Text, sFemale.Text, mFemale.Text,
               lFemale.Text, xlFemale.Text, xxlFemale.Text, xxxlFemale.Text, xxxxlFemale.Text, xxxxxlFemale.Text, totalFemale.Text, fabricMat.Text, quantityMat.Text, meterMat.Text,
               kiloMat.Text, bodyColor1.Text, bodyColor2.Text, bodyColor3.Text, bodyColor4.Text, bodyColor5.Text, bodyColorAmount1.Text, bodyColorAmount2.Text, bodyColorAmount3.Text, bodyColorAmount4.Text,
               bodyColorAmount5.Text, neckCollarCuffs1.Text, neckCollarCuffs2.Text, neckCollarCuffsAmount1.Text, neckCollarCuffsAmount2.Text, garterZipper1.Text, garterZipperAmount1.Text, thread1.Text, thread2.Text, threadAmount1.Text,
               threadAmount2.Text, estimatedBy.Text, materialsOrderedBy.Text, note.Text, amountPerPiece.Text, numberOfPieces.Text, dateReceived.Text, dateReleased.Text, dateProjectReport.Text, fabricMaterialsExp.Text,
               paintExp.Text, cutSewLaborExp.Text, transportationExp.Text, marketingFeeExp.Text, otherExpensesExp1.Text, otherExpensesExp2.Text, preparedBy.Text, checkedBy.Text, totalAmount.Text, expenses.Text,
               netIncome.Text, receivable.Text, randomString(8));
            csv.AppendLine(newLine);
            File.AppendAllText(fileName, csv.ToString());
            File.AppendAllText(fileNameBackup, csv.ToString());
            MessageBox.Show("Job order succesfully created.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void editToCSV()
        {
            deleteAtRowWithCode(orderCodeSelected); //delete then reinsert this record
            var csv = new StringBuilder();
            var newLine = string.Format("" +
                "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}," +
                "{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}," +
                "{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}," +
                "{31},{32},{33},{34},{35},{36},{37},{38},{39},{40}," +
                "{41},{42},{43},{44},{45},{46},{47},{48},{49},{50}," +
                "{51},{52},{53},{54},{55},{56},{57},{58},{59},{60}," +
                "{61},{62},{63},{64},{65},{66},{67},{68},{69},{70}," +
                "{71},{72},{73},{74},{75},{76},{77},{78},{79},{80}," +
                "{81},{82},{83},{84},{85},{86},{87},{88},{89},{90}," +
                "{91},{92},{93}",
                customer.Text, projectTitle.Text, colorCombination.Text, pattern.Text, date.Text, dueDate.Text, quantity.Text, fabric.Text, endorsedBy.Text, jobEndorsedBy.Text, k6Male.Text,
                k8Male.Text, k10Male.Text, k12Male.Text, k14Male.Text, k16Male.Text, k18Male.Text, k20Male.Text, tsMale.Text, xsMale.Text, sMale.Text,
                mMale.Text, lMale.Text, xlMale.Text, xxlMale.Text, xxxlMale.Text, xxxxlMale.Text, xxxxxlMale.Text, totalMale.Text, k6Female.Text, k8Female.Text,
                k10Female.Text, k12Female.Text, k14Female.Text, k16Female.Text, k18Female.Text, k20Female.Text, tsFemale.Text, xsFemale.Text, sFemale.Text, mFemale.Text,
                lFemale.Text, xlFemale.Text, xxlFemale.Text, xxxlFemale.Text, xxxxlFemale.Text, xxxxxlFemale.Text, totalFemale.Text, fabricMat.Text, quantityMat.Text, meterMat.Text,
                kiloMat.Text, bodyColor1.Text, bodyColor2.Text, bodyColor3.Text, bodyColor4.Text, bodyColor5.Text, bodyColorAmount1.Text, bodyColorAmount2.Text, bodyColorAmount3.Text, bodyColorAmount4.Text,
                bodyColorAmount5.Text, neckCollarCuffs1.Text, neckCollarCuffs2.Text, neckCollarCuffsAmount1.Text, neckCollarCuffsAmount2.Text, garterZipper1.Text, garterZipperAmount1.Text, thread1.Text, thread2.Text, threadAmount1.Text,
                threadAmount2.Text, estimatedBy.Text, materialsOrderedBy.Text, note.Text, amountPerPiece.Text, numberOfPieces.Text, dateReceived.Text, dateReleased.Text, dateProjectReport.Text, fabricMaterialsExp.Text, 
                paintExp.Text, cutSewLaborExp.Text, transportationExp.Text, marketingFeeExp.Text, otherExpensesExp1.Text, otherExpensesExp2.Text, preparedBy.Text, checkedBy.Text, totalAmount.Text, expenses.Text,
                netIncome.Text, receivable.Text, orderCodeSelected);
            csv.AppendLine(newLine);
            File.AppendAllText(fileName, csv.ToString());
            File.AppendAllText(fileNameBackup, csv.ToString());
            orderCodeSelected = "";
            MessageBox.Show("Job order succesfully edited.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void loadFromFile()
        {
            try
            {
                string delimiter = ",";
                string tablename = "export";
                StreamReader sr = new StreamReader(@fileName);
                dataset = new DataSet();
                dataset.Tables.Add(tablename);
                string allData = sr.ReadToEnd();
                sr.Close();
                string[] rows = allData.Split("\r".ToCharArray());
                //add first the column names
                string[] items = rows[0].Split(delimiter.ToCharArray());
                foreach (string r in items)
                {
                    dataset.Tables[tablename].Columns.Add(r);
                }
                for (int x = 1; x < rows.Length; x++)
                {
                    items = rows[x].Split(delimiter.ToCharArray());
                    if (items[0].Length>1)
                    {
                        dataset.Tables[tablename].Rows.Add(items);
                    }
                    Application.DoEvents();
                }
                allJobOrdersDataGrid.DataSource = dataset.Tables[0].DefaultView;
                adjustWidthOfDataGridColumns();
            }
            catch (Exception ex)
            {
               MessageBox.Show("Error loading records. Please close the file " + fileName + " if you opened it. \n\nFull Error: \n" + ex.ToString(), "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //this is to edit or produce reports
        private void allJobOrdersDataGrid_DoubleClick(object sender, EventArgs e)
        {
            int rowindex = allJobOrdersDataGrid.CurrentCell.RowIndex;
            string orderCode = allJobOrdersDataGrid.Rows[rowindex].Cells[93].Value.ToString();
            orderCodeSelected = orderCode;
            if (allJobOrdersDataGrid.Rows[0].Cells[rowindex].Value.ToString()=="None" || orderCode=="")
            {
                return;
            }
            allTabs.SelectedTab = orderDetailsTab;
            enableDisableisableTextfieldsInOrderDetails(true, orderDetailsTab);
            enableDisableisableTextfieldsInOrderDetails(true, projectReportTab);
            createJobOrderButton.Text = "Save Changes";
            //fill in values
            customer.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[0].Value.ToString();
            customerProjectReport.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[0].Value.ToString();
            projectTitle.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[1].Value.ToString();
            projectTitleProjectReport.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[1].Value.ToString();
            colorCombination.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[2].Value.ToString();
            pattern.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[3].Value.ToString();
            date.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[4].Value.ToString();
            dueDate.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[5].Value.ToString();
            quantity.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[6].Value.ToString();
            fabric.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[7].Value.ToString();
            endorsedBy.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[8].Value.ToString();
            jobEndorsedBy.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[9].Value.ToString();
            k6Male.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[10].Value.ToString();
            k8Male.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[11].Value.ToString();
            k10Male.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[12].Value.ToString();
            k12Male.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[13].Value.ToString();
            k14Male.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[14].Value.ToString();
            k16Male.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[15].Value.ToString();
            k18Male.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[16].Value.ToString();
            k20Male.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[17].Value.ToString();
            tsMale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[18].Value.ToString();
            xsMale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[19].Value.ToString();
            sMale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[20].Value.ToString();
            mMale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[21].Value.ToString();
            lMale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[22].Value.ToString();
            xlMale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[23].Value.ToString();
            xxlMale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[24].Value.ToString();
            xxxlMale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[25].Value.ToString();
            xxxxlMale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[26].Value.ToString();
            xxxxxlMale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[27].Value.ToString();
            totalMale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[28].Value.ToString();
            k6Female.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[29].Value.ToString();
            k8Female.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[30].Value.ToString();
            k10Female.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[31].Value.ToString();
            k12Female.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[32].Value.ToString();
            k14Female.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[33].Value.ToString();
            k16Female.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[34].Value.ToString();
            k18Female.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[35].Value.ToString();
            k20Female.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[36].Value.ToString();
            tsFemale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[37].Value.ToString();
            xsFemale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[38].Value.ToString();
            sFemale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[39].Value.ToString();
            mFemale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[40].Value.ToString();
            lFemale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[41].Value.ToString();
            xlFemale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[42].Value.ToString();
            xxlFemale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[43].Value.ToString();
            xxxlFemale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[44].Value.ToString();
            xxxxlFemale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[45].Value.ToString();
            xxxxxlFemale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[46].Value.ToString();
            totalFemale.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[47].Value.ToString();
            fabricMat.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[48].Value.ToString();
            quantityMat.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[49].Value.ToString();
            meterMat.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[50].Value.ToString();
            kiloMat.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[51].Value.ToString();
            bodyColor1.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[52].Value.ToString();
            bodyColor2.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[53].Value.ToString();
            bodyColor3.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[54].Value.ToString();
            bodyColor4.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[55].Value.ToString();
            bodyColor5.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[56].Value.ToString();
            bodyColorAmount1.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[57].Value.ToString();
            bodyColorAmount2.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[58].Value.ToString();
            bodyColorAmount3.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[59].Value.ToString();
            bodyColorAmount4.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[60].Value.ToString();
            bodyColorAmount5.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[61].Value.ToString();
            neckCollarCuffs1.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[62].Value.ToString();
            neckCollarCuffs2.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[63].Value.ToString();
            neckCollarCuffsAmount1.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[64].Value.ToString();
            neckCollarCuffsAmount2.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[65].Value.ToString();
            garterZipper1.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[66].Value.ToString();
            garterZipperAmount1.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[67].Value.ToString();
            thread1.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[68].Value.ToString();
            thread2.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[69].Value.ToString();
            threadAmount1.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[70].Value.ToString();
            threadAmount2.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[71].Value.ToString();
            estimatedBy.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[72].Value.ToString();
            materialsOrderedBy.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[73].Value.ToString();
            note.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[74].Value.ToString();
            amountPerPiece.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[75].Value.ToString();
            numberOfPieces.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[76].Value.ToString();
            dateReceived.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[77].Value.ToString();
            dateReleased.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[78].Value.ToString();
            dateProjectReport.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[79].Value.ToString();
            fabricMaterialsExp.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[80].Value.ToString();
            paintExp.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[81].Value.ToString();
            cutSewLaborExp.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[82].Value.ToString();
            transportationExp.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[83].Value.ToString();
            marketingFeeExp.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[84].Value.ToString();
            otherExpensesExp1.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[85].Value.ToString();
            otherExpensesExp2.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[86].Value.ToString();
            preparedBy.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[87].Value.ToString();
            checkedBy.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[88].Value.ToString();
            totalAmount.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[89].Value.ToString();
            expenses.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[90].Value.ToString();
            netIncome.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[91].Value.ToString();
            receivable.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[92].Value.ToString();
        }

        private void allJobOrdersDataGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            int rowindex = allJobOrdersDataGrid.CurrentCell.RowIndex;
            string orderCode = allJobOrdersDataGrid.Rows[rowindex].Cells[93].Value.ToString();
            if (MessageBox.Show("Are you sure you want to delete job order " + orderCode + "?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                deleteAtRowWithCode(orderCode);
                ClearTextBoxes(orderDetailsTab);
                ClearTextBoxes(projectReportTab);
                enableDisableisableTextfieldsInOrderDetails(false, orderDetailsTab);
                enableDisableisableTextfieldsInOrderDetails(false, projectReportTab);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void deleteAtRowWithCode(string orderCode)
        {
            var tempFile = Path.GetTempFileName();
            var linesToKeep = File.ReadLines(fileName).Where(line => !line.Contains(orderCode));
            File.WriteAllLines(tempFile, linesToKeep);
            File.Delete(fileName);
            File.Move(tempFile, fileName);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadFromFile();
            enableDisableisableTextfieldsInOrderDetails(false, orderDetailsTab);
            enableDisableisableTextfieldsInOrderDetails(false, projectReportTab);
        }

        private void newJobOrder_Click(object sender, EventArgs e)
        {
            enableDisableisableTextfieldsInOrderDetails(true, orderDetailsTab);
            enableDisableisableTextfieldsInOrderDetails(true, projectReportTab);
            allTabs.SelectedTab = orderDetailsTab;
            createJobOrderButton.Text = "Create New Job Order";
            orderCodeSelected = "";
        }

        private void enableDisableisableTextfieldsInOrderDetails(bool method, Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    c.Enabled = method;
                }
                else if (c is DateTimePicker)
                {
                    c.Enabled = method;
                }
                else if (c is Button)
                {
                    c.Enabled = method;
                }
                if (c.HasChildren)
                {
                    enableDisableisableTextfieldsInOrderDetails(method, c);
                }
            }
        }

        public void ClearTextBoxes(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
                if (c.HasChildren)
                {
                    ClearTextBoxes(c);
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel this job order", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                ClearTextBoxes(orderDetailsTab);
                ClearTextBoxes(projectReportTab);
                enableDisableisableTextfieldsInOrderDetails(false, orderDetailsTab);
                enableDisableisableTextfieldsInOrderDetails(false, projectReportTab);
                allTabs.SelectedTab = allOrdersTab;
            }
        }

        private void adjustWidthOfDataGridColumns()
        {
            int numColumns = allJobOrdersDataGrid.ColumnCount;
            for (int x=0; x<numColumns; x++)
            {
                if (columnsToShow.Contains(x))
                {
                    allJobOrdersDataGrid.Columns[x].Visible = true;
                }
                else
                {
                    allJobOrdersDataGrid.Columns[x].Visible = false;
                }
            }
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

        private void cancelProjectReportButton_Click(object sender, EventArgs e)
        {
            cancelButton_Click(sender, e);
        }

        private void saveProjectReportButton_Click(object sender, EventArgs e)
        {
            createJobOrderButton_Click(sender, e);
        }
    }
}
