using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace PortScannerNS
{
	/// <summary>
	/// Summary description for ServiceHandler.
	/// </summary>
	public class ServiceHandler : System.Windows.Forms.Form
	{
		
		public bool Finished=false;
		public string ServiceName=null;
		public System.ServiceProcess.ServiceController serviceController;

		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;

		public ServiceHandler()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(16, 8);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(224, 23);
			this.progressBar1.TabIndex = 0;
			// 
			// ServiceHandler
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(256, 45);
			this.ControlBox = false;
			this.Controls.Add(this.progressBar1);
			this.Name = "ServiceHandler";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ServiceHandler";
			this.Load += new System.EventHandler(this.ServiceHandler_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ServiceHandler_Load(object sender, System.EventArgs e)
		{
			progressBar1.Visible=true;
			progressBar1.Minimum=1;
			progressBar1.Maximum=10;
			progressBar1.Value=1;
			progressBar1.Step=1;
		}

		public void StartTimer()
		{
			timer1.Tick+= new EventHandler(checkOpFinished);
			timer1.Interval= 1;
			timer1.Start();
		}
		
		public void StartService()
		{
			if (ServiceName==null)
				throw new ServiceException();
			StartTimer();

			serviceController = new System.ServiceProcess.ServiceController(ServiceName);			
			
			if (serviceController.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
			{
				try 
				{
					// Start the service, and wait until its status is "Running".
					serviceController.Start();
					serviceController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running);
					Finished=true;
					// Display the current service status.
					MessageBox.Show("Mail Service is Started ...",
						"Service Operation Notification",
						MessageBoxButtons.OK,MessageBoxIcon.Information);
					this.DialogResult=DialogResult.OK;
				}
				catch (InvalidOperationException)
				{	
					Finished=true;
					MessageBox.Show("Unable to Start Mail Service ...",
						"Service Operation Notification",
						MessageBoxButtons.OK,MessageBoxIcon.Error);
					this.DialogResult=DialogResult.Cancel;
					this.Close();
					return;
				}

			}
			else
			{
				Finished=true;
				MessageBox.Show("Mail Service is alrerady Started ...",
					"Service Operation Notification",
					MessageBoxButtons.OK,MessageBoxIcon.Information);

				this.DialogResult=DialogResult.OK;
			}
			this.Close();
		}

		public void StopService()
		{
			if (ServiceName==null)
				throw new ServiceException();
			StartTimer();

			serviceController = new System.ServiceProcess.ServiceController(ServiceName);
			
			if (serviceController.Status == System.ServiceProcess.ServiceControllerStatus.Running)
			{
				try 
				{
					// Start the service, and wait until its status is "Running".
					serviceController.Stop();
					serviceController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped);
					Finished=true;
					// Display the current service status.
					MessageBox.Show("Mail Service is Stopped successfully...",
						"Service Operation Notification",
						MessageBoxButtons.OK,MessageBoxIcon.Information);
					this.DialogResult=DialogResult.OK;
					
				
				}
				catch (InvalidOperationException)
				{
					Finished=true;
					MessageBox.Show("Unable to Stop Mail Service ...",
						"Service Operation Notification",
						MessageBoxButtons.OK,MessageBoxIcon.Error);
					this.DialogResult=DialogResult.Cancel;
					this.Close();
					return;
				}

			}
			else
			{
				Finished=true;
				MessageBox.Show("Mail Service is alrerady Stopped ...",
					"Service Operation Notification",
					MessageBoxButtons.OK,MessageBoxIcon.Information);

				this.DialogResult=DialogResult.OK;
				
			}
			this.Close();
		}

		public void checkOpFinished(Object obj, System.EventArgs arg)
		{
			timer1.Stop();

			progressBar1.PerformStep();
			Invalidate();
			progressBar1.Update();
			
			if (Finished)
			{
				timer1.Dispose();				
				return;
			}
			timer1.Enabled=true;
		}
	}
}
