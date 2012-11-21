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
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Threading;

using Taobao.Infrastructure.Toolkit.AppDomains;
using Taobao.Infrastructure;
using Taobao.Infrastructure.AppAgents;

namespace Apploader.Console
{
    public class Program
    {
        static AppDomainLoader _loader;
        static ILog _log;
        static FileStream _file;
        static bool _single;
        static void Main(string[] args)
        {
            WriteTip("服务动态发布宿主启动");
            WriteTip("请求启动锁定");
            TryKeekAlive();

            //配置初始化
            //为支持多进程特性 by wsky 2012-11-21
            if (args != null && args.Length > 0 && (Program._single = true))
                Start(args[0], false);
            else
                Start();

            //简单cmd处理
            while (true)
            {
                if (HandleCommand()) break;
                Thread.Sleep(100);
            }

            WriteTip("卸载所有应用", true);
            _loader.Clear();
            WriteTip("服务宿主退出", true);
            Thread.Sleep(5000);
        }
        private static bool IsCommand(string cmd)
        {
            return System.Console.ReadLine().Equals(cmd, StringComparison.InvariantCultureIgnoreCase);
        }
        private static void WriteTip(string tip)
        {
            WriteTip(tip, false);
        }
        private static void WriteTip(string tip, bool log)
        {
            var info = string.Format("========={0}========", tip);
            if (log && _log != null)
                _log.Info(info);
            else
                System.Console.WriteLine(info);
        }
        private static void TryKeekAlive()
        {
            while (true)
            {
                try
                {
                    _file = new FileStream("KeekAlive", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    break;
                }
                catch
                {
                    Thread.Sleep(10000);
                }
            }
        }
        private static void PrepareErrorHandle()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (_log == null)
                    System.Console.WriteLine((e.ExceptionObject as Exception).Message);
                else if (e.IsTerminating)
                    _log.Fatal("-->发生严重错误|IsTerminating="
                        + e.IsTerminating, e.ExceptionObject as Exception);
                else
                    _log.Error("-->发生意外错误|IsTerminating="
                        + e.IsTerminating, e.ExceptionObject as Exception);
            };
        }
        private static bool HandleCommand()
        {
            if (IsCommand("exit"))
                return true;
            else if (IsCommand("refresh"))
            {
                _loader.Clear();
                _loader.Scan();
            }
            else if (IsCommand("clear"))
            {
                System.Console.Clear();
                WriteTip("服务动态发布宿主");
            }
            return false;
        }

        //保持原有行为，非多进程支持
        public static void Start()
        {
            Program.Start(ConfigurationManager.AppSettings["serviceRoot"], true);
        }
        public static void Start(string root, bool auto)
        {
            PrepareErrorHandle();

            Directory.CreateDirectory(root);
            //配置初始化
            log4net.Config.XmlConfigurator.Configure();
            //特化的log
            var log = new Log4NetLogger(log4net.LogManager.GetLogger(typeof(Program)));
            Program._log = new AgentHandlerLog(log);

            _loader = new AppDomainLoader(root, Program._log, auto);
            WriteTip("开始扫描发布目录：" + root, true);
            if (!Program._single)
                _loader.Scan();
            else
                _loader.LoadFrom(root);
            WriteTip("扫描完毕", true);

            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["appAgent_enable"])) return;

            //激活AppAgent
            WriteTip("启用AppAgent", true);
            new DefaultAgent(log
                , ConfigurationManager.AppSettings["appAgent_master"]
                , ConfigurationManager.AppSettings["appAgent_name"]
                , ConfigurationManager.AppSettings["appAgent_description"]
                , new CommandHandle())
                .Run();
        }
        public static void Stop()
        {
            _loader.Clear();
        }
        //log类型适配并使其支持appagent输出
        class AgentHandlerLog : DefaultAgentHandlerLog, Apploader.ILog
        {
            public AgentHandlerLog(Taobao.Infrastructure.ILog log) : base(log) { }
        }
        //apploader管理命令实现
        class CommandHandle : IMessageHandle
        {
            private static readonly StringComparison _comparison = StringComparison.InvariantCultureIgnoreCase;
            private static readonly string _description = "请提交有效的AppLoader管理命令，如：list|scan|refresh|reload [app]|unload [app]";

            #region IMessageHandle Members
            public void Handle(string msg, StreamWriter writer)
            {
                var args = (msg ?? "").Split(' ');

                try
                {
                    if (string.IsNullOrEmpty(args[0]))
                        writer.WriteLine(_description);
                    else if (args[0].Equals("list", _comparison))
                        this.List(writer, args);
                    else if (args[0].Equals("scan", _comparison))
                        this.Scan(writer, args);
                    else if (args[0].Equals("refresh", _comparison))
                        this.Refresh(writer, args);
                    else if (args[0].Equals("reload", _comparison))
                        this.Reload(writer, args);
                    else if (args[0].Equals("unload", _comparison))
                        this.Unload(writer, args);
                }
                catch (Exception e)
                {
                    writer.WriteLine(e.Message);
                }
                finally
                {
                    writer.Flush();
                }
            }
            #endregion

            private void List(StreamWriter writer, params string[] args)
            {
                _loader.GetAppPaths().ToList().ForEach(o => { writer.WriteLine(o); writer.Flush(); });
            }
            private void Scan(StreamWriter writer, params string[] args)
            {
                if (Program._single) return;
                _loader.Scan();
            }
            private void Refresh(StreamWriter writer, params string[] args)
            {
                if (Program._single) return;
                _loader.Clear();
                _loader.Scan();
            }
            private void Reload(StreamWriter writer, params string[] args)
            {
                var app = args != null && args.Length > 1 ? args[1] : null;
                if (string.IsNullOrEmpty(app))
                {
                    writer.WriteLine("请指定app目录");
                    writer.Flush();
                    return;
                }
                _loader.Reload(app);
            }
            private void Unload(StreamWriter writer, params string[] args)
            {
                var app = args != null && args.Length > 1 ? args[1] : null;
                if (string.IsNullOrEmpty(app))
                {
                    writer.WriteLine("请指定app目录");
                    writer.Flush();
                    return;
                }
                _loader.Unload(app);
            }
        }
    }
}