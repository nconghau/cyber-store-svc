using System.Text.Json;
using DotnetApiPostgres.Api.Mediator.Commands.Admin;
using DotnetApiPostgres.Api.Mediator.Queries.Web;
using DotnetApiPostgres.Api.Models.Common;
using DotnetApiPostgres.Api.Models.DTOs;
using DotnetApiPostgres.Api.Models.Entities;
using DotnetApiPostgres.Api.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiPostgres.Api.Models;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IMediator mediator, ILogger<ProductController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<JsonResponse<List<Product>>> Init()
    {
        var initData = new List<ProductDto>()
        {
            new ProductDto()
            {
                Id=IdGenerator.GenerateId(),
                Name="Apple iPhone 14 Pro Max",
                Price=1499,
                DiscountedPrice=1399,
                ShortDescription="Enhanced capabilities thanks toan enlarged display of 6.7 inchesand work without rechargingthroughout the day. Incredible photosas in weak, yesand in bright lightusing the new systemwith two cameras.",
                Description="<p><strong>Details</strong></p><p>Just as a book is judged by its cover, the first thing you notice when you pick up a modern smartphone is the display. Nothing surprising, because advanced technologies allow you to practically level the display frames and cutouts for the front camera and speaker, leaving no room for bold design solutions. And how good that in such realities Apple everything is fine with displays. Both critics and mass consumers always praise the quality of the picture provided by the products of the Californian brand. And last year's 6.7-inch Retina panels, which had ProMotion, caused real admiration for many.</p><p></p><p><strong>Screen</strong></p><p>Screen diagonal6.7' The screen resolution2796x1290The screen refresh rate120 HzThe pixel density460 ppiScreen typeOLEDAdditionallyDynamic Island\nAlways-On display\nHDR display\nTrue Tone\nWide color (P3)</p><p></p><p><strong>CPU</strong></p><p>CPUA16 BionicNumber of cores6</p>",
                Tags=JsonSerializer.Serialize(new[] { "NewArrival", "Bestseller", "50%" }),
                Brand="Apple",
                ImgUrl="https://res.cloudinary.com/dkeupjars/image/upload/v1734360880/cyber_store_project/Iphone_14_pro_1_vyyak6.png",
                ImgUrls=JsonSerializer.Serialize(new[] {
                    "https://res.cloudinary.com/dkeupjars/image/upload/v1734362487/cyber_store_project/Image_au9qul.png",
                    "https://res.cloudinary.com/dkeupjars/image/upload/v1734362485/cyber_store_project/image_63_zpaof4.png",
                    "https://res.cloudinary.com/dkeupjars/image/upload/v1734362485/cyber_store_project/image_61_szh2ce.png",
                    "https://res.cloudinary.com/dkeupjars/image/upload/v1734362485/cyber_store_project/image_62_hrfev0.png"
                }),
                Properties = JsonSerializer.Serialize(new
                {
                    colors = new[]
                    {
                        new { name = "Black", hexCode = "#000000" },
                        new { name = "Purple", hexCode = "#800080" },
                        new { name = "Gold", hexCode = "#FFD700" },
                        new { name = "Silver", hexCode = "#C0C0C0" }
                    },
                    storageOptions = new[]
                    {
                        new { capacity = "128GB", isSelected = false },
                        new { capacity = "256GB", isSelected = true },
                        new { capacity = "512GB", isSelected = false },
                        new { capacity = "1TB", isSelected = false }
                    },
                    specifications = new[]
                    {
                        new { name = "Screen size", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363308/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_1_ikcqgy.svg", value = "6.7'" },
                        new { name = "CPU", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363308/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_1_ikcqgy.svg", value = "Apple A16 Bionic" },
                        new { name = "Number of Cores", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363321/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_ljtxls.svg", value = "6" },
                        new { name = "Main camera", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363319/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_2_cka9eq.svg", value = "48-12-12 MP" },
                        new { name = "Front-camera", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363320/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_3_woxnf4.svg", value = "12 MP" },
                        new { name = "Battery capacity", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363320/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_4_blf3kp.svg", value = "4323 mAh" }
                    },
                    deliveryInfo = new
                    {
                        freeDelivery = "1-2 days",
                        inStock = "Today",
                        guaranteed = "1 year"
                    },
                    availability = new
                    {
                        addToWishlist = true,
                        addToCart = true
                    }
                })
            },
            new ProductDto()
            {
                Id = IdGenerator.GenerateId(),
                Name = "Apple iPhone 14 Pro Max 128GB",
                Price = 1599,
                DiscountedPrice = 1499,
                ShortDescription = "Enhanced 6.7-inch display with all-day battery life.",
                Description = "<p><strong>Details</strong></p><p>Just as a book is judged by its cover...</p>",
                Tags = JsonSerializer.Serialize(new[] { "NewArrival", "50%" }),
                Brand = "Apple",
                ImgUrl = "https://res.cloudinary.com/dkeupjars/image/upload/v1734360880/cyber_store_project/Iphone_14_pro_1_vyyak6.png",
                Properties = JsonSerializer.Serialize(new
                {
                    colors = new[]
                    {
                        new { name = "Black", hexCode = "#000000" },
                        new { name = "Purple", hexCode = "#800080" },
                        new { name = "Gold", hexCode = "#FFD700" },
                        new { name = "Silver", hexCode = "#C0C0C0" }
                    },
                    storageOptions = new[]
                    {
                        new { capacity = "128GB", isSelected = false },
                        new { capacity = "256GB", isSelected = true },
                        new { capacity = "512GB", isSelected = false },
                        new { capacity = "1TB", isSelected = false }
                    },
                    specifications = new[]
                    {
                        new { name = "Screen size", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363308/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_1_ikcqgy.svg", value = "6.7'" },
                        new { name = "CPU", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363308/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_1_ikcqgy.svg", value = "Apple A16 Bionic" },
                        new { name = "Number of Cores", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363321/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_ljtxls.svg", value = "6" },
                        new { name = "Main camera", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363319/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_2_cka9eq.svg", value = "48-12-12 MP" },
                        new { name = "Front-camera", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363320/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_3_woxnf4.svg", value = "12 MP" },
                        new { name = "Battery capacity", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363320/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_4_blf3kp.svg", value = "4323 mAh" }
                    },
                    deliveryInfo = new
                    {
                        freeDelivery = "1-2 days",
                        inStock = "Today",
                        guaranteed = "1 year"
                    },
                    availability = new
                    {
                        addToWishlist = true,
                        addToCart = true
                    }
                })
            },
            new ProductDto()
            {
                Id = IdGenerator.GenerateId(),
                Name = "Samsung Galaxy S23 Ultra",
                Price = 1799,
                DiscountedPrice = 1699,
                ShortDescription = "Powerful Snapdragon processor with stunning AMOLED display.",
                Description = "<p><strong>Details</strong></p><p>With an ultra-fast processor and stunning 120Hz AMOLED display...</p>",
                Tags = JsonSerializer.Serialize(new[] { "Bestseller", "30%" }),
                Brand = "Samsung",
                ImgUrl = "https://res.cloudinary.com/dkeupjars/image/upload/v1734360881/cyber_store_project/S23_ultra_vz2pjn.png",
                Properties = JsonSerializer.Serialize(new
                {
                    colors = new[]
                    {
                        new { name = "Black", hexCode = "#000000" },
                        new { name = "Purple", hexCode = "#800080" },
                        new { name = "Gold", hexCode = "#FFD700" },
                        new { name = "Silver", hexCode = "#C0C0C0" }
                    },
                    storageOptions = new[]
                    {
                        new { capacity = "128GB", isSelected = false },
                        new { capacity = "256GB", isSelected = true },
                        new { capacity = "512GB", isSelected = false },
                        new { capacity = "1TB", isSelected = false }
                    },
                    specifications = new[]
                    {
                        new { name = "Screen size", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363308/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_1_ikcqgy.svg", value = "6.7'" },
                        new { name = "CPU", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363308/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_1_ikcqgy.svg", value = "Apple A16 Bionic" },
                        new { name = "Number of Cores", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363321/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_ljtxls.svg", value = "6" },
                        new { name = "Main camera", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363319/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_2_cka9eq.svg", value = "48-12-12 MP" },
                        new { name = "Front-camera", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363320/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_3_woxnf4.svg", value = "12 MP" },
                        new { name = "Battery capacity", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363320/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_4_blf3kp.svg", value = "4323 mAh" }
                    },
                    deliveryInfo = new
                    {
                        freeDelivery = "1-2 days",
                        inStock = "Today",
                        guaranteed = "1 year"
                    },
                    availability = new
                    {
                        addToWishlist = true,
                        addToCart = true
                    }
                })
            },
            new ProductDto()
            {
                Id = IdGenerator.GenerateId(),
                Name = "Google Pixel 8 Pro",
                Price = 1299,
                DiscountedPrice = 1199,
                ShortDescription = "AI-enhanced camera with real-time processing.",
                Description = "<p><strong>Details</strong></p><p>Capture professional photos with ease using Google's AI camera technology...</p>",
                Tags = JsonSerializer.Serialize(new[] { "Popular", "20%" }),
                Brand = "Google",
                ImgUrl = "https://res.cloudinary.com/dkeupjars/image/upload/v1734360882/cyber_store_project/pixel_8_pro_1_mgslmn.png",
                Properties = JsonSerializer.Serialize(new
                {
                    colors = new[]
                    {
                        new { name = "Black", hexCode = "#000000" },
                        new { name = "Purple", hexCode = "#800080" },
                        new { name = "Gold", hexCode = "#FFD700" },
                        new { name = "Silver", hexCode = "#C0C0C0" }
                    },
                    storageOptions = new[]
                    {
                        new { capacity = "128GB", isSelected = false },
                        new { capacity = "256GB", isSelected = true },
                        new { capacity = "512GB", isSelected = false },
                        new { capacity = "1TB", isSelected = false }
                    },
                    specifications = new[]
                    {
                        new { name = "Screen size", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363308/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_1_ikcqgy.svg", value = "6.7'" },
                        new { name = "CPU", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363308/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_1_ikcqgy.svg", value = "Apple A16 Bionic" },
                        new { name = "Number of Cores", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363321/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_ljtxls.svg", value = "6" },
                        new { name = "Main camera", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363319/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_2_cka9eq.svg", value = "48-12-12 MP" },
                        new { name = "Front-camera", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363320/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_3_woxnf4.svg", value = "12 MP" },
                        new { name = "Battery capacity", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363320/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_4_blf3kp.svg", value = "4323 mAh" }
                    },
                    deliveryInfo = new
                    {
                        freeDelivery = "1-2 days",
                        inStock = "Today",
                        guaranteed = "1 year"
                    },
                    availability = new
                    {
                        addToWishlist = true,
                        addToCart = true
                    }
                })
            },
            new ProductDto()
            {
                Id = IdGenerator.GenerateId(),
                Name = "OnePlus 11 5G",
                Price = 899,
                DiscountedPrice = 849,
                ShortDescription = "Powerful performance and long-lasting battery life.",
                Description = "<p><strong>Details</strong></p><p>With a 5G processor, this phone delivers high speeds and amazing performance...</p>",
                Tags = JsonSerializer.Serialize(new[] { "NewArrival", "10%" }),
                Brand = "OnePlus",
                ImgUrl = "https://res.cloudinary.com/dkeupjars/image/upload/v1734360883/cyber_store_project/OnePlus_11_5G_z8q7on.png",
                Properties = JsonSerializer.Serialize(new
                {
                    colors = new[]
                    {
                        new { name = "Black", hexCode = "#000000" },
                        new { name = "Purple", hexCode = "#800080" },
                        new { name = "Gold", hexCode = "#FFD700" },
                        new { name = "Silver", hexCode = "#C0C0C0" }
                    },
                    storageOptions = new[]
                    {
                        new { capacity = "128GB", isSelected = false },
                        new { capacity = "256GB", isSelected = true },
                        new { capacity = "512GB", isSelected = false },
                        new { capacity = "1TB", isSelected = false }
                    },
                    specifications = new[]
                    {
                        new { name = "Screen size", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363308/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_1_ikcqgy.svg", value = "6.7'" },
                        new { name = "CPU", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363308/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_1_ikcqgy.svg", value = "Apple A16 Bionic" },
                        new { name = "Number of Cores", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363321/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_ljtxls.svg", value = "6" },
                        new { name = "Main camera", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363319/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_2_cka9eq.svg", value = "48-12-12 MP" },
                        new { name = "Front-camera", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363320/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_3_woxnf4.svg", value = "12 MP" },
                        new { name = "Battery capacity", icon = "https://res.cloudinary.com/dkeupjars/image/upload/v1734363320/cyber_store_project/smartphone-rotate-2-svgrepo-com_2_4_blf3kp.svg", value = "4323 mAh" }
                    },
                    deliveryInfo = new
                    {
                        freeDelivery = "1-2 days",
                        inStock = "Today",
                        guaranteed = "1 year"
                    },
                    availability = new
                    {
                        addToWishlist = true,
                        addToCart = true
                    }
                })
            },
            new ProductDto()
            {
                Id = IdGenerator.GenerateId(),
                Name = "Xiaomi Mi 13 Pro",
                Price = 999,
                DiscountedPrice = 899,
                ShortDescription = "Flagship-grade performance with a stunning AMOLED screen.",
                Description = "<p><strong>Details</strong></p><p>The Mi 13 Pro combines cutting-edge performance with a beautiful AMOLED display...</p>",
                Tags = JsonSerializer.Serialize(new[] { "Bestseller", "25%" }),
                Brand = "Xiaomi",
                ImgUrl = "https://res.cloudinary.com/dkeupjars/image/upload/v1734360884/cyber_store_project/Mi_13_Pro_vbb8ve.png",
            },
            new ProductDto()
            {
                Id = IdGenerator.GenerateId(),
                Name = "Oppo Find X5 Pro",
                Price = 1249,
                DiscountedPrice = 1199,
                ShortDescription = "Innovative camera and smooth performance for daily use.",
                Description = "<p><strong>Details</strong></p><p>The Oppo Find X5 Pro is designed for photography enthusiasts...</p>",
                Tags = JsonSerializer.Serialize(new[] { "Popular", "15%" }),
                Brand = "Oppo",
                ImgUrl = "https://res.cloudinary.com/dkeupjars/image/upload/v1734360885/cyber_store_project/Oppo_Find_X5_Pro_qir4du.png",
            },
            new ProductDto()
            {
                Id = IdGenerator.GenerateId(),
                Name = "Sony Xperia 1 IV",
                Price = 1699,
                DiscountedPrice = 1599,
                ShortDescription = "For professional photographers and content creators.",
                Description = "<p><strong>Details</strong></p><p>The Xperia 1 IV offers unmatched camera capabilities for creatives...</p>",
                Tags = JsonSerializer.Serialize(new[] { "NewArrival", "40%" }),
                Brand = "Sony",
                ImgUrl = "https://res.cloudinary.com/dkeupjars/image/upload/v1734360886/cyber_store_project/Sony_Xperia_1_IV_jljjdu.png",
            },
            new ProductDto()
            {
                Id = IdGenerator.GenerateId(),
                Name = "Asus ROG Phone 7",
                Price = 799,
                DiscountedPrice = 749,
                ShortDescription = "A gaming phone with top-tier performance and a high refresh rate.",
                Description = "<p><strong>Details</strong></p><p>Get ready for a smoother gaming experience with the Asus ROG Phone 7...</p>",
                Tags = JsonSerializer.Serialize(new[] { "Bestseller", "5%" }),
                Brand = "Asus",
                ImgUrl = "https://res.cloudinary.com/dkeupjars/image/upload/v1734360887/cyber_store_project/ROG_Phone_7_xxxplt.png",
            },
            new ProductDto()
            {
                Id = IdGenerator.GenerateId(),
                Name = "Motorola Edge 40",
                Price = 649,
                DiscountedPrice = 599,
                ShortDescription = "Mid-range performance with a 90Hz OLED display.",
                Description = "<p><strong>Details</strong></p><p>The Motorola Edge 40 provides solid performance at an affordable price...</p>",
                Tags = JsonSerializer.Serialize(new[] { "Popular", "10%" }),
                Brand = "Motorola",
                ImgUrl = "https://res.cloudinary.com/dkeupjars/image/upload/v1734360888/cyber_store_project/Motorola_Edge_40_xl9hhr.png",
            }
        };

        var jsonResponse = await _mediator.Send(new InitProductCommand()
        {
            Datas = initData
        });
        return jsonResponse;
    }

    [HttpPost]
    public async Task<JsonResponse<List<ProductDto>>> GetAllProduct([FromBody] GetAllProductQuery data)
    {
        var jsonResponse = await _mediator.Send(data);
        return jsonResponse;
    }

    [HttpPost]
    public async Task<PostgresDataSource<Product>> GetProductByQuery([FromBody] GetProductByQuery data)
    {
        var jsonResponse = await _mediator.Send(data);
        return jsonResponse;
    }

    [HttpPost]
    public async Task<JsonResponse<ProductDto>> GetProductByField([FromBody] GetProductByFieldQuery data)
    {
        var jsonResponse = await _mediator.Send(data);
        return jsonResponse;
    }

}