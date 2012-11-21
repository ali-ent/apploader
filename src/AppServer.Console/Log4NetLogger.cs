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

namespace AppServer.Console
{
    public class Log4NetLogger : Taobao.Infrastructure.ILog
    {
        protected log4net.ILog _log;
        public Log4NetLogger(log4net.ILog log)
        {
            this._log = log;
        }

        #region ILog Members
        public virtual bool IsDebugEnabled
        {
            get { return this._log.IsDebugEnabled; }
        }
        public virtual bool IsErrorEnabled
        {
            get { return this._log.IsErrorEnabled; }
        }
        public virtual bool IsFatalEnabled
        {
            get { return this._log.IsFatalEnabled; }
        }
        public virtual bool IsInfoEnabled
        {
            get { return this._log.IsInfoEnabled; }
        }
        public virtual bool IsWarnEnabled
        {
            get { return this._log.IsWarnEnabled; }
        }

        public virtual void Debug(object message)
        {
            this._log.Debug(message);
        }
        public virtual void DebugFormat(string format, params object[] args)
        {
            this._log.DebugFormat(format, args);
        }
        public virtual void Debug(object message, Exception exception)
        {
            this._log.Debug(message, exception);
        }

        public virtual void Info(object message)
        {
            this._log.Info(message);
        }
        public virtual void InfoFormat(string format, params object[] args)
        {
            this._log.InfoFormat(format, args);
        }
        public virtual void Info(object message, Exception exception)
        {
            this._log.Info(message, exception);
        }

        public virtual void Warn(object message)
        {
            this._log.Warn(message);
        }
        public virtual void WarnFormat(string format, params object[] args)
        {
            this._log.WarnFormat(format, args);
        }
        public virtual void Warn(object message, Exception exception)
        {
            this._log.Warn(message, exception);
        }

        public virtual void Error(object message)
        {
            this._log.Error(message);
        }
        public virtual void ErrorFormat(string format, params object[] args)
        {
            this._log.ErrorFormat(format, args);
        }
        public virtual void Error(object message, Exception exception)
        {
            this._log.Error(message, exception);
        }

        public virtual void Fatal(object message)
        {
            this._log.Fatal(message);
        }
        public virtual void FatalFormat(string format, params object[] args)
        {
            this._log.FatalFormat(format, args);
        }
        public virtual void Fatal(object message, Exception exception)
        {
            this._log.Fatal(message, exception);
        }
        #endregion
    }
}