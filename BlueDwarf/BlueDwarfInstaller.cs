using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BlueDwarf
{
    [RunInstaller(true)]
    public partial class BlueDwarfInstaller : Installer
    {
        public BlueDwarfInstaller()
        {
            InitializeComponent();
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            Process.Start(Assembly.GetExecutingAssembly().Location);
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            var currentProcess = Process.GetCurrentProcess();
            foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName).Where(p => p.Id != currentProcess.Id))
                process.Kill();
        }
    }
}
