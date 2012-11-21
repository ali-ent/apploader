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
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Taobao.Infrastructure.AppAgents;

namespace AppServer.Console
{
    /// <summary>Multi-Process Apploader Host, like an app server.
    /// </summary>
    class Program
    {
        static log4net.ILog Log;
        static string Apploader;
        static string Root;
        static IDictionary<string, Process> Apps;

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Log = log4net.LogManager.GetLogger(typeof(Program));
            Root = ConfigurationManager.AppSettings["serviceRoot"];
            Apploader = ConfigurationManager.AppSettings["apploader"];
            Apps = new Dictionary<string, Process>();

            Log.Info("==== AppServer && Apploader ====");
            Log.InfoFormat("从目录{0}下开始启动各应用", Root);

            //TODO:支持指定pid，可通过读取对应目录下的xxxconfig来完成
            Directory.GetDirectories(Root)
                .Where(o => Filter(o)).ToList().ForEach(o => Start(o));

            //激活AppAgent
            Log.Info("启用AppAgent");
            new DefaultAgent(new Log4NetLogger(Log)
                , ConfigurationManager.AppSettings["appAgent_master"]
                , ConfigurationManager.AppSettings["appAgent_name"]
                , ConfigurationManager.AppSettings["appAgent_description"]
                , new CommandHandle())
                .Run();

            Log.Info("==== 启动完成 ====\n\n");

            RenderOutput();

            System.Console.ReadKey();
        }
        static bool Filter(string dir)
        {
            //忽略shadowcopy，这是兼容逻辑
            return !dir.Equals(Path.Combine(Root, "shadowcopy"), StringComparison.InvariantCultureIgnoreCase);
        }
        static void Start(string path)
        {
            var child = new Process();
            child.StartInfo.FileName = Apploader;
            child.StartInfo.Arguments = path;
            child.StartInfo.CreateNoWindow = true;
            child.StartInfo.UseShellExecute = false;
            child.StartInfo.RedirectStandardInput = true;
            child.StartInfo.RedirectStandardOutput = true;
            child.StartInfo.RedirectStandardError = true;
            child.Start();

            Apps.Add(path, child);

            Log.InfoFormat("App[PID={0}] at [{1}] is running...", child.Id, path);
        }
        static void RenderOutput()
        {
            foreach (var app in Apps)
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    var line = string.Empty;
                    while (!app.Value.StandardOutput.EndOfStream
                        && !string.IsNullOrWhiteSpace(line = app.Value.StandardOutput.ReadLine()))
                        Log.DebugFormat("[PID={0}] {1}"
                            , app.Value.Id
                            , line);
                });
            }
            //var tokenSource = new CancellationTokenSource();
            //var token = tokenSource.Token;
            //var factory = new System.Threading.Tasks.TaskFactory();
            //var task = factory.StartNew(() => { while (!token.IsCancellationRequested) { } });
            //tokenSource.Cancel();
        }

        class CommandHandle : IMessageHandle
        {
            private static readonly StringComparison _comparison = StringComparison.InvariantCultureIgnoreCase;
            private static readonly string _description = "请提交有效的AppServer管理命令，如：list|stop [pid]|start [pid]";

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
                Program.Apps.ToList().ForEach(o =>
                {
                    writer.WriteLine(string.Format("[PID={0}] {1}", o.Value.Id, o.Key));
                    writer.Flush();
                });
            }
        }
    }
}
