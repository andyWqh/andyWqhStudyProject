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
   
}
