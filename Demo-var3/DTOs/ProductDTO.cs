using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Demo_var3.DTOs
{
    public class ProductDTO
    {
        public string Article { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public int Cost { get; set; }
        public int Sale {  get; set; }
        public int Quantity { get; set; }
        public string Photo { get; set; }
        
        public float FinalCost => (float)Math.Round((float)(Cost * (100 - Sale)), 2);
        public Visibility HasSale => Sale > 0 ? Visibility.Visible : Visibility.Collapsed;
        public string Background => Quantity == 0 ? "#808080" : (Sale >= 12 ? "#FFDEAD" : "Transparent");
        public ImageSource Image => Photo.Get();
    }
}
