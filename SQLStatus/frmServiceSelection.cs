using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace ServiceManager
{
    public partial class frmServiceSelection : Form
    {
        public frmServiceSelection()
        {
            InitializeComponent();
        }

        private void frmServiceSelection_Load(object sender, EventArgs e)
        {
            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController service in services)
            {
                lstSerives.Items.Add(service.ServiceName);
            }
        }

        /// <summary>
        /// Return selected service name
        /// </summary>
        /// <returns>Service Name</returns>
        public string returnServiceName()
        {
            return lstSerives.SelectedItem.ToString();
        }
    }
}