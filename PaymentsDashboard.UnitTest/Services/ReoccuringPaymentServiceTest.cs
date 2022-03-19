using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Modells;
using PaymentsDashboard.Services;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PaymentsDashboard.UnitTest.Services
{
	[TestClass]
	public class ReoccuringPaymentServiceTest
	{
		private DataContext context;

		private ReoccuringPayment reoccuringPayment1 = new ReoccuringPayment()
		{
			Id = Guid.NewGuid(),
			Amount = new decimal(1.23),
			ReoccuringType = ReoccuringType.Yearly,
			StartDate = DateTime.Now.AddMonths(-1).AddDays(-3).ToString("yyyy-MM-dd"),
			Tags = new List<Tag>()
			{
				new Tag()
				{
					TagId = Guid.NewGuid(),
					Type = TagType.Primary,
					HexColorCode = "#aaaaaa",
					ReoccuringPayments = new List<ReoccuringPayment>()
				}
			},
			Title = "ReoccuringPayment A"
		};

		private ReoccuringPayment reoccuringPayment2 = new ReoccuringPayment()
		{
			Id = Guid.NewGuid(),
			Amount = new decimal(22.22),
			ReoccuringType = ReoccuringType.Weekly,
			StartDate = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd"),
			Tags = new List<Tag>()
			{
				new Tag()
				{
					TagId = Guid.NewGuid(),
					Type = TagType.Primary,
					HexColorCode = "#111222",
					ReoccuringPayments = new List<ReoccuringPayment>()
				},
				new Tag()
				{
					TagId = Guid.NewGuid(),
					Type = TagType.Secondary,
					HexColorCode = "#abcabc",
					ReoccuringPayments = new List<ReoccuringPayment>()
				}
			},
			Title = "ReoccuringPayment B"
		};

		[TestInitialize]
		public void Init()
		{
			foreach (var tag in reoccuringPayment1.Tags)
			{
				tag.ReoccuringPayments.Add(reoccuringPayment1);
			}

			foreach (var tag in reoccuringPayment2.Tags)
			{
				tag.ReoccuringPayments.Add(reoccuringPayment2);
			}


			var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "ReoccuringPaymentsDataBase").Options;

			context = new DataContext(options, It.IsAny<IHttpContextAccessor>());
			context.ReoccuringPayments.RemoveRange(context.ReoccuringPayments);
			context.Tags.RemoveRange(context.Tags);

			context.ReoccuringPayments.Add(reoccuringPayment1);
			context.ReoccuringPayments.Add(reoccuringPayment2);
			context.Tags.AddRange(reoccuringPayment1.Tags);
			context.Tags.AddRange(reoccuringPayment2.Tags);
			context.SaveChanges();
		}

		[TestMethod]
		public void GetAllReoccuringPayments_ReturnAllReoccuringPayments()
		{
			var service = new PaymentService(context);

			var result = service.GetAllReoccuringPayments().ToList();

			Assert.AreEqual(2, result.Count);
			Assert.IsTrue(result.Any(p => p.Id.Equals(reoccuringPayment1.Id)));
			Assert.IsTrue(result.Any(p => p.Id.Equals(reoccuringPayment2.Id)));
		}

		[TestMethod]
		public void GetReoccuringPaymentById_ValidId_ReturnsReoccuringPayment()
		{
			var service = new PaymentService(context);

			var result = service.GetReoccuringPaymentById(reoccuringPayment1.Id);

			Assert.AreEqual(reoccuringPayment1.Id, result.Id);
			Assert.AreEqual(reoccuringPayment1.Tags.Count, result.Tags.Count);
		}

		[TestMethod]
		public void GetReoccuringPaymentById_NotExistingId_ReturnsNull()
		{
			var service = new PaymentService(context);

			var result = service.GetReoccuringPaymentById(Guid.NewGuid());

			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetReoccuringPaymentById_NotTracked_ReturnsReoccuringPayment()
		{
			var service = new PaymentService(context);

			var result = service.GetReoccuringPaymentById(reoccuringPayment2.Id, false);

			Assert.AreEqual(reoccuringPayment2.Id, result.Id);
			Assert.AreEqual(reoccuringPayment2.Tags.Count, result.Tags.Count);
		}

		[TestMethod]
		public void DeleteReoccuringPaymentById_ValidId_DeletesReoccuringPayment()
		{
			var service = new PaymentService(context);

			var result = service.DeleteReoccuringPaymentById(reoccuringPayment1.Id);

			Assert.AreEqual(reoccuringPayment1.Id, result.Id);
			Assert.IsTrue(!service.GetAllReoccuringPayments().Any(p => p.Id.Equals(reoccuringPayment1.Id)));
		}

		[TestMethod]
		public void DeleteReoccuringPaymentById_InvalidId_DeletesReoccuringPayment()
		{
			var service = new PaymentService(context);

			var result = service.DeleteReoccuringPaymentById(Guid.NewGuid());

			Assert.IsNull(result);
		}

		[TestMethod]
		public void CreateReoccuringPayment_ValidReoccuringPayment_AddsReoccuringPayment()
		{
			ReoccuringPayment newReoccuringPayment = new ReoccuringPayment()
			{
				Id = Guid.NewGuid(),
				Amount = new decimal(123.123),
				StartDate = DateTime.Now.AddDays(-17).ToString("yyyy-MM-dd"),
				Tags = new List<Tag>()
				{
					reoccuringPayment1.Tags.First(),
					reoccuringPayment2.Tags.First(t => t.Type.Equals(TagType.Secondary))
				},
				Title = "ReoccuringPayment B"
			};

			foreach (var tag in newReoccuringPayment.Tags)
			{
				tag.ReoccuringPayments.Add(newReoccuringPayment);
			}

			var service = new PaymentService(context);

			var result = service.CreateReoccuringPayment(newReoccuringPayment);
			var ReoccuringPayments = service.GetAllReoccuringPayments().ToList();

			Assert.IsTrue(ReoccuringPayments.Any(p => p.Id.Equals(newReoccuringPayment.Id)));
			Assert.AreEqual(newReoccuringPayment.Tags.Count, ReoccuringPayments.Single(p => p.Id.Equals(newReoccuringPayment.Id)).Tags.Count);
		}

		[TestMethod]
		public void UpdateReoccuringPayment_ValidReoccuringPayment_UpdatesReoccuringPayment()
		{
			decimal newValue = new decimal(111.11);
			reoccuringPayment1.Amount = newValue;

			var service = new PaymentService(context);

			var result = service.UpdateReoccuringPayment(reoccuringPayment1);
			var ReoccuringPayments = service.GetAllReoccuringPayments().ToList();

			Assert.IsTrue(ReoccuringPayments.Any(p => p.Id.Equals(reoccuringPayment1.Id)));
			Assert.AreEqual(reoccuringPayment1.Tags.Count, ReoccuringPayments.Single(p => p.Id.Equals(reoccuringPayment1.Id)).Tags.Count);
			Assert.AreEqual(newValue, ReoccuringPayments.Single(p => p.Id.Equals(reoccuringPayment1.Id)).Amount);
		}

		[TestMethod]
		public void UpdateReoccuringPayment_InvalidReoccuringPayment_NoUpdate()
		{
			decimal newValue = new decimal(111.11);
			reoccuringPayment1.Amount = newValue;
			reoccuringPayment1.Id = Guid.NewGuid();

			var service = new PaymentService(context);

			var result = service.UpdateReoccuringPayment(reoccuringPayment1);
			Assert.IsNull(result);
		}
	}
}

