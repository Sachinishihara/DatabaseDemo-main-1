using Microsoft.EntityFrameworkCore;
using WebStore.Entities;
//using WebStore.Entities;

namespace WebStore.Assignments
{
    /// Additional tutorial materials https://dotnettutorials.net/lesson/linq-to-entities-in-entity-framework-core/

    /// <summary>
    /// This class demonstrates various LINQ query tasks 
    /// to practice querying an EF Core database.
    /// 
    /// ASSIGNMENT INSTRUCTIONS:
    ///   1. For each method labeled "TODO", write the necessary
    ///      LINQ query to return or display the required data.
    ///      
    ///   2. Print meaningful output to the console (or return
    ///      collections, as needed).
    ///      
    ///   3. Test each method by calling it from your Program.cs
    ///      or test harness.
    /// </summary>
    public class LinqQueriesAssignment
    {

         

        private readonly learningassignment4Context _dbContext;

        public LinqQueriesAssignment(learningassignment4Context context)
        {
            _dbContext = context;
        }


        /// <summary>
        /// 1. List all customers in the database:
        ///    - Print each customer's full name (First + Last) and Email.
        /// </summary>
        public async Task Task01_ListAllCustomers()
        {
            // TODO: Write a LINQ query that fetches all customers
            //       and prints their names + emails to the console.
            // HINT: context.Customers
            
            var customers = await _dbContext.Customers
               // .AsNoTracking() // optional for read-only
               .ToListAsync();

            Console.WriteLine("=== TASK 01: List All Customers ===");

            foreach (var c in customers)
            {
                string FirstName = c.FirstName;
                string LastName = c.LastName;
                Console.WriteLine($"{c.FirstName} {c.LastName} - {c.Email}");
            }

            
        }

        /// <summary>
        /// 2. Fetch all orders along with:
        ///    - Customer Name
        ///    - Order ID
        ///    - Order Status
        ///    - Number of items in each order (the sum of OrderItems.Quantity)
        /// </summary>
        public async Task Task02_ListOrdersWithItemCount()
        {
            // TODO: Write a query to return all orders,
            //       along with the associated customer name, order status,
            //       and the total quantity of items in that order.

            // HINT: Use Include/ThenInclude or projection with .Select(...).
            //       Summing the quantities: order.OrderItems.Sum(oi => oi.Quantity).

            var orders = await _dbContext.Orders
                .Include(o => o.Customer) // Include the associated customer
                .Include(o => o.OrderItems) // Include the associated order items
                .ToListAsync();

            Console.WriteLine(" ");
            Console.WriteLine("=== TASK 02: List Orders With Item Count ===");

            // Iterate through each order to print details
            foreach (var order in orders)
            {
                string customerName = $"{order.Customer.FirstName} {order.Customer.LastName}";
                int orderId = order.OrderId;
                string orderStatus = order.OrderStatus;
                int itemCount = order.OrderItems.Sum(oi => oi.Quantity);

                // Print the order details to the console
                Console.WriteLine($"Order ID: {orderId} | Customer: {customerName} | Status: {orderStatus} | Items: {itemCount}");
            }
        }

        /// <summary>
        /// 3. List all products (ProductName, Price),
        ///    sorted by price descending (highest first).
        /// </summary>
        public async Task Task03_ListProductsByDescendingPrice()
        {// Fetching all products and sorting them by descending price
            var products = await _dbContext.Products
                .OrderByDescending(p => p.Price) // Sorting products by price in descending order
                .ToListAsync();

            Console.WriteLine("=== Task 03: List Products By Descending Price ===");

            // Iterate through each product and print its name and price
            foreach (var product in products)
            {
                Console.WriteLine($"Product: {product.ProductName} | Price: {product.Price:C}"); // Format as currency
            }
        }

        /// <summary>
        /// 4. Find all "Pending" orders (order status = "Pending")
        ///    and display:
        ///      - Customer Name
        ///      - Order ID
        ///      - Order Date
        ///      - Total price (sum of unit_price * quantity - discount) for each order
        /// </summary>
        public async Task Task04_ListPendingOrdersWithTotalPrice()
        {
            // TODO: Write a query to fetch only PENDING orders,
            //       and calculate their total price.
            // HINT: The total can be computed from each OrderItem:
            //       (oi.UnitPrice * oi.Quantity) - oi.Discount
            // Fetching all "Pending" orders along with their related customer and order items
            var pendingOrders = await _dbContext.Orders
                .Where(o => o.OrderStatus == "Pending") // Filtering orders with "Pending" status
                .Include(o => o.Customer) // Including customer details
                .Include(o => o.OrderItems) // Including order items
                .ToListAsync();

            Console.WriteLine("=== Task 04: List Pending Orders With Total Price ===");

            // Iterate through each pending order and display the details
            foreach (var order in pendingOrders)
            {
                // Construct the customer name
                string customerName = $"{order.Customer.FirstName} {order.Customer.LastName}";
                int orderId = order.OrderId;
                DateTime orderDate = (DateTime)order.OrderDate;

                // Calculate the total price for the order by summing the price of all items
                decimal totalPrice = (decimal)order.OrderItems
                    .Sum(oi => (oi.UnitPrice * oi.Quantity) - oi.Discount); // Calculating the total price for the order

                // Output the details
                Console.WriteLine($"Customer: {customerName} | Order ID: {orderId} | Date: {orderDate} | Total Price: {totalPrice:C}");
            }
        }

        /// <summary>
        /// 5. List the total number of orders each customer has placed.
        ///    Output should show:
        ///      - Customer Full Name
        ///      - Number of Orders
        /// </summary>
        public async Task Task05_OrderCountPerCustomer()
        {
            // TODO: Write a query that groups by Customer,
            //       counting the number of orders each has.

            // HINT: 
            //  1) Join Orders and Customers, or
            //  2) Use the navigation (context.Orders or context.Customers),
            //     then group by customer ID or by the customer entity.

            // Query to group by Customer and count the number of orders for each customer
            var orderCountPerCustomer = await _dbContext.Customers
                .Select(c => new
                {
                    FullName = $"{c.FirstName} {c.LastName}", // Concatenate first and last name
                    OrderCount = c.Orders.Count() // Count the number of orders placed by the customer
                })
                .ToListAsync();

            Console.WriteLine("=== Task 05: Order Count Per Customer ===");

            // Output the customer full name and their order count
            foreach (var customer in orderCountPerCustomer)
            {
                Console.WriteLine($"{customer.FullName}: {customer.OrderCount} Orders");
            }
        }

        /// <summary>
        /// 6. Show the top 3 customers who have placed the highest total order value overall.
        ///    - For each customer, calculate SUM of (OrderItems * Price).
        ///      Then pick the top 3.
        /// </summary>
        public async Task Task06_Top3CustomersByOrderValue()
        {
            // TODO: Calculate each customer's total order value 
            //       using their Orders -> OrderItems -> (UnitPrice * Quantity - Discount).
            //       Sort descending and take top 3.

            // HINT: You can do this in a single query or multiple steps.
            //       One approach:
            //         1) Summarize each Order's total
            //         2) Summarize for each Customer
            //         3) Order by descending total
            //         4) Take(3)

            // Query to calculate the total order value for each customer
            var topCustomers = await _dbContext.Customers
                .Select(c => new
                {
                    FullName = $"{c.FirstName} {c.LastName}", // Concatenate first and last name
                    TotalOrderValue = c.Orders
                        .SelectMany(o => o.OrderItems) // Flatten all OrderItems in each Order
                        .Sum(oi => (oi.UnitPrice * oi.Quantity) - oi.Discount) // Calculate total value for each OrderItem
                })
                .OrderByDescending(c => c.TotalOrderValue) // Order by the total order value descending
                .Take(3) // Take the top 3 customers
                .ToListAsync();

            Console.WriteLine("=== Task 06: Top 3 Customers By Order Value ===");

            // Output the full name and the total order value of the top 3 customers
            foreach (var customer in topCustomers)
            {
                Console.WriteLine($"{customer.FullName}: ${customer.TotalOrderValue:F2}");
            }
        }

        /// <summary>
        /// 7. Show all orders placed in the last 30 days (relative to now).
        ///    - Display order ID, date, and customer name.
        /// </summary>
        public async Task Task07_RecentOrders()
        {
            // TODO: Filter orders to only those with OrderDate >= (DateTime.Now - 30 days).
            //       Output ID, date, and the customer's name.

            // Get all orders in the last 30 days, along with the related customer
            var recentOrders = await _dbContext.Orders
                .Where(o => o.OrderDate >= DateTime.Now.AddDays(-30)) // Filter for the last 30 days
                .Include(o => o.Customer)  // Include the Customer data
                .ToListAsync();  // Fetch the orders and related customer data

            // Process and select the relevant information for each order
            var result = recentOrders
                .Select(o => new
                {
                    CustomerName = $"{o.Customer.FirstName} {o.Customer.LastName}",
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate
                })
                .OrderByDescending(r => r.OrderDate)  // Order by the most recent order date
                .ToList();  // Execute the query and bring the data into memory

            // Display the results
            Console.WriteLine("=== Task 07: Recent Orders ===");

            foreach (var order in result)
            {
                Console.WriteLine($"Customer: {order.CustomerName}, Order ID: {order.OrderId}, Order Date: {order.OrderDate}");
            }
        }

        /// <summary>
        /// 8. For each product, display how many total items have been sold
        ///    across all orders.
        ///    - Product name, total sold quantity.
        ///    - Sort by total sold descending.
        /// </summary>
        public async Task Task08_TotalSoldPerProduct()
        {
            // TODO: Group or join OrdersItems by Product.
            //       Summation of quantity.

            // Load all order items with the related products
            var orderItems = await _dbContext.OrderItems
                .Include(oi => oi.Product)
                .ToListAsync();  // Fetch the data into memory first

            // Process the data on the client side
            var result = orderItems
                .GroupBy(oi => oi.Product)  // Group by the Product
                .Select(g => new
                {
                    ProductName = g.Key.ProductName,  // Use the correct property name here
                    TotalQuantitySold = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(r => r.TotalQuantitySold)  // Sort by total quantity sold
                .ToList();  // Execute the query on the client side

            // Display the results
            Console.WriteLine("=== Task 08: Total Sold Per Product ===");

            foreach (var item in result)
            {
                Console.WriteLine($"Product: {item.ProductName}, Total Quantity Sold: {item.TotalQuantitySold}");
            }
        }

        /// <summary>
        /// 9. List any orders that have at least one OrderItem with a Discount > 0.
        ///    - Show Order ID, Customer name, and which products were discounted.
        /// </summary>
        public async Task Task09_DiscountedOrders()
        {
            // TODO: Identify orders with any OrderItem having (Discount > 0).
            //       Display order details, plus the discounted products.

            var discountedOrders = await _dbContext.Orders
        .Where(o => o.OrderItems.Any(oi => oi.Discount > 0)) // Filter orders with at least one discounted item
        .Include(o => o.Customer) // Include customer information
        .Include(o => o.OrderItems) // Include order items
        .ThenInclude(oi => oi.Product) // Include the products in the order items
        .Select(o => new
        {
            o.OrderId,
            CustomerName = $"{o.Customer.FirstName} {o.Customer.LastName}",
            DiscountedProducts = o.OrderItems.Where(oi => oi.Discount > 0).Select(oi => oi.Product.ProductName)
        })
        .ToListAsync();

            Console.WriteLine("=== Task 09: Discounted Orders ===");

            foreach (var order in discountedOrders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Customer: {order.CustomerName}");
                Console.WriteLine("Discounted Products:");
                foreach (var product in order.DiscountedProducts)
                {
                    Console.WriteLine($"- {product}");
                }
            }
        }

        /// <summary>
        /// 10. (Open-ended) Combine multiple joins or navigation properties
        ///     to retrieve a more complex set of data. For example:
        ///     - All orders that contain products in a certain category
        ///       (e.g., "Electronics"), including the store where each product
        ///       is stocked most. (Requires `Stocks`, `Store`, `ProductCategory`, etc.)
        ///     - Or any custom scenario that spans multiple tables.
        /// </summary>
        public async Task Task10_AdvancedQueryExample()
        {
            // TODO: Design your own complex query that demonstrates
            //       multiple joins or navigation paths. For example:
            //       - Orders that contain any product from "Electronics" category.
            //       - Then, find which store has the highest stock of that product.
            //       - Print results.

            // Here's an outline you could explore:
            // 1) Filter products by category name "Electronics"
            // 2) Find any orders that contain these products
            // 3) For each of those products, find the store with the max stock
            //    (requires .MaxBy(...) in .NET 6+ or custom code in older versions)
            // 4) Print a combined result

            // (Implementation is left as an exercise.)

            // Step 1: Get the current date and the date 90 days ago.
            var thirtyDaysAgo = DateTime.Now.AddDays(-90);

            // Step 2: Fetch orders placed within the last 90 days, along with related customer, order items, and product data.
            var recentOrders = await _dbContext.Orders
                .Where(o => o.OrderDate >= thirtyDaysAgo)  // Orders placed in the last 90 days
                .Include(o => o.Customer)  // Include Customer data
                .Include(o => o.OrderItems)  // Include OrderItems data
                .Where(o => o.Customer.FirstName == "Anna" && o.Customer.LastName == "Svensson")  // Filter by customer name
                .ToListAsync();

            // Step 3: Group by customer and order, and calculate the total order value, including product name.
            var result = recentOrders
                .GroupBy(o => new { o.Customer.CustomerId, o.OrderId, o.Customer.FirstName, o.Customer.LastName })
                .Select(g => new
                {
                    CustomerName = $"{g.First().Customer.FirstName} {g.First().Customer.LastName}",
                    OrderId = g.Key.OrderId,
                    OrderDate = g.First().OrderDate,
                    TotalOrderValue = g.Sum(oi => oi.OrderItems.Sum(item => item.Quantity * item.UnitPrice)),

                    // Retrieve product names for each order item by joining with Product table
                    ProductNames = g.SelectMany(o => o.OrderItems)
                                    .Join(_dbContext.Products,
                                          oi => oi.ProductId,
                                          p => p.ProductId,
                                          (oi, p) => p.ProductName) // Join with the Product table and get product names
                                    .Distinct()  // Get distinct product names for each order
                                    .ToList()  // Convert the result to a list of product names
                })
                .OrderByDescending(r => r.OrderDate)  // Order by the most recent order
                .ToList();

            // Step 4: Print results
            Console.WriteLine("=== Task 10: Recent Orders (Last 90 Days) ===");

            foreach (var order in result)
            {
                Console.WriteLine($"Customer: {order.CustomerName}, Order ID: {order.OrderId}, Order Date: {order.OrderDate}, Total Value: {order.TotalOrderValue:C2}");
                Console.WriteLine("Products Ordered:");
                foreach (var productName in order.ProductNames)
                {
                    Console.WriteLine($" - {productName}");
                }
            }
        }

        }
}
