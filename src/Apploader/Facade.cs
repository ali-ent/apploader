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
using System.Reflection;
using System.Configuration;

namespace Taobao.Infrastructure.Toolkit.AppDomains
{
    /// <summary>
    /// 运行于独立domain中
    /// </summary>
    public class Facade : MarshalByRefObject
    {
        /// <summary>
        /// 从文件中加载程序集
        /// </summary>
        /// <param name="path"></param>
        /// <returns>若含有入口则返回入口类型全名</returns>
        public string LoadAssemblyFromFile(string path)
        {
            var entranceTypeName = ConfigurationManager.AppSettings["AppDomainLoaderEntrance"];
            //加载到appdomain
            var assembly = Assembly.LoadFrom(path);//LoadFile会导致锁定

            if (string.IsNullOrEmpty(entranceTypeName))
            {
                var entrance = assembly.GetType("Entrance", false);
                return entrance == null ? string.Empty : entrance.FullName;
            }
            else
            {
                var entrance = Type.GetType(entranceTypeName, false);
                return entrance == null || entrance.Assembly.FullName != assembly.FullName
                    ? string.Empty
                    : entrance.FullName;
            }
        }
    }
}