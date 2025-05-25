# 🛠️ C# Windows Forms Compiler Front-End

## 📘 Project Overview

This project is a C# Windows Forms application that simulates a front-end compiler. It performs lexical and syntactic analysis of a simple programming language, displaying tokens, identifiers, and parsed structures through a graphical user interface (GUI). It's designed for educational use, ideal for understanding how compilers parse and validate source code.

---

## 🖼️ Features

- **Lexical analysis**: Tokenizes source code into keywords, identifiers, numbers, operators, etc.
- **Syntax analysis**: Validates correct grammar structure using recursive descent parsing.
- Supports:
  - Variable and array declarations
  - Expressions and arithmetic operations
  - Function definitions and function calls
  - Conditional (`if-else`) and looping (`while`) constructs
  - Return statements and parameter handling
- GUI displays:
  - Token stream
  - Identifier list
  - Parsing and error messages

---

## 🏗️ Architecture

- **Lexer**: Scans the input and generates a list of tokens and lexemes.
- **Parser**: Uses recursive descent parsing to analyze token structure.
- **GUI**: Displays analysis results (tokens, identifiers, parsing flow) interactively.

---

## 📁 File Structure








---

## 🧠 Key Methods & Responsibilities

### 🔹 Lexical Matching Helpers

- `bool Match(string expected)`: Checks if the current token matches the expected value.
- `string Current()`: Returns the current token.
- `void Advance()`: Moves to the next token.
- `string CurrentLexeme()`: Returns the current lexeme.
- `string Peek(int lookahead)`: Looks ahead to a future token without consuming it.

---

### 🔹 Declarations

- `bool IsDeclaration()`: Determines if the current token is a valid type keyword.
- `bool ParseArrayDeclaration(out string identifierName)`: Parses array declarations, checks type, size, initialization, and final semicolon.
- `void ParseArrayInitializer()`: Parses `{}`-style inline array initializers.

---

### 🔹 Expressions and Terms

- `void Term()`: Parses an individual expression term (number, identifier, or nested expression).
- `void Expression()`: Parses full expressions including arithmetic and comparison operations.
- `bool IsPartOfExpression()`: Checks if the function call is used within a larger expression.

---

### 🔹 Function Calls

- `void ParseFunctionCall()`: Parses a function call expression, validates parameters and closing parentheses.

---

### 🔹 Blocks and Statements

- `void Block()`: Parses a block of code inside `{}` and delegates to `Declaration()` or `Statement()`.
- `void CompoundStatement()`: Parses a series of statements within a block.
- `void Statement()`: Recognizes and parses one of several types of statements:
  - `if`
  - `else`
  - `while`
  - `return`
  - `assignment`
  - `function call`

---

### 🔹 Control Flow

- `void IfStatement()`: Parses `if` conditions and the enclosed code block.
- `void ElseStatement()`: Parses an `else` branch following an `if`.
- `void WhileStatement()`: Parses `while` loop condition and body.
- `void Condition()`: Parses and validates logical conditions for `if` and `while`.

---

### 🔹 Assignment Handling

- `void AssignmentStatement()`: Parses assignment of values to variables or array elements. Ensures assignment operator and semicolon.

---

### 🔹 Parameters

- `void ParseParameterList()`: Parses comma-separated parameters in function definitions.
- `void ParseParameter()`: Parses a single parameter declaration (type and identifier, with optional array).

---

### 🔹 Utility

- `void CheckVariableExists(string varName)`: Ensures that a variable has been declared before use.

---

## 💡 Sample Code Input

```c
int x;
int[] arr = new int[5];
void print(int x) {
    if (x == 0) {
        return;
    }
    print(x - 1);
}


