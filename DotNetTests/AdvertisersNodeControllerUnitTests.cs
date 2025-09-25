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
            // ���������� ����� -> ������
            var emptyResponse = "Null or empty file";
            var response = _controller.SetAdvertisers(null);
            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(((BadRequestObjectResult)response).Value, Is.EqualTo(emptyResponse));

            // ������ ���� -> ������
            var bytes = Encoding.UTF8.GetBytes("");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            response = _controller.SetAdvertisers(file);
            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(((BadRequestObjectResult)response).Value, Is.EqualTo(emptyResponse));

            // ���������� ���� -> Ok
            bytes = Encoding.UTF8.GetBytes("�����1:/ru");
            file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            response = _controller.SetAdvertisers(file);
            Assert.That(response, Is.InstanceOf<OkResult>());

            // ���� � ���������� ������� -> Ok
            bytes = Encoding.UTF8.GetBytes("�����1:/ru\n�����2");
            file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            response = _controller.SetAdvertisers(file);
            Assert.That(response, Is.InstanceOf<OkResult>());
        }

        [Test]
        public void GetAdvertisersTest()
        {
            var bytes = Encoding.UTF8.GetBytes("�����1:/ru\n�����2:/ru/1\n�����3:/ru/1/2\n�����4:/ru/1/2,/ru/3/4\n");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            _controller.SetAdvertisers(file);

            // /ru -> ������ �����1
            var response = _controller.GetAdvertisers("/ru");
            Assert.That(response, Is.EquivalentTo(new[] { "�����1" }));

            // /ru/1 -> �����1, �����2
            response = _controller.GetAdvertisers("/ru/1");
            Assert.That(response, Is.EquivalentTo(new[] { "�����1", "�����2" }));

            // /ru/1/2 -> �����1, �����2, �����3, �����4
            response = _controller.GetAdvertisers("/ru/1/2");
            Assert.That(response, Is.EquivalentTo(new[] { "�����1", "�����2", "�����3", "�����4" }));

            // /ru/3/4 -> �����1, �����4
            response = _controller.GetAdvertisers("/ru/3/4");
            Assert.That(response, Is.EquivalentTo(new[] { "�����1", "�����4" }));
        }
    }
}