using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using System.Net.NetworkInformation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api
{
    public class BlogPosts
    {
        private readonly ILogger<BlogPosts> _logger;

        public BlogPosts(ILogger<BlogPosts> logger)
        {
            _logger = logger;
        }



        [Function($"{nameof(BlogPosts)}_GetAll")]
        public IActionResult GetAll(
            [HttpTrigger(AuthorizationLevel.Function,
        "get",
        Route = "blogposts")]
        HttpRequest req,
            [CosmosDBInput(
        databaseName: "SwaBlog",
        containerName: "BlogContainer",
        Connection  = "CosmosDbConnectionString",
        SqlQuery =@"
        SELECT
        c.id,
        c.Title,
        c.Author,
        c.PublishedDate,
        LEFT(c.BlogpostMarkdown, 500)
            As BlogpostMarkdown,
        LENGTH(c.BlogpostMarkdown) <= 500
            As PreviewIsComplete,
        c.Tags
        FROM c
        WHERE c.Status = 2")] IEnumerable<Blogpost> blogposts)
        {
            return new OkObjectResult(blogposts);
        }

        [Function($"{nameof(BlogPosts)}_GetSingle")]
        public IActionResult GetSingle(
    [HttpTrigger(
        AuthorizationLevel.Function,
        "get",
        Route = "blogposts/{author}/{id}")
    ] HttpRequest req,
    [CosmosDBInput(
        databaseName: "SwaBlog",
        containerName: "BlogContainer",
        Connection  = "CosmosDbConnectionString",
        SqlQuery =@"
        SELECT
        c.id,
        c.Title,
        c.Author,
        c.PublishedDate,
        c.BlogpostMarkdown,
        c.Tags
        FROM c
        WHERE c.Status = 2
        AND c.id = {id}
        AND c.Author = {author}")
    ] IEnumerable<Blogpost> blogposts)
        {
            if (!blogposts.Any())
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(blogposts.First());
        }

    }

}
