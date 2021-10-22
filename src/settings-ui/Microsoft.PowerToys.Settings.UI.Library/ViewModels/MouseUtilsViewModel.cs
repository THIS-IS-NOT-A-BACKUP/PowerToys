﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.PowerToys.Settings.UI.Library.Helpers;
using Microsoft.PowerToys.Settings.UI.Library.Interfaces;

namespace Microsoft.PowerToys.Settings.UI.Library.ViewModels
{
    public class MouseUtilsViewModel : Observable
    {
        private GeneralSettings GeneralSettingsConfig { get; set; }

        public MouseUtilsViewModel(ISettingsRepository<GeneralSettings> settingsRepository, Func<string, int> ipcMSGCallBackFunc)
        {
            // To obtain the general settings configurations of PowerToys Settings.
            if (settingsRepository == null)
            {
                throw new ArgumentNullException(nameof(settingsRepository));
            }

            GeneralSettingsConfig = settingsRepository.SettingsConfig;

            _isFindMyMouseEnabled = GeneralSettingsConfig.Enabled.FindMyMouse;

            // set the callback functions value to hangle outgoing IPC message.
            SendConfigMSG = ipcMSGCallBackFunc;
        }

        public bool IsFindMyMouseEnabled
        {
            get => _isFindMyMouseEnabled;
            set
            {
                if (_isFindMyMouseEnabled != value)
                {
                    _isFindMyMouseEnabled = value;

                    GeneralSettingsConfig.Enabled.FindMyMouse = value;
                    OnPropertyChanged(nameof(IsFindMyMouseEnabled));

                    OutGoingGeneralSettings outgoing = new OutGoingGeneralSettings(GeneralSettingsConfig);
                    SendConfigMSG(outgoing.ToString());

                    // TODO: Implement when this module has properties.
                    // NotifyPropertyChanged();
                }
            }
        }

        private Func<string, int> SendConfigMSG { get; }

        private bool _isFindMyMouseEnabled;
    }
}
