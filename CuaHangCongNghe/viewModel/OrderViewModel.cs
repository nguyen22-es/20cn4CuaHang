using System;
using System.Collections.Generic;
using System.Linq;

namespace CuaHangCongNghe.Models
{
    public class OrderViewModel
    {
    public int Id { get; set; }
    public string UserId { get; set; }
    public int Status { get; set; }
    public string NameStatus { get; set; }
    public DateTime OrderDate { get; set; }
    



    public List<OrderItemViewModel> ItemViewModels { get; set; } = new List<OrderItemViewModel>();


    public double FullPrice => ItemViewModels.Sum(x => x.Price);
        public double FullAmount => ItemViewModels.Sum(x => x.quantity);



    }
}
