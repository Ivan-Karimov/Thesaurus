namespace ThesaurusWebAPI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Thesaurus;
    using ThesaurusDTOs;

    public class ThesaurusController : Controller
    {
        private readonly IThesaurus thesaurus;

        public ThesaurusController(IThesaurus thesaurus) => this.thesaurus = thesaurus;

        [HttpPost]
        [Route("api/thesaurus")]
        public async Task<IActionResult> AddWord([FromBody] WordDTO word)
        {
            try
            {
                await thesaurus.AddWordAsync(word.Word, word.Meaning, word.Synonyms);
                return StatusCode(201);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("api/thesaurus/{wordId}")]
        public async Task<IActionResult> AddSynonyms(int wordId, [FromBody] WordDTO word)
        {
            try
            {
                await thesaurus.AddSynonymsForWordAsync(wordId, word.Synonyms);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound($"ID = {wordId}");
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("api/thesaurus/{wordId}")]
        public async Task<IActionResult> GetSynonyms(int wordId)
        {
            try
            {
                var result = await thesaurus.GetSynonymsOfWordAsync(wordId);
                return Json(result);
            }
            catch (InvalidOperationException)
            {
                return NotFound($"ID = {wordId}");
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("api/thesaurus")]
        public async Task<IActionResult> GetAllWords()
        {
            try
            {
                var result = await thesaurus.GetAllWordsGroupedAsync();
                return Json(result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("api/thesaurus/{wordId}")]
        public async Task<IActionResult> RemoveWord(int wordId)
        {
            try
            {
                await thesaurus.RemoveWordAsync(wordId);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound($"ID = {wordId}");
            }
            catch
            {
                return StatusCode(500);
            }

        }
    }
}
