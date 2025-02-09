using Bogus;
using CyberStoreSVC.Models.DTOs;

namespace CyberStoreSVC.Utils
{
    public static class DataGenerator
	{
        public static string GenerateVietnamesePhoneNumber(Faker faker)
        {
            string[] vietNamPrefixes = { "032", "033", "034", "035", "036", "037", "038", "039",
                                     "070", "076", "077", "078", "079", "081", "082", "083",
                                     "084", "085", "086", "087", "088", "089", "090", "091",
                                     "092", "093", "094", "096", "097", "098", "099" };

            string prefix = faker.PickRandom(vietNamPrefixes);
            string lastPart = faker.Random.Replace("#######"); // Generates a 7-digit random number

            return $"{prefix}{lastPart}";
        }

        public static List<OrderItemDTO> GenerateFakeOrderItems(int minItems, int maxItems)
        {
            var faker = new Faker("vi");
            int itemCount = faker.Random.Int(minItems, maxItems);

            var orderItems = new List<OrderItemDTO>();

            for (int i = 0; i < itemCount; i++)
            {
                orderItems.Add(new OrderItemDTO
                {
                    Id = IdGenerator.GenerateId(),
                    ProductId = IdGenerator.GenerateId(),
                    ProductName = faker.Commerce.ProductName(), // Generates a random product name
                    Price = (int)faker.Random.Decimal(50000, 2000000), // Prices in VND
                    Qty = faker.Random.Int(1, 2)
                });
            }

            return orderItems;
        }
    }
}

