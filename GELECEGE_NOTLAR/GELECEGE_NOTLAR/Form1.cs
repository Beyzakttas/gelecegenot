using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GELECEGE_NOTLAR
{
    public partial class Form1 : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=GelecegeNotDB;Integrated Security=True");
            Timer timer;
     public Form1()
        {
            InitializeComponent();
            InitializeTimer();
        }
        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 60000; // 1 dakikada bir kontrol etmek için
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        
        //Bu kodlar, bir zamanlayıcı(timer) kullanarak veritabanında saklanan notları kontrol eder
        //ve eğer notların gösterim tarihi geçmişse, kullanıcıya hatırlatma mesajı gösterir.
        private void Timer_Tick(object sender, EventArgs e)
        {
            CheckReminders();
        }
        private void CheckReminders()
        {
            DateTime currentDate = DateTime.Now;

            using (SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=GelecegeNotDB;Integrated Security=True"))
            {
                connection.Open();
                string query = "SELECT Baslik, Icerik FROM Notlar WHERE GoruntulemeTarihi <= @CurrentDate";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrentDate", currentDate);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string baslik = reader["Baslik"].ToString();
                                string icerik = reader["Icerik"].ToString();
                                MessageBox.Show($"Başlık: {baslik}\nİçerik: {icerik}", "Hatırlatma", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'gelecegeNotDBDataSet.Notlar' table. You can move, or remove it, as needed.
            this.notlarTableAdapter.Fill(this.gelecegeNotDBDataSet.Notlar);
            listele();
        }
        void listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adtr = new SqlDataAdapter("SELECT * FROM Notlar", baglanti);
            adtr.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        void temizle()
        {
            txtıd.Text = string.Empty;
            txtBaslik.Text = string.Empty;
            txtIcerik.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void btnekle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO  Notlar (Baslik, Icerik, GoruntulemeTarihi) VALUES (@Baslik, @Icerik, @GoruntulemeTarihi)", baglanti);
            komut.Parameters.AddWithValue("@Baslik", txtBaslik.Text);
            komut.Parameters.AddWithValue("@Icerik", txtIcerik.Text);
            komut.Parameters.AddWithValue("@GoruntulemeTarihi", dateTimePicker1.Value);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kayıt eklendi");
            listele();
            temizle();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
           
            if (dataGridView1.CurrentRow != null)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("UPDATE Notlar SET Baslik = @Baslik, Icerik = @Icerik, GoruntulemeTarihi = @GoruntulemeTarihi WHERE NotID = @NotID", baglanti);
                komut.Parameters.AddWithValue("@Baslik", txtBaslik.Text);
                komut.Parameters.AddWithValue("@Icerik", txtIcerik.Text);
                komut.Parameters.AddWithValue("@GoruntulemeTarihi", dateTimePicker1.Value);
                komut.Parameters.AddWithValue("@NotID", txtıd.Text); // satıra not ıd ekler
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kayıt düzenlendi");
                listele();
                temizle();
            }
            else
            {
                MessageBox.Show("Düzenlenecek kayıt seçilmedi.");
            }
        }
        //kayıtları siler ve mesaj gonderir.
        private void btnSil_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Kayıt siliniyor!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("DELETE FROM Notlar WHERE NotID= @NotID", baglanti);
                komut.Parameters.AddWithValue("@NotID", dataGridView1.CurrentRow.Cells["NotID"].Value.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kayıt silindi");
                listele();
                temizle();
            }
        }

        private void btntemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }
        //satırlarda yazan bilgileri ilgili textbox lara doldurur.
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtıd.Text = dataGridView1.CurrentRow.Cells["NotID"].Value.ToString();
            txtBaslik.Text = dataGridView1.CurrentRow.Cells["Baslik"].Value.ToString();
            txtIcerik.Text = dataGridView1.CurrentRow.Cells["Icerik"].Value.ToString();

            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["GoruntulemeTarihi"].Value.ToString());
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // DataGridView'da son satırı seç
            DataGridViewRow lastRow = dataGridView1.Rows[dataGridView1.Rows.Count - 1];

            // Son satırdaki bilgileri al
            string baslik = lastRow.Cells["Baslik"].Value.ToString();
            string icerik = lastRow.Cells["Icerik"].Value.ToString();

            // MessageBox, TextBox veya başka bir kontrol kullanarak mesajı gösterebilirsiniz
            MessageBox.Show($"Başlık: {baslik}\nİçerik: {icerik}", "Son Gönderilen Not", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
    }
    }
}
