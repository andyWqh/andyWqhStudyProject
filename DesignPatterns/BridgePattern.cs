/************************************************************************************
 * Copyright (c) 2017 All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 *机器名称：ANDYWQH
 *公司名称：
 *命名空间：DesignPatterns
 *文件名：  BridgePattern
 *版本号：  V1.0.0.0
 *唯一标识：587191e3-c0f2-4277-b968-ff9c0a4b59a7
 *当前的用户域：andyWqh
 *创建人：  andysunWqh
 *电子邮箱：andyWqh@163.com
 *创建时间：2017/9/13 23:23:25
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：2017/9/13 23:23:25
 *修改人： andysunWqh
 *版本号： V1.0.0.0
 *描述：设计模式-桥接模式
 *
/***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    /// <summary>
    /// 桥接模式即将抽象部分与实现部分脱耦，使它们可以独立变化。对于上面的问题中，抽象化也就是RemoteControl类，实现部分也就是On()、Off()、NextChannel()等这样的方法（即遥控器的实现），上面的设计中，抽象化和实现部分在一起，桥接模式的目的就是使两者分离，根据面向对象的封装变化的原则，我们可以把实现部分的变化（也就是遥控器功能的变化）封装到另外一个类中，这样的一个思路也就是桥接模式的实现，大家可以对照桥接模式的实现代码来解决我们的分析思路。
    /// 抽象概念中的遥控器，扮演抽象化角色
    /// </summary>
   public class BridgePattern
    {
        //属性
        public TvPattern Implementor { get; set; }

       /// <summary>
        /// 开电视机，这里抽象类中不再提供实现了，而是调用实现类中的实现
       /// </summary>
        public virtual void On()
        {
            Implementor.On();
        }

       /// <summary>
       /// 关电视机
       /// </summary>
        public virtual void Off()
        {
            Implementor.Off();
        }

       /// <summary>
       /// 换频道
       /// </summary>
        public virtual void SetChannel()
        {
            Implementor.TurnChange();
        }

    }

    /// <inheritdoc />
    /// <summary>
    /// 具体遥控器
    /// </summary>
   public class ConcreteRemote : BridgePattern
    {
        public override void SetChannel()
        {
            Console.WriteLine("---------------------");
            base.SetChannel();
            Console.WriteLine("---------------------");
        }
    }


    /// <summary>
    /// 电视机提供抽象方法，即模板蓝图
    /// </summary>
    public abstract class TvPattern
    {
        public abstract void On();
        public abstract void Off();
        public abstract void TurnChange();
    }

    /// <inheritdoc />
    /// <summary>
    /// 长虹牌电视机 重写基类的抽象方法
    /// 提供具体的实现
    /// </summary>
    public class ChangeHongTv : TvPattern
    {
        public override void On()
        {
            Console.WriteLine("长虹牌电视机已经打开了");
        }

        public override void Off()
        {
            Console.WriteLine("长虹牌电视机已经关掉了");
        }

        public override void TurnChange()
        {
            Console.WriteLine("长虹牌电视机换频道");
        }
    }

    public class SamsungTv : TvPattern
    {
        public override void On()
        {
            Console.WriteLine("三星牌电视机已经打开了");
        }

        public override void Off()
        {
            Console.WriteLine("三星牌电视机已经关掉了");
        }

        public override void TurnChange()
        {
            Console.WriteLine("三星牌电视机换频道");
        }
    }


    public class Clinet
    {
        private static void Create(string[] args)
        {
            // 创建一个遥控器
            BridgePattern remoteControl = new ConcreteRemote();
            //长虹电视机
            remoteControl.Implementor = new ChangeHongTv();
            remoteControl.On();
            remoteControl.Off();
            remoteControl.SetChannel();

            //三星电视机
            remoteControl.Implementor = new SamsungTv();
            remoteControl.On();
            remoteControl.Off();
            remoteControl.SetChannel();
            Console.ReadKey();
        }
    }
}
