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
	public class PaymentServiceTest
	{
		private DataContext context;

		private Payment payment1 = new Payment()
		{
			PaymentId = Guid.NewGuid(),
			Amount = new decimal(1.23),
			Date = DateTime.Now.AddMonths(-1).AddDays(-3).ToString("yyyy-MM-dd"),
			Tags = new List<Tag>()
			{
				new Tag()
				{
					TagId = Guid.NewGuid(),
					Type = TagType.Primary,
					HexColorCode = "#aaaaaa",
					Payments = new List<Payment>()
				}
			},
			Title = "Payment A"
		};

		private Payment payment2 = new Payment()
		{
			PaymentId = Guid.NewGuid(),
			Amount = new decimal(22.22),
			Date = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd"),
			Tags = new List<Tag>()
			{
				new Tag()
				{
					TagId = Guid.NewGuid(),
					Type = TagType.Primary,
					HexColorCode = "#111222",
					Payments = new List<Payment>()
				},
				new Tag()
				{
					TagId = Guid.NewGuid(),
					Type = TagType.Secondary,
					HexColorCode = "#abcabc",
					Payments = new List<Payment>()
				}
			},
			Title = "Payment B"
		};

		[TestInitialize]
		public void Init()
		{
			foreach (var tag in payment1.Tags)
			{
				tag.Payments.Add(payment1);
			}

			foreach (var tag in payment2.Tags)
			{
				tag.Payments.Add(payment2);
			}


			var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "PaymentsDataBase").Options;

			context = new DataContext(options, It.IsAny<IHttpContextAccessor>());
			context.Payments.RemoveRange(context.Payments);
			context.Tags.RemoveRange(context.Tags);

			context.Payments.Add(payment1);
			context.Payments.Add(payment2);
			context.Tags.AddRange(payment1.Tags);
			context.Tags.AddRange(payment2.Tags);
			context.SaveChanges();
		}

		[TestMethod]
		public void GetAllPayments_ReturnAllPayments()
		{
			var service = new PaymentService(context);

			var result = service.GetAllPayments().ToList();

			Assert.AreEqual(2, result.Count);
			Assert.IsTrue(result.Any(p => p.PaymentId.Equals(payment1.PaymentId)));
			Assert.IsTrue(result.Any(p => p.PaymentId.Equals(payment2.PaymentId)));
		}

		[TestMethod]
		public void GetPaymentById_ValidId_ReturnsPayment()
		{
			var service = new PaymentService(context);

			var result = service.GetPaymentById(payment1.PaymentId);

			Assert.AreEqual(payment1.PaymentId, result.PaymentId);
			Assert.AreEqual(payment1.Tags.Count, result.Tags.Count);
		}

		[TestMethod]
		public void GetPaymentById_NotExistingId_ReturnsNull()
		{
			var service = new PaymentService(context);

			var result = service.GetPaymentById(Guid.NewGuid());

			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetPaymentById_NotTracked_ReturnsPayment()
		{
			var service = new PaymentService(context);

			var result = service.GetPaymentById(payment2.PaymentId, false);

			Assert.AreEqual(payment2.PaymentId, result.PaymentId);
			Assert.AreEqual(payment2.Tags.Count, result.Tags.Count);
		}

		[TestMethod]
		public void GetPaymentsByMonths_CurrentMonth_ReturnsAllPaymentsInCurrentMonth()
		{
			var service = new PaymentService(context);

			var result = service.GetPaymentsByMonths(0).ToList();

			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(result.Any(p => p.PaymentId.Equals(payment2.PaymentId)));
		}

		[TestMethod]
		public void GetPaymentsByMonths_LastMonth_ReturnsAllPaymentsInCurrentMonth()
		{
			var service = new PaymentService(context);

			var result = service.GetPaymentsByMonths(1).ToList();

			Assert.AreEqual(1, result.Count);
			Assert.IsTrue(result.Any(p => p.PaymentId.Equals(payment1.PaymentId)));
		}

		[TestMethod]
		public void DeletePaymentById_ValidId_DeletesPayment()
		{
			var service = new PaymentService(context);

			var result = service.DeletePaymentById(payment1.PaymentId);

			Assert.AreEqual(payment1.PaymentId, result.PaymentId);
			Assert.IsTrue(!service.GetAllPayments().Any(p => p.PaymentId.Equals(payment1.PaymentId)));
		}

		[TestMethod]
		public void DeletePaymentById_InvalidId_DeletesPayment()
		{
			var service = new PaymentService(context);

			var result = service.DeletePaymentById(Guid.NewGuid());

			Assert.IsNull(result);
		}

		[TestMethod]
		public void CreatePayment_ValidPayment_AddsPayment()
		{
			Payment newPayment = new Payment()
			{
				PaymentId = Guid.NewGuid(),
				Amount = new decimal(123.123),
				Date = DateTime.Now.AddDays(-17).ToString("yyyy-MM-dd"),
				Tags = new List<Tag>()
				{
					payment1.Tags.First(),
					payment2.Tags.First(t => t.Type.Equals(TagType.Secondary))
				},
				Title = "Payment B"
			};

			foreach (var tag in newPayment.Tags)
			{
				tag.Payments.Add(newPayment);
			}

			var service = new PaymentService(context);

			var result = service.CreatePayment(newPayment);
			var payments = service.GetAllPayments().ToList();

			Assert.IsTrue(payments.Any(p => p.PaymentId.Equals(newPayment.PaymentId)));
			Assert.AreEqual(newPayment.Tags.Count, payments.Single(p => p.PaymentId.Equals(newPayment.PaymentId)).Tags.Count);
		}

		[TestMethod]
		public void UpdatePayment_ValidPayment_UpdatesPayment()
		{
			decimal newValue = new decimal(111.11);
			payment1.Amount = newValue;

			var service = new PaymentService(context);

			var result = service.UpdatePayment(payment1);
			var payments = service.GetAllPayments().ToList();

			Assert.IsTrue(payments.Any(p => p.PaymentId.Equals(payment1.PaymentId)));
			Assert.AreEqual(payment1.Tags.Count, payments.Single(p => p.PaymentId.Equals(payment1.PaymentId)).Tags.Count);
			Assert.AreEqual(newValue, payments.Single(p => p.PaymentId.Equals(payment1.PaymentId)).Amount);
		}

		[TestMethod]
		public void UpdatePayment_InvalidPayment_NoUpdate()
		{
			decimal newValue = new decimal(111.11);
			payment1.Amount = newValue;
			payment1.PaymentId = Guid.NewGuid();

			var service = new PaymentService(context);

			var result = service.UpdatePayment(payment1);
			Assert.IsNull(result);
		}

		[TestMethod]
		public void RemoveCycle_ForManyPayments_ShouldRemoveCycle()
		{
			var service = new PaymentService(context);

			var result = service.GetAllPayments();
			var cleaned = result.RemoveCycle();

			Assert.IsNull(cleaned.First().Tags.First().Payments);
			Assert.IsTrue(!cleaned.Any(p => p.Tags.Any(t => t.Payments != null)));
		}

		[TestMethod]
		public void RemoveCycle_ForSinglePayment_ShouldRemoveCycle()
		{
			var service = new PaymentService(context);

			var result = service.GetPaymentById(payment1.PaymentId);
			var cleaned = result.RemoveCycle();

			Assert.AreEqual(0, cleaned.Tags.First().Payments.Count);
			Assert.IsTrue(!cleaned.Tags.Any(t => t.Payments != null && t.Payments.Count > 0));
		}
	}
}

