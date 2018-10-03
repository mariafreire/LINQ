using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Basic_LINQ_Query_Operations
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'employeesDataSet.tblEmploy' table. You can move, or remove it, as needed.
            this.tblEmployTableAdapter.Fill(this.employeesDataSet.tblEmploy);

        }

        private void tblEmployBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tblEmployBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.employeesDataSet);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //query that select all records
            var record = from employee in employeesDataSet.tblEmploy.AsEnumerable()
                         select employee;
            tblEmployBindingSource.DataSource = record.AsDataView();
            this.Text = "All employees";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //sorting
            var record = from employee in employeesDataSet.tblEmploy.AsEnumerable()
                         orderby employee.Last_Name
                         select employee;
            tblEmployBindingSource.DataSource = record.AsDataView();
            this.Text = "All employees sorted by last name";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //show all records whose last name starts with "s"
            var record = from employee in employeesDataSet.tblEmploy.AsEnumerable()
                         where employee.Last_Name.StartsWith("S")
                         //sometimes we need to use the following code
                         //where employee.Field<string>("Last_Name").StartWith("S")
                         orderby employee.Last_Name
                         select employee;
            tblEmployBindingSource.DataSource = record.AsDataView();
            this.Text = "All employees whose last names begins with the letter \"S\"";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var record = from employee in employeesDataSet.tblEmploy.AsEnumerable()
                         where employee.Rate > 10
                         orderby employee.Rate
                         select employee;
            tblEmployBindingSource.DataSource = record.AsDataView();
            this.Text = "All employees whose rate is greater than $10";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //compound condition
            var record = from employee in employeesDataSet.tblEmploy.AsEnumerable()
                         where employee.Rate < 10 && employee.Status.ToString() == "p"
                         orderby employee.Rate
                         select employee;
            tblEmployBindingSource.DataSource = record.AsDataView();
            this.Text = "All part-time employees whose rate is less than $10";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //find the highest salary
            decimal topPay = (from em in employeesDataSet.tblEmploy.AsEnumerable()
                              select em.Rate).Max();
            //average salary of all employees
            decimal AverageRate = (from em in employeesDataSet.tblEmploy.AsEnumerable()
                                   select em.Rate).Average();
            //average salary of all part time employees
            decimal AverageRateP = (from em in employeesDataSet.tblEmploy.AsEnumerable()
                                    where em.Status == "P"
                                    select em.Rate).Average();
            //average salary of all full time employees
            decimal AverageRateF = (from em in employeesDataSet.tblEmploy.AsEnumerable()
                                    where em.Status == "F"
                                    select em.Rate).Average();
            MessageBox.Show("Highest Pay: " + topPay.ToString("c2") + Environment.NewLine +
                "Average Pay: " + AverageRate.ToString("c2") + Environment.NewLine +
                "Part-time average pay: " + AverageRateP.ToString("c2") + Environment.NewLine +
                "Full-time average pay: " + AverageRateF.ToString("c2"));
        }

        //alternative ways of using aggregate methods

        // assign datatable as a variable
        // var employee = employeesDataSet.tblEmploy.AsEnumerable();

        // find average salary of all employess
        // decimal averageRate = employee.Average(x => x.Field<decimal>("Rate"));

        // find highest salary
        // decimal topPay = employee.Max(x => x.Field<decimal>("Rate"));

        // find lowest salary
        // decimal bottomPay = employee.Min(x => x.Field<decimal>("Rate"));
        // MessageBox.Show("Bottom pay: " + bottomPay.ToString("c2") +
        // @"Top pay: " + topPay.ToString("C2") +
        // @"Average Rate: " + averageRate.ToString("C2") +

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //a example of using a textbox in a LINQ query:
            //all whose last name contains whatever in txtName(case insensitive)
            var record = from employee in employeesDataSet.tblEmploy.AsEnumerable()
                         where employee.Last_Name.ToUpper().Contains(textBox1.Text.ToUpper().Trim())
                         //or where employee.Field<string>("last_Name").Contains(textBox1.Text)
                         orderby employee.Last_Name
                         select employee;
            tblEmployBindingSource.DataSource = record.AsDataView();
            this.Text = "People whose last names contains " + textBox1.Text;
        }
    }
}
