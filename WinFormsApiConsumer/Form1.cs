using WebApiLab2.DTOs;

namespace WinFormsApiConsumer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync("https://localhost:7257/api/departments").Result;
            if(response.IsSuccessStatusCode)
            {
                var departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDTO>>().Result;
                dataGridView1.DataSource = departments;
            }
        }
    }
}
