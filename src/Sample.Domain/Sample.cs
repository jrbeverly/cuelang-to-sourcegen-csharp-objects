namespace Sample.Domain;

public static class Sample
{
    public static Order BuildOrder() => new OrderBuilder()
        .WithOrderId("order-1")
        .WithCustomerId("customer-1")
        .AddLine(new OrderLineBuilder()
            .WithProductId("product-1")
            .WithQuantity(new QuantityBuilder().WithValue(2).Build())
            .Build())
        .Build();

    public static Order RoundTrip(Order order) => order.ToMutable().ToImmutable();

    public static Synthesis.Runtime.ValidationResult ValidateQuantity(int value) => Quantity.Validate(value);

    public static bool ValuesAreEqual(Quantity left, Quantity right) => left == right;

    public static string Describe(PaymentMethod paymentMethod) => paymentMethod.Match(
        card => card.CardToken,
        invoice => invoice.InvoiceRef,
        _ => "credit");
}
