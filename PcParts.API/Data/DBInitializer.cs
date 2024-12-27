using PcParts.API.Data;
using PcParts.API.Models;

namespace SolarFlare.Data
{
    public class DBInitializer
    {
        public static void Initialize(BotContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var product1 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "AMD Ryzen 7 9800X3D, 4,7 GHz (5,2 GHz Turbo Boost) socket AM5 processor",
                Price = 649.00f,
                Quantity = 15,
                Description = "The AMD Ryzen 7 9800X3D processor, codenamed “Granite Ridge,” features eight cores and is suitable for fitting on an AM5 socket motherboard. The processor operates at a clock speed of 4700 MHz and a turbo mode up to a maximum of 5200 MHz. This boxed processor comes without a cooler. The AMD Ryzen 7 9800X3D processor is manufactured using the 4 nm FinFET process. In addition, enjoy the benefits of next-gen AMD 3D V-Cache technology for low latency and even more gaming performance.",
                Category = "CPU"
            };
            
            var product2 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Intel Core i9-14900K, 3,2 GHz (6,0 GHz Turbo Boost) socket 1700 processor",
                Price = 519.00f,
                Quantity = 24,
                Description = "The Intel Core i9-14900K, codenamed Raptor Lake-S, features 8 Performance cores and 16 Efficiency cores and is suitable for fitting on a socket 1700 motherboard. The processor features 36 MB Smart cache and operates at a speed of 3.2 GHz.",
                Category = "CPU"
            };
            
            var product3 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "ASUS ROG Strix GeForce RTX 4090 OC 24GB graphic card",
                Price = 2799.00f,
                Quantity = 4,
                Description = "The ASUS ROG Strix GeForce RTX 4090 OC 24GB is a high-end graphics card based on the NVIDIA GeForce RTX 4090 chip and features 24GB of GDDR6X memory addressed via a 384-bit wide interface. The GPU has a clock speed (OC) of 2640 MHz, a boost speed (gaming) of 2610 MHz and the memory has a speed of 21000 MHz. The GeForce RTX 4090 delivers the peak performance gamers crave, powered by NVIDIA's Ada Lovelace RTX architecture. It is built with improved 3rd generation RT Cores and 4th generation Tensor Cores, new streaming multiprocessors and super-fast GDDR6X memory for a great gaming experience.",
                Category = "Graphic Card"
            };
            
            var product4 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "GIGABYTE GeForce RTX 4080 SUPER GAMING OC 16G graphic card",
                Price = 1249.00f,
                Quantity = 8,
                Description = "The GIGABYTE GeForce RTX 4080 SUPER GAMING OC 16G is a high-end graphics card based on the NVIDIA GeForce RTX 4080 SUPER chip and features 16 GB of GDDR6X memory addressed via a 256-bit wide interface. The GPU has a boost speed of 2595 MHz and the memory has a speed of 23000 MHz.",
                Category = "Graphic Card"
            };
            
            var product5 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "SAMSUNG 990 PRO 4 TB SSD",
                Price = 319.00f,
                Quantity = 35,
                Description = "Samsung's 990 PRO SSD has a storage capacity of 4 TB. This M.2 SSD has a read speed of 7450 MB/sec and a write speed of 6900 MB/sec. The SSD also features an M.2 (2280) build with a PCIe Gen 4.0 x4, NVMe 2.0 interface. Samsung's 990 PRO series M.2 SSD combines Samsung V7 V-NAND 3-bit MLC with the Samsung controller for stunning read/write speeds to create a new standard for high-end gaming and 4K & 3D video editing.",
                Category = "SSD"
            };
            
            var product6 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "SAMSUNG 970 EVO Plus 2 TB SSD",
                Price = 139.90f,
                Quantity = 70,
                Description = "Samsung's 970 EVO Plus has a storage capacity of 2 TB. This M.2 SSD has a read speed of 3500 MB/sec and a write speed of 3300 MB/sec. The SSD also features an M.2 (2280) build with a PCIe Gen 3.0 x4, NVMe 1.3 interface. Samsung's 970 EVO Plus series M.2 SSD combines Samsung V-NAND 3-bit MLC with the Samsung Phoenix controller for stunning read/write speeds, creating a new standard for high-end gaming and 4K & 3D video editing.",
                Category = "SSD"
            };
            
            var product7 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "ASUS ROG MAXIMUS Z890 EXTREME socket 1851 motherboard",
                Price = 1649.00f,
                Quantity = 25,
                Description = "The ASUS ROG MAXIMUS Z890 EXTREME motherboard is based on the Intel Z890 chipset and supports Intel processors for socket 1851. It has four DDR5 slots for up to 192 GB of RAM. The ASUS ROG MAXIMUS Z890 EXTREME is also equipped with two PCIe 5.0 x16 slots and one PCIe 4.0 x16 slot. The ASUS ROG MAXIMUS Z890 EXTREME also features 8-channel sound, a 2.5 Gigabit LAN interface, Wi-Fi 7, Bluetooth 5.4, four SATA3 ports, four M.2 ports, five USB-A 3.2 (10 Gbit/s), two USB-C 3.2 (10 Gbit/s) interfaces and one USB-C 3.2 (20 Gbit/s) interface.",
                Category = "Motherboard"
            };
            
            var product8 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "MSI MAG B650 TOMAHAWK WIFI socket AM5 motherboard",
                Price = 209.90f,
                Quantity = 25,
                Description = "The MSI MAG B650 TOMAHAWK WIFI motherboard is based on the AMD B650 chipset and supports AMD processors for socket AM5. This motherboard has four dual-channel DDR5 DIMM slots for up to 128 GB of RAM. Other features of the MSI MAG B650 TOMAHAWK WIFI include two PCIe 4.0 x16 slots and one PCIe 3.0 x1 slot. In addition, the MSI MAG B650 TOMAHAWK WIFI features 8-channel sound, a 2.5 Gigabit LAN interface, WLAN, Bluetooth, six SATA3 ports, three M.2 ports, two USB-A 2.0, four USB-A 3.2 (5 Gbit/s) and three USB-A 3.2 (10 Gbit/s) interfaces.",
                Category = "Motherboard"
            };
            
            context.Products.AddRange(product1, product2, product3, product4, product5, product6, product7, product8);
            context.SaveChanges();

            var order1 = new Order
            {
                Id = Guid.NewGuid(),
                Name = "Jan Janssens",
                Email = "janjanssens@gmail.com",
                Phone = "32468799972",
                Street = "Kerkstraat",
                City = "Antwerpen",
                ZipCode = "2000",
                IsDelivery = true,
                Products = new List<Product> { product2, product3, product7 }
            };
            
            var order2 = new Order
            {
                Id = Guid.NewGuid(),
                Email = "johndoe@gmail.com",
                Name = "John Doe",
                Phone = "32468799974",
                Street = "Nieuwelaan",
                City = "Brussel",
                ZipCode = "1000",
                IsDelivery = true,
                Products = new List<Product> { product6 }
            };
            
            var order3 = new Order
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe",
                Email = "janedoe@gmail.com",
                Phone = "32468799976",
                Street = "Lentestraat",
                City = "Antwerpen",
                ZipCode = "2000",
                IsDelivery = false,
                Products = new List<Product> { product3, product4 }
            };
            
            context.Orders.AddRange(order1, order2, order3);
            context.SaveChanges();
        }
    }
}
