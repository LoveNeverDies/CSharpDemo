using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSharpDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void DownloadSmallSpeak_Click(object sender, EventArgs e)
        {
            Down<SmallSpeakPUA>();
        }

        public void Down<T>() where T : BaseSmallSpeak, new()
        {
            var helper = new T();
            var html = helper.HttpGet(string.Empty);
            helper.HandleHtml(html);
        }
    }
}
