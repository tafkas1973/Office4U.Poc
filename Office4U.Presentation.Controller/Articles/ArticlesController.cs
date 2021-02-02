using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Office4U.Common;
using Office4U.Presentation.Controller.Extensions;
using Office4U.ReadApplication.Articles.DTOs;
using Office4U.ReadApplication.Articles.Interfaces;
using Office4U.WriteApplication.Articles.DTOs;
using Office4U.WriteApplication.Articles.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Office4U.Presentation.Controller.Articles
{
    [Authorize(Policy = "RequireManageArticlesRole")]
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IGetArticlesQuery _listQuery;
        private readonly IGetArticleQuery _singleQuery;
        private readonly ICreateArticleCommand _createCommand;
        private readonly IUpdateArticleCommand _updateCommand;
        private readonly IDeleteArticleCommand _deleteCommand;

        public ArticlesController(
            IGetArticlesQuery listQuery,
            IGetArticleQuery singleQuery,
            ICreateArticleCommand createCommand,
            IUpdateArticleCommand updateCommand,
            IDeleteArticleCommand deleteCommand
            )
        {
            _listQuery = listQuery;
            _singleQuery = singleQuery;
            _createCommand = createCommand;
            _updateCommand = updateCommand;
            _deleteCommand = deleteCommand;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles(
            [FromQuery] ArticleParams articleParams)
        {
            var articles = await _listQuery.Execute(articleParams);

            Response.AddPaginationHeader(articles.CurrentPage, articles.PageSize, articles.TotalCount, articles.TotalPages);

            return Ok(articles.AsEnumerable());
        }

        [HttpGet("{id}", Name = "GetArticle")]
        public async Task<ActionResult<ArticleDto>> GetArticle(int id)
        {
            return Ok(await _singleQuery.Execute(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(ArticleForCreationDto articleForCreationDto)
        {
            var createdArticle = await _createCommand.Execute(articleForCreationDto);

            if (createdArticle != null)
            {
                return CreatedAtRoute("GetArticle", new { id = createdArticle.Id }, createdArticle);
            }

            return BadRequest("Failed to create article");
        }

        [HttpPut]
        // TODO: restful: also specify id in parm list?
        // TODO: strictly a command should not return anything, except when not long running
        // https://stackoverflow.com/questions/43433318/cqrs-command-return-values
        // https://lostechies.com/jimmybogard/2013/12/19/put-your-controllers-on-a-diet-posts-and-commands/
        public async Task<ActionResult> UpdateArticle(ArticleForUpdateDto articleUpdateDto)
        {
            var updated = await _updateCommand.Execute(articleUpdateDto);

            if (updated) return NoContent();

            return BadRequest("Failed to update article");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var deleted = await _deleteCommand.Execute(id);

            if (deleted) return Ok();

            return BadRequest("Failed to delete article");
        }
    }
}