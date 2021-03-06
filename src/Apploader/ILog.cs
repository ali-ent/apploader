﻿/*
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

namespace Apploader
//namespace Taobao.Infrastructure
{
    /// <summary>
    /// 日志记录器
    /// 日志记录请总是使用此类
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 是否启用Debug级别
        /// </summary>
        bool IsDebugEnabled { get; }
        /// <summary>
        /// 是否启用Error级别
        /// </summary>
        bool IsErrorEnabled { get; }
        /// <summary>
        /// 是否启用Fatal级别
        /// </summary>
        bool IsFatalEnabled { get; }
        /// <summary>
        /// 是否启用Info级别
        /// </summary>
        bool IsInfoEnabled { get; }
        /// <summary>
        /// 是否启用Warn级别
        /// </summary>
        bool IsWarnEnabled { get; }

        #region Debug
        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="message">内容格式</param>
        void Debug(object message);
        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="format">内容格式</param>
        /// <param name="args">参数</param>
        void DebugFormat(string format, params object[] args);
        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="message">错误内容</param>
        /// <param name="exception">异常</param>
        void Debug(object message, Exception exception);
        #endregion

        #region Info
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="message">错误内容</param>
        void Info(object message);
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="format">内容格式</param>
        /// <param name="args">参数</param>
        void InfoFormat(string format, params object[] args);
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="message">错误内容</param>
        /// <param name="exception">异常</param>
        void Info(object message, Exception exception);
        #endregion

        #region Warn
        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="message">内容格式</param>
        void Warn(object message);
        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="format">内容格式</param>
        /// <param name="args">参数</param>
        void WarnFormat(string format, params object[] args);
        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="message">错误内容</param>
        /// <param name="exception">异常</param>
        void Warn(object message, Exception exception);
        #endregion

        #region Error
        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="message">内容格式</param>
        void Error(object message);
        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="format">内容格式</param>
        /// <param name="args">参数</param>
        void ErrorFormat(string format, params object[] args);
        /// <summary> 
        /// 记录错误
        /// </summary>
        /// <param name="message">错误内容</param>
        /// <param name="exception">异常</param>
        void Error(object message, Exception exception);
        #endregion

        #region Fatal
        /// <summary>
        /// 记录致命错误
        /// </summary>
        /// <param name="message">内容格式</param>
        void Fatal(object message);
        /// <summary>
        /// 记录致命错误
        /// </summary>
        /// <param name="format">内容格式</param>
        /// <param name="args">参数</param>
        void FatalFormat(string format, params object[] args);
        /// <summary> 
        /// 记录致命错误
        /// </summary>
        /// <param name="message">错误内容</param>
        /// <param name="exception">异常</param>
        void Fatal(object message, Exception exception);
        #endregion
    }
}
