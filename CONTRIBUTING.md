# Contribution to DotVast.HashTool.WinUI

You can contribute to HashTool with issues and PRs. Simply filing issues for problems you encounter is a great way to contribute. Contributing implementations is greatly appreciated.

## Reporting Issues

We always welcome bug reports and overall feedback. Here are a few tips on how you can make reporting your issue as effective as possible.

### Finding Existing Issues

Before filing a new issue, please search our [open issues](https://github.com/KiyanYang/DotVast.HashTool.WinUI/issues) to check if it already exists.

If you do find an existing issue, please include your own feedback in the discussion. Do consider upvoting (👍 reaction) the original post, as this helps us prioritize popular issues in our backlog.

### Writing a Good Bug Report

Good bug reports make it easier for maintainers to verify and root cause the underlying problem. The better a bug report, the faster the problem will be resolved. Ideally, a bug report should contain the following information:

* A high-level description of the problem.
* A _minimal reproduction_, i.e. the smallest size of steps required to reproduce the wrong behavior.
* A description of the _expected behavior_, contrasted with the _actual behavior_ observed.
* Information on the environment: OS/distro, CPU arch, SDK version, etc.
* Additional information, e.g. is it a regression from previous versions? are there any known workarounds?

When ready to submit a bug report, please use the issue template.

## Contributing Changes

Project maintainers will merge changes that improve the product significantly.

### DOs and DON'Ts

Please do:

- **DO** follow our [coding style](#coding-style).
- **DO** give priority to the current style of the project or file you're changing even if it diverges from the general guidelines.
- **DO** keep the discussions focused. When a new or related topic comes up it's often better to create new issue than to side track the discussion.
- **DO** clearly state on an issue that you are going to take on implementing it.

Please do not:

- **DON'T** make PRs for style changes.
- **DON'T** surprise us with big pull requests. Instead, file an issue and start a discussion so we can agree on a direction before you invest a large amount of time.
- **DON'T** commit code that you didn't write. If you find code that you think is a good fit to add to the HashTool, file an issue and start a discussion before proceeding.
- **DON'T** submit PRs that alter licensing related files or CI. If you believe there's a problem with them, file an issue and we'll be happy to discuss it.

### Commit Messages

Please use [Conventional Commits](https://www.conventionalcommits.org/).

Also do your best to factor commits appropriately, not too large with unrelated things in the same commit, and not too small with the same small change applied N times in N different commits.

### Branch Naming

Similar to [Conventional Commits](https://www.conventionalcommits.org/), please name the branch as follows:

```
<type>/<username>/<short-description>

examples:

feat/kiyanyang/win11-context-menu
```

### Coding Style

Respect the [.editorconfig](/.editorconfig) file specified in the source tree. For unspecified styles, please refer to:

- C#: Follow the .NET Runtime's [C# coding style](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md)
- C++: [C++ Core Guidelines](https://github.com/isocpp/CppCoreGuidelines/blob/master/CppCoreGuidelines.md)
