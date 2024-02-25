# basm

High level interpreted compile target

# How to build

## With `bflat`

[bflat](https://flattened.net/) compiler is required:

- Mac and Linux run `build.sh`
- Windows run `build.bat`

## With C# compiler

run `dotnet build` from the root

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
