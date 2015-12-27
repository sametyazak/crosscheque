
namespace LinkedLoopList.Evaluator
{
    // Generate header specific to lexer CSharp file
    using System;
    using Stream = System.IO.Stream;
    using TextReader = System.IO.TextReader;
    using Hashtable = System.Collections.Hashtable;
    using Comparer = System.Collections.Comparer;

    using TokenStreamException = antlr.TokenStreamException;
    using TokenStreamIOException = antlr.TokenStreamIOException;
    using TokenStreamRecognitionException = antlr.TokenStreamRecognitionException;
    using CharStreamException = antlr.CharStreamException;
    using CharStreamIOException = antlr.CharStreamIOException;
    using ANTLRException = antlr.ANTLRException;
    using CharScanner = antlr.CharScanner;
    using InputBuffer = antlr.InputBuffer;
    using ByteBuffer = antlr.ByteBuffer;
    using CharBuffer = antlr.CharBuffer;
    using Token = antlr.Token;
    using IToken = antlr.IToken;
    using CommonToken = antlr.CommonToken;
    using SemanticException = antlr.SemanticException;
    using RecognitionException = antlr.RecognitionException;
    using NoViableAltForCharException = antlr.NoViableAltForCharException;
    using MismatchedCharException = antlr.MismatchedCharException;
    using TokenStream = antlr.TokenStream;
    using LexerSharedInputState = antlr.LexerSharedInputState;
    using BitSet = antlr.collections.impl.BitSet;

    public class ExpressionLexer : antlr.CharScanner, TokenStream
    {
        public const int EOF = 1;
        public const int NULL_TREE_LOOKAHEAD = 3;
        public const int QUESTION_MARK = 4;
        public const int COLON = 5;
        public const int LITERAL_OR = 6;
        public const int LITERAL_AND = 7;
        public const int EQUAL = 8;
        public const int NOT_EQUAL = 9;
        public const int LITERAL_IN = 10;
        public const int LITERAL_NOT = 11;
        public const int LTH = 12;
        public const int LTE = 13;
        public const int GT = 14;
        public const int GTE = 15;
        public const int LPAREN = 16;
        public const int COMMA = 17;
        public const int RPAREN = 18;
        public const int PLUS = 19;
        public const int MINUS = 20;
        public const int MULT = 21;
        public const int DIV = 22;
        public const int MOD = 23;
        public const int ID = 24;
        public const int NUMBER = 25;
        public const int STRING_LITERAL = 26;
        public const int TRUE = 27;
        public const int FALSE = 28;
        public const int WS = 29;
        public const int CHAR_LITERAL = 30;
        public const int ESC = 31;
        public const int DIGIT = 32;
        public const int SIGN_MINUS = 33;
        public const int SIGN_PLUS = 34;

        public ExpressionLexer(Stream ins)
            : this(new ByteBuffer(ins))
        {
        }

        public ExpressionLexer(TextReader r)
            : this(new CharBuffer(r))
        {
        }

        public ExpressionLexer(InputBuffer ib)
            : this(new LexerSharedInputState(ib))
        {
        }

        public ExpressionLexer(LexerSharedInputState state)
            : base(state)
        {
            initialize();
        }
        private void initialize()
        {
            caseSensitiveLiterals = true;
            setCaseSensitive(true);
            literals = new Hashtable(100, (float)0.4, null, Comparer.Default);
            literals.Add("NOT", 11);
            literals.Add("AND", 7);
            literals.Add("IN", 10);
            literals.Add("OR", 6);
        }

        override public IToken nextToken()			//throws TokenStreamException
        {
            IToken theRetToken = null;
        tryAgain:
            for (; ; )
            {
                IToken _token = null;
                int _ttype = Token.INVALID_TYPE;
                resetText();
                try     // for char stream error handling
                {
                    try     // for lexical error handling
                    {
                        switch (cached_LA1)
                        {
                            case '\t':
                            case '\n':
                            case '\r':
                            case ' ':
                                {
                                    mWS(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '(':
                                {
                                    mLPAREN(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case ')':
                                {
                                    mRPAREN(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '+':
                                {
                                    mPLUS(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '-':
                                {
                                    mMINUS(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '*':
                                {
                                    mMULT(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '/':
                                {
                                    mDIV(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '%':
                                {
                                    mMOD(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case ',':
                                {
                                    mCOMMA(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '\'':
                                {
                                    mCHAR_LITERAL(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '"':
                                {
                                    mSTRING_LITERAL(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '=':
                                {
                                    mEQUAL(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '!':
                                {
                                    mNOT_EQUAL(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case ':':
                                {
                                    mCOLON(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '?':
                                {
                                    mQUESTION_MARK(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                {
                                    mNUMBER(true);
                                    theRetToken = returnToken_;
                                    break;
                                }
                            default:
                                if ((cached_LA1 == 'F') && (cached_LA2 == 'A') && (LA(3) == 'L') && (LA(4) == 'S'))
                                {
                                    mFALSE(true);
                                    theRetToken = returnToken_;
                                }
                                else if ((cached_LA1 == 'T') && (cached_LA2 == 'R') && (LA(3) == 'U') && (LA(4) == 'E'))
                                {
                                    mTRUE(true);
                                    theRetToken = returnToken_;
                                }
                                else if ((cached_LA1 == '<') && (cached_LA2 == '='))
                                {
                                    mLTE(true);
                                    theRetToken = returnToken_;
                                }
                                else if ((cached_LA1 == '>') && (cached_LA2 == '='))
                                {
                                    mGTE(true);
                                    theRetToken = returnToken_;
                                }
                                else if ((cached_LA1 == '<') && (true))
                                {
                                    mLTH(true);
                                    theRetToken = returnToken_;
                                }
                                else if ((cached_LA1 == '>') && (true))
                                {
                                    mGT(true);
                                    theRetToken = returnToken_;
                                }
                                else if ((tokenSet_0_.member(cached_LA1)) && (true) && (true) && (true))
                                {
                                    mID(true);
                                    theRetToken = returnToken_;
                                }
                                else
                                {
                                    if (cached_LA1 == EOF_CHAR) { uponEOF(); returnToken_ = makeToken(Token.EOF_TYPE); } else { throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn()); }
                                }
                                break;
                        }
                        if (null == returnToken_) goto tryAgain; // found SKIP token
                        _ttype = returnToken_.Type;
                        _ttype = testLiteralsTable(_ttype);
                        returnToken_.Type = _ttype;
                        return returnToken_;
                    }
                    catch (RecognitionException e)
                    {
                        throw new TokenStreamRecognitionException(e);
                    }
                }
                catch (CharStreamException cse)
                {
                    if (cse is CharStreamIOException)
                    {
                        throw new TokenStreamIOException(((CharStreamIOException)cse).io);
                    }
                    else
                    {
                        throw new TokenStreamException(cse.Message);
                    }
                }
            }
        }

        public void mWS(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = WS;

            {
                switch (cached_LA1)
                {
                    case ' ':
                        {
                            match(' ');
                            break;
                        }
                    case '\t':
                        {
                            match('\t');
                            break;
                        }
                    case '\n':
                        {
                            match('\n');
                            break;
                        }
                    case '\r':
                        {
                            match('\r');
                            break;
                        }
                    default:
                        {
                            throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
                        }
                }
            }
            _ttype = Token.SKIP;
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mLPAREN(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = LPAREN;

            match('(');
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mFALSE(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = FALSE;

            match("FALSE");
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mTRUE(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = TRUE;

            match("TRUE");
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mRPAREN(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = RPAREN;

            match(')');
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mPLUS(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = PLUS;

            match('+');
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mMINUS(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = MINUS;

            match('-');
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mMULT(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = MULT;

            match('*');
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mDIV(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = DIV;

            match('/');
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mMOD(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = MOD;

            match('%');
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mCOMMA(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = COMMA;

            match(',');
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mCHAR_LITERAL(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = CHAR_LITERAL;

            match('\'');
            {
                switch (cached_LA1)
                {
                    case '\\':
                        {
                            mESC(false);
                            break;
                        }
                    case '\u0000':
                    case '\u0001':
                    case '\u0002':
                    case '\u0003':
                    case '\u0004':
                    case '\u0005':
                    case '\u0006':
                    case '\u0007':
                    case '\u0008':
                    case '\t':
                    case '\n':
                    case '\u000b':
                    case '\u000c':
                    case '\r':
                    case '\u000e':
                    case '\u000f':
                    case '\u0010':
                    case '\u0011':
                    case '\u0012':
                    case '\u0013':
                    case '\u0014':
                    case '\u0015':
                    case '\u0016':
                    case '\u0017':
                    case '\u0018':
                    case '\u0019':
                    case '\u001a':
                    case '\u001b':
                    case '\u001c':
                    case '\u001d':
                    case '\u001e':
                    case '\u001f':
                    case ' ':
                    case '!':
                    case '"':
                    case '#':
                    case '$':
                    case '%':
                    case '&':
                    case '(':
                    case ')':
                    case '*':
                    case '+':
                    case ',':
                    case '-':
                    case '.':
                    case '/':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case ':':
                    case ';':
                    case '<':
                    case '=':
                    case '>':
                    case '?':
                    case '@':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                    case '[':
                    case ']':
                    case '^':
                    case '_':
                    case '`':
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                    case '{':
                    case '|':
                    case '}':

                    case 'ð':
                    case 'Ð':
                    case 'ý':
                    case 'Ý':
                    case 'þ':
                    case 'Þ':
                    case 'ö':
                    case 'Ö':
                    case 'ü':
                    case 'Ü':
                    case 'ç':
                    case 'Ç':

                    case '~':
                    case '\u007f':
                        {
                            matchNot('\'');
                            break;
                        }
                    default:
                        {
                            throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
                        }
                }
            }
            match('\'');
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        protected void mESC(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = ESC;

            match('\\');
            {
                switch (cached_LA1)
                {
                    case 'n':
                        {
                            match('n');
                            break;
                        }
                    case 'r':
                        {
                            match('r');
                            break;
                        }
                    case 't':
                        {
                            match('t');
                            break;
                        }
                    case 'b':
                        {
                            match('b');
                            break;
                        }
                    case 'f':
                        {
                            match('f');
                            break;
                        }
                    case '"':
                        {
                            match('"');
                            break;
                        }
                    case '\'':
                        {
                            match('\'');
                            break;
                        }
                    case '\\':
                        {
                            match('\\');
                            break;
                        }
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                        {
                            {
                                matchRange('0', '3');
                            }
                            {
                                if (((cached_LA1 >= '0' && cached_LA1 <= '9')) && ((cached_LA2 >= '\u0000' && cached_LA2 <= '\u007f')) && (true) && (true))
                                {
                                    {
                                        matchRange('0', '9');
                                    }
                                    {
                                        if (((cached_LA1 >= '0' && cached_LA1 <= '9')) && ((cached_LA2 >= '\u0000' && cached_LA2 <= '\u007f')) && (true) && (true))
                                        {
                                            matchRange('0', '9');
                                        }
                                        else if (((cached_LA1 >= '\u0000' && cached_LA1 <= '\u007f')) && (true) && (true) && (true))
                                        {
                                        }
                                        else
                                        {
                                            throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
                                        }

                                    }
                                }
                                else if (((cached_LA1 >= '\u0000' && cached_LA1 <= '\u007f')) && (true) && (true) && (true))
                                {
                                }
                                else
                                {
                                    throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
                                }

                            }
                            break;
                        }
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                        {
                            {
                                matchRange('4', '7');
                            }
                            {
                                if (((cached_LA1 >= '0' && cached_LA1 <= '9')) && ((cached_LA2 >= '\u0000' && cached_LA2 <= '\u007f')) && (true) && (true))
                                {
                                    {
                                        matchRange('0', '9');
                                    }
                                }
                                else if (((cached_LA1 >= '\u0000' && cached_LA1 <= '\u007f')) && (true) && (true) && (true))
                                {
                                }
                                else
                                {
                                    throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
                                }

                            }
                            break;
                        }
                    default:
                        {
                            throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
                        }
                }
            }
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mSTRING_LITERAL(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = STRING_LITERAL;

            match('"');
            {    // ( ... )*
                for (; ; )
                {
                    switch (cached_LA1)
                    {
                        case '\\':
                            {
                                mESC(false);
                                break;
                            }
                        case '\u0000':
                        case '\u0001':
                        case '\u0002':
                        case '\u0003':
                        case '\u0004':
                        case '\u0005':
                        case '\u0006':
                        case '\u0007':
                        case '\u0008':
                        case '\t':
                        case '\n':
                        case '\u000b':
                        case '\u000c':
                        case '\r':
                        case '\u000e':
                        case '\u000f':
                        case '\u0010':
                        case '\u0011':
                        case '\u0012':
                        case '\u0013':
                        case '\u0014':
                        case '\u0015':
                        case '\u0016':
                        case '\u0017':
                        case '\u0018':
                        case '\u0019':
                        case '\u001a':
                        case '\u001b':
                        case '\u001c':
                        case '\u001d':
                        case '\u001e':
                        case '\u001f':
                        case ' ':
                        case '!':
                        case '#':
                        case '$':
                        case '%':
                        case '&':
                        case '\'':
                        case '(':
                        case ')':
                        case '*':
                        case '+':
                        case ',':
                        case '-':
                        case '.':
                        case '/':
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case ':':
                        case ';':
                        case '<':
                        case '=':
                        case '>':
                        case '?':
                        case '@':
                        case 'A':
                        case 'B':
                        case 'C':
                        case 'D':
                        case 'E':
                        case 'F':
                        case 'G':
                        case 'H':
                        case 'I':
                        case 'J':
                        case 'K':
                        case 'L':
                        case 'M':
                        case 'N':
                        case 'O':
                        case 'P':
                        case 'Q':
                        case 'R':
                        case 'S':
                        case 'T':
                        case 'U':
                        case 'V':
                        case 'W':
                        case 'X':
                        case 'Y':
                        case 'Z':
                        case '[':
                        case ']':
                        case '^':
                        case '_':
                        case '`':
                        case 'a':
                        case 'b':
                        case 'c':
                        case 'd':
                        case 'e':
                        case 'f':
                        case 'g':
                        case 'h':
                        case 'i':
                        case 'j':
                        case 'k':
                        case 'l':
                        case 'm':
                        case 'n':
                        case 'o':
                        case 'p':
                        case 'q':
                        case 'r':
                        case 's':
                        case 't':
                        case 'u':
                        case 'v':
                        case 'w':
                        case 'x':
                        case 'y':
                        case 'z':
                        case '{':
                        case '|':
                        case '}':
                        case 'ð':
                        case 'Ð':
                        case 'ý':
                        case 'Ý':
                        case 'þ':
                        case 'Þ':
                        case 'ö':
                        case 'Ö':
                        case 'ü':
                        case 'Ü':
                        case 'ç':
                        case 'Ç':
                        case '~':
                        case '\u007f':
                            {
                                matchNot('"');
                                break;
                            }
                        default:
                            {
                                goto _loop59_breakloop;
                            }
                    }
                }
            _loop59_breakloop: ;
            }    // ( ... )*
            match('"');
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mEQUAL(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = EQUAL;

            match("=");
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mNOT_EQUAL(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = NOT_EQUAL;

            match("!=");
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mLTE(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = LTE;

            match("<=");
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mLTH(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = LTH;

            match("<");
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mGTE(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = GTE;

            match(">=");
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mGT(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = GT;

            match(">");
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mCOLON(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = COLON;

            match(":");
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mQUESTION_MARK(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = QUESTION_MARK;

            match("?");
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        protected void mDIGIT(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = DIGIT;

            matchRange('0', '9');
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mNUMBER(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = NUMBER;

            { // ( ... )+
                int _cnt80 = 0;
                for (; ; )
                {
                    if (((cached_LA1 >= '0' && cached_LA1 <= '9')))
                    {
                        mDIGIT(false);
                    }
                    else
                    {
                        if (_cnt80 >= 1) { goto _loop80_breakloop; } else { throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn()); ; }
                    }

                    _cnt80++;
                }
            _loop80_breakloop: ;
            }    // ( ... )+
            {
                if ((cached_LA1 == '.'))
                {
                    match('.');
                    { // ( ... )+
                        int _cnt83 = 0;
                        for (; ; )
                        {
                            if (((cached_LA1 >= '0' && cached_LA1 <= '9')))
                            {
                                mDIGIT(false);
                            }
                            else
                            {
                                if (_cnt83 >= 1) { goto _loop83_breakloop; } else { throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn()); ; }
                            }

                            _cnt83++;
                        }
                    _loop83_breakloop: ;
                    }    // ( ... )+
                }
                else
                {
                }

            }
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }

        public void mID(bool _createToken) //throws RecognitionException, CharStreamException, TokenStreamException
        {
            int _ttype; IToken _token = null; int _begin = text.Length;
            _ttype = ID;

            {
                switch (cached_LA1)
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                        {
                            matchRange('a', 'z');
                            break;
                        }
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                        {
                            matchRange('A', 'Z');
                            break;
                        }
                    case '_':
                        {
                            match('_');
                            break;
                        }
                    case '$':
                        {
                            match('$');
                            break;
                        }
                    default:
                        {
                            throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
                        }
                }
            }
            {    // ( ... )*
                for (; ; )
                {
                    switch (cached_LA1)
                    {
                        case 'a':
                        case 'b':
                        case 'c':
                        case 'd':
                        case 'e':
                        case 'f':
                        case 'g':
                        case 'h':
                        case 'i':
                        case 'j':
                        case 'k':
                        case 'l':
                        case 'm':
                        case 'n':
                        case 'o':
                        case 'p':
                        case 'q':
                        case 'r':
                        case 's':
                        case 't':
                        case 'u':
                        case 'v':
                        case 'w':
                        case 'x':
                        case 'y':
                        case 'z':
                            {
                                matchRange('a', 'z');
                                break;
                            }
                        case 'A':
                        case 'B':
                        case 'C':
                        case 'D':
                        case 'E':
                        case 'F':
                        case 'G':
                        case 'H':
                        case 'I':
                        case 'J':
                        case 'K':
                        case 'L':
                        case 'M':
                        case 'N':
                        case 'O':
                        case 'P':
                        case 'Q':
                        case 'R':
                        case 'S':
                        case 'T':
                        case 'U':
                        case 'V':
                        case 'W':
                        case 'X':
                        case 'Y':
                        case 'Z':
                            {
                                matchRange('A', 'Z');
                                break;
                            }
                        case '_':
                            {
                                match('_');
                                break;
                            }
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            {
                                matchRange('0', '9');
                                break;
                            }
                        default:
                            {
                                goto _loop87_breakloop;
                            }
                    }
                }
            _loop87_breakloop: ;
            }    // ( ... )*
            {    // ( ... )*
                for (; ; )
                {
                    if ((cached_LA1 == '.'))
                    {
                        match('.');
                        {
                            switch (cached_LA1)
                            {
                                case 'a':
                                case 'b':
                                case 'c':
                                case 'd':
                                case 'e':
                                case 'f':
                                case 'g':
                                case 'h':
                                case 'i':
                                case 'j':
                                case 'k':
                                case 'l':
                                case 'm':
                                case 'n':
                                case 'o':
                                case 'p':
                                case 'q':
                                case 'r':
                                case 's':
                                case 't':
                                case 'u':
                                case 'v':
                                case 'w':
                                case 'x':
                                case 'y':
                                case 'z':
                                    {
                                        matchRange('a', 'z');
                                        break;
                                    }
                                case 'A':
                                case 'B':
                                case 'C':
                                case 'D':
                                case 'E':
                                case 'F':
                                case 'G':
                                case 'H':
                                case 'I':
                                case 'J':
                                case 'K':
                                case 'L':
                                case 'M':
                                case 'N':
                                case 'O':
                                case 'P':
                                case 'Q':
                                case 'R':
                                case 'S':
                                case 'T':
                                case 'U':
                                case 'V':
                                case 'W':
                                case 'X':
                                case 'Y':
                                case 'Z':
                                    {
                                        matchRange('A', 'Z');
                                        break;
                                    }
                                case '_':
                                    {
                                        match('_');
                                        break;
                                    }
                                default:
                                    {
                                        throw new NoViableAltForCharException(cached_LA1, getFilename(), getLine(), getColumn());
                                    }
                            }
                        }
                        {    // ( ... )*
                            for (; ; )
                            {
                                switch (cached_LA1)
                                {
                                    case 'a':
                                    case 'b':
                                    case 'c':
                                    case 'd':
                                    case 'e':
                                    case 'f':
                                    case 'g':
                                    case 'h':
                                    case 'i':
                                    case 'j':
                                    case 'k':
                                    case 'l':
                                    case 'm':
                                    case 'n':
                                    case 'o':
                                    case 'p':
                                    case 'q':
                                    case 'r':
                                    case 's':
                                    case 't':
                                    case 'u':
                                    case 'v':
                                    case 'w':
                                    case 'x':
                                    case 'y':
                                    case 'z':
                                        {
                                            matchRange('a', 'z');
                                            break;
                                        }
                                    case 'A':
                                    case 'B':
                                    case 'C':
                                    case 'D':
                                    case 'E':
                                    case 'F':
                                    case 'G':
                                    case 'H':
                                    case 'I':
                                    case 'J':
                                    case 'K':
                                    case 'L':
                                    case 'M':
                                    case 'N':
                                    case 'O':
                                    case 'P':
                                    case 'Q':
                                    case 'R':
                                    case 'S':
                                    case 'T':
                                    case 'U':
                                    case 'V':
                                    case 'W':
                                    case 'X':
                                    case 'Y':
                                    case 'Z':
                                        {
                                            matchRange('A', 'Z');
                                            break;
                                        }
                                    case '_':
                                        {
                                            match('_');
                                            break;
                                        }
                                    case '0':
                                    case '1':
                                    case '2':
                                    case '3':
                                    case '4':
                                    case '5':
                                    case '6':
                                    case '7':
                                    case '8':
                                    case '9':
                                        {
                                            matchRange('0', '9');
                                            break;
                                        }
                                    default:
                                        {
                                            goto _loop91_breakloop;
                                        }
                                }
                            }
                        _loop91_breakloop: ;
                        }    // ( ... )*
                    }
                    else
                    {
                        goto _loop92_breakloop;
                    }

                }
            _loop92_breakloop: ;
            }    // ( ... )*
            _ttype = testLiteralsTable(_ttype);
            if (_createToken && (null == _token) && (_ttype != Token.SKIP))
            {
                _token = makeToken(_ttype);
                _token.setText(text.ToString(_begin, text.Length - _begin));
            }
            returnToken_ = _token;
        }


        private static long[] mk_tokenSet_0_()
        {
            long[] data = { 68719476736L, 576460745995190270L, 0L, 0L };
            return data;
        }
        public static readonly BitSet tokenSet_0_ = new BitSet(mk_tokenSet_0_());

    }
}
