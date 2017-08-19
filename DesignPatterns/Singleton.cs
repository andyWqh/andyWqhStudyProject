using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    /// <summary>
    /// 单例模式实现
    /// </summary>
   public class Singleton
    {
       //定义一个静态变量保存类的实例
       private static Singleton uniqueInstance;

       //定义一个锁标识确保线程同步
       private static readonly object locker = new object();

       /// <summary>
       /// 定义私有的无惨构造函数，通过类定义私有的构造函数可以保证在外界不能通过new关键字进行创建类的实例
       /// </summary>
       private Singleton() { }

       /// <summary>
       /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
       /// </summary>
       /// <returns></returns>
       public static Singleton GetInstance()
       {
           // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
           // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
           // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
           // 双重锁定只需要一句判断就可以了
           if (uniqueInstance == null)
           {
               lock (locker)
               {
                   if (uniqueInstance == null)
                   {
                       uniqueInstance = new Singleton();   
                   }
               }
           }
           return uniqueInstance;
       }
    }
/****************单例模式总结**********************/
   /*****
     *从“单例”字面意思上理解为——一个类只有一个实例，所以单例模式也就是保证一个类只有一个实例的一种实现方法罢了(设计模式其实就是帮助         我们解决实际开发过程中的方法, 该方法是为了降低对象之间的耦合度,然而解决方法有很多种,所以前人就总结了一些常用的解决方法为书籍,从       而把这本书就称为设计模式)，下面给出单例模式的一个官方定义：确保一个类只有一个实例,并提供一个全局访问点。
     *为了在多线程中也能进行单例模式：一般都会采用“双重锁机制”；即在lock语句前面加一句（uniqueInstance==null）的判断就可以避免锁       所增加的额外开销，这种实现方式我们就叫它 “双重锁定”
   **********/

}
