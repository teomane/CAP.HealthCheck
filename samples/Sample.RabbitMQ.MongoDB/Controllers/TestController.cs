using System;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sample.RabbitMQ.MongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ICapPublisher _capPublisher;
        private readonly ILogger<TestController> _logger;

        public TestController(ICapPublisher capPublisher, ILogger<TestController> logger)
        {
            _capPublisher = capPublisher;
            _logger = logger;
        }
        
        [HttpGet("publish")]
        public IActionResult Publish()
        {
            string data = $"SampleData at {DateTime.Now}";
            
            _capPublisher.Publish("cap.test.key", data);
            _logger.LogInformation($"Data Published: {data}");
            
            return Ok();
        }

        [CapSubscribe("cap.test.key")]
        public void Subscribe(string data)
        {
            _logger.LogInformation($"Data arrived: {data}");
        }
    }
}