using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Parameters;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public object DataGridViewStd { get; private set; }
        private Database myclass;// defining the myclass in the form layer rather than in each buttons' own layer

        public Form1()
        {
            InitializeComponent();
            myclass = new Database();
        }

        private void button1_Click(object sender, EventArgs e)
        //button for viewing the data stored
        //changes view order based on the value in "comboBoxOrderBy"
        {
            string orderBy = "";
            switch (comboBoxOrderBy.SelectedItem.ToString())
            {
                case "Surname Alphabetical":
                    orderBy = "ORDER BY LastName ASC";
                    break;
                case "Forename Alphabetical":
                    orderBy = "ORDER BY FirstName ASC";
                    break;
                case "By ID":
                    orderBy = "ORDER BY ClientID ASC";
                    break;
                default:
                    orderBy = "";
                    break;
            }
            
            string query = "select * from Client " + orderBy;
            D1.DataSource= myclass.Execute(query);            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public class Database
        {

            public MySqlConnection con;
            public string server;
            public string database;
            public string username;
            public string password;

            public  Database()
            {
                //setting up a variable to be used as the connection string as this is used every time a connection is made
                string Connectionstring = "server=ysjcs.net; Database=tomchachula; Uid=tom.chachula; Pwd=JA5MFUU4;";
                con = new MySqlConnection(Connectionstring);
            }
            public DataTable Execute(string query)
            {
                con.Open();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand(query, con);
                //following line was used regularly for debugging, can be used to output any queries sent to the database so they can be checked for errors
                //MessageBox.Show(query);
                MySqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
                return dt;
            }
            //creating the insert function
            public void insert(string query, MySqlParameter[] Parameters = null)
            {
                try
                {
                    con.Open();
                    MySqlCommand command = new MySqlCommand(query, con);
                    if (Parameters != null)
                    {
                        command.Parameters.AddRange(Parameters);
                    }
                    command.ExecuteNonQuery();
                }
                //added some error handling to prevent the code from crashing after any erroneous data entry
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during insert operation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }

            //creating the update function. While they are very similar, they are kept seperate for code sanitisation
            //this is useful for testing the two functions seperately
            public void update(string query, MySqlParameter[] Parameters = null)
            {
                try
                {
                    con.Open();
                    MySqlCommand command = new MySqlCommand(query, con);
                    if (Parameters != null)
                    {
                        command.Parameters.AddRange(Parameters);
                    }
                    command.ExecuteNonQuery();
                }
                //same error handling message as in insert function
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during update operation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        //creating the event sequence for when the insert button (button2) is clicked
        private void button2_Click(object sender, EventArgs e)
        {
            //first a blank insert query is created
            string query = "insert into Client(ClientID, FirstName, LastName, Postcode, Phone, Email, Category) values(@ClientID, @FirstName, @LastName, @Postcode, @Phone, @Email, @Category)";
            //then a parameter list is created with spaces for each column in the table
            MySqlParameter[] param = new MySqlParameter[7];
            //and they are all populated with the values in the relevant textboxes
            param[0] = new MySqlParameter("@ClientID", MySqlDbType.Int32);
            param[0].Value = textBox1.Text;
            param[1] = new MySqlParameter("@FirstName", MySqlDbType.VarChar);
            param[1].Value = textBox2.Text;
            param[2] = new MySqlParameter("@LastName", MySqlDbType.VarChar);
            param[2].Value = textBox3.Text;
            param[3] = new MySqlParameter("@Postcode", MySqlDbType.VarChar);
            param[3].Value = textBox4.Text;
            param[4] = new MySqlParameter("@Phone", MySqlDbType.Int32);
            param[4].Value = textBox5.Text;
            param[5] = new MySqlParameter("@Email", MySqlDbType.VarChar);
            param[5].Value = textBox6.Text;
            param[6] = new MySqlParameter("@Category", MySqlDbType.Enum);
            param[6].Value = comboBox1.Text;
            //finally the query is sent to the insert function
            myclass.insert(query, param);
        }
        
        //the update event sequence is very similar
        private void button4_Click(object sender, EventArgs e)
        {
            //but the blank query created uses the relevant keywords update, set, and where (to avoid the change being sent to every entry)
            string query = "UPDATE Client SET FirstName = @FirstName, LastName = @LastName, Postcode = @Postcode, Phone = @Phone, Email = @Email, Category = @Category WHERE ClientID = @ClientID";
            //parameter assignment is the same
            MySqlParameter[] param = new MySqlParameter[7];
            param[0] = new MySqlParameter("@ClientID", MySqlDbType.Int32);
            param[0].Value = textBox1.Text;
            param[1] = new MySqlParameter("@FirstName", MySqlDbType.VarChar);
            param[1].Value = textBox2.Text;
            param[2] = new MySqlParameter("@LastName", MySqlDbType.VarChar);
            param[2].Value = textBox3.Text;
            param[3] = new MySqlParameter("@Postcode", MySqlDbType.VarChar);
            param[3].Value = textBox4.Text;
            param[4] = new MySqlParameter("@Phone", MySqlDbType.Int32);
            param[4].Value = textBox5.Text;
            param[5] = new MySqlParameter("@Email", MySqlDbType.VarChar);
            param[5].Value = textBox6.Text;
            param[6] = new MySqlParameter("@Category", MySqlDbType.Enum);
            param[6].Value = comboBox1.Text;
            //finally the finished query is sent to the update function
            myclass.update(query, param);
        }
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Populating the Order By comboBox
            comboBoxOrderBy.Items.Add("By ID");
            comboBoxOrderBy.Items.Add("Surname Alphabetical");
            comboBoxOrderBy.Items.Add("Forename Alphabetical");
            
            //setting the default selection to ID
            comboBoxOrderBy.SelectedIndex = 0;
        }

        private void comboBoxOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

   

}
