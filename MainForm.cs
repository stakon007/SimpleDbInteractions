namespace SimpleDbInteractions
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using System.IO;
    using Newtonsoft.Json;

    public partial class MainForm : Form
    {
        List<Person> LoadedPersons = new List<Person>();

        public MainForm()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            txtFilename.Text = "sampleDb.txt";
            lblName.Text = string.Empty;
            lblAge.Text = string.Empty;
            lblHobby.Text = string.Empty;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = @"json|*.json";
                dlg.Title = "Select a file in json format";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtFilename.Text = dlg.FileName;
                }
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            LoadedPersons = LoadPersonsFromFile(txtFilename.Text);
            lblLoadedPersonsNo.Text = LoadedPersons.Count.ToString();

            if (LoadedPersons.Count == 0)
            {
                lblLoadedPersonsNo.ForeColor = Color.Black;
            }
            else
            {
                lblLoadedPersonsNo.ForeColor = Color.Green;
            }
        }

        private void BtnShowInfo_Click(object sender, EventArgs e)
        {
            Person per = GetPersonById(txtID.Text);

            if (per == null)
            {
                MessageBox.Show($"No person with Id = '{txtID.Text}' was found!");
            }
            else
            {
                PopulatePersonData(per);
            }
        }

        private List<Person> LoadPersonsFromFile(string filename)
        {
            try
            {
                string json = File.ReadAllText(filename);
                List<Person> persons = JsonConvert.DeserializeObject<List<Person>>(json);
                return persons;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while reading file {filename}.{Environment.NewLine}Exception: {e.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Person>();
            }
        }

        private Person GetPersonById(string id)
        {
            return LoadedPersons.FirstOrDefault(x => x.Id == id);
        }

        private void PopulatePersonData(Person per)
        {
            if (per == null)
            {
                lblName.Text = string.Empty;
                lblAge.Text = string.Empty;
                lblHobby.Text = string.Empty;
            }
            else
            {
                lblName.Text = per.Name;
                lblAge.Text = per.Age.ToString();
                lblHobby.Text = per.Hobby;
            }
        }
    }
}
