using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace EpervierAgencyCs_User
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MySqlConnection cn;
        bool Connecté = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Se connecter")
            {
                cn = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=bd_agency;UID=root;PWD=root;");
                try
                {
                
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                button1.Text = "Se déconnecter";
                    Connecté = true;
                }    
                catch (Exception ex){ MessageBox.Show(ex.Message); }


            }else //Se déconnecter
            {
                cn.Close();
                button1.Text = "Se connecter";
                Connecté= false;    
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Entrer un Nom de famille.");
            }else if(textBox2.Text == "")
            {
                MessageBox.Show("Entrez l'âge.");
            }else
            {
                if(Connecté)
                {
                   MySqlCommand cmd = new MySqlCommand("INSERT INTO user(nom,prenom,age,pays,ville) VALUES(@nom,@prenom,@age,@pays,@ville)", cn);
                    cmd.Parameters.AddWithValue("@nom", textBox1.Text);
                    cmd.Parameters.AddWithValue("@prenom", textBox2.Text);
                    cmd.Parameters.AddWithValue("@age", int.Parse(textBox3.Text));
                    cmd.Parameters.AddWithValue("@pays", textBox4.Text);
                    cmd.Parameters.AddWithValue("@ville", textBox5.Text);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    MessageBox.Show("Ajouté.");

                }
                else
                {
                    MessageBox.Show("Vous n'êtes pas connectés à la base de donnée");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Connecté)
            {
                listView1.Items.Clear();// pour vider la liste
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user", cn);
                using(MySqlDataReader Lire = cmd.ExecuteReader())
                {
                    while (Lire.Read())
                    {
                        string ID = Lire["ID"].ToString();//Tostring pour convertir en string
                        string Nom = Lire["nom"].ToString().ToString();
                        string Prenom = Lire["prenom"].ToString();
                        string Age = Lire["age"].ToString();
                        string Pays = Lire["pays"].ToString();
                        string Ville = Lire["ville"].ToString();

                        listView1.Items.Add(new ListViewItem(new[] { ID,Nom,Prenom,Age,Pays,Ville }));
                    }
                }
            }
            else { MessageBox.Show("Vous n'êtes pas connectés à la base de donnée"); } 
        }

        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void supprimerToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if(Connecté) //on vérifie si on est connecté à la base de données
            {
                if(listView1.SelectedItems.Count > 0)
                {
                    ListViewItem element = listView1.SelectedItems[0];
                    string Id = element.SubItems[0].Text;
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM user WHERE ID=@id", cn);
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();
                    element.Remove();
                    MessageBox.Show("Supprimé");
                }
            }
        }
    }
}
