/************************************************************************************
 * Copyright (c) 2017 All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 *机器名称：ANDYWQH
 *公司名称：
 *命名空间：DesignPatterns
 *文件名：  DecoratorPatterns
 *版本号：  V1.0.0.0
 *唯一标识：b412b2fa-b27d-4fd6-b827-ec21134a10b1
 *当前的用户域：andyWqh
 *创建人：  andysunWqh
 *电子邮箱：andyWqh@163.com
 *创建时间：2017/9/14 23:21:59
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：2017/9/14 23:21:59
 *修改人： andysunWqh
 *版本号： V1.0.0.0
 *描述：装饰者模式
 *
/***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
   public class DecoratorCustomer
    {
        private  void Main(string[] args)
       {
           // 我买了个苹果手机
           Phone phone = new ApplePhone();

           // 现在想贴膜了
           Decorator applePhoneWithSticker = new Sticker(phone);
           // 扩展贴膜行为
           applePhoneWithSticker.Print();
           Console.WriteLine("----------------------\n");

           // 现在我想有挂件了
           Decorator applePhoneWithAccessories = new Accessories(phone);
           // 扩展手机挂件行为
           applePhoneWithAccessories.Print();
           Console.WriteLine("----------------------\n");

           // 现在我同时有贴膜和手机挂件了
           var sticker = new Sticker(phone);
           var applePhoneWithAccessoriesAndSticker = new Accessories(sticker);
           applePhoneWithAccessoriesAndSticker.Print();
           Console.ReadLine();
       }
    }

    /// <summary>
    /// 手机抽象类，即装饰者模式中的抽象组建类
    /// </summary>
    public abstract class Phone
    {
        public abstract void Print();
    }

    /// <inheritdoc />
    /// <summary>
    /// 苹果手机，即装饰着模式中的具体组件类
    /// </summary>
    public class ApplePhone : Phone
    {
        /// <summary>
        /// 重写基类方法
        /// </summary>
        public override void Print()
        {
            Console.WriteLine("开始执行具体的对象-苹果手机");
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// 装饰抽象类,要让装饰完全取代抽象组件，所以必须继承自Photo
    /// </summary>
    public abstract class Decorator : Phone
    {
        private readonly Phone _phone;

        protected Decorator(Phone p)
        {
            this._phone = p;
        }
        public override void Print()
        {
            if (_phone != null)
            {
                _phone.Print();
            }
        }
    }

    public class Sticker : Decorator
    {
        public Sticker(Phone p) : base(p)
        {
            
        }
        public override void Print()
        {
            base.Print();

            //添加新行为
            AddSticker();
        }

        /// <summary>
        /// 新增行为
        /// </summary>
        public void AddSticker()
        {
            Console.WriteLine("现在苹果手机有贴膜了");
        }
    }

   /// <inheritdoc />
   /// <summary>
   /// 手机挂件
   /// </summary>
    public class Accessories : Decorator
    {
        public Accessories(Phone p)
            : base(p)
        {
        }

        public override void Print()
        {
            base.Print();

            // 添加新的行为
            AddAccessories();
        }

        /// <summary>
        /// 新的行为方法
        /// </summary>
        public void AddAccessories()
        {
            Console.WriteLine("现在苹果手机有漂亮的挂件了");
        }
    }
}
