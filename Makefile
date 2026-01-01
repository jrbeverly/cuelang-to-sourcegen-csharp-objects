SLN := Synthesis.slnx

.DEFAULT_GOAL := help
.PHONY: help generate build test clean

help: ## Show available targets
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) \
		| awk 'BEGIN {FS = ":.*?## "} {printf "  \033[36m%-12s\033[0m %s\n", $$1, $$2}'

generate: ## Export the CUE model to generated/model.json
	mkdir -p generated
	cue export -e Model --out json ./model > generated/model.json

build: ## Restore and build the whole solution
	dotnet build $(SLN)

test: build ## Build, then run the test suite
	dotnet test $(SLN) --nologo --no-build

clean: ## Remove build outputs (bin/obj) across the solution
	dotnet clean $(SLN)
