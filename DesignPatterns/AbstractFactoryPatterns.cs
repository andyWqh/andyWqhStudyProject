using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
   public class AbstractFactoryPatterns
    {
       public void CreateJueWeiFood()
       {
           //赣州工厂生产鸭脖和鸭架
           AbstractFactory ganzhouFactory = new GanZhouFactory();
           YaBo ganzhouYaBo = ganzhouFactory.CreateYaBo();
           ganzhouYaBo.CreateYaBo();
           YaJia ganzhouYaJia = ganzhouFactory.CreateYaJia();
           ganzhouYaJia.CreateYaJia();

           //深圳工厂生产鸭脖和鸭架
           AbstractFactory shenzhenFactory = new ShenZhenFactory();
           YaBo shenzhenYaBo = shenzhenFactory.CreateYaBo();
           shenzhenYaBo.CreateYaBo();
           YaJia shenzhenYaJia = shenzhenFactory.CreateYaJia();
           shenzhenFactory.CreateYaJia();

           

       }

       /// <summary>
       /// 抽象工厂类，提供创建两个不同地方的鸭脖和鸭架抽象方法
       /// </summary>
       public abstract class AbstractFactory
       { 
           //抽象工厂提供一系列产品的抽象类，
           //这里作为实例，给出绝味中鸭脖和鸭架的创建接口
           public abstract YaBo CreateYaBo();
           public abstract YaJia CreateYaJia();
       }

       /// <summary>
       /// 赣州客家人特色口味的鸭脖和鸭架
       /// </summary>
       public class GanZhouFactory : AbstractFactory
       {
           /// <summary>
           /// 制作赣州特色鸭脖
           /// </summary>
           /// <returns></returns>
           public override YaBo CreateYaBo()
           {
              return  new GanZhouYaBo();
           }

           /// <summary>
           /// 制作赣州特色鸭架
           /// </summary>
           /// <returns></returns>
           public override YaJia CreateYaJia()
           {
              return new GanZhouYaJia();
           }
       }

       /// <summary>
       /// 广东深圳口味的鸭脖和鸭架
       /// </summary>
       public class ShenZhenFactory : AbstractFactory
       {
           /// <summary>
           /// 制作深圳口味的鸭脖
           /// </summary>
           /// <returns></returns>
           public override YaBo CreateYaBo()
           {
               return new ShenZhenYaBo();
           }

           /// <summary>
           /// 制作深圳口味鸭架
           /// </summary>
           /// <returns></returns>
           public override YaJia CreateYaJia()
           {
              return new ShenZhenYaJia();
           }
       }

       /// <summary>
       /// 鸭脖的抽象类，提供每个地方制作鸭脖的基类
       /// </summary>
       public abstract class YaBo
       {
           /// <summary>
           /// 制作鸭脖的方法
           /// </summary>
           public abstract void CreateYaBo();
       }

       /// <summary>
       /// 鸭架抽象类，提供每个地方的鸭架基类
       /// </summary>
       public abstract class YaJia
       {
           /// <summary>
           /// 制作鸭架的方法
           /// </summary>
           public abstract void CreateYaJia();
       }

       /// <summary>
       /// 赣州鸭脖类，赣州客家人都喜欢辣味的鸭脖
       /// </summary>
       public class GanZhouYaBo : YaBo
       {
           /// <summary>
           /// 制作赣州特色的鸭脖
           /// </summary>
           public override void CreateYaBo()
           {
               Console.WriteLine("赣州特色的鸭脖");
           }
       }

       public class ShenZhenYaBo : YaBo
       {
           /// <summary>
           /// 制作广东深圳口味比较淡的鸭脖
           /// </summary>
           public override void CreateYaBo()
           {
               Console.WriteLine("深圳口淡口味的鸭脖");
           }
       }

       /// <summary>
       /// 赣州客家人特色口味的绝味鸭架
       /// </summary>
       public class GanZhouYaJia : YaJia
       {
           /// <summary>
           /// 制作含赣州客家人特色口味的绝味鸭架
           /// </summary>
           public override void CreateYaJia()
           {
               Console.WriteLine("赣州客家人特色口味的绝味鸭架");
           }
       }

       /// <summary>
       /// 广东深圳口味的鸭架
       /// </summary>
       public class ShenZhenYaJia : YaJia
       {
           /// <summary>
           /// 制作广东深圳口味的鸭架
           /// </summary>
           public override void CreateYaJia()
           {
               Console.WriteLine("广东深圳口味的鸭架");
           }
       }
    }
   /***************抽象工厂模式总结*******************/
   /***
    *抽象工厂模式：提供一个创建产品的接口来负责创建相关或依赖的对象，而不具体明确指定具体类
    *抽象工厂允许客户使用抽象的接口来创建一组相关产品，而不需要知道或关心实际生产出的具体产品是什么。这样客户就可以从具体产品中被解耦.
    *抽象工厂模式将具体产品的创建延迟到具体工厂的子类中，这样将对象的创建封装起来，可以减少客户端与具体产品类之间的依赖，从而使系统耦合      度低，这样更有利于后期的维护和扩展，这真是抽象工厂模式的优点所在，然后抽象模式同时也存在不足的地方。
    *抽象工厂模式很难支持新种类产品的变化。这是因为抽象工厂接口中已经确定了可以被创建的产品集合，如果需要添加新产品，此时就必须去修改抽      象工厂的接口，这样就涉及到抽象工厂类的以及所有子类的改变，这样也就违背了“开发——封闭”原则。
    *使用场景：
    *一个系统不要求依赖产品类实例如何被创建、组合和表达的表达，这点也是所有工厂模式应用的前提。
    *这个系统有多个系列产品，而系统中只消费其中某一系列产品。
    *系统要求提供一个产品类的库，所有产品以同样的接口出现，客户端不需要依赖具体实现。
   ***/
}
