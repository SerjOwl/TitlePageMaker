using System.Windows.Forms;

namespace TitlePageMaker
{
    public partial class Form1 : Form
    {
        public string inputFolderPdf;
        public string outputFolderPdf;
        public string templateFileDocx;
        public string dataFileDocx;
        public string outputFolderDocx;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Документ Word (*.docx)|*.docx";
                openFileDialog.FilterIndex = 1;

                //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                openFileDialog.Multiselect = false;

                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    templateFileDocx = openFileDialog.FileName;
                    textBox1.Text = openFileDialog.FileName.Split('\\').Last();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Файл .CSV (*.csv)|*.csv";
                openFileDialog.FilterIndex = 1;

                //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                openFileDialog.Multiselect = false;

                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    dataFileDocx = openFileDialog.FileName;
                    textBox2.Text = openFileDialog.FileName.Split('\\').Last();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Выберите папку для вывода .docx-файлов";
                folderBrowserDialog.UseDescriptionForTitle = true;

                //folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    outputFolderDocx = folderBrowserDialog.SelectedPath;
                    textBox3.Text = folderBrowserDialog.SelectedPath.Split('\\').Last();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Выберите папку с .docx-файлами";
                folderBrowserDialog.UseDescriptionForTitle = true;

                //folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    inputFolderPdf = folderBrowserDialog.SelectedPath;
                    textBox4.Text = folderBrowserDialog.SelectedPath.Split('\\').Last();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Выберите папку для вывода .pdf-файлов";
                folderBrowserDialog.UseDescriptionForTitle = true;

                //folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    outputFolderPdf = folderBrowserDialog.SelectedPath;
                    textBox5.Text = folderBrowserDialog.SelectedPath.Split('\\').Last();
                }
            }
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            DocsMaker titleMaker = new DocsMaker(outputFolderDocx, templateFileDocx);

            await Task.Run(() => titleMaker.GetData(dataFileDocx));

            await Task.Run(() => titleMaker.MakeDocs());

            MessageBox.Show(".docx-файлы успешно созданы");
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            PdfsMaker pdfsMaker = new PdfsMaker(inputFolderPdf, outputFolderPdf);

            await Task.Run( () => pdfsMaker.MakePdfs() );

            MessageBox.Show(".pdf-файлы успешно созданы");
        }
    }
}
