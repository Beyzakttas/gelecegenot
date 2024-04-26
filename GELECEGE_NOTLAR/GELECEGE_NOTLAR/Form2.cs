using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GELECEGE_NOTLAR
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string kullaniciAdi = txtKullaniciAdi.Text;
            string sifre = txtSifre.Text;

            // Burada kullanıcı adı ve şifre kontrolü yapılabilir.
            // Örneğin, veritabanında kullanıcı adı ve şifre kontrol edilebilir.
            // Doğru giriş bilgileri sağlandığında, ikinci forma geçiş yapılabilir.

            if (kullaniciAdi == "admin" && sifre == "123") // Örnek kontrol, gerçek giriş bilgilerinizi kullanın
            {
                Form1 Form1= new Form1();
                Form1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış!");
            }
        }
    }
}
        