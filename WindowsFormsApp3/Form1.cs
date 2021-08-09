using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Net;
using System.Web;
using System.IO;
using NAudio.Wave;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int ok = 0;
        

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = num_num.SelectionStart;
            if (i == 0 && e.KeyChar == 8) e.Handled = true;

            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else e.Handled = true;
            if (num_num.Text.Length > 11 && e.KeyChar != 8) e.Handled = true;
        }

        public void xuat()
        {
            long a;
            if (num_num.Text.Length == 0) a = 0;
            else a = Convert.ToInt64(num_num.Text);
            string res1 = readnumber_eng(a);
            string res2 = readnumber_vn(res1);
            if (ok == 0) textBox2.Text = res2;
            else textBox2.Text = res1;
        }

        private void num_num_TextChanged(object sender, EventArgs e)
        {
            xuat();

        }

        public static string readnumber_vn(string s)
        {
            string st, st1;
            string[] eng = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety", "hundred", "thousand", "million", "billion" };
            string[] viet = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín", "mười", "mười một", "mười hai", "mười ba", "mười bốn", "mười lăm", "mười sáu", "mười bảy", "mười tám", "mười chín", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi", "trăm", "ngàn", "triệu", "tỉ", };


            int ok = 0;
            st = ""; st1 = "";
            for (int i = 0; i < s.Length; ++i)
            {
                if ((s[i] == ' ') || (s[i] == '-') || (i == s.Length - 1))
                {
                    if (i == s.Length - 1) st1 = st1 + s[i];
                    for (int j = 0; j < 32; ++j)
                    {
                        if (st1 == eng[j])
                        {
                            string z = viet[j];
                            if (ok == 1)
                            {
                                if (z == "một") z = "mốt";
                                if (z == "năm") z = "lăm";
                            }
                            if (ok == 0 && i == s.Length - 1 && j <= 10 && s.LastIndexOf(" ") != -1) st = st + "lẻ" + " ";

                            st = st + z + " ";
                        }
                    }
                    st1 = "";
                    if (ok == 1) ok = 0;
                    if (s[i] == '-') ok = 1;

                }
                else st1 += s[i];
            }
            return st;
        }

        public static string readnumber_eng(long a)
        {
            string st, st1, st2;
            int x, y;
            st = NumberToWords(a);
            x = st.LastIndexOf(" ");
            y = st.Length;
            if (x > 0)
            {

                st1 = st.Substring(0, x);
                st2 = st.Substring(x + 1);
                return st1 + " and " + st2;
            }
            else return st;



        }

        public static string NumberToWords(long number)
        {

            if (number == 0)
                return "zero";

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Number:";
            label2.Text = "Lesson 1: How to read a 12-digit number";
            groupBox1.Text = "Language:";
            radioButton1.Text = "Vietnamese";
            radioButton2.Text = "English";
          
            ok = 1;
            xuat();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Nhập số:";
            label2.Text = "Bài số 1: Đọc số có 12 chữ số";
            groupBox1.Text = "Ngôn ngữ:";
            radioButton1.Text = "Tiếng Việt";
            radioButton2.Text = "Tiếng Anh";
          
            ok = 0;
            xuat();
        }

        public static void PlayMp3FromUrl(string url)
        {
            using (Stream ms = new MemoryStream())
            {
                using (Stream stream = WebRequest.Create(url)
                    .GetResponse().GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }

                ms.Position = 0;
                using (WaveStream blockAlignedStream =
                    new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(ms))))
                {
                    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(blockAlignedStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ans = textBox2.Text;
            if (radioButton1.Checked)
            ans = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl={1}&total=1&idx=0&textlen={2}&client=gtx",
                                                       HttpUtility.UrlEncode(ans),
                                                       "vi",
                                                       ans.Length);
            else if (radioButton2.Checked)
                ans = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl={1}&total=1&idx=0&textlen={2}&client=gtx",
                                                       HttpUtility.UrlEncode(ans),
                                                       "en",
                                                       ans.Length);
            PlayMp3FromUrl(ans);
        }
    } 
}
