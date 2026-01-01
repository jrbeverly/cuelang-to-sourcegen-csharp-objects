namespace Synthesis.Generator.Tests;

public sealed class GeneratorTests
{
    [Fact]
    public void Builder_constructs_an_immutable_value()
    {
        var order = Sample.Domain.Sample.BuildOrder();

        Assert.Equal("order-1", order.OrderId);
        Assert.Equal("customer-1", order.CustomerId);
        var line = Assert.Single(order.Lines);
        Assert.Equal("product-1", line.ProductId);
        Assert.Equal(2, line.Quantity.Value);
    }

    [Fact]
    public void Immutable_value_converts_to_mutable_and_back()
    {
        var order = Sample.Domain.Sample.BuildOrder();

        var roundTripped = Sample.Domain.Sample.RoundTrip(order);

        Assert.NotSame(order, roundTripped);
        Assert.Equal(order.OrderId, roundTripped.OrderId);
        Assert.Equal(order.CustomerId, roundTripped.CustomerId);
        Assert.Equal(order.Lines[0], roundTripped.Lines[0]);
    }

    [Fact]
    public void Validation_accepts_valid_input()
    {
        Assert.True(Sample.Domain.Sample.ValidateQuantity(1).IsValid);
    }

    [Fact]
    public void Validation_rejects_invalid_input()
    {
        var invalid = Sample.Domain.Sample.ValidateQuantity(0);

        Assert.False(invalid.IsValid);
        Assert.Equal("Value must be positive.", invalid.Error);
    }

    [Fact]
    public void Generated_values_have_structural_equality()
    {
        var left = new Sample.Domain.QuantityBuilder().WithValue(2).Build();
        var right = new Sample.Domain.QuantityBuilder().WithValue(2).Build();

        Assert.True(Sample.Domain.Sample.ValuesAreEqual(left, right));
        Assert.False(Sample.Domain.Sample.ValuesAreEqual(
            left,
            new Sample.Domain.QuantityBuilder().WithValue(3).Build()));
    }

    [Fact]
    public void Union_match_handles_every_case()
    {
        Sample.Domain.PaymentMethod card = new Sample.Domain.PaymentMethod.Card("card-1");
        Sample.Domain.PaymentMethod invoice = new Sample.Domain.PaymentMethod.Invoice("invoice-1");
        Sample.Domain.PaymentMethod credit = new Sample.Domain.PaymentMethod.Credit();

        Assert.Equal("card-1", Sample.Domain.Sample.Describe(card));
        Assert.Equal("invoice-1", Sample.Domain.Sample.Describe(invoice));
        Assert.Equal("credit", Sample.Domain.Sample.Describe(credit));
    }
}
