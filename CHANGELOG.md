# Changelog

## [3.0.0](https://github.com/atc-net/atc-azure-digitaltwin/compare/v2.0.0...v3.0.0) (2026-02-21)


### ⚠ BREAKING CHANGES

* Event ID values changed for model, telemetry, import job, and component operations.

### Features

* add event route management to IDigitalTwinService ([2af1dbd](https://github.com/atc-net/atc-azure-digitaltwin/commit/2af1dbdff236aadd2fd1b861948cad8f93ab79d6))
* add import jobs API and component operations ([a79d54d](https://github.com/atc-net/atc-azure-digitaltwin/commit/a79d54d9653c5e40893e712940d54836bbac7348))
* add model dependency ordering for bulk upload ([ea53977](https://github.com/atc-net/atc-azure-digitaltwin/commit/ea539770fccd5540263eb6e57035fa3be9ac5e0f))
* add query and telemetry publish CLI commands ([60e66e6](https://github.com/atc-net/atc-azure-digitaltwin/commit/60e66e6dc95335d6c19de23ec3eddfcb034a99e8))
* add telemetry publishing to DigitalTwinService ([34d8cac](https://github.com/atc-net/atc-azure-digitaltwin/commit/34d8cac63683881853b44848c341c0d7371139bd))
* implement event route and twin update CLI commands ([3262968](https://github.com/atc-net/atc-azure-digitaltwin/commit/32629682ba423006090a0d8fd77bbd3cfa021ef9))
* register all services in DI extensions ([7aecda6](https://github.com/atc-net/atc-azure-digitaltwin/commit/7aecda6d50efd724a5aab273098a5baf8aea286d))


### Bug Fixes

* add missing return in DeleteModel error path ([6460714](https://github.com/atc-net/atc-azure-digitaltwin/commit/646071432477c475be969a4e7f2477061a0effa1))
* address PR review feedback for resource disposal and docs ([0e26a92](https://github.com/atc-net/atc-azure-digitaltwin/commit/0e26a921755ea297f53475fceda25a2b64524865))
* align GetHashCode with Equals in comparers ([1b4275f](https://github.com/atc-net/atc-azure-digitaltwin/commit/1b4275f409fca93ab1be86a0e56a8bf0e93ba4bd))
* call GetTwin instead of GetTwins in TwinGetCommand ([0b63519](https://github.com/atc-net/atc-azure-digitaltwin/commit/0b63519923c3a281f02f27dc6c62aa8da2825601))
* catch all exceptions in DigitalTwinParser.Parse ([07d3207](https://github.com/atc-net/atc-azure-digitaltwin/commit/07d32078f2eb621317261e11b72b8871cdd964f4))
* check create response in CreateOrUpdateRelationshipAsync and fix typo ([4f5e0cd](https://github.com/atc-net/atc-azure-digitaltwin/commit/4f5e0cdf5056aa7d48cbc6bbcbec021021e631f2))
* consistent variable casing and log ordering in DigitalTwinService ([07a922f](https://github.com/atc-net/atc-azure-digitaltwin/commit/07a922f07b48e7b43aacab24b0bb6c7615db2d0a))
* create ModelParser per call to eliminate thread-safety issue ([c52d890](https://github.com/atc-net/atc-azure-digitaltwin/commit/c52d890a1460326664072052e69621bd484914ac))
* move Clear before loading in ValidateModels ([ca1ac60](https://github.com/atc-net/atc-azure-digitaltwin/commit/ca1ac604ffaa7ba37d1f97412ca71c3186268a4c))
* parse JSON to match model ID in ModelCreateSingleCommand ([38f09e9](https://github.com/atc-net/atc-azure-digitaltwin/commit/38f09e9b297fb4122ee2ab3c22ce7b44d066b25a))
* propagate base.Validate() in ConnectionBaseCommandSettings ([3ff655f](https://github.com/atc-net/atc-azure-digitaltwin/commit/3ff655f9a00f7e2f1e2253b6f0d194dc0acdc06b))
* propagate parse failure in ValidateModels ([465753b](https://github.com/atc-net/atc-azure-digitaltwin/commit/465753bf25d11e98418b91b743265a31bbe4c4ef))
* propagate UpdateRelationshipAsync failure in CreateOrUpdateRelationshipAsync ([97a2868](https://github.com/atc-net/atc-azure-digitaltwin/commit/97a2868e52ff51e0a16bb58a978d94379dfafc53))
* recursively convert nested JSON in JsonElementToObject ([6522c6b](https://github.com/atc-net/atc-azure-digitaltwin/commit/6522c6b46c9dedcb866737a5ab43bc09a70312ba))
* register parser and model repository as transient in DI ([5756ef9](https://github.com/atc-net/atc-azure-digitaltwin/commit/5756ef9471b6ad92d805df113825e47af22bd7d5))
* rename CLI --relationshipId to --relationshipName to match service contract ([ef0fe6f](https://github.com/atc-net/atc-azure-digitaltwin/commit/ef0fe6fc0b7b2d45d1dd7a7d5b62a3f82b2cdac7))
* rename Succeeeded to Succeeded in IDigitalTwinParser ([fd5b0ee](https://github.com/atc-net/atc-azure-digitaltwin/commit/fd5b0ee758754d227d5faff0c8802e555bf9ea71))
* replace null-forgiving operator with explicit null guard in ModelDeleteAllCommand ([1083149](https://github.com/atc-net/atc-azure-digitaltwin/commit/1083149a001d1ea9148e9692d46fc48410b6c135))
* resolve analyzer errors and thread CancellationToken in core library ([bb800be](https://github.com/atc-net/atc-azure-digitaltwin/commit/bb800beb04dd24273075a3a4537a6c6d269dbe4e))
* resolve analyzer errors in sample project ([8a4d5fe](https://github.com/atc-net/atc-azure-digitaltwin/commit/8a4d5febb2edefffd71dc480b7c5b0a52c3bdb2d))
* use FirstOrDefault in GetRelationshipAsync to avoid exception swallowing ([38e7273](https://github.com/atc-net/atc-azure-digitaltwin/commit/38e7273ecb7180d5bc7e3fe02fd2a6c816b42a16))
* use GetRelationshipAsync for relationship lookup by ID ([13cd714](https://github.com/atc-net/atc-azure-digitaltwin/commit/13cd7142634e650bc182372fcc2cb7d5f1ad3be0))


### Performance Improvements

* use HashSet for unparseable index lookups in topological sort ([7eb50f6](https://github.com/atc-net/atc-azure-digitaltwin/commit/7eb50f6f6947b6cc4e6675cc6dc38e506364eec6))


### Code Refactoring

* consolidate logging event IDs under DigitalTwinService scope ([fc36d1f](https://github.com/atc-net/atc-azure-digitaltwin/commit/fc36d1f1799d02d5e20fb73ce551f032d68233c1))
