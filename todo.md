- Introduce anonymous label `@@:`
  - anonymous labels can appear as often as one prefer
  - one can jump to them by doing: `GOTO @F` or `GOTO @B` to go to the next or
    previous occourence of `@@:` respectively
  - one can optionally specify a number: `@F2` or `@B2` to jump that many `@@:`
    labels forward or backward

- Introduce floating point values
  - remove type `NUM` and add types: `INT` and `FLOAT`

- Implement more logical condition, see `evaluateCondition` in parser

- introduce variable casting
  - int -> float | string | bool
  - float -> int | string | bool
  - string -> int | float | bool

- check the isPunctuation function for characters that are not needed
