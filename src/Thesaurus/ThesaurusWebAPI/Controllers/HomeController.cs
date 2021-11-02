namespace ThesaurusWebAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Text;

    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var usage = new StringBuilder();
            usage.AppendLine("POST api/thesaurus to add word with meaning and synonyms (all data from body)");
            usage.AppendLine("PUT api/thesaurus/{wordId} to add synonyms for word (synonyms from body)");
            usage.AppendLine("GET api/thesaurus/{wordId} to get all synonyms and meaning of the word");
            usage.AppendLine("GET api/thesaurus to get all synonyms grouped by meaning");
            usage.AppendLine("DELETE api/thesaurus/{wordId} to remove word");
            return Ok(usage.ToString());
        }
    }
}
