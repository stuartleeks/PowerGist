﻿using Microsoft.PowerShell.Host.ISE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GripDev.PowerGist.Addin.ISEInterop
{

    public class CloseItems
    {
        private ObjectModelRoot host;
        public CloseItems()
        {
            host = Config.ObjectModelRoot;
            if (host == null)
            {
                throw new ArgumentNullException("ObjectModelRoot not set in config");
            }
        }
        public void Invoke()
        {
            if (!ProceedSaveAndClose())
            {
                return;
            }

            SaveAndCloseFiles();
        }

        private void SaveAndCloseFiles()
        {
            var list = host.CurrentPowerShellTab.Files.ToArray();
            foreach (var f in list)
            {
				try
				{
					f.Save();
					host.CurrentPowerShellTab.Files.Remove(f);
				}
				catch (Exception ex)
				{
					//Temp hack when attempting to save files that aren't saved. 
					MessageBox.Show("Error when saving file: " + ex.ToString());
				}
            }
        }

        private static bool ProceedSaveAndClose()
        {
            return MessageBox.Show("Save & Close current files then open new gist?", "Overwrite?", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
    }
}
