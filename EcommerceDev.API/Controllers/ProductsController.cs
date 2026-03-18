using EcommerceDev.Application.Commands.Products.CreateProduct;
using EcommerceDev.Application.Commands.Products.DownloadAllImagesForProduct;
using EcommerceDev.Application.Commands.Products.DownloadImageForProduct;
using EcommerceDev.Application.Commands.Products.UploadImageForProductCommand;
using EcommerceDev.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace EcommerceDev.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand request)
        {
            var result = await
                _mediator.DispatchAsync<CreateProductCommand, ResultViewModel<Guid>>(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("{id:guid}/images")]
        public async Task<IActionResult> UploadPhoto(Guid id, IFormFile file)
        {
            var stream = new MemoryStream();

            await file.CopyToAsync(stream);

            stream.Position = 0;

            var command = new UploadImageForProductCommand(id, file.FileName, stream);

            var response = await _mediator.DispatchAsync<UploadImageForProductCommand, ResultViewModel<bool>>(command);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("{id:guid}/images/{imageId:guid}")]
        public async Task<IActionResult> DownloadImage(Guid id, Guid imageId)
        {
            var query = new DownloadImageForProductQuery(imageId);

            var result = await _mediator.DispatchAsync<DownloadImageForProductQuery, ResultViewModel<Stream>>(query);

            if (!result.IsSuccess || result.Data == null)
            {
                return BadRequest(result.Message);
            }

            return File(result.Data, "image/jpeg");
        }

        [HttpGet("{id:guid}/images")]
        public async Task<IActionResult> DownloadImages(Guid id)
        {
            var query = new DownloadAllImagesForProductQuery(id);

            var result = await _mediator.DispatchAsync<DownloadAllImagesForProductQuery, ResultViewModel<List<Stream>>>(query);

            var streams = result.Data;

            var memoryStream = new MemoryStream();

            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var stream in streams)
                {
                    var entry = zipArchive.CreateEntry($"{Guid.NewGuid().ToString()}.jpeg");

                    using var entryStream = entry.Open();

                    stream.CopyTo(entryStream);
                }
            }

            memoryStream.Position = 0;

            var zipFileName = $"photos_{id}.zip";

            return File(memoryStream, "application/zip", zipFileName);
        }
    }
}

