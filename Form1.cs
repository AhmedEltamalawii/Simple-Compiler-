using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static compiler.Form1;

namespace compiler
{
    public partial class Form1 : Form
    {
        HashSet<string> keywords = new HashSet<string>()
{
    "int", "float", "double", "string", "if", "else", "while", "for", "return", "bool", "class", "public", "private", "void", "real","new",
};

        public List<string> identifiers = new List<string>();
        List<string> tokens = new List<string>();
        List<string> lexemes = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void lexerButton_Click(object sender, EventArgs e)
        {
            tokensListBox.Items.Clear();
            IdentifiersListBox.Items.Clear();
            identifiers.Clear();
            tokens.Clear();

            string code = codeTextBox.Text;

            string pattern = @"(?<Identifier>[a-zA-Z_][a-zA-Z0-9_]*)|(?<Number>-?\d+(\.\d+)?)|(?<String>""[^""]*"")|(?<Symbol>==|!=|\+=|\-=|<=|>=|[{}()\[\]<>;,=+\-*/])|(?<Whitespace>\s+)|(?<Unknown>.)";

            MatchCollection matches = Regex.Matches(code, pattern);

            foreach (Match match in matches)
            {
                if (match.Groups["Whitespace"].Success)
                    continue;

                if (match.Groups["String"].Success)
                {
                    string value = match.Value;
                    tokensListBox.Items.Add($"Type: String, value: {value}");
                    tokens.Add("string");
                    lexemes.Add(value);
                    continue;
                }

                if (match.Groups["Identifier"].Success)
                {
                    string value = match.Value;
                    if (keywords.Contains(value))
                    {
                        tokensListBox.Items.Add($"Type: Keyword, value: {value}");
                        tokens.Add(value);
                    }
                    else
                    {
                        tokensListBox.Items.Add($"Type: Identifier, value: {value}");
                        tokens.Add("id");
                        lexemes.Add(value);

                        if (!identifiers.Contains(value))
                        {
                            identifiers.Add(value);
                            IdentifiersListBox.Items.Add(value);
                        }
                    }
                }
                else if (match.Groups["Number"].Success)
                {
                    tokensListBox.Items.Add($"Type: Number, value: {match.Value}");
                    tokens.Add("num");
                }
                else if (match.Groups["Symbol"].Success)
                {
                    string symbol = match.Value;
                    string type;
                    switch (symbol)
                    {
                        case "(": type = "openParenthesis"; break;
                        case ")": type = "closeParenthesis"; break;
                        case "[": type = "openBracket"; break;
                        case "]": type = "closeBracket"; break;
                        case "{": type = "openBrace"; break;
                        case "}": type = "closeBrace"; break;
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                            type = "ArithmeticOperator"; break;
                        case "=":
                        case "+=":
                        case "-=":
                            type = "AssignmentOperator"; break;
                        case "==":
                        case "!=":
                        case "<=":
                        case ">=":
                        case "<":
                        case ">":
                            type = "ComparisonOperator"; break;
                        case ";":
                            type = "Semicolon"; break;
                        case ",":
                            type = "Comma"; break;
                        default:
                            type = "symbol"; break;
                    }
                    tokensListBox.Items.Add($"Type: {type}, Value: {match.Value}");
                    tokens.Add(type);
                }
                else if (match.Groups["Unknown"].Success)
                {
                    tokensListBox.Items.Add($"Type: Unknown, Value: {match.Value}");
                    tokens.Add(match.Value);
                }
            }
        }


        private void parserButton_Click(object sender, EventArgs e)
        {

            SimpleParser parser = new SimpleParser(tokens, lexemes, this);
            bool result = parser.parse();

            if (result)
            {
                MessageBox.Show("Syntax Correct", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Syntax Error", "Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class SimpleParser
        {
            private List<string> tokens;
            private int currentTokenIndex = 0;
            private Form1 form;
            private List<string> lexemes;

            public SimpleParser(List<string> tokens, List<string> lexemes, Form1 form)
            {
                this.tokens = tokens;
                this.lexemes = lexemes;
                this.form = form;

            }

            public bool parse()
            {
                try
                {
                    Program();
                    if (currentTokenIndex < tokens.Count)
                    {
                        throw new Exception("Extra tokens after program end");
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Parser error: {ex.Message}");
                    return false;
                }
            }

            private void Program()
            {
                if (Match("openBrace"))
                    Block();
                else
                    DeclarationList();
            }

            private void DeclarationList()
            {
                while (currentTokenIndex < tokens.Count)
                {
                    if (IsDeclaration())
                    {
                        Declaration();
                    }
                    else
                    {
                        Statement();
                    }
                }
            }

            private void Declaration()
            {
                if (Match("public") || Match("private"))
                {
                    Advance();
                }

                if (Match("int") || Match("float") || Match("double") || Match("string") || Match("bool") || Match("void") || Match("real"))
                {
                    string type = Current();
                    Advance(); // skip type

                    // Handle C# style array declaration (int[] x = new int[20])
                    if (Match("openBracket") && Peek(1) == "closeBracket")
                    {
                        Advance(); // skip '['
                        Advance(); // skip ']'

                        if (!Match("id"))
                            throw new Exception("Expected identifier after array type");

                        string arrayName = Current();
                        Advance();

                        if (Match("AssignmentOperator"))
                        {
                            Advance(); // skip '='

                            if (!Match("new"))
                                throw new Exception("Expected 'new' for array initialization");
                            Advance();

                            if (!Match(type))
                                throw new Exception($"Expected type '{type}' after 'new'");
                            Advance();

                            if (!Match("openBracket"))
                                throw new Exception("Expected '[' for array size");
                            Advance();

                            if (!Match("num") && !Match("id"))
                                throw new Exception("Expected array size");
                            Advance();

                            if (!Match("closeBracket"))
                                throw new Exception("Expected ']' after array size");
                            Advance();
                        }

                        if (!form.identifiers.Contains(arrayName))
                        {
                            form.identifiers.Add(arrayName);
                            form.IdentifiersListBox.Items.Add(arrayName);
                        }

                        if (!Match("Semicolon"))
                            throw new Exception("Expected ';' after array declaration");
                        Advance();

                        return;
                    }
                    else if (!Match("id"))
                        throw new Exception("Expected identifier after type.");

                    string varName = Current();
                    Advance();

                    // Handle function declaration
                    if (Match("openParenthesis"))
                    {
                        Advance(); // skip '('

                        // Handle parameters if any
                        if (!Match("closeParenthesis"))
                        {
                            ParseParameterList();
                        }

                        if (!Match("closeParenthesis"))
                            throw new Exception("Expected ')' after function parameters");
                        Advance();

                        if (!Match("openBrace"))
                            throw new Exception("Expected '{' for function body");
                        Advance();

                        // Handle function body
                        while (!Match("closeBrace") && currentTokenIndex < tokens.Count)
                        {
                            if (Match("return"))
                            {
                                Advance();
                                if (Match("Semicolon"))
                                {
                                    Advance();
                                    break;
                                }
                                else
                                {
                                    Expression();
                                    if (!Match("Semicolon"))
                                        throw new Exception("Expected ';' after return expression");
                                    Advance();
                                    break;
                                }
                            }
                            else
                            {
                                Statement();
                            }
                        }

                        if (!Match("closeBrace"))
                            throw new Exception("Expected '}' to close function body");
                        Advance();

                        if (!form.identifiers.Contains(varName))
                        {
                            form.identifiers.Add(varName);
                            form.IdentifiersListBox.Items.Add(varName);
                        }
                        return;
                    }

                    // Handle variable assignment
                    if (Match("AssignmentOperator"))
                    {
                        Advance(); // skip '='

                        if (!Match("num") && !Match("id") && !Match("string"))
                            throw new Exception("Expected value after assignment");

                        string value = CurrentLexeme();
                        Advance();

                        if (!form.identifiers.Contains(varName))
                        {
                            form.identifiers.Add(varName);
                            form.IdentifiersListBox.Items.Add(varName);
                        }

                        if (!Match("Semicolon"))
                            throw new Exception("Expected ';' after variable declaration");
                        Advance();

                        form.tokensListBox.Items.Add($"Variable declaration: {type} {varName} = {value}");
                        return;
                    }
                    else if (!Match("Semicolon"))
                    {
                        throw new Exception("Expected ';' after variable declaration");
                    }
                    else
                    {
                        if (!form.identifiers.Contains(varName))
                        {
                            form.identifiers.Add(varName);
                            form.IdentifiersListBox.Items.Add(varName);
                        }
                        Advance(); // skip semicolon
                        return;
                    }
                }
                else
                {
                    throw new Exception($"Unknown start of declaration: {Current()}");
                }
            }

            private bool IsDeclaration()
            {
                return Match("int") || Match("double") || Match("string") || Match("float")
                    || Match("bool") || Match("void") || Match("real") || Match("public") || Match("private");
            }

            // some important function
            private bool Match(string expected)
            {
                return currentTokenIndex < tokens.Count && tokens[currentTokenIndex] == expected;
            }
            private string Current()
            {
                if (currentTokenIndex < tokens.Count)
                    return tokens[currentTokenIndex];
                return null;
            }
            private void Advance()
            {
                currentTokenIndex++;
            }
            private string CurrentLexeme()
            {
                if (currentTokenIndex < lexemes.Count)
                    return lexemes[currentTokenIndex];
                return null;
            }

            // divide code
            private void Term()
            {
                if (Match("id"))
                {
                    if (Peek(1) == "openParenthesis")
                    {
                        ParseFunctionCall();
                    }
                    else
                    {
                        Advance();
                    }
                }
                else if (Match("num"))
                    Advance();
                else if (Match("openParenthesis"))
                {
                    // for more one parenthesisis
                    Expression();
                }
                else
                {
                    throw new Exception("Expected identifier or number in expression.");
                }
            }
            private void Block()
            {
                Advance();
                while (currentTokenIndex < tokens.Count && !Match("closeBrace"))
                {
                    if (IsDeclaration())
                        Declaration();
                    else
                        Statement();
                }
                if (!Match("closeBrace"))
                    throw new Exception("Expected '}' to close block");
                Advance();
            }
            private void CheckVariableExists(string varName)
            {
                if (!form.identifiers.Contains(varName))
                    throw new Exception($"Undeclared variable: {varName}");
            }

            private string Peek(int lookahead)
            {
                if (currentTokenIndex + lookahead < tokens.Count)
                    return tokens[currentTokenIndex + lookahead];
                return null;
            }
            private void ParseFunctionCall()
            {
                if (!Match("id"))
                    throw new Exception("Expected function name");

                string funcName = CurrentLexeme();
                Advance();

                if (!Match("openParenthesis"))
                    throw new Exception("Expected '(' after function name");
                Advance();

                if (!Match("closeParenthesis"))
                {
                    Expression();
                    while (Match("Comma"))
                    {
                        Advance();
                        Expression();
                    }
                }

                if (!Match("closeParenthesis"))
                    throw new Exception("Expected ')' to close function call");
                Advance();
                bool isStandaloneStatement = !IsPartOfExpression();

                if (isStandaloneStatement)
                {
                    if (!Match("Semicolon"))
                        throw new Exception("Expected ';' after function call statement");
                    Advance();
                }

                form.tokensListBox.Items.Add($"Function call: {funcName}()");
            }


            // Expersion 
            private bool IsPartOfExpression()
            {
                if (currentTokenIndex >= tokens.Count - 1)
                    return false;

                // Get next token without consuming it
                string nextToken = tokens[currentTokenIndex + 1];

                // List of tokens that indicate the function call is part of larger expression
                string[] expressionTokens = {
                    "ArithmeticOperator",
                    "ComparisonOperator",
                    "AssignmentOperator",
                    "Comma",
                    "closeParenthesis",
                    "openBracket",
                    "Semicolon"
                };

                return expressionTokens.Contains(nextToken);
            }
            private void Expression()
            {
                if (Match("openParenthesis"))
                {
                    Advance();
                    Expression();

                    if (Match("openParenthesis"))
                        throw new Exception("Expected ')' to close parenthesis");
                    Advance();
                }
                else
                    Term();

                while (Match("ArithmeticOperator") || Match("ComparisonOperator"))
                {
                    string op = Current();
                    if (op == "=")
                    {
                        throw new Exception("Expected comparison operator '==' instead of assignment operator '=' in expression");
                    }
                    Advance();
                    Term();
                }
            }

            // Array
            private bool ParseArrayDeclaration(out string identifierName)
            {
                identifierName = null;

                try
                {
                    if (currentTokenIndex >= tokens.Count)
                        return false;

                    if (!Match("openBracket"))
                        return false;
                    Advance();

                    if ((!Match("num") && !Match("id")))
                    {
                        throw new Exception("Expected array size (number or constant)");
                    }
                    Advance();

                    if (!Match("closeBracket"))
                        throw new Exception("Expected ']' to close array declaration");
                    Advance();

                    if (currentTokenIndex >= tokens.Count || !Match("id"))
                        throw new Exception("Expected array name after type");

                    identifierName = Current();
                    Advance();

                    if (Match("AssignmentOperator"))
                    {
                        Advance();

                        if (currentTokenIndex >= tokens.Count || !Match("new"))
                            throw new Exception("Expected 'new' for array initialization");
                        Advance();

                        if (currentTokenIndex >= tokens.Count ||
                           !(Match("int") || Match("float") || Match("string") || Match("bool") || Match("real")))
                            throw new Exception("Expected array type after 'new'");
                        Advance();

                        if (currentTokenIndex >= tokens.Count || !Match("openBracket"))
                            throw new Exception("Expected '[' for array size");
                        Advance();

                        if (currentTokenIndex >= tokens.Count || (!Match("num") && !Match("id")))
                            throw new Exception("Expected array size");
                        Advance();

                        if (currentTokenIndex >= tokens.Count || !Match("closeBracket"))
                            throw new Exception("Expected ']' after array size");
                        Advance();
                    }
                    else if (Match("openBrace"))
                    {
                        ParseArrayInitializer();
                    }

                    if (currentTokenIndex >= tokens.Count || !Match("Semicolon"))
                        throw new Exception("Expected ';' at end of declaration");
                    Advance();

                    if (form != null)
                    {
                        form.tokensListBox.Items.Add($"Type: Array, Identifier: {identifierName}");
                        if (!form.identifiers.Contains(identifierName))
                        {
                            form.identifiers.Add(identifierName);
                            form.IdentifiersListBox.Items.Add(identifierName);
                        }

                    }


                    return true;
                }
                catch (Exception ex)
                {
                    if (form != null)
                        MessageBox.Show($"Parser error: {ex.Message}");
                    return false;
                }
            }
            private void ParseArrayInitializer()
            {
                if (!Match("openBrace"))
                    throw new Exception("Expected '{' to start array initializer");
                Advance();

                if (Match("closeBrace"))
                {
                    Advance();
                    return;
                }

                if (!Match("num") && !Match("id"))
                    throw new Exception("Expected number or identifier in array initializer");
                Advance();

                while (Match("Comma"))
                {
                    Advance();

                    if (!Match("num") && !Match("id"))
                        throw new Exception("Expected number or identifier after comma");
                    Advance();
                }

                if (!Match("closeBrace"))
                    throw new Exception("Expected '}' to end array initializer");
                Advance();
            }

            // if & While
            public void Condition()
            {
                Expression();
                if (Match("AssignmentOperator"))
                {
                    throw new Exception("Expected comparison operator '==' instead of assignment operator '=' in condition");
                }
                if (Match("ComparisonOperator"))
                {
                    string op = Current();
                    if (op == "=")
                    {
                        throw new Exception("Expected comparison operator '==' instead of assignment operator '=' in condition");
                    }
                    Advance();
                    Expression();
                }
            }
            private void WhileStatement()
            {
                Advance();
                if (!Match("openParenthesis"))
                    throw new Exception("Expected '(' after 'if'");
                Advance();

                Condition();

                if (!Match("closeParenthesis"))
                    throw new Exception("Expected '(' after 'if'");
                Advance();
            }
            private void ElseStatement()
            {
                Advance();
                Statement();
            }
            private void IfStatement()
            {
                Advance(); // skip 'if'
                if (!Match("openParenthesis"))
                    throw new Exception("Expected '(' after 'if'");
                Advance();

                Condition();

                if (!Match("closeParenthesis"))
                    throw new Exception("Expected ')' after condition");
                Advance();

                if (!Match("openBrace"))
                    throw new Exception("Expected '{' after if condition");
                Advance();

                while (!Match("closeBrace") && currentTokenIndex < tokens.Count)
                {
                    Statement();
                }

                if (!Match("closeBrace"))
                    throw new Exception("Expected '}' to close if block");
                Advance();
            }

            // Statement
            private void CompoundStatement()
            {
                while (!Match("closeBrace") && currentTokenIndex < tokens.Count)
                {
                    Statement();
                }
                if (!Match("closeBrace"))
                    throw new Exception("Expected '}' at end of compound statement.");
                Advance();
            }
            private void Statement()
            {
                if (Match("openBrace"))
                    Block();
                else if (Match("if"))
                    IfStatement();
                else if (Match("while"))
                    WhileStatement();
                else if (Match("else"))
                    ElseStatement();
                else if (Match("return"))
                {
                    Advance();
                    if (Match("Semicolon"))
                    {
                        Advance(); // skip ';'
                        return;
                    }
                    Expression();
                    if (!Match("Semicolon"))
                        throw new Exception("Expected ';' after return expression.");
                    Advance();
                }
                else if (Match("id"))
                {
                    if (Peek(1) == "openParenthesis")
                    {
                        ParseFunctionCall();
                    }
                    else
                    {
                        AssignmentStatement();
                    }
                }
                else
                {
                    throw new Exception("Unsupported statement");
                }
            }
            private void AssignmentStatement()
            {
                if (!Match("id"))
                    throw new Exception("Expected identifier in assignment");
                string varName = CurrentLexeme();
                //CheckVariableExists(varName);
                Advance();

                if (Match("openBracket"))
                {
                    Advance();

                    Expression();

                    if (!Match("closeBracket"))
                        throw new Exception("Expected ']' after array index");
                    Advance();
                }

                if (!Match("AssignmentOperator"))
                    throw new Exception("Expected Assignment Operator");
                Advance();

                Expression();

                if (!Match("Semicolon"))
                    throw new Exception("Expected ';' after assignment");
                Advance();

                form.tokensListBox.Items.Add($"Assignment: {varName} = ...");
            }

            // parameter 
            private void ParseParameterList()
            {
                ParseParameter();
                while (Match("Comma"))
                {
                    Advance();
                    ParseParameter();
                }
            }
            private void ParseParameter()
            {
                if (!(Match("int") || Match("float") || Match("double") || Match("string") || Match("bool") || Match("real")))
                    throw new Exception("Expected parameter type.");
                string paraType = Current();
                Advance();


                if (!Match("id"))
                    throw new Exception("Expected parameter name.");
                string paraName = Current();
                Advance();

                if (Match("openBracket"))
                {
                    Advance(); // skip '['

                    if (!Match("closeBracket"))
                        throw new Exception("Expected ']' for array parameter");

                    Advance(); // skip ']'

                    form.tokensListBox.Items.Add($"Array parameter: {paraType} {paraName}[]");
                }
                else
                {
                    form.tokensListBox.Items.Add($"Regular parameter: {paraType} {paraName}");
                }

            }

            // 
        }
    }
}
