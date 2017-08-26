using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    /// <summary>
    /// 客户点餐
    /// </summary>
    public class Customer 
    {
        public void EatFood()
        {
            #region old Code
            // 客户A想点一个西红柿炒蛋        
            CreateFood foodA = CreateFoodFactory.DoCooking("西红寺炒蛋");
            foodA.GetFood();

            //客户B想点一个土豆肉丝
            CreateFood foodB = CreateFoodFactory.DoCooking("土豆肉丝");
            foodB.GetFood(); 
            #endregion

            #region new Code
             // 初始化做菜的两个工厂（）
            CreatorFactory shreddedPorkWithPotatoesFactory = new ShreddedPorkWithPotatoesFactory();
            CreatorFactory tomatoScrambledEggsFactory = new TomatoScrambledEggsFactory();

            // 开始做西红柿炒蛋
            CreateFood tomatoScrambleEggs = tomatoScrambledEggsFactory.CreateFoodFactory();
            tomatoScrambleEggs.GetFood();

            //开始做土豆肉丝
            CreateFood shreddedPorkWithPotatoes = shreddedPorkWithPotatoesFactory.CreateFoodFactory();
            shreddedPorkWithPotatoes.GetFood();

            #endregion
        }
    }

    public abstract class CreateFood
    {
        public abstract void GetFood();
    }

    /// <summary>
    /// 西红柿炒鸡蛋这道菜
    /// </summary>
    public class TomatoScrambledEggs : CreateFood
    {
        public override void GetFood()
        {
            Console.WriteLine("一份西红柿炒蛋！");
        }
    }

    /// <summary>
    /// 土豆肉丝这道菜
    /// </summary>
    public class ShreddedPorkWithPotatoes : CreateFood
    {
        public override void GetFood()
        {
            Console.WriteLine("一份土豆肉丝");
        }
    }

    /// <summary>
    /// 简单工厂模式，负责做菜提供给顾客使用
    /// </summary>
    public class CreateFoodFactory
    {
        public static CreateFood DoCooking(string menuType)
        {
            CreateFood food = null;
            if (menuType.Equals("土豆肉丝"))
            {
                food = new ShreddedPorkWithPotatoes();
            }
            else if (menuType.Equals("西红柿炒蛋"))
            {
                food = new TomatoScrambledEggs();
            }
            return food;
        }
    }

    /******改进后的工厂模式*******/
    /// <summary>
    /// 抽象工厂
    /// </summary>
    public abstract class CreatorFactory
    {
        /// <summary>
        /// 抽象工厂方法
        /// </summary>
        /// <returns></returns>
        public abstract CreateFood CreateFoodFactory();
    }

    /// <summary>
    /// 西红柿炒蛋工厂类
    /// </summary>
    public class TomatoScrambledEggsFactory : CreatorFactory
    {
        /// <summary>
        /// 负责创建西红柿炒蛋这道菜
        /// </summary>
        /// <returns></returns>
        public override CreateFood CreateFoodFactory()
        {
           return   new TomatoScrambledEggs();
        }
    }

    /// <summary>
    /// 土豆肉丝工厂类
    /// </summary>
    public class ShreddedPorkWithPotatoesFactory : CreatorFactory
    {
        /// <summary>
        /// 负责创建土豆肉丝这道菜
        /// </summary>
        /// <returns></returns>
        public override CreateFood CreateFoodFactory()
        {
            return new ShreddedPorkWithPotatoes();
        }
    }

    /*********************简单工厂模式总结******************/
    /*****
     * 看完简单工厂模式的实现之后，你和你的小伙伴们肯定会有这样的疑惑（因为我学习的时候也有）——这样我们只是把变化移到了工厂类中罢了，好       像没有变化的问题，因为如果客户想吃其他菜时，此时我们还是需要修改工厂类中的方法（也就是多加case语句，没应用简单工厂模式之前，修        改的是客户类）。我首先要说：你和你的小伙伴很对，这个就是简单工厂模式的缺点所在（这个缺点后面介绍的工厂方法可以很好地解决），然         而，简单工厂模式与之前的实现也有它的优点：
     * 优点：
     * 简单工厂模式解决了客户端直接依赖于具体对象的问题，客户端可以消除直接创建对象的责任，而仅仅是消费产品。简单工厂模式实现了对责任的       分割。
     * 简单工厂模式也起到了代码复用的作用，因为之前的实现（自己做饭的情况）中，换了一个人同样要去在自己的类中实现做菜的方法，然后有了简       单工厂之后，去餐馆吃饭的所有人都不用那么麻烦了，只需要负责消费就可以了。此时简单工厂的烧菜方法就让所有客户共用了。（同时这点也是       简单工厂方法的缺点——因为工厂类集中了所有产品创建逻辑，一旦不能正常工作，整个系统都会受到影响，也没什么不好理解的，就如事物都有两       面性一样道理）
     * 缺点：
     * 工厂类集中了所有产品创建逻辑，一旦不能正常工作，整个系统都会受到影响（通俗地意思就是：一旦餐馆没饭或者关门了，很多不愿意做饭的人       就没饭吃了）
     * 系统扩展困难，一旦添加新产品就不得不修改工厂逻辑，这样就会造成工厂逻辑过于复杂。
     * 使用场景:
     * 当工厂类负责创建的对象比较少时可以考虑使用简单工厂模式（）。
     * 客户如果只知道传入工厂类的参数，对于如何创建对象的逻辑不关心时可以考虑使用简单工厂模式。
     * ****/
}
