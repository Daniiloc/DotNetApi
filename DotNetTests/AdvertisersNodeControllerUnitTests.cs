using DotNetApi.Controllers;
using DotNetApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DotNetTests
{
    public class AdvertisersNodeControllerUnitTests
    {
        private IAdvertiserNodeService _advertiserNodeService;
        private AdvertisersNodeController _controller;

        [SetUp]
        public void Setup()
        {
            _advertiserNodeService = new AdvertiserNodeService();
            _controller = new AdvertisersNodeController(_advertiserNodeService);
        }

        [Test]
        public void SetAdvertisersTreeTest()
        {
            // Отсутствие файла -> Ошибка
            var emptyResponse = "Null or empty file";
            var response = _controller.SetAdvertisers(null);
            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(((BadRequestObjectResult)response).Value, Is.EqualTo(emptyResponse));

            // Пустой файл -> Ошибка
            var bytes = Encoding.UTF8.GetBytes("");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            response = _controller.SetAdvertisers(file);
            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(((BadRequestObjectResult)response).Value, Is.EqualTo(emptyResponse));

            // Корректный файл -> Ok
            bytes = Encoding.UTF8.GetBytes("Ответ1:/ru");
            file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            response = _controller.SetAdvertisers(file);
            Assert.That(response, Is.InstanceOf<OkResult>());

            // Файл с ошибочными данными -> Ok
            bytes = Encoding.UTF8.GetBytes("Ответ1:/ru\nОтвет2");
            file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            response = _controller.SetAdvertisers(file);
            Assert.That(response, Is.InstanceOf<OkResult>());
        }

        [Test]
        public void GetAdvertisersTest()
        {
            var bytes = Encoding.UTF8.GetBytes("Ответ1:/ru\nОтвет2:/ru/1\nОтвет3:/ru/1/2\nОтвет4:/ru/1/2,/ru/3/4\n");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            _controller.SetAdvertisers(file);

            // /ru -> только Ответ1
            var response = _controller.GetAdvertisers("/ru");
            Assert.That(response, Is.EquivalentTo(new[] { "Ответ1" }));

            // /ru/1 -> Ответ1, Ответ2
            response = _controller.GetAdvertisers("/ru/1");
            Assert.That(response, Is.EquivalentTo(new[] { "Ответ1", "Ответ2" }));

            // /ru/1/2 -> Ответ1, Ответ2, Ответ3, Ответ4
            response = _controller.GetAdvertisers("/ru/1/2");
            Assert.That(response, Is.EquivalentTo(new[] { "Ответ1", "Ответ2", "Ответ3", "Ответ4" }));

            // /ru/3/4 -> Ответ1, Ответ4
            response = _controller.GetAdvertisers("/ru/3/4");
            Assert.That(response, Is.EquivalentTo(new[] { "Ответ1", "Ответ4" }));
        }
    }
}