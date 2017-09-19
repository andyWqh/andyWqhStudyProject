/************************************************************************************
 * Copyright (c) 2017 All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 *机器名称：ANDYWQH
 *公司名称：
 *命名空间：DesignPatterns
 *文件名：  AdapterPatterns
 *版本号：  V1.0.0.0
 *唯一标识：b804e3eb-1bfe-4c57-bd01-a4f17ca1d454
 *当前的用户域：andyWqh
 *创建人：  andysunWqh
 *电子邮箱：andyWqh@163.com
 *创建时间：2017/9/14 23:08:27
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：2017/9/14 23:08:27
 *修改人： andysunWqh
 *版本号： V1.0.0.0
 *描述：类适配器和对象适配器
 *
/***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
   public class Client
    {
        public void Main(string[] args)
        {
            //现在客户端可以通过调适配器
            IThreeHole threeHole = new PowerAdapter();
            threeHole.Request();
        }
       
    }

    /// <summary>
   /// 三个孔的插头，也就是适配器模式中的目标角色
    /// </summary>
    public interface IThreeHole
    {
        void Request();
    }

    public abstract class TwoHole
    {
        public void SpectificRequset()
        {
            Console.WriteLine("我是两个孔的插头");
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// 适配器类，接口要放在类的后面
    /// 适配器类提供了三个孔插头的行为，但其本质是调用两个孔插头的方法
    /// </summary>
    internal class PowerAdapter : TwoHole, IThreeHole
    {
        public void Request()
        {
            //调用两个孔的方法
            this.SpectificRequset();
        }
    }
}
