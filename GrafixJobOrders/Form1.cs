using System;
using System.Windows.Forms;

namespace GrafixJobOrders
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void createJobOrderButton_Click(object sender, EventArgs e)
        {
            grafixData1.JobOrders.AddJobOrdersRow(grafixData1.JobOrders.NewJobOrdersRow());
            jobOrdersBindingSource1.MoveLast();
            jobOrdersTableAdapter.Update(grafixData1.JobOrders);
            dataGridViewJobOrders.Refresh();
            MessageBox.Show("Added");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'grafixData1.JobOrders' table. You can move, or remove it, as needed.
            this.jobOrdersTableAdapter.Fill(this.grafixData1.JobOrders);
            //jobOrdersBindingSource1.MoveLast();
        }
    }
}
