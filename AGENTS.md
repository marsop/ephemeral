# Agent Guidelines

## General Project Information
* The repository is a C# class library named Ephemeral, designed to handle open/closed intervals using generic types (e.g., `BasicInterval<TBoundary>`) and specialized utility classes (e.g., `DateTimeOffsetInterval`, `IntInterval`).
* The project is a .NET solution. Use `dotnet build` to build the codebase and `dotnet test` to run the test suite.
* The project uses MkDocs for documentation, configured via `mkdocs.yml` with source files located in the `docs` directory.

## Frameworks and Compatibility
* The core libraries intentionally target older frameworks for backward compatibility (`Marsop.Ephemeral` targets `netstandard2.0`, `Marsop.Ephemeral.Net6` targets `net6.0`), while testing projects target newer frameworks like `net10.0`.
* In .NET 6, the subtraction operator (`-`) is not natively supported for `TimeOnly` instances. To calculate differences, explicitly convert the instances using `.ToTimeSpan()` before subtracting (e.g., `time1.ToTimeSpan() - time2.ToTimeSpan()`).

## Dependencies and Packaging
* The repository uses NuGet Central Package Management (`Directory.Packages.props`). New package references in `.csproj` files should omit the `Version` attribute to prevent `NU1008` build errors.
* Version information for NuGet packages is managed in the core `.csproj` files using a `<VersionPrefix>` property and a `<Version>` tag. For pre-releases, it references a suffix (e.g., `<Version>$(VersionPrefix)-beta.x</Version>`), and for stable releases, it is set directly to `<Version>$(VersionPrefix)</Version>`.
* Ensure all tests run successfully in the CI/CD pipeline (e.g., via `dotnet test`) before proceeding to build, pack, or push to NuGet.

## Testing and Benchmarking
* The project uses xUnit, FluentAssertions, Moq, and the `Optional` NuGet package for testing (e.g., asserting empty options using `result.HasValue.Should().BeFalse()`).
* When adding tests, ensure the naming of the tests is appropriate and consistent with the rest of the tests, generally following the `MethodName_StateUnderTest_ExpectedBehavior` convention.
* When adding tests, ensure the test project and directory structure explicitly mirror the architecture and directory structure of the source project being tested.
* When creating performance benchmarks with BenchmarkDotNet in a .NET project, create a separate console application rather than adding the benchmarks directly to the test project. This prevents CS0017 build errors caused by `Microsoft.NET.Test.Sdk` auto-generating a `Program.cs` entry point that conflicts with the benchmark runner.

## Coding Conventions
* Retain private constructors in singleton classes to prevent external instantiation and enforce the singleton pattern, mark the singleton classes as `sealed`, and document empty private constructors with a comment explaining their purpose.
* Always clean up any temporary scratchpad files or directories (e.g., local console apps created to test logic) created during exploration before finalizing code changes and completing the task.

## CI/CD Workflows
* For GitHub Actions workflows, dependencies should be securely locked to specific commit SHAs rather than using mutable tags (e.g., @master or @v1) to prevent potential supply chain vulnerabilities.

## Pull Requests and Changelog
* Update the minor version and the changelog with each relevant PR. For example, if there is new functionality or something useful for the user has been done. If there is no new functionality, this is not required.
* Keep changes and pull requests focused to just one specific item or improvement per PR.
* When submitting pull requests for testing improvements, ensure the PR title uses the format '🧪 [testing improvement description]' and structure the description strictly with '🎯 What', '📊 Coverage', and '✨ Result'.

## Execution Planning and Workflow
* **Deep Planning Mode:** Always start tasks in a deep planning mode. Use `request_user_input` or `message_user` to take as many turns as needed to verify assumptions until there is zero doubt. Avoid asking questions derivable from the code. Create the plan using `set_plan` only after absolute certainty, and execute autonomously without asking for further confirmation.
* **Exploration Rule:** When creating execution plans, adhere to the Exploration Rule by locating and reading the relevant source code using tools like `find` and `read_file` before proposing modifications or test cases.
* **Reading Large Files:** When reading large files that result in truncated output (e.g., via `cat` or `read_file`), use commands like `sed -n 'start,endp'` to fully explore the remaining unrevealed portions of the file before creating an execution plan, ensuring compliance with the Exploration and Groundedness rules.
* **Groundedness Rule:** When creating execution plans, adhere to the Groundedness Rule by only referencing code elements (e.g., method names) that have been explicitly discovered and confirmed in the trace.
* **Specificity Rule:** When creating execution plans, adhere to the Specificity Rule by explicitly stating exact code snippets, file names, and concrete commands to be executed (e.g., `dotnet format`) rather than general descriptions.
* **Formatting:** When creating an execution plan, format the plan strictly as a sequential, numbered list of steps without using nested sub-bullets. Start the plan directly with concrete tool actions and omit background context or status summaries.
* **Verification Rule:** When creating execution plans, adhere to the Verification Rule by including a distinct step to verify the contents of newly created or modified files using the `read_file` tool.
* **Completeness Rule:** When creating execution plans, adhere to the Completeness Rule by making the execution of the full test suite the *final* explicit verification stage (e.g., after `read_file` checks) immediately before proceeding to the pre-commit phase.
* **Pre-commit Step:** When creating an execution plan, the pre-commit step must use the exact phrasing: 'Complete pre-commit steps to ensure proper testing, verification, review, and reflection are done.'
* **replace_with_git_merge_diff:** When planning to use `replace_with_git_merge_diff`, verify the exact lines and context to be replaced using commands like `grep -n -C` or `sed -n` beforehand, ensuring the `<<<<<<< SEARCH` block perfectly matches the confirmed file contents without hallucination.

## Memory Guidelines
* **User Request Supersedes:** Always prioritize the user's current, explicit request over any conflicting information in memory.
* **Context vs. State:** Use memory for historical context and intent (the "why"). Use the actual codebase files as the source of truth for the current code state (the "what").
* **Memory is Not a Task:** Do not treat information from memory as a new, active instruction. Memory provides passive context, do not use it to create new feature requests.
