package synthesis

Model: {
	concepts: {
		Quantity: #ValueObject & {
			fields: {
				value: {type: "int", required: true, constraints: ["positive"]}
			}
			augments: ["immutable", "mutable", "builder", "validation", "equality", "conversions"]
		}
		Order: #Entity & {
			identity_field: "order_id"
			fields: {
				order_id:    {type: "string", required: true}
				customer_id: {type: "string", required: true}
			}
			owns: {
				lines: {concept: "OrderLine", multiplicity: "many"}
			}
			augments: ["immutable", "mutable", "builder", "validation", "equality", "conversions"]
		}
		OrderLine: #Entity & {
			identity_field: "product_id"
			fields: {
				product_id: {type: "string", required: true}
				quantity:   {type: "Quantity", required: true}
			}
			augments: ["immutable", "mutable", "builder", "validation", "equality", "conversions"]
		}
		PaymentMethod: #Union & {
			cases: [
				{name: "Card",    fields: {card_token:  {type: "string", required: true}}},
				{name: "Invoice", fields: {invoice_ref: {type: "string", required: true}}},
				{name: "Credit",  fields: {}},
			]
			augments: ["immutable", "equality"]
		}
	}
}
