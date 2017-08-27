using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 在软件系统中，有时需要创建一个复杂对象，并且这个复杂对象由其各部分子对象通过一定的步骤组合而成。例如一个采购系统中，如果需要采购员去采购一批电脑时，在这个实际需求中，电脑就是一个复杂的对象，它是由CPU、主板、硬盘、显卡、机箱等组装而成的，如果此时让采购员一台一台电脑去组装的话真是要累死采购员了，这里就可以采用建造者模式来解决这个问题，我们可以把电脑的各个组件的组装过程封装到一个建造者类对象里，建造者只要负责返还给客户端全部组件都建造完毕的产品对象就可以了。然而现实生活中也是如此的，如果公司要采购一批电脑，此时采购员不可能自己去买各个组件并把它们组织起来，此时采购员只需要像电脑城的老板说自己要采购什么样的电脑就可以了，电脑城老板自然会把组装好的电脑送到公司
*/
namespace DesignPatterns
{
    /// <summary>
    /// 以组装电脑为例，每台电脑组装过程都是一样的
    /// 但是使用同样的构建过程可以创建不同的表示(即可以组装成不一样的电脑，配置不一样)
    /// 组装电脑的这个场景就可以应用建造者模式来设计
    /// </summary>
    public class BuilderCustomer
    {
        public void RequestBuilderComputer()
        {
            // 客户找到电脑城老板说要买电脑，这里要装一台电脑
            // 创建指挥者
            Director boss = new Director();
            //创建电脑建造者
            Builder XiaoWang = new ConcreteXiaoWang();
            //老板指派XiaoWang去组装电脑
            boss.ConstructComputer(XiaoWang);
            //获取组装好的电脑
            Computer computer = XiaoWang.GetComputer();
            //展示组装好的电脑
            computer.ShowInfo();
        }
    }

    /// <summary>
    /// 小王和小李难道会自愿地去组装嘛，谁不想休息的，这必须有一个人叫他们去组装才会去的
    /// 这个人当然就是老板了，也就是建造者模式中的指挥者
    /// 指挥创建过程类
    /// </summary>
    public class Director
    {
        public void ConstructComputer(Builder builder)
        {
            //组装CPU
            builder.BuilderCPU();
            //组装主板
            builder.BuildPartMainBoard();
        }
    }

    /// <summary>
    /// 抽象建造者，即为组装电脑人员
    /// </summary>
    public abstract class Builder
    {
        /// <summary>
        /// 组装CPU动作
        /// </summary>
        public abstract void BuilderCPU();

        /// <summary>
        /// 组装主板动作
        /// </summary>
        public abstract void BuildPartMainBoard();

        // 当然还有装硬盘，电源等组件，这里省略
        /// <summary>
        /// 获得组装好的电脑
        /// </summary>
        /// <returns></returns>
        public abstract Computer GetComputer();
    }

    /// <summary>
    /// 电脑类
    /// </summary>
    public class Computer
    {
        //电脑组件集合
        public IList<string> partList = new List<string>();

        /// <summary>
        /// 将单个组件添加到电脑组件集合中
        /// </summary>
        /// <param name="part"></param>
        public void AddPart(string part)
        {
            partList.Add(part);
        }

        public void ShowInfo()
        {
            Console.WriteLine("电脑正在组装中。。。。");
            foreach (var part in partList)
            {
                Console.WriteLine("组件"+part+"已装好");
            }
            Console.WriteLine("电脑组装完毕!");
        }
    }

    public class ConcreteXiaoWang :Builder
    {
        Computer computer = new Computer();
        public override void BuilderCPU()
        {
            computer.AddPart("CPU-XiaoWang");
        }

        public override void BuildPartMainBoard()
        {
            computer.AddPart("Xiaowang-MainBoard");
        }

        public override Computer GetComputer()
        {
            return computer;
        }
    }

    /****总结说明
     * 建造者模式（Builder Pattern）:将一个复杂对象的构建与它的表示分离，使得同样的构建过程可以创建不同的表示。

     * 建造者模式使得建造代码与表示代码的分离，可以使客户端不必知道产品内部组成的细节，从而降低了客户端与具体产品之间的耦合度
     * 
     * 在建造者模式中，指挥者是直接与客户端打交道的，指挥者将客户端创建产品的请求划分为对各个部件的建造请求，再将这些请求委派到具体建造者角色，具体建造者角色是完成具体产品的构建工作的，却不为客户所知道。
     * 
     * 建造者模式主要用于“分步骤来构建一个复杂的对象”，其中“分步骤”是一个固定的组合过程，而复杂对象的各个部分是经常变化的（也就是说电脑的内部组件是经常变化的，这里指的的变化如硬盘的大小变了，CPU由单核变双核等）
     * 
     * 产品不需要抽象类，由于建造模式的创建出来的最终产品可能差异很大，所以不大可能提炼出一个抽象产品类。
     * 
     * 抽象工厂模式解决了“系列产品”的需求变化，而建造者模式解决的是 “产品部分” 的需要变化。
     * 
     * 建造者隐藏了具体产品的组装过程，所以要改变一个产品的内部表示，只需要再实现一个具体的建造者就可以了，从而能很好地应对产品组成组件的需求变化。
     * ***/
}
