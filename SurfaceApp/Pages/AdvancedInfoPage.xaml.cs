﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AdvancedInfo.Handlers;
using Windows.Storage;
using System.Linq;
using Windows.UI.Xaml.Input;
using Windows.System;

#nullable enable

namespace SurfaceApp
{
    public sealed partial class AdvancedInfoPage : Page
    {
        private RegistryHandler? reghandler;
        private ModemHandler? modemhandler;
        private InternalHandler? internalhandler;

        public AdvancedInfoPage()
        {
            this.InitializeComponent();

            RetrieveData();
        }

        private async void RetrieveData()
        {
            try
            {
                internalhandler = new InternalHandler();
                RAM.Text = internalhandler.RAM;
                Build.Text = internalhandler.FirmwareBuild;
            }
            catch
            {

            }

            try
            {
                modemhandler = await ModemHandler.LoadHandlerAsync();
                PhoneListView.ItemsSource = modemhandler.ModemInformation;
            }
            catch
            {

            }

            try
            {
                reghandler = new RegistryHandler();

                if (reghandler.ReleaseName != null)
                {
                    VersionNameText.Text = VersionNameText.Text.Replace("XXXX", reghandler.ReleaseName);
                }
            }
            catch
            {

            }

            StorageFolder local = ApplicationData.Current.LocalFolder;
            System.Collections.Generic.IDictionary<string, object> retrivedProperties = await local.Properties.RetrievePropertiesAsync(new string[] { "System.FreeSpace" });
            ulong freeSpace = (UInt64)retrivedProperties["System.FreeSpace"];
            retrivedProperties = await local.Properties.RetrievePropertiesAsync(new string[] { "System.Capacity" });
            ulong totalSpace = (UInt64)retrivedProperties["System.Capacity"];

            ulong usedSpace = totalSpace - freeSpace;

            InternalStorageUsage.Maximum = totalSpace;
            InternalStorageUsage.Value = usedSpace;

            UsageDesc.Text = FormatBytes(freeSpace) + " free out of " + FormatBytes(totalSpace);
        }

        private static string FormatBytes(ulong bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        private async void ReleasePanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://woa-project.github.io/DuoWOA"));
        }
    }
}
