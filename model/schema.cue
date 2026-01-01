package synthesis

#Field: {
	type:         string
	required:     bool
	constraints?: [...string]
}

#Owns: {
	concept:      string
	multiplicity: "many"
}

#UnionCase: {
	name:   string
	fields: {[string]: #Field}
}

#ValueObject: {
	kind:     "value_object"
	fields:   {[string]: #Field}
	augments: [...string]
}

#Entity: {
	kind:           "entity"
	identity_field: string
	fields:         {[string]: #Field}
	owns?:          {[string]: #Owns}
	augments:       [...string]
}

#Union: {
	kind:     "union"
	cases:    [...#UnionCase]
	augments: [...string]
}
