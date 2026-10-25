# Unit Testing

## General

This project uses [TUnit](https://tunit.dev) as testing framework including the mocking functionality. Execution takes place using the new [Microsoft Testing Platform (MTP)](https://learn.microsoft.com/en-us/dotnet/core/testing/microsoft-testing-platform-intro). The results are added in the github actions summary and allure.

## Test-Writing

Tests must be clearly focussed. Always add descriptions for what is expected in the assertion. A test **SHOULD** only consist of one assertion. If more than one assertion is used they **MUST** be grouped within an `Assert.Multiple` scope.

## Architectural Tests

Architectural guidelines may be enforced using Unit Tests.

## Closing Words

Try to keep the code coverage high. 100% is not desirable, but it **SHOULD NOT** fall below 60% and **MUST NOT** fall below 50%.
