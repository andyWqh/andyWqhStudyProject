//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFModelFirst
{
    using System;
    using System.Collections.Generic;
    
    public partial class userCard
    {
        public int cardId { get; set; }
        public string cardNo { get; set; }
        public decimal totalCash { get; set; }
        public System.DateTime createDate { get; set; }
        public int userId { get; set; }
    
        public virtual User User { get; set; }
    }
}
