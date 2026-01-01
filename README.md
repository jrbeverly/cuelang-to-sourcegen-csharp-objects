# CUE-to-C# Object Synthesis

> [!WARNING]
> Abandoned repository as part of a series of generic explorations in Cuelang capabilities.

Exports a compact CUE domain model to JSON and consumes it with a Roslyn incremental generator to produce typed C# domain objects.

```sh
make generate
make test
```

Yielding a model like:

```json
{
  "concepts": {
    "Quantity": {
      "kind": "value_object",
      "fields": {
        "value": { "type": "int", "constraints": ["positive"] }
      }
    },
    "Order": {
      "kind": "entity",
      "identity_field": "order_id",
      "owns": {
        "lines": { "concept": "OrderLine", "multiplicity": "many" }
      }
    },
    "PaymentMethod": {
      "kind": "union",
      "cases": ["Card", "Invoice", "Credit"]
    }
  }
}
```

for some C# code like this:

```csharp
public sealed record Order
{
    public string OrderId { get; init; }
    public string CustomerId { get; init; }
    public IReadOnlyList<OrderLine> Lines { get; init; }

    public static Order Create(
        string orderId,
        string customerId,
        IReadOnlyList<OrderLine> lines)
    {
        if (orderId is null) throw new ArgumentException("OrderId is required.");
        if (customerId is null) throw new ArgumentException("CustomerId is required.");

        return new(orderId, customerId, lines);
    }

    public bool Equals(Order? other) =>
        other is not null && OrderId == other.OrderId;

    public MutableOrder ToMutable() => ...;
}
```

## Notes

- Optional CUE fields are absent from exported JSON when unset, so the generator uses key presence as the signal.
- CUE list order is preserved in JSON, producing stable generated output.
- The sample expands records, builders, mutable projections, validation, and a discriminated union from one model.
- Tests compile the generated code and exercise nested builders, round trips, structural equality, validation, and exhaustive matching.
- this feels much closer to the actual target
- define domain data once; extremely strict, rich, multi-purpose model
- model carries enough properties/metadata to generate a wide range of behaviours
- C# generation examples; clone/deep clone, equality, validation, mutability, immutable variants, projections/sub-definitions
- also generate common helper/extension behaviours; empty checks, collection checks, other mathematical/logical properties
- initial schema should describe concepts; code generation handles the implementation details
- starting to feel like QLang may be the wrong application for this
- example: `empty`
- technically could be described as business logic; conceptually feels more like a property of the model
- “empty” may mean specific projected/meaningful fields contain no value; not literally every array/property is empty
- example; cache list may contain data while main projected list is empty
- should be able to define what constitutes semantic emptiness directly on the model
- model definition expresses the concept; generated code implements the actual `IsEmpty` logic
- broader direction; distinguish domain semantics from business logic
- some behaviours that look like logic are really declarative properties of the domain model
