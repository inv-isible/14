//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _13
{
    using _13;
    using System;
    using System.Collections.Generic;

    public partial class ProductSale
    {
        public int ID { get; set; }
        public int AgentID { get; set; }
        public int ProductID { get; set; }
        public System.DateTime SaleDate { get; set; }
        public int ProductCount { get; set; }
        public virtual Agent Agent { get; set; }
        public virtual Product Product { get; set; }
        public decimal Stoimost
        {
            get
            {
                decimal st = Product.MinCostForAgent * ProductCount;
                return st;
            }
        }
        public string Datacount
        {
            get
            {
                string data = Convert.ToString(SaleDate);
                string count = Convert.ToString(ProductCount);
                string dc = data + " " + count;
                return dc;
            }
        }
    }
}