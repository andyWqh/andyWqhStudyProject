using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;

namespace andyWqhService
{
    /// <summary>
    /// 继承服务基类ServiceBase
    /// </summary>
    public partial class AndyWqhService : ServiceBase
    {
        System.Timers.Timer timerDelay;
        public AndyWqhService()
        {
            //初始化组件
            InitializeComponent();
            //初始化服务参数
            InitService();
        }

        /// <summary>
        /// 初始化服务参数
        /// </summary>
        private void InitService()
        {
            base.AutoLog = false;
            base.CanShutdown = true;
            base.CanStop = true;
            base.CanPauseAndContinue = true;
            base.ServiceName = "andyWqh";  //这个名字很重要，设置不一致会产生 1083 错误哦！
        }

        protected override void OnStart(string[] args)
        {
            try
            {

                ///delay start the SynData 30seconds
                timerDelay = new System.Timers.Timer(30000);
                timerDelay.Elapsed += new System.Timers.ElapsedEventHandler(timerDelay_Elapsed);
                timerDelay.Start();
            }
            catch (Exception ex)
            {
                this.PrintExceptions(ex);  
            }
        }

        private void timerDelay_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timerDelay.Enabled = false;
            timerDelay.Close();
            try
            {
                this.AddTextLine("TeamWorldServiceLog");
            }
            catch (Exception ex)
            {

            }
        }

        private void AddTextLine(string line)
        {
            try
            {
                FileStream fs = new FileStream(@"D:\TeamWorldServiceLog.txt", FileMode.OpenOrCreate, FileAccess.Write);

                StreamWriter m_streamWriter = new StreamWriter(fs);

                m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);

                m_streamWriter.WriteLine(line + "\r\n");

                m_streamWriter.Flush();

                m_streamWriter.Close();

                fs.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void PrintExceptions(Exception exc)
        {
            Exception current = exc;
            while (current != null)
            {
                this.AddTextLine(current.Message);
                this.AddTextLine(current.StackTrace);

                current = current.InnerException;
            }
        }

        protected override void OnStop()
        {
            string str = "服务开启";
            this.AddTextLine(str);
        }

        protected override void OnContinue()
        {
            string str = "服务继续运行";
            this.AddTextLine(str);
            base.OnContinue();
        }

        protected override void OnPause()
        {
            string str = "服务暂停";
            this.AddTextLine(str);
            base.OnPause();
        }
    }
}
