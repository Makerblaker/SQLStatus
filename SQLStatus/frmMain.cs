using System;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace ServiceManager
{
    public partial class frmMain : Form
    {
        private delegate void ChangeStatusCallBack(string text);

        public ServiceController Service = new ServiceController();

        public frmMain()
        {
            InitializeComponent();
            txtService.Text = "";

            // Disable all form buttons
            buttonUpdate(false);

            // Get a list of all services on the computer
            frmServiceSelection selectService = new frmServiceSelection();

            // If they select OK
            if (selectService.ShowDialog() == DialogResult.OK)
            {
                // Return selected service name
                Service.ServiceName = selectService.returnServiceName();
                txtService.Text = selectService.returnServiceName();
                lblStatus.Text = Service.Status.ToString();

                // Enable all form controlls
                buttonUpdate(true);
            }
        }

        /// <summary>
        /// Change the lblStatus label but check if its from another thread.
        /// </summary>
        /// <param name="text"></param>
        private void ChangeStatus(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblStatus.InvokeRequired)
            {
                ChangeStatusCallBack d = new ChangeStatusCallBack(ChangeStatus);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblStatus.Text = text;
            }
        }

        /// <summary>
        /// Exit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Begin the StartService thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            Thread StartSelectedService = new Thread(new ThreadStart(StartService));
            StartSelectedService.Start();
        }

        /// <summary>
        /// Begin the StopService thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            Thread StopSelectedService = new Thread(new ThreadStart(StopService));
            StopSelectedService.Start();
        }

        /// <summary>
        /// Start the SQL Service
        /// </summary>
        private void StartService()
        {
            ChangeStatus("Starting...");
            Service.Start();
            Service.WaitForStatus(ServiceControllerStatus.Running);
            ChangeStatus("Running");
        }

        /// <summary>
        /// Stop the SQL Service
        /// </summary>
        private void StopService()
        {
            ChangeStatus("Stopping...");
            Service.Stop();
            Service.WaitForStatus(ServiceControllerStatus.Stopped);
            ChangeStatus("Stopped");
        }

        /// <summary>
        /// Enable or disable all form controls.
        /// </summary>
        /// <param name="enabled">Form controls status</param>
        private void buttonUpdate(bool enabled)
        {
            foreach (Control con in this.Controls)
            {
                con.Enabled = enabled;
            }
        }
    }
}