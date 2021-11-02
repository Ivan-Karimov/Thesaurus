namespace Thesaurus.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ThesaurusDTOs;
    using ThesaurusWebAPI.Controllers;

    [TestClass]
    public class ThesaurusControllerTests
    {
        [TestMethod]
        public async Task Should_AddWord()
        {
            var wordDto = new WordDTO
            {
                Word = "word",
                Meaning = "meaning",
                Synonyms = new List<string> { "synonym1", "synonym2" }
            };
            var thesaurusMock = new Mock<IThesaurus>();
            thesaurusMock.Setup(t => t.AddWordAsync(wordDto.Word, wordDto.Meaning, wordDto.Synonyms))
                .ReturnsAsync(1);
            var controller = new ThesaurusController(thesaurusMock.Object);

            var result = await controller.AddWord(wordDto);

            Assert.AreEqual(201, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public async Task Should_AddSynonymsForWord()
        {
            var wordId = 1;
            var wordDto = new WordDTO
            {
                Synonyms = new List<string> { "synonym1", "synonym2" }
            };
            var thesaurusMock = new Mock<IThesaurus>();
            thesaurusMock.Setup(t => t.AddSynonymsForWordAsync(wordId, wordDto.Synonyms));
            var controller = new ThesaurusController(thesaurusMock.Object);

            var result = await controller.AddSynonyms(wordId, wordDto);

            Assert.AreEqual(200, ((OkResult)result).StatusCode);
        }

        [TestMethod]
        public async Task Should_Return404_When_WordForSynonymsDoesntExists()
        {
            var wordId = 1;
            var wordDto = new WordDTO
            {
                Synonyms = new List<string> { "synonym1", "synonym2" }
            };
            var thesaurusMock = new Mock<IThesaurus>();
            thesaurusMock.Setup(t => t.AddSynonymsForWordAsync(wordId, wordDto.Synonyms))
                .ThrowsAsync(new InvalidOperationException("Can't find word with ID = 1."));
            var controller = new ThesaurusController(thesaurusMock.Object);

            var result = await controller.AddSynonyms(wordId, wordDto);

            Assert.AreEqual(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [TestMethod]
        public async Task Should_GetSynonymsOfWord_When_WordExists()
        {
            var wordId = 1;
            var thesaurusMock = new Mock<IThesaurus>();
            thesaurusMock.Setup(t => t.GetSynonymsOfWordAsync(wordId))
                .ReturnsAsync(new SynonymsDTO
                {
                    Meaning = "Meaning",
                    Synonyms = new List<WordDTO> { new WordDTO { Id = 1, Word = "Word1" }, new WordDTO { Id = 2, Word = "Word2" } }
                });
            var controller = new ThesaurusController(thesaurusMock.Object);

            var result = await controller.GetSynonyms(wordId);

            Assert.AreEqual("Meaning", ((SynonymsDTO)((JsonResult)result).Value).Meaning);
            Assert.AreEqual(2, ((SynonymsDTO)((JsonResult)result).Value).Synonyms.Count);
            Assert.AreEqual(1, ((SynonymsDTO)((JsonResult)result).Value).Synonyms[0].Id);
            Assert.AreEqual("Word1", ((SynonymsDTO)((JsonResult)result).Value).Synonyms[0].Word);
            Assert.AreEqual(2, ((SynonymsDTO)((JsonResult)result).Value).Synonyms[1].Id);
            Assert.AreEqual("Word2", ((SynonymsDTO)((JsonResult)result).Value).Synonyms[1].Word);
        }

        [TestMethod]
        public async Task Should_Return404_When_WordAndSynonymsDontExists()
        {
            var wordId = 1;
            var thesaurusMock = new Mock<IThesaurus>();
            thesaurusMock.Setup(t => t.GetSynonymsOfWordAsync(wordId))
                .ThrowsAsync(new InvalidOperationException("Can't find word with ID = 1."));
            var controller = new ThesaurusController(thesaurusMock.Object);

            var result = await controller.GetSynonyms(wordId);

            Assert.AreEqual(404, ((NotFoundObjectResult)result).StatusCode);
        }

        [TestMethod]
        public async Task Should_ReturnAllWordsGrouped()
        {
            var thesaurusMock = new Mock<IThesaurus>();
            thesaurusMock.Setup(t => t.GetAllWordsGroupedAsync())
                .ReturnsAsync(new List<SynonymsDTO>
                {
                    new SynonymsDTO
                    {
                        Meaning = "Meaning1",
                        Synonyms = new List<WordDTO>
                        {
                            new WordDTO { Word = "Word1", Id = 1 },
                            new WordDTO { Word = "Word2", Id = 2 }
                        }
                    },
                    new SynonymsDTO
                    {
                        Meaning = "Meaning2",
                        Synonyms = new List<WordDTO>
                        {
                            new WordDTO { Word = "Word3", Id = 3 },
                            new WordDTO { Word = "Word4", Id = 4 }
                        }
                    }
                });
            var controller = new ThesaurusController(thesaurusMock.Object);

            var result = await controller.GetAllWords();

            var synonymsDtos = (List<SynonymsDTO>)((JsonResult)result).Value;
            Assert.AreEqual(2, synonymsDtos.Count);
            Assert.AreEqual("Meaning1", synonymsDtos[0].Meaning);
            Assert.AreEqual(1, synonymsDtos[0].Synonyms[0].Id);
            Assert.AreEqual("Word1", synonymsDtos[0].Synonyms[0].Word);
            Assert.AreEqual(2, synonymsDtos[0].Synonyms[1].Id);
            Assert.AreEqual("Word2", synonymsDtos[0].Synonyms[1].Word);
            Assert.AreEqual("Meaning2", synonymsDtos[1].Meaning);
            Assert.AreEqual(3, synonymsDtos[1].Synonyms[0].Id);
            Assert.AreEqual("Word3", synonymsDtos[1].Synonyms[0].Word);
            Assert.AreEqual(4, synonymsDtos[1].Synonyms[1].Id);
            Assert.AreEqual("Word4", synonymsDtos[1].Synonyms[1].Word);
        }

        [TestMethod]
        public async Task Should_Return500_WhenHandledException()
        {
            var thesaurusMock = new Mock<IThesaurus>();
            thesaurusMock.Setup(t => t.GetAllWordsGroupedAsync())
                .Throws(new Exception());
            var controller = new ThesaurusController(thesaurusMock.Object);

            var result = await controller.GetAllWords();

            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public async Task Should_RemoveWord_WhenWordExists()
        {
            var wordId = 1;
            var thesaurusMock = new Mock<IThesaurus>();
            thesaurusMock.Setup(t => t.RemoveWordAsync(wordId));
            var controller = new ThesaurusController(thesaurusMock.Object);

            var result = await controller.RemoveWord(wordId);

            Assert.AreEqual(200, ((OkResult)result).StatusCode);
        }

        [TestMethod]
        public async Task Should_Remove404_WhenWordForRemoveDoesntExists()
        {
            var wordId = 1;
            var thesaurusMock = new Mock<IThesaurus>();
            thesaurusMock.Setup(t => t.RemoveWordAsync(wordId))
                .ThrowsAsync(new InvalidOperationException("Can't find word with ID = 1."));
            var controller = new ThesaurusController(thesaurusMock.Object);

            var result = await controller.RemoveWord(wordId);

            Assert.AreEqual(404, ((NotFoundObjectResult)result).StatusCode);
        }
    }
}
