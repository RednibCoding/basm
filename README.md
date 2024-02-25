# BASM

The BASM programming language, short for "Basic Assembly," is designed to serve
as a higher-level intermediate language that acts as a compilation target for
various programming languages. BASM is both a compiler (to it's own IL) and a
virtual machine (VM).

# Design Philosophy

The BASM programming language is engineered with a distinct philosophy
prioritizing ease of implementation and portability across platforms, while
still achieving reasonable performance. This approach is guided by the Pareto
principle, often referred to as the 80/20 rule, suggesting that with 20% of the
effort, it's possible to attain 80% of the maximum achievable performance. This
principle underlines BASM's design and operational goals, making it a unique
player in the field of programming languages and virtual machines.

BASM's primary objective is not to maximize efficiency at the cost of complexity
or portability but to ensure that the language and its corresponding virtual
machine can be easily implemented and ported across different platforms. This
philosophy acknowledges the trade-offs between the ultimate performance and
broader accessibility and usability of the language and its ecosystem.

# Ease of Implementation

A core tenet of BASM is to reduce the complexity inherent in implementing both
the compiler and the virtual machine. This simplicity facilitates quicker
development cycles, easier maintenance, and the ability to extend the language
and runtime environment with minimal effort. By focusing on straightforward
implementation, BASM lowers the barrier to entry for developers looking to
target new platforms or experiment with language design and compilation
techniques.

# Portability

Portability is a cornerstone of BASM's design. The language and VM are crafted
to ensure that they can be easily adapted to various operating systems and
hardware architectures. This flexibility allows BASM programs to run in diverse
environments without necessitating significant alterations to the codebase,
making BASM an attractive option for applications requiring cross-platform
compatibility.

# Performance Considerations

While BASM emphasizes ease of implementation and portability, it also aims to
deliver respectable performance. By achieving 80% of the potential performance
with a fraction of the effort required for full optimization, BASM positions
itself as sufficiently performant for a wide array of use cases. This level of
efficiency is adequate for many applications, striking a balance between the
resource-intensive pursuit of peak performance and the practical needs of
software development.

# Conclusion

BASM distinguishes itself through its pragmatic approach to language design and
virtual machine implementation. By prioritizing ease of implementation and
portability, and acknowledging the practical trade-offs between optimal
performance and broader usability, BASM offers a compelling toolset for
developers and educators alike.

# How to Build the BASM Project

Building your BASM project can be accomplished in two primary ways depending on
the tools and environment you're working with. Below are detailed instructions
for both methods.

## Building with `bflat`

The `bflat` compiler is a specialized tool designed for compiling BASM language
projects. It simplifies the process, ensuring your BASM code is properly
compiled into an executable format. Before proceeding, ensure you have the
`bflat` compiler installed on your system. You can download and find
installation instructions at [flattened.net](https://flattened.net/).

## On Mac and Linux

1. Open a **Terminal Window**: Navigate to your project's root directory where
   the `build.sh` script is located.
2. **Grant Execution Permissions**: If you haven't already, ensure the
   `build.sh` script is executable by running:

```bash
chmod +x build.sh
```

3. **Execute the Build Script**: Run the following command in the terminal:

```bash
./build.sh
```

This script will automatically clean the previous build, create a new build
directory, and invoke the `bflat` compiler with the necessary options to compile
the BASM project.

## On Windows

1. **Open Command Prompt**: Navigate to your project's root directory where the
   `build.bat` script is located.
2. **Execute the Build Script**: Run the following command in the command
   prompt:

```cmd
.\build.bat
```

The `build.bat` script performs similar actions as its Linux/Mac counterpart,
tailored for the Windows environment. It cleans up old builds and compiles the
project using `bflat`.

# Building with C# Compiler

You can also utilize the dotnet build command.

1. **Open a Terminal** or **Command Prompt**: Navigate to your project's root
   directory where the `.csproj` or `.sln` file is located.
2. `Execute the Build Command`: Run the following command:

```cmd
dotnet build
```

This command compiles the project, its dependencies, and produces a binary
executable. `dotnet build` automatically restores any missing dependencies and
processes the project files.

### Note:

- Ensure you have the `.NET SDK` installed on your system to use `dotnet`
  commands. If you're unsure, you can check by running `dotnet --version` in
  your terminal or command prompt.
- For more detailed options and configurations for the `dotnet build` command,
  you can refer to the official .NET documentation or use `dotnet build --help`
  to explore available parameters.

# Testing

In the `examples folder` are some test programs. You can execute them all with
the `test.bat` or `test.sh` script, depending on your platform.

# TODO

- [x] create a basic lexer, parser and tree walking interpreter

- [x] add different data types
  - [x] add int
  - [x] add float type
  - [x] add bool type
  - [x] add string type

- [x] variable creation and assignment
- [x] reassignment of variables
- [x] basic arithmetic expressions
- [x] `if` and `else` control flow
- [x] `goto` and `gosub` and named labels

- [x] add keywords:
  - [x] print,
  - [x] if,
  - [x] then,
  - [x] goto,
  - [x] input,
  - [x] let,
  - [x] set,
  - [x] gosub,
  - [x] return,
  - [x] clear,
  - [x] list,
  - [x] run,
  - [x] end,

- [x] implement string concatenation via `+` operator

- [x] add some example programs under `examples` folder
- [x] use these for testing. run: `test.bat` or `test.sh` to run the entire
      examples folder

- [ ] Introduce anonymous label `@@:`

  - anonymous labels can appear as often as one likes
  - one can jump to them by doing: `goto @F` or `goto @B` to go to the next or
    previous occourence of `@@:` respectively
  - one can optionally specify a number: `@F2` or `@B2` to jump that many `@@:`
    labels forward or backward

- [ ] Implement more logical condition, see `evaluateCondition` function in
      parser

- [ ] check the `isPunctuation` function and remove characters that are not
      needed

- [ ] add more datatypes and match mostly c's data types

  | Type   | Minimum size (bits) |
  | ------ | ------------------- |
  | char   | 8                   |
  | uchar  | 8                   |
  | short  | 16                  |
  | ushort | 16                  |
  | int    | 32                  |
  | uint   | 32                  |
  | long   | 64                  |
  | ulong  | 64                  |
  | float  | 32                  |
  | double | 64                  |

- [ ] introduce variable type casting

- [ ] introduce ffi to c to be able to call c functions
  - [ ] then get rid of intrinsics like `print` and `input` and use c functions
        instead

- [ ] get rid of the temporary tree walking interpreter an build a proper VM
  - [ ] produce an AST instead of directly interpreting the code
  - [ ] design an instruction set for our VM
  - [ ] write a code generator that traverses the AST and produces bytecode
  - [ ] write the actual VM that executes these instructions

- [ ] write a simple vs code extension for syntax highlighting
- [ ] write a language server for linting in vs code
