namespace Thesaurus.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ThesaurusRepositoryTests
    {
        [TestMethod]
        public void TestMethod()
        {
            var repository = new Thesaurus();
            var id = repository.AddWordAsync("test", new List<string> { "test1", "test2" }).Result;
            var id1 = repository.AddWordAsync("test", new List<string> { "test1", "test2" }).Result;

            var result = repository.GetAllWordsGroupedAsync().Result;
        }
    }
}
