using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;

namespace Text_to_Speech
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SpeechRecognizer sr = new SpeechRecognizer();
        private void Form2_Load(object sender, EventArgs e)
        {
            sr.SpeechRecognized += sr_SpeechRecognized;
        }

        void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            richTextBox1.AppendText(e.Result.Text.ToString() + " ");
        }
    }
}
