using ImageProcessingApi.IRepository;
using ImageProcessingApi.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImageProcessingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        private static readonly Dictionary<int, ImageData> Images = new();
        private readonly IPluginManager _pluginManager;
        private readonly string _uploadFolder;

        public ImagesController(IPluginManager pluginManager)
        {
            _pluginManager = pluginManager;
            _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        [HttpPost("upload")]
        public IActionResult UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not provided.");
            }

            var id = Images.Count + 1;
            var image = ImageData.FromFormFile(id, _uploadFolder, file);
            Images[id] = image;
            return Ok(new { imageId = id });
        }

        [HttpGet("{id}")]
        public IActionResult GetImage(int id)
        {
            if (!Images.ContainsKey(id))
                return NotFound("Image not found");

            var image = Images[id];
            var imageBytes = System.IO.File.ReadAllBytes(image.FilePath);
            return File(imageBytes, "image/jpeg");
        }

        [HttpPost("{id}/apply")]
        public IActionResult ApplyEffects(int id, [FromBody] List<EffectRequest> effects)
        {
            if (!Images.ContainsKey(id))
                return NotFound("Image not found");

            var image = Images[id];
            foreach (var effect in effects)
            {
                var plugin = _pluginManager.GetPlugin(effect.Name);
                plugin?.ApplyEffect(ref image, effect.Parameters);
            }

            return Ok(image);
        }


        // GET: api/<ImagesController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<ImagesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<ImagesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ImagesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ImagesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }

    public class EffectRequest
    {
        public string Name { get; set; }
        public PluginParameters Parameters { get; set; }
    }
}
