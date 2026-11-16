using System;
using System.Collections.Generic;
using System.Linq;

namespace Task1_StoreSimulation
{
    public struct SimulationResult
    {
        public decimal TotalProfit;
        public int MaxItemsCardNumber;
        public int MaxItemsCount;
        public int MaxSpentCardNumber;
        public decimal MaxSpentAmount;
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Product(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }

    public class Customer
    {
        public int CardNumber { get; private set; }
        public decimal Money { get; set; }
        public decimal SpentMoney { get; set; }
        public int ItemsBought { get; set; }

        public Customer(int cardNumber, decimal startingMoney)
        {
            CardNumber = cardNumber;
            Money = startingMoney;
            SpentMoney = 0;
            ItemsBought = 0;
        }
    }

    public class Store
    {
        public List<Product> Products { get; private set; }
        public decimal Profit { get; private set; }
        private Random _rnd;

        public Store()
        {
            _rnd = new Random();
            Profit = 0;
            Products = new List<Product>
            {
                new Product("Ноутбук", 35000m, 5),
                new Product("Смартфон", 12000m, 15),
                new Product("Навушники", 2500m, 30),
                new Product("Монітор", 8000m, 10),
                new Product("Клавіатура", 1500m, 20),
                new Product("Мишка", 800m, 25),
                new Product("Планшет", 15000m, 8),
                new Product("Годинник", 4000m, 12)
            };
        }

        public SimulationResult Run(List<Customer> customers)
        {
            bool active = true;
            while (active)
            {
                var available = Products.Where(p => p.Quantity > 0).ToList();
                if (available.Count == 0) break;

                bool boughtAny = false;
                foreach (var c in customers)
                {
                    var affordable = available.Where(p => p.Price <= c.Money).ToList();
                    if (affordable.Count > 0)
                    {
                        var product = affordable[_rnd.Next(affordable.Count)];
                        product.Quantity--;
                        c.Money -= product.Price;
                        c.SpentMoney += product.Price;
                        c.ItemsBought++;
                        Profit += product.Price;
                        boughtAny = true;
                    }
                }
                if (!boughtAny) active = false;
            }
            return GenerateResult(customers);
        }

        private SimulationResult GenerateResult(List<Customer> customers)
        {
            var res = new SimulationResult();
            res.TotalProfit = this.Profit;
            if (customers.Count > 0)
            {
                var mI = customers.OrderByDescending(c => c.ItemsBought).First();
                var mS = customers.OrderByDescending(c => c.SpentMoney).First();
                res.MaxItemsCardNumber = mI.CardNumber;
                res.MaxItemsCount = mI.ItemsBought;
                res.MaxSpentCardNumber = mS.CardNumber;
                res.MaxSpentAmount = mS.SpentMoney;
            }
            return res;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Write("Введіть кількість покупців: ");

            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
            {
                Console.WriteLine("Помилка вводу!");
                return;
            }

            Random rnd = new Random();
            var customers = new List<Customer>();

            for (int i = 0; i < count; i++)
            {
                int card = rnd.Next(1, 1001);
                decimal money = rnd.Next(1000, 10001);
                customers.Add(new Customer(card, money));
            }

            Store store = new Store();
            var result = store.Run(customers);

            Console.WriteLine("\n=== РЕЗУЛЬТАТИ ===");
            Console.WriteLine($"Прибуток: {result.TotalProfit} грн.\n");

            Console.WriteLine("Покупець, який придбав найбільше товарів:");
            Console.WriteLine($"Номер картки: {result.MaxItemsCardNumber}");
            Console.WriteLine($"Куплено товарів: {result.MaxItemsCount} шт.\n");

            Console.WriteLine("Покупець, який витратив найбільше грошей:");
            Console.WriteLine($"Номер картки: {result.MaxSpentCardNumber}");
            Console.WriteLine($"Сума витрат: {result.MaxSpentAmount} грн.");
            Console.ReadLine();
        }
    }
}