using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.IO;

namespace Text_to_Speech
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer obj;
        SpeechRecognizer sRecognize = new SpeechRecognizer();
        public Form1()
        {
            InitializeComponent();
            sRecognize.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sRecognize_SpeechRecognized);
            obj = new SpeechSynthesizer();
            obj.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(obj_SpeakCompleted);
            btn_Pause.Enabled = false;
            foreach (InstalledVoice voice in obj.GetInstalledVoices())
            {
                cbVoice.Items.Add(voice.VoiceInfo.Name);            
            }
        }

        void sRecognize_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            rtb.AppendText(e.Result.Text.ToString() + " ");
        }

        void obj_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            btn_Speak.Enabled = true;
            btn_Pause.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            obj = new SpeechSynthesizer();
            btn_Pause.Enabled = false;
            btn_Resume.Enabled = false;
            btn_Stop.Enabled = false;
        }

        private void btn_Speak_Click(object sender, EventArgs e)
        {
            if (cbVoice.SelectedIndex >= 0)
            {
                obj.Dispose();
                if (rtb.Text != "")
                {
                    obj = new SpeechSynthesizer();
                    obj.SelectVoice(cbVoice.Text);
                    obj.SpeakAsync(rtb.Text);
                    btn_Pause.Enabled = true;
                    btn_Stop.Enabled = true;
                }
            }
            else 
            {
                MessageBox.Show("Pease select a voice","Text to Speech",
                                 MessageBoxButtons.OK,MessageBoxIcon.Warning);
                cbVoice.Focus();

            }
        }

        private void btn_Pause_Click(object sender, EventArgs e)
        {
            if (obj != null)
            {
                if (obj.State == SynthesizerState.Speaking)
                {
                    obj.Pause();
                    btn_Resume.Enabled = true;
                    btn_Speak.Enabled = false;
                }
            }
        }

        private void btn_Resume_Click(object sender, EventArgs e)
        {
            if (obj != null)
            { 
                if(obj.State == SynthesizerState.Paused)
                {
                    obj.Resume();
                    btn_Resume.Enabled = false;
                    btn_Speak.Enabled = true;
                }
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            if (obj != null)
            {
                obj.Dispose();
                btn_Speak.Enabled = true;
                btn_Resume.Enabled = false;
                btn_Pause.Enabled = false;
                btn_Stop.Enabled = false;
            }
        }

        private void btn_Download_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            if(browser.ShowDialog() == DialogResult.OK)
            {
                SpeechSynthesizer MySyn = new SpeechSynthesizer();
                MySyn.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(MySyn_SpeakCompleted);
                MySyn.SetOutputToWaveFile(string.Concat(browser.SelectedPath, "\\MyTTS.wav"));
                PromptBuilder builder = new PromptBuilder();
                builder.AppendText(rtb.Text);
                MySyn.SpeakAsync(builder);
            }
        }

        void MySyn_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            MessageBox.Show("Audio Downloaded successfully","Text to Speech"
                            ,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            rtb.Text = File.ReadAllText(openFileDialog1.FileName.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

    }
}
