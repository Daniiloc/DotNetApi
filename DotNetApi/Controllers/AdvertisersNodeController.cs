using DotNetApi.Models;
using DotNetApi.Models.Helpers;
using DotNetApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertisersNodeController : ControllerBase
    {
        private readonly static List<AdvertiserNode> _rootAdvertiserNodes = [];
        private readonly IAdvertiserNodeService _advertiserNodeService;

        public AdvertisersNodeController(IAdvertiserNodeService advertiserNodeService)
        {
            _advertiserNodeService = advertiserNodeService;
        }

        [HttpPost("SetAdvertisers")]
        public IActionResult SetAdvertisers(IFormFile? file)
        {
            if (file == null || file.Length == 0) return BadRequest("Null or empty file");
            try
            {
                _rootAdvertiserNodes.Clear();
                var lines = new List<(string, string)>();
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    while (!stream.EndOfStream)
                    {
                        var line = stream.ReadLine()?.Split(":");
                        if (line != null && line.Length == 2 && line[1].TrimStart().StartsWith('/'))
                            line[1].Split(',').ToList().ForEach(location => lines.Add((line[0], location.ToLower().Trim())));
                    }
                }

                lines = lines.OrderBy(l => l.Item2.Length).ToList();
                lines.ForEach(line => _advertiserNodeService.AddNode(_rootAdvertiserNodes, line.Item2, line.Item1));
                _rootAdvertiserNodes.ForEach(_advertiserNodeService.FillAdvertisers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet("GetAdvertisers")]
        public string[] GetAdvertisers(string location)
        {
            location = location.ToLower().Trim();
            return _rootAdvertiserNodes.GetNodeByValueFromList(location, true)?.Advertisers.ToArray() ?? [];
        }
    }
}
