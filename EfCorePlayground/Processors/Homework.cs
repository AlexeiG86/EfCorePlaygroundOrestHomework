using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfCorePlayground.Data;
using EfCorePlayground.Models;

namespace EfCorePlayground.Processors {
    public class Homework {
        public void Run() {
            CreateProducts();
            int productId = GetRandomProduct();
            int customerId = GetRandomCustomer();
            int orderId = CreateOrder(customerId, productId); 
            UpdateItemQuantity(orderId,productId,16);
           


            BuyAnotherProduct(orderId, productId);





        }

        private void BuyAnotherProduct(int orderId, int productId)
        {
            using var context = new CustomerContext();
            //var orderItem = context.OrderItems.FirstOrDefault(x => x.OrderId == orderId);


            var orderItem = new OrderItem()
            {
                OrderId = orderId,
                ProductId = productId,
                CreatedOn = DateTime.Now,
                Quantity = 2,
                
                
            };
            context.OrderItems.Add(orderItem);
            context.SaveChanges();

        }

        private void UpdateItemQuantity(int orderId, int productId, int v)
        {
            using var context = new CustomerContext();
           
            //gi inside orderItems table
            // pick the order id based on the parameter
            // based on that order id and product id pick the orderitemid
            // update the quantity


            var orderIt = context.OrderItems.Where(x => x.OrderId == orderId);//we go to order item and bring all orderitem coresponding to our paramater orderId
            var productnew =orderIt.FirstOrDefault(x => x.ProductId == productId);//we use orderItem from above and ,selecting particular productId that will match our paraameter
            productnew.Quantity = v;//we updating our previor selectect product quataty with the cuantaty that we receive from our parameter
            context.SaveChanges();
        }

        private int CreateOrder(int customerId, int productId)
        {
            using var context = new CustomerContext();

            var order = new Order()
            {
                CustomerId = customerId,
                OrderTotal = 200,
                DiscountAmount = 20,
                CreatedOn = DateTime.Now,
                IsComplete = false,
            };
            context.Orders.Add(order);
            context.SaveChanges();

            var existingOrder = context.Orders.FirstOrDefault(x => x.CustomerId == customerId);
            var orderItem = new OrderItem()
            {
                OrderId = existingOrder.OrderId,
                ProductId = productId,
                CreatedOn = DateTime.Now,
                Quantity = 1
            };
            context.OrderItems.Add(orderItem);
            context.SaveChanges();
            return existingOrder.OrderId;
        }


        private int GetRandomCustomer()
        {
            using var context = new CustomerContext();
            if (!context.Customers.Any()) //if no products in table we will ad some products
            {
                Random random = new Random();
                int range = (new DateTime(2000, 1, 1) - new DateTime(1990, 1, 1)).Days;
                for (int i = 1; i <= 10; i++)
                {
                    var customer = new Customer()
                    {
                        FirstName = $"Vasile{i}",
                        LastName = $"Vasilevici{i}",
                        Email = $"gmail.{i}.com",
                        DateOfBirth = new DateTime(1990, 1, 1).AddDays(random.Next(range))
                    };
                    context.Customers.Add(customer);
                }
                context.SaveChanges();
            }

            var min = context.Customers.Min(x => x.CustomerId); //
            var max = context.Customers.Max(x => x.CustomerId);
            var randon = new Random();
            return randon.Next(min, max);
        }

        private int GetRandomProduct()
        {
            using var context = new CustomerContext(); //init the context
            var min = context.Products.Min(x => x.ProductId); //
            var max = context.Products.Max(x => x.ProductId);
            var randon = new Random();
            return randon.Next(min, max);
        }

        public void CreateProducts()
        {
            using var context = new CustomerContext();
            if (!context.Products.Any()) //if no products in table we will ad some products
            {
                for (int i = 1; i <= 10; i++)
                {
                    var product = new Product()
                    {
                        SKU = $"SKU{i}",
                        Price = 100 + i,
                        Name = $"Sony Headphones model-{i}",
                        Description =$"This is model 2 of {i}"
                    };
                    context.Products.Add(product);
                }

                context.SaveChanges();
            }
        }




        //private int GetRandomProduct() {
        //    using var context = new CustomerContext();
        //    var min = context.Products.Min(p => p.ProductId);
        //    var max = context.Products.Max(p => p.ProductId);
        //    var random = new Random();
        //    return random.Next(min, max);
        //}
    }
}
