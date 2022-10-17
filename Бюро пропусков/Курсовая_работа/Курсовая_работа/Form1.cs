using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace Курсовая_работа
{
    public partial class Form1 : Form
    {

        Encoding encod = Encoding.GetEncoding(1251);

        public Form1()
        {
            InitializeComponent();



            DateTime localDate = DateTime.Now;


            label26.Text = localDate.ToString("D");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = "c:\\";
            open.Filter = "base files (*.base)|*.base";
            open.FilterIndex = 2;
            open.RestoreDirectory = true;
            string data;

            if (open.ShowDialog() == DialogResult.OK)
            {

                string[] file_reader1 = File.ReadAllLines(open.FileName, encod);

                if (file_reader1.Length == 0)
                {
                    MessageBox.Show("Файл " + open.SafeFileName + " пуст!");
                }

                else
                {
                    dataGridView1.Rows.Clear();
                    StreamReader file_reader2 = new StreamReader(open.FileName, encod);

                    while (file_reader2.EndOfStream == false)
                    {
                        data = file_reader2.ReadToEnd();
                        string[] line = data.Split('\n');
                        for (int i = 0; i < line.Length; i++)
                        {

                            label22.Text = i.ToString();

                            string[] cell = line[i].Split(';');
                            if (cell.Length == 13)
                            {
                                try
                                {
                                    int all = 0;
                                    string test = Convert.ToString(cell[11]);
                                    DateTime lastService = DateTime.Parse((string)test);
                                    if ((DateTime.Now >= lastService.AddMonths(0)) && cell[9] == "ПОСТОЯННЫЙ")
                                    { cell[12] = "1"; }
                                    else { cell[12] = "0"; }

                                    if (cell[9] == "ВРЕМЕННЫЙ")
                                    {
                                        cell[12] = "";
                                        all = line.Length - 1;
                                        label22.Text = all.ToString();
                                    }

                                    dataGridView1.Rows.Add(cell[0], cell[1], cell[2], cell[3], cell[4], cell[5], cell[6], cell[7], cell[8], cell[9], cell[10], test, cell[12]);

                                }
                                catch { }
                            }
                        }
                    }
                    file_reader2.Close();
                }

            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                for (int j = i + 1; j < dataGridView1.Rows.Count; j++)
                    if (Convert.ToString(dataGridView1.Rows[i].Cells[0].Value) == Convert.ToString(dataGridView1.Rows[j].Cells[0].Value))
                        dataGridView1.Rows.Remove(dataGridView1.Rows[j]);

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var result = new System.Windows.Forms.DialogResult();
            result = MessageBox.Show("Сохранить изменения?", "Сохранить",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Base|*.base";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter file_writer = new StreamWriter(save.FileName, false, encod);
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                        {
                            if (j != 12) file_writer.Write(dataGridView1.Rows[i].Cells[j].Value + ";", false, encod);
                            else
                                file_writer.Write(dataGridView1.Rows[i].Cells[j].Value + "", false, encod);
                        }
                        file_writer.WriteLine();
                    }
                    file_writer.Close();
                }
            }
        }

        private void удалить_Click(object sender, EventArgs e)
        {
            var result = new System.Windows.Forms.DialogResult();
            result = MessageBox.Show("Удалить данные?", "Удалить",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                int index = dataGridView1.SelectedCells[0].RowIndex;
                try
                {
                    dataGridView1.Rows.RemoveAt(index);

                    int all = Int32.Parse(label22.Text) - 1;
                    label22.Text = all.ToString();

                }
                catch (InvalidOperationException)
                { MessageBox.Show("Выделите нужную строку в таблице, чтобы удалить её!"); }
            }

        }

        private void удалить_все_Click(object sender, EventArgs e)
        {
            var result = new System.Windows.Forms.DialogResult();
            result = MessageBox.Show("Удалить все данные?", "Удалить все",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                dataGridView1.Rows.Clear();
                label22.Text = "0";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.AllowUserToAddRows = false;
            /*Если пользователь вводит фамилию */
            int k = 0;
            if (name_findBox.Text != "")
            {


                //Проверка на ввод цифр
                string fam = name_findBox.Text.ToUpper();
                for (int i = 0; i < fam.Length; i++)
                {
                    if (fam[i] >= '0' && fam[i] <= '9')
                    {
                        MessageBox.Show("Фамилия не может состоять из цифр!");
                        name_findBox.Clear();
                        break;
                    }
                }


                dataGridView2.Rows.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (Convert.ToString(dataGridView1.Rows[i].Cells[1].Value) == name_findBox.Text.ToUpper())
                    {
                        dataGridView2.Rows.Add(Convert.ToString(dataGridView1.Rows[i].Cells[0].Value), Convert.ToString(dataGridView1.Rows[i].Cells[1].Value), Convert.ToString(dataGridView1.Rows[i].Cells[2].Value), Convert.ToString(dataGridView1.Rows[i].Cells[3].Value), Convert.ToString(dataGridView1.Rows[i].Cells[4].Value), Convert.ToString(dataGridView1.Rows[i].Cells[5].Value), Convert.ToString(dataGridView1.Rows[i].Cells[6].Value), Convert.ToString(dataGridView1.Rows[i].Cells[7].Value), Convert.ToString(dataGridView1.Rows[i].Cells[8].Value), Convert.ToString(dataGridView1.Rows[i].Cells[9].Value), Convert.ToString(dataGridView1.Rows[i].Cells[10].Value), Convert.ToString(dataGridView1.Rows[i].Cells[11].Value), Convert.ToString(dataGridView1.Rows[i].Cells[12].Value));
                        k++;
                    }

                }

                if (k == 0)
                {
                    MessageBox.Show("Такой фамилии нет!");
                    name_findBox.Clear();
                }

            }

            /*Если пользователь вводит табельный номер */
            int p = 0;
            string number = number_findBox.Text;
            if (number_findBox.Text != "")
            {

                //проверка ввода цифр
                try
                {
                    Convert.ToInt64(number_findBox.Text);
                }

                catch (FormatException)
                {
                    MessageBox.Show("Символы в номере должны быть цифрами!");
                    numberBox.Clear();
                }


                dataGridView2.Rows.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (number == Convert.ToString(dataGridView1.Rows[i].Cells[8].Value))
                    {
                        dataGridView2.Rows.Add(Convert.ToString(dataGridView1.Rows[i].Cells[0].Value), Convert.ToString(dataGridView1.Rows[i].Cells[1].Value), Convert.ToString(dataGridView1.Rows[i].Cells[2].Value), Convert.ToString(dataGridView1.Rows[i].Cells[3].Value), Convert.ToString(dataGridView1.Rows[i].Cells[4].Value), Convert.ToString(dataGridView1.Rows[i].Cells[5].Value), Convert.ToString(dataGridView1.Rows[i].Cells[6].Value), Convert.ToString(dataGridView1.Rows[i].Cells[7].Value), Convert.ToString(dataGridView1.Rows[i].Cells[8].Value), Convert.ToString(dataGridView1.Rows[i].Cells[9].Value), Convert.ToString(dataGridView1.Rows[i].Cells[10].Value), Convert.ToString(dataGridView1.Rows[i].Cells[11].Value), Convert.ToString(dataGridView1.Rows[i].Cells[12].Value));
                        p++;
                    }
                }

                if (p == 0)
                {
                    MessageBox.Show("Информации на данный номер нет!");
                    number_findBox.Clear();
                }
            }


            /*Если пользователь вводит номер карты*/
            int t = 0;
            string number2 = permit_findBox.Text;
            if (permit_findBox.Text != "")
            {

                //проверка ввода цифр
                try
                {
                    Convert.ToInt32(permit_findBox.Text);
                }

                catch (FormatException)
                {
                    MessageBox.Show("Символы в номере пропуска должны быть цифрами!");
                    permit_findBox.Clear();
                }

                dataGridView2.Rows.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (number2 == Convert.ToString(dataGridView1.Rows[i].Cells[0].Value))
                    {
                        dataGridView2.Rows.Add(Convert.ToString(dataGridView1.Rows[i].Cells[0].Value), Convert.ToString(dataGridView1.Rows[i].Cells[1].Value), Convert.ToString(dataGridView1.Rows[i].Cells[2].Value), Convert.ToString(dataGridView1.Rows[i].Cells[3].Value), Convert.ToString(dataGridView1.Rows[i].Cells[4].Value), Convert.ToString(dataGridView1.Rows[i].Cells[5].Value), Convert.ToString(dataGridView1.Rows[i].Cells[6].Value), Convert.ToString(dataGridView1.Rows[i].Cells[7].Value), Convert.ToString(dataGridView1.Rows[i].Cells[8].Value), Convert.ToString(dataGridView1.Rows[i].Cells[9].Value), Convert.ToString(dataGridView1.Rows[i].Cells[10].Value), Convert.ToString(dataGridView1.Rows[i].Cells[11].Value), Convert.ToString(dataGridView1.Rows[i].Cells[12].Value));
                        t++;
                    }
                }

                if (t == 0)
                {
                    MessageBox.Show("Информации на данный номер пропуска нет!");
                    permit_findBox.Clear();
                }
            }



        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            surnameBox.Clear();
            nameBox.Clear();
            patronymicBox.Clear();
            number_permitBox.Clear();
            numberBox.Clear();
            day1Box.Text = "01";
            month1Box.Text = "января";
            year1Box.Text = "2018";
            phoneBox.Text = "";
            positionBox.Text = "(нет)";
            departmentBox.Text = "(нет)";
            typeBox.Text = "постоянный";
            day2Box.Text = "01";
            year2Box.Text = "2018";
            month2Box.Text = "января";
            day3Box.Text = "01";
            year3Box.Text = "2018";
            month3Box.Text = "января";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }
        private void textBox9_Leave(object sender, EventArgs e)
        {

        }
        private void textBox9_Leave_1(object sender, EventArgs e)
        {
            if (day1Box.Text == "")
            {
                day1Box.Text = "01";
            }
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            if (year1Box.Text == "")
            {
                year1Box.Text = "2018";
            }
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }



        private void textBox11_Leave(object sender, EventArgs e)
        {
            if (day2Box.Text == "")
            {
                day2Box.Text = "01";
            }
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            if (year2Box.Text == "")
            {
                year2Box.Text = "2018";
            }
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            if (day3Box.Text == "")
            {
                day3Box.Text = "01";
            }
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            if (year3Box.Text == "")
            {
                year3Box.Text = "2018";
            }
        }

        private void comboBox6_Leave(object sender, EventArgs e)
        {
            if (month1Box.Text == "")
            {
                month1Box.Text = "января";
            }
        }

        private void comboBox5_Leave(object sender, EventArgs e)
        {
            if (month2Box.Text == "")
            {
                month2Box.Text = "января";
            }
        }

        private void comboBox7_Leave(object sender, EventArgs e)
        {
            if (month3Box.Text == "")
            {
                month3Box.Text = "января";
            }

        }

        private void открытьРуководствоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string commandText = "Руководство пользователя.pdf";
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = commandText;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        private void оПрограммеToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = new System.Windows.Forms.DialogResult();
            result = MessageBox.Show("Сохранить изменения?", "Сохранить как...",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Base|*.base|Word Documents|*.doc|Текстовый файл|*.txt";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter file_writer = new StreamWriter(save.FileName, false, encod);
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                        {
                            if (j != 12) file_writer.Write(dataGridView1.Rows[i].Cells[j].Value + ";", false, encod);
                            else
                                file_writer.Write(dataGridView1.Rows[i].Cells[j].Value + "", false, encod);
                        }
                        file_writer.WriteLine();
                    }
                    file_writer.Close();
                }
            }
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = new System.Windows.Forms.DialogResult();
            result = MessageBox.Show("Закрыть программу?", "Закрыть",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }


        private void button5_Click_1(object sender, EventArgs e)
        {
            //вывести данные

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string a = Convert.ToString(dataGridView2.Rows[i].Cells[1].Value);
                string b = Convert.ToString(dataGridView2.Rows[i].Cells[2].Value);
                string c = Convert.ToString(dataGridView2.Rows[i].Cells[3].Value);
                string d = Convert.ToString(dataGridView2.Rows[i].Cells[6].Value);
                string f = Convert.ToString(dataGridView2.Rows[i].Cells[7].Value);
                string g = Convert.ToString(dataGridView2.Rows[i].Cells[0].Value);
                string h = Convert.ToString(dataGridView2.Rows[i].Cells[8].Value);
                string s = Convert.ToString(dataGridView2.Rows[i].Cells[9].Value);
                string org = Convert.ToString(dataGridView2.Rows[i].Cells[5].Value);
                string month1 = Convert.ToString(dataGridView2.Rows[i].Cells[4].Value);
                string month3 = Convert.ToString(dataGridView2.Rows[i].Cells[10].Value);
                string time_5 = Convert.ToString(dataGridView2.Rows[i].Cells[11].Value);


                if ((a != "") && (b != "") && (c != "") && (d != "") && (f != "") && (g != "") && (h != "") && (s != "") && (month1 != "") && (month3 != "") && (time_5 != "") && (org != ""))
                {

                    string[] month2 = month1.Split('.');

                    if (month2[1] == "01") { month2[1] = "января"; }
                    if (month2[1] == "02") { month2[1] = "февраля"; }
                    if (month2[1] == "03") { month2[1] = "марта"; }
                    if (month2[1] == "04") { month2[1] = "апреля"; }
                    if (month2[1] == "05") { month2[1] = "мая"; }
                    if (month2[1] == "06") { month2[1] = "июня"; }
                    if (month2[1] == "07") { month2[1] = "июля"; }
                    if (month2[1] == "08") { month2[1] = "июня"; }
                    if (month2[1] == "09") { month2[1] = "августа"; }
                    if (month2[1] == "10") { month2[1] = "сентября"; }
                    if (month2[1] == "11") { month2[1] = "октября"; }
                    if (month2[1] == "12") { month2[1] = "ноября"; }



                    string[] month_4 = month3.Split('.');

                    if (month_4[1] == "01") { month_4[1] = "января"; }
                    if (month_4[1] == "02") { month_4[1] = "февраля"; }
                    if (month_4[1] == "03") { month_4[1] = "марта"; }
                    if (month_4[1] == "04") { month_4[1] = "апреля"; }
                    if (month_4[1] == "05") { month_4[1] = "мая"; }
                    if (month_4[1] == "06") { month_4[1] = "июня"; }
                    if (month_4[1] == "07") { month_4[1] = "июля"; }
                    if (month_4[1] == "08") { month_4[1] = "июня"; }
                    if (month_4[1] == "09") { month_4[1] = "августа"; }
                    if (month_4[1] == "10") { month_4[1] = "сентября"; }
                    if (month_4[1] == "11") { month_4[1] = "октября"; }
                    if (month_4[1] == "12") { month_4[1] = "ноября"; }



                    string[] month_6 = time_5.Split('.');

                    if (month_6[1] == "01") { month_6[1] = "января"; }
                    if (month_6[1] == "02") { month_6[1] = "февраля"; }
                    if (month_6[1] == "03") { month_6[1] = "марта"; }
                    if (month_6[1] == "04") { month_6[1] = "апреля"; }
                    if (month_6[1] == "05") { month_6[1] = "мая"; }
                    if (month_6[1] == "06") { month_6[1] = "июня"; }
                    if (month_6[1] == "07") { month_6[1] = "июля"; }
                    if (month_6[1] == "08") { month_6[1] = "июня"; }
                    if (month_6[1] == "09") { month_6[1] = "августа"; }
                    if (month_6[1] == "10") { month_6[1] = "сентября"; }
                    if (month_6[1] == "11") { month_6[1] = "октября"; }
                    if (month_6[1] == "12") { month_6[1] = "ноября"; }

                    surname_editBox.Text = Convert.ToString(dataGridView2.Rows[i].Cells[1].Value);
                    name_editBox.Text = Convert.ToString(dataGridView2.Rows[i].Cells[2].Value);
                    patromic_editBox.Text = Convert.ToString(dataGridView2.Rows[i].Cells[3].Value);
                    dapartament_editBox.Text = Convert.ToString(dataGridView2.Rows[i].Cells[6].Value);
                    position_editBox.Text = Convert.ToString(dataGridView2.Rows[i].Cells[7].Value);
                    number_permit_editBox.Text = Convert.ToString(dataGridView2.Rows[i].Cells[0].Value);
                    number_editBox.Text = Convert.ToString(dataGridView2.Rows[i].Cells[8].Value);
                    type_editBox.Text = Convert.ToString(dataGridView2.Rows[i].Cells[9].Value);
                    phone_editBox.Text = Convert.ToString(dataGridView2.Rows[i].Cells[5].Value);
                    day1_editBox.Text = month2[0];
                    month1_editBox.Text = month2[1];
                    year1_editBox.Text = month2[2];
                    day2_editBox.Text = month_4[0];
                    month2_editBox.Text = month_4[1];
                    year2_editBox.Text = month_4[2];
                    day3_editBox.Text = month_6[0];
                    month3_editBox.Text = month_6[1];
                    year3_editBox.Text = month_6[2];
                }
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {
            //сохранить изменения
            //проверяем все ли данные заполнены
            if (surname_editBox.Text == "" || name_editBox.Text == "" || patromic_editBox.Text == "" || number_permit_editBox.Text == "" || dapartament_editBox.Text == "" || type_editBox.Text == "")
            {
                MessageBox.Show("Заполните все пустые поля для сохранения!");
            }




            string fam = surname_editBox.Text;
            for (int i = 0; i < fam.Length; i++)
            {
                if (fam[i] >= '0' && fam[i] <= '9')
                {
                    MessageBox.Show("Фамилия не может состоять из цифр!");
                    surname_editBox.Clear();
                    break;
                }
            }


            string name = name_editBox.Text;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] >= '0' && name[i] <= '9')
                {
                    MessageBox.Show("Имя не может состоять из цифр!");
                    name_editBox.Clear();
                    break;
                }
            }


            string fname = patromic_editBox.Text;
            for (int i = 0; i < fname.Length; i++)
            {
                if (fname[i] >= '0' && fname[i] <= '9')
                {
                    MessageBox.Show("Отчество не может состоять из цифр!");
                    patromic_editBox.Clear();
                    break;
                }
            }



            if (number_editBox.Text != "")
            {

                try
                {
                    Convert.ToInt64(number_editBox.Text);
                }

                catch (FormatException)
                {
                    MessageBox.Show("Символы в табельном номере должны быть цифрами!");
                    number_editBox.Clear();

                }

            }


            if (surname_editBox.Text != "" && name_editBox.Text != "" && patromic_editBox.Text != "" && day1_editBox.Text != "" && year1_editBox.Text != "" && month1_editBox.Text != "" && day2_editBox.Text != "" && year2_editBox.Text != "" && month2_editBox.Text != "" && day3_editBox.Text != "" && year3_editBox.Text != "" && month3_editBox.Text != "" && dapartament_editBox.Text != "" && type_editBox.Text == "постоянный" || type_editBox.Text == "временный" || type_editBox.Text == "ПОСТОЯННЫЙ" || type_editBox.Text == "ВРЕМЕННЫЙ")
            {


                var result = new System.Windows.Forms.DialogResult();
                result = MessageBox.Show("Сохранить изменения?", "Сохранить",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {



                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {

                        dataGridView1.Rows[i].Selected = false;
                        for (int j = 0; j < dataGridView1.ColumnCount; j++)
                            if (dataGridView1.Rows[i].Cells[j].Value != null)
                                if (dataGridView1.Rows[i].Cells[j].Value.ToString().Contains(number_editBox.Text))
                                {
                                    dataGridView1.Rows[i].Selected = true;
                                    break;
                                }
                    }



                    int index = dataGridView1.SelectedCells[0].RowIndex;
                    try
                    {
                        dataGridView1.Rows.RemoveAt(index);

                    }
                    catch (InvalidOperationException)
                    { }

                    string month1 = month1_editBox.Text;
                    if (month1 == "января") { month1 = "01"; }
                    if (month1 == "февраля") { month1 = "02"; }
                    if (month1 == "марта") { month1 = "03"; }
                    if (month1 == "апреля") { month1 = "04"; }
                    if (month1 == "мая") { month1 = "05"; }
                    if (month1 == "июня") { month1 = "06"; }
                    if (month1 == "июля") { month1 = "07"; }
                    if (month1 == "августа") { month1 = "08"; }
                    if (month1 == "сентября") { month1 = "09"; }
                    if (month1 == "октября") { month1 = "10"; }
                    if (month1 == "ноября") { month1 = "11"; }
                    if (month1 == "декабря") { month1 = "12"; }

                    string month2 = month2_editBox.Text;
                    if (month2 == "января") { month2 = "01"; }
                    if (month2 == "февраля") { month2 = "02"; }
                    if (month2 == "марта") { month2 = "03"; }
                    if (month2 == "апреля") { month2 = "04"; }
                    if (month2 == "мая") { month2 = "05"; }
                    if (month2 == "июня") { month2 = "06"; }
                    if (month2 == "июля") { month2 = "07"; }
                    if (month2 == "августа") { month2 = "08"; }
                    if (month2 == "сентября") { month2 = "09"; }
                    if (month2 == "октября") { month2 = "10"; }
                    if (month2 == "ноября") { month2 = "11"; }
                    if (month2 == "декабря") { month2 = "12"; }

                    string month3 = month3_editBox.Text;
                    if (month3 == "января") { month3 = "01"; }
                    if (month3 == "февраля") { month3 = "02"; }
                    if (month3 == "марта") { month3 = "03"; }
                    if (month3 == "апреля") { month3 = "04"; }
                    if (month3 == "мая") { month3 = "05"; }
                    if (month3 == "июня") { month3 = "06"; }
                    if (month3 == "июля") { month3 = "07"; }
                    if (month3 == "августа") { month3 = "08"; }
                    if (month3 == "сентября") { month3 = "09"; }
                    if (month3 == "октября") { month3 = "10"; }
                    if (month3 == "ноября") { month3 = "11"; }
                    if (month3 == "декабря") { month3 = "12"; }

                    string number_add = number_permit_editBox.Text;
                    string number_add2 = number_editBox.Text;
                    bool repet = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        if ((Convert.ToString(dataGridView1.Rows[i].Cells[0].Value) == number_add) || (Convert.ToString(dataGridView1.Rows[i].Cells[8].Value) == number_add2))
                        {
                            repet = true;
                            break;
                        }
                    if (repet != true)
                    {
                        dataGridView1.Rows.Add(number_permit_editBox.Text, surname_editBox.Text.ToUpper(), name_editBox.Text.ToUpper(), patromic_editBox.Text.ToUpper(), day1_editBox.Text + "." + month1 + "." + year1_editBox.Text, phone_editBox.Text, dapartament_editBox.Text.ToUpper(), position_editBox.Text.ToUpper(), number_editBox.Text, type_editBox.Text.ToUpper(), day2_editBox.Text + "." + month2 + "." + year2_editBox.Text, day3_editBox.Text + "." + month3 + "." + year3_editBox.Text, "0");
                    }
                }


            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //очистить поля
            surname_editBox.Clear();
            name_editBox.Clear();
            patromic_editBox.Clear();
            number_permit_editBox.Clear();
            number_editBox.Clear();
            day1_editBox.Text = "01";
            month1_editBox.Text = "января";
            year1_editBox.Text = "2018";
            phone_editBox.Text = "";
            position_editBox.Text = "(нет)";
            dapartament_editBox.Text = "(нет)";
            type_editBox.Text = "постоянный";
            day2_editBox.Text = "01";
            year2_editBox.Text = "2018";
            month2_editBox.Text = "января";
            day3_editBox.Text = "01";
            year3_editBox.Text = "2018";
            month3_editBox.Text = "января";
        }



        /*ДОБАВИТЬ*/
        private void button8_Click(object sender, EventArgs e)
        {
            //проверяем все ли данные заполнены
            if (surnameBox.Text == "" || nameBox.Text == "" || patronymicBox.Text == "" || number_permitBox.Text == "" || departmentBox.Text == "" || typeBox.Text == "")
            {
                MessageBox.Show("Заполните все пустые поля для добавления!");
            }



            string fam = surnameBox.Text;
            for (int i = 0; i < fam.Length; i++)
            {
                if (fam[i] >= '0' && fam[i] <= '9')
                {
                    MessageBox.Show("Фамилия не может состоять из цифр!");
                    surnameBox.Clear();
                    break;
                }
            }


            string name = nameBox.Text;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] >= '0' && name[i] <= '9')
                {
                    MessageBox.Show("Имя не может состоять из цифр!");
                    nameBox.Clear();
                    break;
                }
            }


            string fname = patronymicBox.Text;
            for (int i = 0; i < fname.Length; i++)
            {
                if (fname[i] >= '0' && fname[i] <= '9')
                {
                    MessageBox.Show("Отчество не может состоять из цифр!");
                    patronymicBox.Clear();
                    break;
                }
            }



            if (numberBox.Text != "" || number_permitBox.Text != "")
            {
                try
                {
                    Convert.ToInt64(numberBox.Text);
                }

                catch (FormatException)
                {
                    MessageBox.Show("Символы в номере пропуска должны быть цифрами!");
                    numberBox.Clear();

                }

                try
                {
                    Convert.ToInt64(number_permitBox.Text);
                }

                catch (FormatException)
                {
                    MessageBox.Show("Символы в табельном номере должны быть цифрами!");
                    number_permitBox.Clear();

                }

            }





            if (surnameBox.Text != "" && nameBox.Text != "" && patronymicBox.Text != "" && day1Box.Text != "" && year1Box.Text != "" && month1Box.Text != "" && day2Box.Text != "" && year2Box.Text != "" && month2Box.Text != "" && day3Box.Text != "" && year3Box.Text != "" && month3Box.Text != "" && departmentBox.Text != "" && typeBox.Text == "постоянный" || typeBox.Text == "временный" || typeBox.Text == "ПОСТОЯННЫЙ" || typeBox.Text == "ВРЕМЕННЫЙ")
            {

                string month1 = month1Box.Text;
                if (month1 == "января") { month1 = "01"; }
                if (month1 == "февраля") { month1 = "02"; }
                if (month1 == "марта") { month1 = "03"; }
                if (month1 == "апреля") { month1 = "04"; }
                if (month1 == "мая") { month1 = "05"; }
                if (month1 == "июня") { month1 = "06"; }
                if (month1 == "июля") { month1 = "07"; }
                if (month1 == "августа") { month1 = "08"; }
                if (month1 == "сентября") { month1 = "09"; }
                if (month1 == "октября") { month1 = "10"; }
                if (month1 == "ноября") { month1 = "11"; }
                if (month1 == "декабря") { month1 = "12"; }

                string month2 = month2Box.Text;
                if (month2 == "января") { month2 = "01"; }
                if (month2 == "февраля") { month2 = "02"; }
                if (month2 == "марта") { month2 = "03"; }
                if (month2 == "апреля") { month2 = "04"; }
                if (month2 == "мая") { month2 = "05"; }
                if (month2 == "июня") { month2 = "06"; }
                if (month2 == "июля") { month2 = "07"; }
                if (month2 == "августа") { month2 = "08"; }
                if (month2 == "сентября") { month2 = "09"; }
                if (month2 == "октября") { month2 = "10"; }
                if (month2 == "ноября") { month2 = "11"; }
                if (month2 == "декабря") { month2 = "12"; }

                string month3 = month3Box.Text;
                if (month3 == "января") { month3 = "01"; }
                if (month3 == "февраля") { month3 = "02"; }
                if (month3 == "марта") { month3 = "03"; }
                if (month3 == "апреля") { month3 = "04"; }
                if (month3 == "мая") { month3 = "05"; }
                if (month3 == "июня") { month3 = "06"; }
                if (month3 == "июля") { month3 = "07"; }
                if (month3 == "августа") { month3 = "08"; }
                if (month3 == "сентября") { month3 = "09"; }
                if (month3 == "октября") { month3 = "10"; }
                if (month3 == "ноября") { month3 = "11"; }
                if (month3 == "декабря") { month3 = "12"; }

                string number_add = number_permitBox.Text;
                string number_add2 = numberBox.Text;
                bool repet = false;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    if ((Convert.ToString(dataGridView1.Rows[i].Cells[8].Value) == number_add2) || (Convert.ToString(dataGridView1.Rows[i].Cells[0].Value) == number_add))
                    {
                        repet = true;
                        break;
                    }
                if (repet != true)
                {
                    dataGridView1.Rows.Add(number_permitBox.Text, surnameBox.Text.ToUpper(), nameBox.Text.ToUpper(), patronymicBox.Text.ToUpper(), day1Box.Text + "." + month1 + "." + year1Box.Text, phoneBox.Text, departmentBox.Text.ToUpper(), positionBox.Text.ToUpper(), numberBox.Text, typeBox.Text.ToUpper(), day2Box.Text + "." + month2 + "." + year2Box.Text, day3Box.Text + "." + month3 + "." + year3Box.Text, "0");
                    int all = Int32.Parse(label22.Text) + 1;
                    label22.Text = all.ToString();
                }

                else MessageBox.Show("Пропуск с данным номером уже существует!");
            }
        }

        private void textBox26_Leave(object sender, EventArgs e)
        {
            if (day1_editBox.Text == "")
            {
                day1_editBox.Text = "01";
            }
        }

        private void textBox34_Leave(object sender, EventArgs e)
        {
            if (day2_editBox.Text == "")
            {
                day2_editBox.Text = "01";
            }
        }

        private void textBox33_Leave(object sender, EventArgs e)
        {
            if (day3_editBox.Text == "")
            {
                day3_editBox.Text = "01";
            }
        }

        private void textBox27_Leave(object sender, EventArgs e)
        {
            if (year1_editBox.Text == "")
            {
                year1_editBox.Text = "2018";
            }
        }

        private void textBox36_Leave(object sender, EventArgs e)
        {
            if (year2_editBox.Text == "")
            {
                year2_editBox.Text = "2018";
            }
        }

        private void textBox35_Leave(object sender, EventArgs e)
        {
            if (year3_editBox.Text == "")
            {
                year3_editBox.Text = "2018";
            }
        }

        private void month1_editBox_Leave(object sender, EventArgs e)
        {
            if (month1_editBox.Text == "")
            {
                month1_editBox.Text = "января";
            }
        }

        private void month2_editBox_Leave(object sender, EventArgs e)
        {
            if (month2_editBox.Text == "")
            {
                month2_editBox.Text = "января";
            }
        }

        private void month3_editBox_Leave(object sender, EventArgs e)
        {
            if (month3_editBox.Text == "")
            {
                month3_editBox.Text = "января";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToString(dataGridView1.Rows[i].Cells[9].Value) == "ПОСТОЯННЫЙ")
                {
                    dataGridView2.Rows.Add(Convert.ToString(dataGridView1.Rows[i].Cells[0].Value), Convert.ToString(dataGridView1.Rows[i].Cells[1].Value), Convert.ToString(dataGridView1.Rows[i].Cells[2].Value), Convert.ToString(dataGridView1.Rows[i].Cells[3].Value), Convert.ToString(dataGridView1.Rows[i].Cells[4].Value), Convert.ToString(dataGridView1.Rows[i].Cells[5].Value), Convert.ToString(dataGridView1.Rows[i].Cells[6].Value), Convert.ToString(dataGridView1.Rows[i].Cells[7].Value), Convert.ToString(dataGridView1.Rows[i].Cells[8].Value), Convert.ToString(dataGridView1.Rows[i].Cells[9].Value), Convert.ToString(dataGridView1.Rows[i].Cells[10].Value), Convert.ToString(dataGridView1.Rows[i].Cells[11].Value), Convert.ToString(dataGridView1.Rows[i].Cells[12].Value));
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToString(dataGridView1.Rows[i].Cells[9].Value) == "ВРЕМЕННЫЙ")
                {
                    dataGridView2.Rows.Add(Convert.ToString(dataGridView1.Rows[i].Cells[0].Value), Convert.ToString(dataGridView1.Rows[i].Cells[1].Value), Convert.ToString(dataGridView1.Rows[i].Cells[2].Value), Convert.ToString(dataGridView1.Rows[i].Cells[3].Value), Convert.ToString(dataGridView1.Rows[i].Cells[4].Value), Convert.ToString(dataGridView1.Rows[i].Cells[5].Value), Convert.ToString(dataGridView1.Rows[i].Cells[6].Value), Convert.ToString(dataGridView1.Rows[i].Cells[7].Value), Convert.ToString(dataGridView1.Rows[i].Cells[8].Value), Convert.ToString(dataGridView1.Rows[i].Cells[9].Value), Convert.ToString(dataGridView1.Rows[i].Cells[10].Value), Convert.ToString(dataGridView1.Rows[i].Cells[11].Value), Convert.ToString(dataGridView1.Rows[i].Cells[12].Value));
                }

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToString(dataGridView1.Rows[i].Cells[12].Value) == "1")
                {
                    dataGridView2.Rows.Add(Convert.ToString(dataGridView1.Rows[i].Cells[0].Value), Convert.ToString(dataGridView1.Rows[i].Cells[1].Value), Convert.ToString(dataGridView1.Rows[i].Cells[2].Value), Convert.ToString(dataGridView1.Rows[i].Cells[3].Value), Convert.ToString(dataGridView1.Rows[i].Cells[4].Value), Convert.ToString(dataGridView1.Rows[i].Cells[5].Value), Convert.ToString(dataGridView1.Rows[i].Cells[6].Value), Convert.ToString(dataGridView1.Rows[i].Cells[7].Value), Convert.ToString(dataGridView1.Rows[i].Cells[8].Value), Convert.ToString(dataGridView1.Rows[i].Cells[9].Value), Convert.ToString(dataGridView1.Rows[i].Cells[10].Value), Convert.ToString(dataGridView1.Rows[i].Cells[11].Value), Convert.ToString(dataGridView1.Rows[i].Cells[12].Value));
                }

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //автоматическое открывание
            dataGridView1.Rows.Clear();
            StreamReader file_reader2 = new StreamReader("MyData.base", encod);

            string data;

            while (file_reader2.EndOfStream == false)
            {
                data = file_reader2.ReadToEnd();
                string[] line = data.Split('\n');


                for (int i = 0; i < line.Length; i++)
                {

                    label22.Text = i.ToString();

                    string[] cell = line[i].Split(';');
                    if (cell.Length == 13)
                    {
                        try
                        {




                            string test = Convert.ToString(cell[11]);
                            DateTime lastService = DateTime.Parse((string)test);
                            if ((DateTime.Now >= lastService.AddMonths(0)) && cell[9] == "ПОСТОЯННЫЙ")
                            { cell[12] = "1"; }
                            else { cell[12] = "0"; }

                            if (cell[9] == "ВРЕМЕННЫЙ")
                            {
                                cell[12] = "";

                            }


                            dataGridView1.Rows.Add(cell[0], cell[1], cell[2], cell[3], cell[4], cell[5], cell[6], cell[7], cell[8], cell[9], cell[10], test, cell[12]);


                        }

                        catch { }
                    }
                }
            }
            file_reader2.Close();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                for (int j = i + 1; j < dataGridView1.Rows.Count; j++)
                    if (Convert.ToString(dataGridView1.Rows[i].Cells[0].Value) == Convert.ToString(dataGridView1.Rows[j].Cells[0].Value))
                        dataGridView1.Rows.Remove(dataGridView1.Rows[j]);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var result = new System.Windows.Forms.DialogResult();
            result = MessageBox.Show("Сохранить изменения?", "Сохранить",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StreamWriter file_writer = new StreamWriter("MyData.base", false, encod);
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                    {
                        if (j != 12) file_writer.Write(dataGridView1.Rows[i].Cells[j].Value + ";", false, encod);
                        else
                            file_writer.Write(dataGridView1.Rows[i].Cells[j].Value + "", false, encod);
                    }
                    file_writer.WriteLine();
                }
                file_writer.Close();
                MessageBox.Show("Данные успешно сохранены!");
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if (!File.Exists("MyNewFile.base"))
                File.Create("MyNewFile.base").Close();

            dataGridView1.Rows.Clear();
            label22.Text = "1";


            dataGridView1.Rows.Add("000", "Иванов", "Иван", "Иванович", "00.00.0000", "(000) 000-0000", "(нет)", "(нет)", "0000000", "ПОСТОЯННЫЙ", "00.00.0000", "00.00.0000", "0");
            int all = Int32.Parse(label22.Text) + 1;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int temp;
            temp = rand.Next(100, 1000);

            number_permitBox.Text = temp.ToString();
        }

    
        
    }
}
