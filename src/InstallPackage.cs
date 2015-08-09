﻿using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace PackageInstaller
{
    internal sealed class InstallPackage
    {
        public const int CommandId = PackageCommands.InstallPackageId;
        public static readonly Guid CommandSet = GuidList.guidVSPackageCmdSet;
        private readonly Package package;
        private Project _project;

        private InstallPackage(Package package)
        {
            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
                menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }
        }

        private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            var button = (OleMenuCommand)sender;
            _project = ProjectHelpers.GetSelectedProject();

            button.Enabled = button.Visible = _project != null;
        }

        public static InstallPackage Instance { get; private set; }

        private IServiceProvider ServiceProvider
        {
            get { return package; }
        }

        public static void Initialize(Package package)
        {
            Instance = new InstallPackage(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            if (_project == null)
                return;

            InstallDialog dialog = new InstallDialog(new Bower(), new Npm());

            //IntPtr hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            //var helper = new WindowInteropHelper(dialog);
            //helper.Owner = hwnd;

            var result = dialog.ShowDialog();

            if (!dialog.DialogResult.HasValue || !dialog.DialogResult.Value)
                return;

            VSPackage._dte.StatusBar.Text = $"Installing {dialog.Package} package from {dialog.Provider.Name}...";

            dialog.Provider.InstallPackage(_project, dialog.Package, dialog.Version);
        }
    }
}
