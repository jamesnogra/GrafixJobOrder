using System;
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
        DataTable dataset = new DataTable();

        public Form1()
        {
            InitializeComponent();
            initTimerForCalculateSizes();
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
               "{91},{92},{93},{94},{95},{96},{97},{98},{99},{100},{101}",
               customer.Text, projectTitle.Text, colorCombination.Text, pattern.Text, date.Text, dueDate.Text, quantity.Text, fabric.Text, endorsedBy.Text, jobEndorsedBy.Text, k6Male.Text,
               k8Male.Text, k10Male.Text, k12Male.Text, k14Male.Text, k16Male.Text, k18Male.Text, k20Male.Text, tsMale.Text, xsMale.Text, sMale.Text,
               mMale.Text, lMale.Text, xlMale.Text, xxlMale.Text, xxxlMale.Text, xxxxlMale.Text, xxxxxlMale.Text, totalMale.Text, k6Female.Text, k8Female.Text,
               k10Female.Text, k12Female.Text, k14Female.Text, k16Female.Text, k18Female.Text, k20Female.Text, tsFemale.Text, xsFemale.Text, sFemale.Text, mFemale.Text,
               lFemale.Text, xlFemale.Text, xxlFemale.Text, xxxlFemale.Text, xxxxlFemale.Text, xxxxxlFemale.Text, totalFemale.Text, fabricMat.Text, quantityMat.Text, meterMat.Text,
               kiloMat.Text, bodyColor1.Text, bodyColor2.Text, bodyColor3.Text, bodyColor4.Text, bodyColor5.Text, bodyColorAmount1.Text, bodyColorAmount2.Text, bodyColorAmount3.Text, bodyColorAmount4.Text,
               bodyColorAmount5.Text, neckCollarCuffs1.Text, neckCollarCuffs2.Text, neckCollarCuffsAmount1.Text, neckCollarCuffsAmount2.Text, garterZipper1.Text, garterZipperAmount1.Text, thread1.Text, thread2.Text, threadAmount1.Text,
               threadAmount2.Text, estimatedBy.Text, materialsOrderedBy.Text, note.Text, amountPerPiece.Text, numberOfPieces.Text, dateReceived.Text, dateReleased.Text, dateProjectReport.Text, fabricMaterialsExp.Text,
               paintExp.Text, cutSewLaborExp.Text, transportationExp.Text, marketingFeeExp.Text, otherExpensesExp1.Text, otherExpensesExp2.Text, preparedBy.Text, checkedBy.Text, totalAmount.Text, expenses.Text,
               netIncome.Text, receivable.Text, randomString(8), photoTextbox.Text, totalPayable.Text, partialPayment.Text, additionalPayment.Text, balance.Text, partialPaymentDate.Text, additionalPaymentDate.Text,
                ((poRadioButton.Checked) ? "PO" : "Cash"));
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
                "{91},{92},{93},{94},{95},{96},{97},{98},{99},{100},{101}",
                customer.Text, projectTitle.Text, colorCombination.Text, pattern.Text, date.Text, dueDate.Text, quantity.Text, fabric.Text, endorsedBy.Text, jobEndorsedBy.Text, k6Male.Text,
                k8Male.Text, k10Male.Text, k12Male.Text, k14Male.Text, k16Male.Text, k18Male.Text, k20Male.Text, tsMale.Text, xsMale.Text, sMale.Text,
                mMale.Text, lMale.Text, xlMale.Text, xxlMale.Text, xxxlMale.Text, xxxxlMale.Text, xxxxxlMale.Text, totalMale.Text, k6Female.Text, k8Female.Text,
                k10Female.Text, k12Female.Text, k14Female.Text, k16Female.Text, k18Female.Text, k20Female.Text, tsFemale.Text, xsFemale.Text, sFemale.Text, mFemale.Text,
                lFemale.Text, xlFemale.Text, xxlFemale.Text, xxxlFemale.Text, xxxxlFemale.Text, xxxxxlFemale.Text, totalFemale.Text, fabricMat.Text, quantityMat.Text, meterMat.Text,
                kiloMat.Text, bodyColor1.Text, bodyColor2.Text, bodyColor3.Text, bodyColor4.Text, bodyColor5.Text, bodyColorAmount1.Text, bodyColorAmount2.Text, bodyColorAmount3.Text, bodyColorAmount4.Text,
                bodyColorAmount5.Text, neckCollarCuffs1.Text, neckCollarCuffs2.Text, neckCollarCuffsAmount1.Text, neckCollarCuffsAmount2.Text, garterZipper1.Text, garterZipperAmount1.Text, thread1.Text, thread2.Text, threadAmount1.Text,
                threadAmount2.Text, estimatedBy.Text, materialsOrderedBy.Text, note.Text, amountPerPiece.Text, numberOfPieces.Text, dateReceived.Text, dateReleased.Text, dateProjectReport.Text, fabricMaterialsExp.Text, 
                paintExp.Text, cutSewLaborExp.Text, transportationExp.Text, marketingFeeExp.Text, otherExpensesExp1.Text, otherExpensesExp2.Text, preparedBy.Text, checkedBy.Text, totalAmount.Text, expenses.Text,
                netIncome.Text, receivable.Text, orderCodeSelected, photoTextbox.Text, totalPayable.Text, partialPayment.Text, additionalPayment.Text, balance.Text, partialPaymentDate.Text, additionalPaymentDate.Text,
                ((poRadioButton.Checked)?"PO":"Cash"));
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
                StreamReader sr = new StreamReader(@fileName);
                dataset = new DataTable();
                //dataset.Tables.Add(tablename);
                string allData = sr.ReadToEnd();
                sr.Close();
                string[] rows = allData.Split("\r".ToCharArray());
                //add first the column names
                string[] items = rows[0].Split(delimiter.ToCharArray());
                for (int x=0; x<items.Length; x++)
                {
                    //dataset.Tables[tablename].Columns.Add(items[x]);
                    dataset.Columns.Add(items[x]);
                }
                for (int x = 1; x < rows.Length; x++)
                {
                    items = rows[x].Split(delimiter.ToCharArray());
                    if (items[0].Length>1)
                    {
                        //dataset.Tables[tablename].Rows.Add(items);
                        dataset.Rows.Add(items);
                    }
                    Application.DoEvents();
                }
                //allJobOrdersDataGrid.DataSource = dataset.Tables[0].DefaultView;
                allJobOrdersDataGrid.DataSource = dataset;
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
            //93 is for the order code
            //set the image
            photoTextbox.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[94].Value.ToString();
            photoPicbox.Image = (photoTextbox.Text.Length > 1) ? new Bitmap(photoTextbox.Text) : null;
            totalPayable.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[95].Value.ToString();
            partialPayment.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[96].Value.ToString();
            additionalPayment.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[97].Value.ToString();
            balance.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[98].Value.ToString();
            partialPaymentDate.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[99].Value.ToString();
            additionalPaymentDate.Text = allJobOrdersDataGrid.Rows[rowindex].Cells[100].Value.ToString();
            //this is for the radio button of the mode of payment; cash or PO
            poRadioButton.Checked = false;
            cashRadioButton.Checked = false;
            if (allJobOrdersDataGrid.Rows[rowindex].Cells[101].Value.ToString() == "PO")
            {
                poRadioButton.Checked = true;
                cashRadioButton.Checked = false;
            }
            else if(allJobOrdersDataGrid.Rows[rowindex].Cells[101].Value.ToString() == "Cash")
            {
                poRadioButton.Checked = false;
                cashRadioButton.Checked = true;
            }
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
                photoPicbox.Image = null;
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
                    allJobOrdersDataGrid.Columns[x].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                else
                {
                    allJobOrdersDataGrid.Columns[x].Visible = false;
                }
            }
        }

        private void changePhotoButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                string originalFile = open.FileName;
                string fileExtension = Path.GetExtension(originalFile);
                photoPicbox.Image = new Bitmap(open.FileName);
                photoTextbox.Text = "Data/Images/" + randomString(8) + fileExtension;
                File.Copy(originalFile, photoTextbox.Text);
            }
        }

        private void calculateSizesTotal(object sender, EventArgs e)
        {
            int totalMaleNum = 0;
            try
            {
                totalMaleNum += (k6Male.Text.Length > 0) ? Int32.Parse(k6Male.Text) : 0;
                totalMaleNum += (k8Male.Text.Length > 0) ? Int32.Parse(k8Male.Text) : 0;
                totalMaleNum += (k10Male.Text.Length > 0) ? Int32.Parse(k10Male.Text) : 0;
                totalMaleNum += (k12Male.Text.Length > 0) ? Int32.Parse(k12Male.Text) : 0;
                totalMaleNum += (k14Male.Text.Length > 0) ? Int32.Parse(k14Male.Text) : 0;
                totalMaleNum += (k16Male.Text.Length > 0) ? Int32.Parse(k16Male.Text) : 0;
                totalMaleNum += (k18Male.Text.Length > 0) ? Int32.Parse(k18Male.Text) : 0;
                totalMaleNum += (k20Male.Text.Length > 0) ? Int32.Parse(k20Male.Text) : 0;
                totalMaleNum += (tsMale.Text.Length > 0) ? Int32.Parse(tsMale.Text) : 0;
                totalMaleNum += (xsMale.Text.Length > 0) ? Int32.Parse(xsMale.Text) : 0;
                totalMaleNum += (sMale.Text.Length > 0) ? Int32.Parse(sMale.Text) : 0;
                totalMaleNum += (mMale.Text.Length > 0) ? Int32.Parse(mMale.Text) : 0;
                totalMaleNum += (lMale.Text.Length > 0) ? Int32.Parse(lMale.Text) : 0;
                totalMaleNum += (xlMale.Text.Length > 0) ? Int32.Parse(xlMale.Text) : 0;
                totalMaleNum += (xxlMale.Text.Length > 0) ? Int32.Parse(xxlMale.Text) : 0;
                totalMaleNum += (xxxlMale.Text.Length > 0) ? Int32.Parse(xxxlMale.Text) : 0;
                totalMaleNum += (xxxxlMale.Text.Length > 0) ? Int32.Parse(xxxxlMale.Text) : 0;
                totalMaleNum += (xxxxxlMale.Text.Length > 0) ? Int32.Parse(xxxxxlMale.Text) : 0;
                totalMale.Text = totalMaleNum.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("One of the male sizes you entered is not a number.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int totalFemaleNum = 0;
            try
            {
                totalFemaleNum += (k6Female.Text.Length > 0) ? Int32.Parse(k6Female.Text) : 0;
                totalFemaleNum += (k8Female.Text.Length > 0) ? Int32.Parse(k8Female.Text) : 0;
                totalFemaleNum += (k10Female.Text.Length > 0) ? Int32.Parse(k10Female.Text) : 0;
                totalFemaleNum += (k12Female.Text.Length > 0) ? Int32.Parse(k12Female.Text) : 0;
                totalFemaleNum += (k14Female.Text.Length > 0) ? Int32.Parse(k14Female.Text) : 0;
                totalFemaleNum += (k16Female.Text.Length > 0) ? Int32.Parse(k16Female.Text) : 0;
                totalFemaleNum += (k18Female.Text.Length > 0) ? Int32.Parse(k18Female.Text) : 0;
                totalFemaleNum += (k20Female.Text.Length > 0) ? Int32.Parse(k20Female.Text) : 0;
                totalFemaleNum += (tsFemale.Text.Length > 0) ? Int32.Parse(tsFemale.Text) : 0;
                totalFemaleNum += (xsFemale.Text.Length > 0) ? Int32.Parse(xsFemale.Text) : 0;
                totalFemaleNum += (sFemale.Text.Length > 0) ? Int32.Parse(sFemale.Text) : 0;
                totalFemaleNum += (mFemale.Text.Length > 0) ? Int32.Parse(mFemale.Text) : 0;
                totalFemaleNum += (lFemale.Text.Length > 0) ? Int32.Parse(lFemale.Text) : 0;
                totalFemaleNum += (xlFemale.Text.Length > 0) ? Int32.Parse(xlFemale.Text) : 0;
                totalFemaleNum += (xxlFemale.Text.Length > 0) ? Int32.Parse(xxlFemale.Text) : 0;
                totalFemaleNum += (xxxlFemale.Text.Length > 0) ? Int32.Parse(xxxlFemale.Text) : 0;
                totalFemaleNum += (xxxxlFemale.Text.Length > 0) ? Int32.Parse(xxxxlFemale.Text) : 0;
                totalFemaleNum += (xxxxxlFemale.Text.Length > 0) ? Int32.Parse(xxxxxlFemale.Text) : 0;
                totalFemale.Text = totalFemaleNum.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("One of the Female sizes you entered is not a number.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void searchButton_Click(object sender, EventArgs e)
        {
            string searchValue = searchKeyword.Text;
            (allJobOrdersDataGrid.DataSource as DataTable).DefaultView.RowFilter = string.Format("`Customer` LIKE '%{0}%' OR `Project Title` LIKE '%{0}%' OR `Date` LIKE '%{0}%' OR `Due Date` LIKE '%{0}%'  OR `Order Code` LIKE '%{0}%'", searchValue);
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            searchKeyword.Text = "";
            (allJobOrdersDataGrid.DataSource as DataTable).DefaultView.RowFilter = null;
        }

        private void searchKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                searchButton_Click(sender, e);
            }
        }

        private Timer calculateSizesTotalTimer;
        public void initTimerForCalculateSizes()
        {
            calculateSizesTotalTimer = new Timer();
            calculateSizesTotalTimer.Tick += new EventHandler(calculateSizesTotal);
            calculateSizesTotalTimer.Interval = 3000; // in miliseconds
            calculateSizesTotalTimer.Start();
        }

        private void paymentModeButton_Click(object sender, EventArgs e)
        {
            allTabs.SelectedTab = paymentModeTab;
        }

        private void paymentModeCancelButton_Click(object sender, EventArgs e)
        {
            cancelButton_Click(sender, e);
        }

        private void paymentModeSaveButton_Click(object sender, EventArgs e)
        {
            createJobOrderButton_Click(sender, e);
        }
    }
}
