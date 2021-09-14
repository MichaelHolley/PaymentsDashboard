using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentsDashboard.Controllers;
using PaymentsDashboard.Data.Modells;
using PaymentsDashboard.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentsDashboard.UnitTest.Controllers
{
	[TestClass]
	public class TagControllerTest
	{
		private Mock<ITagService> tagServiceMock = new Mock<ITagService>();

		private Tag tag1 = new Tag()
		{
			TagId = Guid.NewGuid(),
			HexColorCode = "#aaa111",
			Title = "Tag 1",
			Type = TagType.Primary,
			Payments = new List<Payment>(),
			ReoccuringPayments = new List<ReoccuringPayment>()
		};

		private Tag tag2 = new Tag()
		{
			TagId = Guid.NewGuid(),
			HexColorCode = "#222222",
			Title = "Tag 2",
			Type = TagType.Primary,
			Payments = new List<Payment>(),
			ReoccuringPayments = new List<ReoccuringPayment>()
		};

		private Tag tag3 = new Tag()
		{
			TagId = Guid.NewGuid(),
			HexColorCode = "#333333",
			Title = "Tag 3",
			Type = TagType.Secondary,
			Payments = new List<Payment>(),
			ReoccuringPayments = new List<ReoccuringPayment>()
		};

		[TestInitialize]
		public void Init()
		{
			tagServiceMock.Setup(m => m.GetAllTags()).Returns(new List<Tag>() { tag1, tag2, tag3 }.AsQueryable());
		}

		[TestMethod]
		public void GetTags_ReturnsAllTags()
		{
			TagController controller = new TagController(tagServiceMock.Object);

			var result = controller.GetTags();

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
		}

		[TestMethod]
		public void GetPrimaryTags_ReturnsAllPrimaryTags()
		{
			TagController controller = new TagController(tagServiceMock.Object);

			var result = controller.GetPrimaryTags();

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
		}

		[TestMethod]
		public void GetSecondaryTags_ReturnsAllSecondaryTags()
		{
			TagController controller = new TagController(tagServiceMock.Object);

			var result = controller.GetSecondaryTags();

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
		}

		[TestMethod]
		public void GetTag_NotExistingId_ReturnsNotFound()
		{
			tagServiceMock.Setup(m => m.GetTagById(It.IsAny<Guid>(), It.IsAny<bool>())).Returns((Tag)null);
			TagController controller = new TagController(tagServiceMock.Object);

			var result = controller.GetTag(It.IsAny<Guid>());

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
		}

		[TestMethod]
		public void GetTag_ExistingId_ReturnsOk()
		{
			tagServiceMock.Setup(m => m.GetTagById(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(tag1);
			TagController controller = new TagController(tagServiceMock.Object);

			var result = controller.GetTag(tag1.TagId);

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
		}

		[TestMethod]
		public void DeleteTag_NotExistingId_ReturnsNotFound()
		{
			tagServiceMock.Setup(m => m.DeleteTagById(It.IsAny<Guid>())).Returns((Tag)null);
			TagController controller = new TagController(tagServiceMock.Object);

			var result = controller.DeleteTag(It.IsAny<Guid>());

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
		}

		[TestMethod]
		public void DeleteTag_ExistingId_ReturnsOk()
		{
			tagServiceMock.Setup(m => m.DeleteTagById(It.IsAny<Guid>())).Returns(tag1);
			TagController controller = new TagController(tagServiceMock.Object);

			var result = controller.DeleteTag(It.IsAny<Guid>());

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
		}

		[TestMethod]
		public void CreateOrUpdate_WithIdButNotExisting_ReturnsNotFound()
		{
			tagServiceMock.Setup(m => m.GetTagById(It.IsAny<Guid>(), It.IsAny<bool>())).Returns((Tag)null);
			TagController controller = new TagController(tagServiceMock.Object);

			var result = controller.CreateOrUpdateTag(new Tag() { TagId = Guid.NewGuid(), HexColorCode = "#123123", Title = "Updated Tag", Type = TagType.Primary });

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
		}

		[TestMethod]
		public void CreateOrUpdate_WithIdAndExisting_ReturnsOk()
		{
			tagServiceMock.Setup(m => m.GetTagById(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(tag1);
			TagController controller = new TagController(tagServiceMock.Object);

			var updateTag = new Tag()
			{
				TagId = tag1.TagId,
				HexColorCode = "#123123",
				Title = "Updated Tag",
				Type = TagType.Primary,
				Payments = tag1.Payments,
				ReoccuringPayments = tag1.ReoccuringPayments
			};

			tagServiceMock.Setup(m => m.UpdateTag(It.IsAny<Tag>())).Returns(new Tag() {
				TagId = Guid.NewGuid(),
				HexColorCode = "#123123",
				Title = "Updated Tag",
				Type = TagType.Primary,
				Payments = new List<Payment>(),
				ReoccuringPayments = new List<ReoccuringPayment>()
			});

			var result = controller.CreateOrUpdateTag(updateTag);

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
		}

		[TestMethod]
		public void CreateOrUpdate_WithNoExisting_ReturnsOk()
		{
			tagServiceMock.Setup(m => m.GetTagById(It.IsAny<Guid>(), It.IsAny<bool>()));
			TagController controller = new TagController(tagServiceMock.Object);

			var createTag = new Tag()
			{
				TagId = Guid.Empty,
				HexColorCode = "#123123",
				Title = "Updated Tag",
				Type = TagType.Primary,
				Payments = tag1.Payments,
				ReoccuringPayments = tag1.ReoccuringPayments
			};

			tagServiceMock.Setup(m => m.CreateTag(It.IsAny<Tag>())).Returns(createTag);

			var result = controller.CreateOrUpdateTag(createTag);

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
		}
	}
}
