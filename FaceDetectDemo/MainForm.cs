using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceDetectDemo
{
	public partial class MainForm : Form
	{
		private CaptureFaceForm captureFaceForm;
		public MainForm()
		{
			InitializeComponent();
		}

		private void btnCaptureFace_Click(object sender, EventArgs e)
		{
			if (captureFaceForm == null||captureFaceForm.IsDisposed)
			{
				captureFaceForm = new CaptureFaceForm();
			}
			captureFaceForm.TopMost = true;
			captureFaceForm.Show();
		}
	}
}
