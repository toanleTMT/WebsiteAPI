using Domains.Kafkas.EventDatas.Consumers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebsiteKafka.Kafkas;
using WebsiteKafka.Models;

namespace WebsiteKafka.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IKafkaProducerHandle _kafkaProducerHandle;

        public HomeController(ILogger<HomeController> logger,
            IKafkaProducerHandle kafkaProducerHandle)
        {
            _logger = logger;

            _kafkaProducerHandle = kafkaProducerHandle;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = new SendMessageDto
            {
                Id = 10,
                Name = "Tạo hóa đơn 10",
                Host = "localhost:toan",
                Topic = "message.apache.toan"
            };

            //await _kafkaProducerHandle.ProducerSendMessage(data); 
            return Ok(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}