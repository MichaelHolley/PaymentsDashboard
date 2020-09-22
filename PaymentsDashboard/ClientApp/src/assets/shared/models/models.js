"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Tag = exports.PaymentPostModel = exports.Payment = exports.SortBy = exports.ReoccuringType = void 0;

var SortBy;
(function (SortBy) {
    SortBy[SortBy["Date"] = 0] = "Date";
    SortBy[SortBy["Amount"] = 1] = "Amount";
})(SortBy = exports.SortBy || (exports.SortBy = {}));
var Payment = /** @class */ (function () {
    function Payment() {
    }
    return Payment;
}());
exports.Payment = Payment;
var PaymentPostModel = /** @class */ (function () {
    function PaymentPostModel() {
    }
    return PaymentPostModel;
}());
exports.PaymentPostModel = PaymentPostModel;
var Tag = /** @class */ (function () {
    function Tag() {
    }
    return Tag;
}());
exports.Tag = Tag;
//# sourceMappingURL=models.js.map