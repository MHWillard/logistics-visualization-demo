using System.Diagnostics;
using logistics_visualization_demo.Models;

namespace logistics_visualization_demo.Data
{
    public class DbInitializer
    {
        public static void Initialize(RecordContext context)
        {
            if (context.Orders.Any())
            {
                return;
            }

            var orders = new Order[]
            {
                new Order{CompanyId=1},
                new Order{CompanyId=1},
                new Order{CompanyId=2},
                new Order{CompanyId=3},
                new Order{CompanyId=3},
            };

            context.Orders.AddRange(orders);
            context.SaveChanges();

            var companies = new Company[]
            {
                new Company{Name="Contoso"},
                new Company{Name="Fabrikam"},
                new Company{Name="Adventure Works"}
            };

            context.Companies.AddRange(companies);
            context.SaveChanges();

            var products = new Product[]
            {
                new Product{Name="Widget",Price=10.00m},
                new Product{Name="Gadget", Price=15.00m},
                new Product{Name="Doohickey", Price=20.00m}
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            var orderDetails = new OrderDetail[]
            {
                new OrderDetail{OrderId=1,CompanyId=1, ProductId=1,Quantity=5},
                new OrderDetail{OrderId=1,CompanyId=1, ProductId=2,Quantity=3},
                new OrderDetail{OrderId=2,CompanyId=1, ProductId=3,Quantity=2},
                new OrderDetail{OrderId=3,CompanyId=2, ProductId=1,Quantity=7},
                new OrderDetail{OrderId=4,CompanyId=3, ProductId=2,Quantity=4},
            };

            context.OrderDetails.AddRange(orderDetails);
            context.SaveChanges();
        }
    }
}
