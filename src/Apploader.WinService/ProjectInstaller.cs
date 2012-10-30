/*
    Copyright (c) Alibaba.  All rights reserved. - http://www.alibaba-inc.com/

	Licensed under the Apache License, Version 2.0 (the "License");

	you may not use this file except in compliance with the License.

	You may obtain a copy of the License at
 
		 http://www.apache.org/licenses/LICENSE-2.0
 
	Unless required by applicable law or agreed to in writing, 

	software distributed under the License is distributed on an "AS IS" BASIS, 

	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

	See the License for the specific language governing permissions and limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ServiceProcess;

namespace Apploader.WinService
{
    [RunInstaller(true)]
    public class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceInstaller _serviceInstaller;
        private ServiceProcessInstaller _processInstaller;

        public ProjectInstaller()
        {
            this._processInstaller = new ServiceProcessInstaller();
            this._processInstaller.Account = Statics.Service_Account;
            if (this._processInstaller.Account == ServiceAccount.User)
                this._processInstaller.Username = Statics.Service_Username;

            this._serviceInstaller = new ServiceInstaller();
            this._serviceInstaller.StartType = ServiceStartMode.Automatic;
            this._serviceInstaller.ServiceName = Statics.Service_Name;

            Installers.Add(this._serviceInstaller);
            Installers.Add(this._processInstaller);
        }
    }
}
