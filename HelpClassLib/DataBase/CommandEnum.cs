using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelpClassLib.DataBase
{
    /// <summary>
    /// CommandEnum 的摘要说明。
    /// </summary>
    public class CommandEnum
    {
        public CommandEnum()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public enum DsCommandType
        {
            InsertCommand, UpdateCommand, DeleteCommand
        };
    }
}
