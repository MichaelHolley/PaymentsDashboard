
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Modells;
using PaymentsDashboard.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentsDashboard.UnitTest.Services
{
	[TestClass]
	public class TagServiceTest
	{
		private DataContext context;

		Tag tag1 = new Tag()
		{
			TagId = Guid.NewGuid(),
			Payments = new List<Payment>(),
			ReoccuringPayments = new List<ReoccuringPayment>(),
			Title = "Tag 1",
			HexColorCode = "#123123",
			Type = TagType.Primary
		};

		Tag tag2 = new Tag()
		{
			TagId = Guid.NewGuid(),
			Payments = new List<Payment>(),
			ReoccuringPayments = new List<ReoccuringPayment>(),
			Title = "Tag 2",
			HexColorCode = "#111111",
			Type = TagType.Primary
		};

		Tag tag3 = new Tag()
		{
			TagId = Guid.NewGuid(),
			Payments = new List<Payment>(),
			ReoccuringPayments = new List<ReoccuringPayment>(),
			Title = "Tag 3",
			HexColorCode = "#222222",
			Type = TagType.Secondary,
			Created = DateTime.Now.AddDays(-150),
			Modified = DateTime.Now.AddDays(-10),
		};

		Tag tag4 = new Tag()
		{
			TagId = Guid.NewGuid(),
			Payments = new List<Payment>(),
			ReoccuringPayments = new List<ReoccuringPayment>(),
			Title = "Tag 4",
			HexColorCode = "#222bbb",
			Type = TagType.Secondary
		};

		[TestInitialize]
		public void Init()
		{
			var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "PaymentsDataBase").Options;

			context = new DataContext(options);
			context.Payments.RemoveRange(context.Payments);
			context.ReoccuringPayments.RemoveRange(context.ReoccuringPayments);
			context.Tags.RemoveRange(context.Tags);

			context.Tags.AddRange(new List<Tag>() { tag1, tag2, tag3, tag4 });
			context.SaveChanges();
		}

		[TestMethod]
		public void GetAllTags_ReturnsAllTags()
		{
			var service = new TagService(context);

			var result = service.GetAllTags().ToList();

			Assert.AreEqual(4, result.Count);
			Assert.IsTrue(result.Any(t => t.TagId.Equals(tag1.TagId)));
			Assert.IsTrue(result.Any(t => t.TagId.Equals(tag2.TagId)));
			Assert.IsTrue(result.Any(t => t.TagId.Equals(tag3.TagId)));
			Assert.IsTrue(result.Any(t => t.TagId.Equals(tag4.TagId)));
		}

		[TestMethod]
		public void GetPrimaryTags_ReturnsOnlyPrimaryTags()
		{
			var service = new TagService(context);

			var result = service.GetPrimaryTags().ToList();

			Assert.AreEqual(2, result.Count);
			Assert.IsTrue(result.Any(t => t.TagId.Equals(tag1.TagId)));
			Assert.IsTrue(result.Any(t => t.TagId.Equals(tag2.TagId)));
			Assert.IsTrue(!result.Any(t => t.Type.Equals(TagType.Secondary)));
		}

		[TestMethod]
		public void GetSecondaryTags_ReturnsOnlySecondaryTags()
		{
			var service = new TagService(context);

			var result = service.GetSecondaryTags().ToList();

			Assert.AreEqual(2, result.Count);
			Assert.IsTrue(result.Any(t => t.TagId.Equals(tag3.TagId)));
			Assert.IsTrue(result.Any(t => t.TagId.Equals(tag4.TagId)));
			Assert.IsTrue(!result.Any(t => t.Type.Equals(TagType.Primary)));
		}

		[TestMethod]
		public void GetTagById_ValidId_ReturnsTagById()
		{
			var service = new TagService(context);

			var result = service.GetTagById(tag3.TagId);

			Assert.AreEqual(tag3.TagId, result.TagId);
			Assert.AreEqual(tag3.Type, result.Type);
			Assert.AreEqual(tag3.Title, result.Title);
			Assert.AreEqual(tag3.Modified, result.Modified);
			Assert.AreEqual(tag3.Created, result.Created);
			Assert.AreEqual(tag3.HexColorCode, result.HexColorCode);
			Assert.AreEqual(tag3.Payments.Count, result.Payments.Count);
			Assert.AreEqual(tag3.ReoccuringPayments.Count, result.ReoccuringPayments.Count);
		}

		[TestMethod]
		public void GetTagById_InvalidId_ReturnsNull()
		{
			var service = new TagService(context);

			var result = service.GetTagById(Guid.NewGuid());

			Assert.IsNull(result);
		}

		[TestMethod]
		public void CreateTag_ValidTag_CreatesTag()
		{
			var newTag = new Tag()
			{
				TagId = Guid.NewGuid(),
				Payments = new List<Payment>(),
				ReoccuringPayments = new List<ReoccuringPayment>(),
				Title = "Tag NEW",
				HexColorCode = "#11aabb",
				Type = TagType.Primary
			};

			var service = new TagService(context);

			var result = service.CreateTag(newTag);
			var tags = context.Tags.ToList();

			Assert.IsTrue(tags.Any(t => t.TagId.Equals(newTag.TagId)));
		}

		[TestMethod]
		public void UpdateTag_ValidTag_UpdatesTag()
		{
			var oldTitle = tag2.Title;
			var newTitle = "Updated Title";
			tag2.Title = newTitle;

			var service = new TagService(context);

			var result = service.UpdateTag(tag2);
			var tag = context.Tags.SingleOrDefault(t => t.TagId.Equals(tag2.TagId));

			Assert.AreNotEqual(oldTitle, tag.Title);
			Assert.AreEqual(newTitle, tag.Title);
		}

		[TestMethod]
		public void DeleteTag_ValidId_DeletesTag()
		{
			var service = new TagService(context);

			var result = service.DeleteTagById(tag2.TagId);
			var tags = context.Tags.ToList();

			Assert.IsTrue(!tags.Any(t => t.TagId.Equals(tag2.TagId)));
			Assert.AreEqual(3, tags.Count);
		}

		[TestMethod]
		public void DeleteTag_InvalidId_DeletesTag()
		{
			var service = new TagService(context);

			var result = service.DeleteTagById(Guid.NewGuid());

			Assert.IsNull(result);
		}
	}
}
