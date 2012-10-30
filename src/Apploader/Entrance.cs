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

namespace Taobao.Infrastructure.Toolkit.AppDomains
{
    /// <summary>
    /// 声明AppDomain的加载入口
    /// </summary>
    public abstract class Entrance : MarshalByRefObject
    {
        /// <summary>
        /// AppDomain被加载后的入口方法
        /// </summary>
        /// <returns></returns>
        public abstract void Main();
        /// <summary>
        /// AppDomain的卸载方法
        /// </summary>
        public abstract void Unload();
    }
    //HACK:入口需要在新AppDomain中执行，若用接口需要使用者自行声明为MarshalByRefObject（remoting）
    interface IEntrance { void Main(); }
}