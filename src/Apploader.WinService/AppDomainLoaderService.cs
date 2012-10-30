using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Apploader.WinService
{
    public partial class AppDomainLoaderService : ServiceBase
    {
        public AppDomainLoaderService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Apploader.Console.Program.Start();
        }

        protected override void OnStop()
        {
            Apploader.Console.Program.Stop();
        }
    }
}
