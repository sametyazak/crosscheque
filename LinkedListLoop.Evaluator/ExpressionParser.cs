namespace LinkedLoopList.Evaluator
{
    // Generate the header common to all output files.
    using System;

    using TokenBuffer = antlr.TokenBuffer;
    using TokenStreamException = antlr.TokenStreamException;
    using TokenStreamIOException = antlr.TokenStreamIOException;
    using ANTLRException = antlr.ANTLRException;
    using LLkParser = antlr.LLkParser;
    using Token = antlr.Token;
    using IToken = antlr.IToken;
    using TokenStream = antlr.TokenStream;
    using RecognitionException = antlr.RecognitionException;
    using NoViableAltException = antlr.NoViableAltException;
    using MismatchedTokenException = antlr.MismatchedTokenException;
    using SemanticException = antlr.SemanticException;
    using ParserSharedInputState = antlr.ParserSharedInputState;
    using BitSet = antlr.collections.impl.BitSet;
    using AST = antlr.collections.AST;
    using ASTPair = antlr.ASTPair;
    using ASTFactory = antlr.ASTFactory;
    using ASTArray = antlr.collections.impl.ASTArray;

    public class ExpressionParser : antlr.LLkParser
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


        protected void initialize()
        {
            tokenNames = tokenNames_;
            initializeFactory();
        }


        protected ExpressionParser(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
            initialize();
        }

        public ExpressionParser(TokenBuffer tokenBuf)
            : this(tokenBuf, 1)
        {
        }

        protected ExpressionParser(TokenStream lexer, int k)
            : base(lexer, k)
        {
            initialize();
        }

        public ExpressionParser(TokenStream lexer)
            : this(lexer, 1)
        {
        }

        public ExpressionParser(ParserSharedInputState state)
            : base(state, 1)
        {
            initialize();
        }

        public void expr() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST expr_AST = null;

            conditionalExpression();
            if (0 == inputState.guessing)
            {
                astFactory.addASTChild(ref currentAST, (AST)returnAST);
            }
            match(Token.EOF_TYPE);
            expr_AST = (antlr.CommonAST)currentAST.root;
            returnAST = expr_AST;
        }

        public void conditionalExpression() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST conditionalExpression_AST = null;

            logicalOrExpression();
            if (0 == inputState.guessing)
            {
                astFactory.addASTChild(ref currentAST, (AST)returnAST);
            }
            {
                if ((LA(1) == QUESTION_MARK))
                {
                    antlr.CommonAST tmp10_AST = null;
                    tmp10_AST = (antlr.CommonAST)astFactory.create(LT(1));
                    astFactory.makeASTRoot(ref currentAST, (AST)tmp10_AST);
                    match(QUESTION_MARK);
                    conditionalExpression();
                    if (0 == inputState.guessing)
                    {
                        astFactory.addASTChild(ref currentAST, (AST)returnAST);
                    }
                    match(COLON);
                    conditionalExpression();
                    if (0 == inputState.guessing)
                    {
                        astFactory.addASTChild(ref currentAST, (AST)returnAST);
                    }
                }
                else if ((tokenSet_0_.member(LA(1))))
                {
                }
                else
                {
                    throw new NoViableAltException(LT(1), getFilename());
                }

            }
            conditionalExpression_AST = (antlr.CommonAST)currentAST.root;
            returnAST = conditionalExpression_AST;
        }

        public void logicalOrExpression() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST logicalOrExpression_AST = null;

            logicalAndExpression();
            if (0 == inputState.guessing)
            {
                astFactory.addASTChild(ref currentAST, (AST)returnAST);
            }
            {    // ( ... )*
                for (; ; )
                {
                    if ((LA(1) == LITERAL_OR))
                    {
                        antlr.CommonAST tmp12_AST = null;
                        tmp12_AST = (antlr.CommonAST)astFactory.create(LT(1));
                        astFactory.makeASTRoot(ref currentAST, (AST)tmp12_AST);
                        match(LITERAL_OR);
                        logicalAndExpression();
                        if (0 == inputState.guessing)
                        {
                            astFactory.addASTChild(ref currentAST, (AST)returnAST);
                        }
                    }
                    else
                    {
                        goto _loop6_breakloop;
                    }

                }
            _loop6_breakloop: ;
            }    // ( ... )*
            logicalOrExpression_AST = (antlr.CommonAST)currentAST.root;
            returnAST = logicalOrExpression_AST;
        }

        public void logicalAndExpression() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST logicalAndExpression_AST = null;

            equalityExpr();
            if (0 == inputState.guessing)
            {
                astFactory.addASTChild(ref currentAST, (AST)returnAST);
            }
            {    // ( ... )*
                for (; ; )
                {
                    if ((LA(1) == LITERAL_AND))
                    {
                        antlr.CommonAST tmp13_AST = null;
                        tmp13_AST = (antlr.CommonAST)astFactory.create(LT(1));
                        astFactory.makeASTRoot(ref currentAST, (AST)tmp13_AST);
                        match(LITERAL_AND);
                        equalityExpr();
                        if (0 == inputState.guessing)
                        {
                            astFactory.addASTChild(ref currentAST, (AST)returnAST);
                        }
                    }
                    else
                    {
                        goto _loop9_breakloop;
                    }

                }
            _loop9_breakloop: ;
            }    // ( ... )*
            logicalAndExpression_AST = (antlr.CommonAST)currentAST.root;
            returnAST = logicalAndExpression_AST;
        }

        public void equalityExpr() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST equalityExpr_AST = null;

            relationalExpr();
            if (0 == inputState.guessing)
            {
                astFactory.addASTChild(ref currentAST, (AST)returnAST);
            }
            {
                switch (LA(1))
                {
                    case EOF:
                    case QUESTION_MARK:
                    case COLON:
                    case LITERAL_OR:
                    case LITERAL_AND:
                    case EQUAL:
                    case NOT_EQUAL:
                    case COMMA:
                    case RPAREN:
                        {
                            {    // ( ... )*
                                for (; ; )
                                {
                                    if ((LA(1) == EQUAL || LA(1) == NOT_EQUAL))
                                    {
                                        {
                                            if ((LA(1) == EQUAL))
                                            {
                                                antlr.CommonAST tmp14_AST = null;
                                                tmp14_AST = (antlr.CommonAST)astFactory.create(LT(1));
                                                astFactory.makeASTRoot(ref currentAST, (AST)tmp14_AST);
                                                match(EQUAL);
                                            }
                                            else if ((LA(1) == NOT_EQUAL))
                                            {
                                                antlr.CommonAST tmp15_AST = null;
                                                tmp15_AST = (antlr.CommonAST)astFactory.create(LT(1));
                                                astFactory.makeASTRoot(ref currentAST, (AST)tmp15_AST);
                                                match(NOT_EQUAL);
                                            }
                                            else
                                            {
                                                throw new NoViableAltException(LT(1), getFilename());
                                            }

                                        }
                                        relationalExpr();
                                        if (0 == inputState.guessing)
                                        {
                                            astFactory.addASTChild(ref currentAST, (AST)returnAST);
                                        }
                                    }
                                    else
                                    {
                                        goto _loop14_breakloop;
                                    }

                                }
                            _loop14_breakloop: ;
                            }    // ( ... )*
                            break;
                        }
                    case LITERAL_IN:
                        {
                            antlr.CommonAST tmp16_AST = null;
                            tmp16_AST = (antlr.CommonAST)astFactory.create(LT(1));
                            astFactory.makeASTRoot(ref currentAST, (AST)tmp16_AST);
                            match(LITERAL_IN);
                            inList();
                            if (0 == inputState.guessing)
                            {
                                astFactory.addASTChild(ref currentAST, (AST)returnAST);
                            }
                            break;
                        }
                    case LITERAL_NOT:
                        {
                            antlr.CommonAST tmp17_AST = null;
                            tmp17_AST = (antlr.CommonAST)astFactory.create(LT(1));
                            astFactory.makeASTRoot(ref currentAST, (AST)tmp17_AST);
                            match(LITERAL_NOT);
                            match(LITERAL_IN);
                            inList();
                            if (0 == inputState.guessing)
                            {
                                astFactory.addASTChild(ref currentAST, (AST)returnAST);
                            }
                            break;
                        }
                    default:
                        {
                            throw new NoViableAltException(LT(1), getFilename());
                        }
                }
            }
            equalityExpr_AST = (antlr.CommonAST)currentAST.root;
            returnAST = equalityExpr_AST;
        }

        public void relationalExpr() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST relationalExpr_AST = null;

            arithmeticExpr();
            if (0 == inputState.guessing)
            {
                astFactory.addASTChild(ref currentAST, (AST)returnAST);
            }
            {
                if (((LA(1) >= LTH && LA(1) <= GTE)))
                {
                    {
                        switch (LA(1))
                        {
                            case LTH:
                                {
                                    antlr.CommonAST tmp19_AST = null;
                                    tmp19_AST = (antlr.CommonAST)astFactory.create(LT(1));
                                    astFactory.makeASTRoot(ref currentAST, (AST)tmp19_AST);
                                    match(LTH);
                                    break;
                                }
                            case LTE:
                                {
                                    antlr.CommonAST tmp20_AST = null;
                                    tmp20_AST = (antlr.CommonAST)astFactory.create(LT(1));
                                    astFactory.makeASTRoot(ref currentAST, (AST)tmp20_AST);
                                    match(LTE);
                                    break;
                                }
                            case GT:
                                {
                                    antlr.CommonAST tmp21_AST = null;
                                    tmp21_AST = (antlr.CommonAST)astFactory.create(LT(1));
                                    astFactory.makeASTRoot(ref currentAST, (AST)tmp21_AST);
                                    match(GT);
                                    break;
                                }
                            case GTE:
                                {
                                    antlr.CommonAST tmp22_AST = null;
                                    tmp22_AST = (antlr.CommonAST)astFactory.create(LT(1));
                                    astFactory.makeASTRoot(ref currentAST, (AST)tmp22_AST);
                                    match(GTE);
                                    break;
                                }
                            default:
                                {
                                    throw new NoViableAltException(LT(1), getFilename());
                                }
                        }
                    }
                    arithmeticExpr();
                    if (0 == inputState.guessing)
                    {
                        astFactory.addASTChild(ref currentAST, (AST)returnAST);
                    }
                }
                else if ((tokenSet_1_.member(LA(1))))
                {
                }
                else
                {
                    throw new NoViableAltException(LT(1), getFilename());
                }

            }
            relationalExpr_AST = (antlr.CommonAST)currentAST.root;
            returnAST = relationalExpr_AST;
        }

        public void inList() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST inList_AST = null;

            match(LPAREN);
            {
                arithmeticExpr();
                if (0 == inputState.guessing)
                {
                    astFactory.addASTChild(ref currentAST, (AST)returnAST);
                }
                {    // ( ... )*
                    for (; ; )
                    {
                        if ((LA(1) == COMMA))
                        {
                            match(COMMA);
                            arithmeticExpr();
                            if (0 == inputState.guessing)
                            {
                                astFactory.addASTChild(ref currentAST, (AST)returnAST);
                            }
                        }
                        else
                        {
                            goto _loop24_breakloop;
                        }

                    }
                _loop24_breakloop: ;
                }    // ( ... )*
            }
            match(RPAREN);
            inList_AST = (antlr.CommonAST)currentAST.root;
            returnAST = inList_AST;
        }

        public void arithmeticExpr() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST arithmeticExpr_AST = null;

            sumExpr();
            if (0 == inputState.guessing)
            {
                astFactory.addASTChild(ref currentAST, (AST)returnAST);
            }
            arithmeticExpr_AST = (antlr.CommonAST)currentAST.root;
            returnAST = arithmeticExpr_AST;
        }

        public void inExpr() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST inExpr_AST = null;

            if ((LA(1) == LITERAL_IN))
            {
                normalInExpr();
                if (0 == inputState.guessing)
                {
                    astFactory.addASTChild(ref currentAST, (AST)returnAST);
                }
                inExpr_AST = (antlr.CommonAST)currentAST.root;
            }
            else if ((LA(1) == LITERAL_NOT))
            {
                notInExpr();
                if (0 == inputState.guessing)
                {
                    astFactory.addASTChild(ref currentAST, (AST)returnAST);
                }
                inExpr_AST = (antlr.CommonAST)currentAST.root;
            }
            else
            {
                throw new NoViableAltException(LT(1), getFilename());
            }

            returnAST = inExpr_AST;
        }

        public void normalInExpr() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST normalInExpr_AST = null;

            antlr.CommonAST tmp26_AST = null;
            tmp26_AST = (antlr.CommonAST)astFactory.create(LT(1));
            astFactory.makeASTRoot(ref currentAST, (AST)tmp26_AST);
            match(LITERAL_IN);
            inList();
            if (0 == inputState.guessing)
            {
                astFactory.addASTChild(ref currentAST, (AST)returnAST);
            }
            normalInExpr_AST = (antlr.CommonAST)currentAST.root;
            returnAST = normalInExpr_AST;
        }

        public void notInExpr() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST notInExpr_AST = null;

            antlr.CommonAST tmp27_AST = null;
            tmp27_AST = (antlr.CommonAST)astFactory.create(LT(1));
            astFactory.makeASTRoot(ref currentAST, (AST)tmp27_AST);
            match(LITERAL_NOT);
            match(LITERAL_IN);
            inList();
            if (0 == inputState.guessing)
            {
                astFactory.addASTChild(ref currentAST, (AST)returnAST);
            }
            notInExpr_AST = (antlr.CommonAST)currentAST.root;
            returnAST = notInExpr_AST;
        }

        public void sumExpr() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST sumExpr_AST = null;

            prodExpr();
            if (0 == inputState.guessing)
            {
                astFactory.addASTChild(ref currentAST, (AST)returnAST);
            }
            {    // ( ... )*
                for (; ; )
                {
                    if ((LA(1) == PLUS || LA(1) == MINUS))
                    {
                        {
                            if ((LA(1) == PLUS))
                            {
                                antlr.CommonAST tmp29_AST = null;
                                tmp29_AST = (antlr.CommonAST)astFactory.create(LT(1));
                                astFactory.makeASTRoot(ref currentAST, (AST)tmp29_AST);
                                match(PLUS);
                            }
                            else if ((LA(1) == MINUS))
                            {
                                antlr.CommonAST tmp30_AST = null;
                                tmp30_AST = (antlr.CommonAST)astFactory.create(LT(1));
                                astFactory.makeASTRoot(ref currentAST, (AST)tmp30_AST);
                                match(MINUS);
                            }
                            else
                            {
                                throw new NoViableAltException(LT(1), getFilename());
                            }

                        }
                        prodExpr();
                        if (0 == inputState.guessing)
                        {
                            astFactory.addASTChild(ref currentAST, (AST)returnAST);
                        }
                    }
                    else
                    {
                        goto _loop29_breakloop;
                    }

                }
            _loop29_breakloop: ;
            }    // ( ... )*
            sumExpr_AST = (antlr.CommonAST)currentAST.root;
            returnAST = sumExpr_AST;
        }

        public void prodExpr() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST prodExpr_AST = null;

            postFixExpr();
            if (0 == inputState.guessing)
            {
                astFactory.addASTChild(ref currentAST, (AST)returnAST);
            }
            {    // ( ... )*
                for (; ; )
                {
                    if (((LA(1) >= MULT && LA(1) <= MOD)))
                    {
                        {
                            switch (LA(1))
                            {
                                case MULT:
                                    {
                                        antlr.CommonAST tmp31_AST = null;
                                        tmp31_AST = (antlr.CommonAST)astFactory.create(LT(1));
                                        astFactory.makeASTRoot(ref currentAST, (AST)tmp31_AST);
                                        match(MULT);
                                        break;
                                    }
                                case DIV:
                                    {
                                        antlr.CommonAST tmp32_AST = null;
                                        tmp32_AST = (antlr.CommonAST)astFactory.create(LT(1));
                                        astFactory.makeASTRoot(ref currentAST, (AST)tmp32_AST);
                                        match(DIV);
                                        break;
                                    }
                                case MOD:
                                    {
                                        antlr.CommonAST tmp33_AST = null;
                                        tmp33_AST = (antlr.CommonAST)astFactory.create(LT(1));
                                        astFactory.makeASTRoot(ref currentAST, (AST)tmp33_AST);
                                        match(MOD);
                                        break;
                                    }
                                default:
                                    {
                                        throw new NoViableAltException(LT(1), getFilename());
                                    }
                            }
                        }
                        postFixExpr();
                        if (0 == inputState.guessing)
                        {
                            astFactory.addASTChild(ref currentAST, (AST)returnAST);
                        }
                    }
                    else
                    {
                        goto _loop33_breakloop;
                    }

                }
            _loop33_breakloop: ;
            }    // ( ... )*
            prodExpr_AST = (antlr.CommonAST)currentAST.root;
            returnAST = prodExpr_AST;
        }

        public void postFixExpr() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST postFixExpr_AST = null;
            //IToken  id = null;
            //antlr.CommonAST id_AST = null;
            IToken id2 = null;
            antlr.CommonAST id2_AST = null;

            bool synPredMatched36 = false;
            if (((LA(1) == ID)))
            {
                int _m36 = mark();
                synPredMatched36 = true;
                inputState.guessing++;
                try
                {
                    {
                        match(ID);
                        match(LPAREN);
                    }
                }
                catch (RecognitionException)
                {
                    synPredMatched36 = false;
                }
                rewind(_m36);
                inputState.guessing--;
            }
            if (synPredMatched36)
            {
                id2 = LT(1);
                id2_AST = (antlr.CommonAST)astFactory.create(id2);
                astFactory.makeASTRoot(ref currentAST, (AST)id2_AST);
                match(ID);
                {
                    if ((LA(1) == LPAREN))
                    {
                        parenArgs();
                        if (0 == inputState.guessing)
                        {
                            astFactory.addASTChild(ref currentAST, (AST)returnAST);
                        }
                    }
                    else if ((tokenSet_2_.member(LA(1))))
                    {
                    }
                    else
                    {
                        throw new NoViableAltException(LT(1), getFilename());
                    }

                }
                postFixExpr_AST = (antlr.CommonAST)currentAST.root;
            }
            else if ((tokenSet_3_.member(LA(1))))
            {
                atom();
                if (0 == inputState.guessing)
                {
                    astFactory.addASTChild(ref currentAST, (AST)returnAST);
                }
                postFixExpr_AST = (antlr.CommonAST)currentAST.root;
            }
            else
            {
                throw new NoViableAltException(LT(1), getFilename());
            }

            returnAST = postFixExpr_AST;
        }

        public void parenArgs() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST parenArgs_AST = null;

            antlr.CommonAST tmp34_AST = null;
            tmp34_AST = (antlr.CommonAST)astFactory.create(LT(1));
            astFactory.addASTChild(ref currentAST, (AST)tmp34_AST);
            match(LPAREN);
            {
                if ((tokenSet_3_.member(LA(1))))
                {
                    conditionalExpression();
                    if (0 == inputState.guessing)
                    {
                        astFactory.addASTChild(ref currentAST, (AST)returnAST);
                    }
                    {    // ( ... )*
                        for (; ; )
                        {
                            if ((LA(1) == COMMA))
                            {
                                match(COMMA);
                                conditionalExpression();
                                if (0 == inputState.guessing)
                                {
                                    astFactory.addASTChild(ref currentAST, (AST)returnAST);
                                }
                            }
                            else
                            {
                                goto _loop41_breakloop;
                            }

                        }
                    _loop41_breakloop: ;
                    }    // ( ... )*
                }
                else if ((LA(1) == RPAREN))
                {
                }
                else
                {
                    throw new NoViableAltException(LT(1), getFilename());
                }

            }
            antlr.CommonAST tmp36_AST = null;
            tmp36_AST = (antlr.CommonAST)astFactory.create(LT(1));
            astFactory.addASTChild(ref currentAST, (AST)tmp36_AST);
            match(RPAREN);
            parenArgs_AST = (antlr.CommonAST)currentAST.root;
            returnAST = parenArgs_AST;
        }

        public void atom() //throws RecognitionException, TokenStreamException
        {

            returnAST = null;
            ASTPair currentAST = new ASTPair();
            antlr.CommonAST atom_AST = null;
            IToken i = null;
            antlr.CommonAST i_AST = null;

            switch (LA(1))
            {
                case ID:
                    {
                        antlr.CommonAST tmp37_AST = null;
                        tmp37_AST = (antlr.CommonAST)astFactory.create(LT(1));
                        astFactory.addASTChild(ref currentAST, (AST)tmp37_AST);
                        match(ID);
                        atom_AST = (antlr.CommonAST)currentAST.root;
                        break;
                    }
                case NUMBER:
                    {
                        antlr.CommonAST tmp38_AST = null;
                        tmp38_AST = (antlr.CommonAST)astFactory.create(LT(1));
                        astFactory.addASTChild(ref currentAST, (AST)tmp38_AST);
                        match(NUMBER);
                        atom_AST = (antlr.CommonAST)currentAST.root;
                        break;
                    }
                case MINUS:
                    {
                        match(MINUS);
                        i = LT(1);
                        i_AST = (antlr.CommonAST)astFactory.create(i);
                        astFactory.addASTChild(ref currentAST, (AST)i_AST);
                        match(NUMBER);
                        if (0 == inputState.guessing)
                        {
                            i_AST.setText("-" + i_AST.getText());
                        }
                        atom_AST = (antlr.CommonAST)currentAST.root;
                        break;
                    }
                case STRING_LITERAL:
                    {
                        antlr.CommonAST tmp40_AST = null;
                        tmp40_AST = (antlr.CommonAST)astFactory.create(LT(1));
                        astFactory.addASTChild(ref currentAST, (AST)tmp40_AST);
                        match(STRING_LITERAL);
                        atom_AST = (antlr.CommonAST)currentAST.root;
                        break;
                    }
                case LPAREN:
                    {
                        match(LPAREN);
                        conditionalExpression();
                        if (0 == inputState.guessing)
                        {
                            astFactory.addASTChild(ref currentAST, (AST)returnAST);
                        }
                        match(RPAREN);
                        atom_AST = (antlr.CommonAST)currentAST.root;
                        break;
                    }
                case TRUE:
                    {
                        antlr.CommonAST tmp43_AST = null;
                        tmp43_AST = (antlr.CommonAST)astFactory.create(LT(1));
                        astFactory.addASTChild(ref currentAST, (AST)tmp43_AST);
                        match(TRUE);
                        atom_AST = (antlr.CommonAST)currentAST.root;
                        break;
                    }
                case FALSE:
                    {
                        antlr.CommonAST tmp44_AST = null;
                        tmp44_AST = (antlr.CommonAST)astFactory.create(LT(1));
                        astFactory.addASTChild(ref currentAST, (AST)tmp44_AST);
                        match(FALSE);
                        atom_AST = (antlr.CommonAST)currentAST.root;
                        break;
                    }
                default:
                    {
                        throw new NoViableAltException(LT(1), getFilename());
                    }
            }
            returnAST = atom_AST;
        }

        public new antlr.CommonAST getAST()
        {
            return (antlr.CommonAST)returnAST;
        }

        private void initializeFactory()
        {
            if (astFactory == null)
            {
                astFactory = new ASTFactory("antlr.CommonAST");
            }
            initializeASTFactory(astFactory);
        }
        static public void initializeASTFactory(ASTFactory factory)
        {
            factory.setMaxNodeType(34);
        }

        public static readonly string[] tokenNames_ = new string[] {
		@"""<0>""",
		@"""EOF""",
		@"""<2>""",
		@"""NULL_TREE_LOOKAHEAD""",
		@"""QUESTION_MARK""",
		@"""COLON""",
		@"""OR""",
		@"""AND""",
		@"""EQUAL""",
		@"""NOT_EQUAL""",
		@"""IN""",
		@"""NOT""",
		@"""LTH""",
		@"""LTE""",
		@"""GT""",
		@"""GTE""",
		@"""LPAREN""",
		@"""COMMA""",
		@"""RPAREN""",
		@"""PLUS""",
		@"""MINUS""",
		@"""MULT""",
		@"""DIV""",
		@"""MOD""",
		@"""ID""",
		@"""NUMBER""",
		@"""STRING_LITERAL""",
		@"""TRUE""",
		@"""FALSE""",
		@"""WS""",
		@"""CHAR_LITERAL""",
		@"""ESC""",
		@"""DIGIT""",
		@"""SIGN_MINUS""",
		@"""SIGN_PLUS"""
	};

        private static long[] mk_tokenSet_0_()
        {
            long[] data = { 393250L, 0L };
            return data;
        }
        public static readonly BitSet tokenSet_0_ = new BitSet(mk_tokenSet_0_());
        private static long[] mk_tokenSet_1_()
        {
            long[] data = { 397298L, 0L };
            return data;
        }
        public static readonly BitSet tokenSet_1_ = new BitSet(mk_tokenSet_1_());
        private static long[] mk_tokenSet_2_()
        {
            long[] data = { 16711666L, 0L };
            return data;
        }
        public static readonly BitSet tokenSet_2_ = new BitSet(mk_tokenSet_2_());
        private static long[] mk_tokenSet_3_()
        {
            long[] data = { 521207808L, 0L };
            return data;
        }
        public static readonly BitSet tokenSet_3_ = new BitSet(mk_tokenSet_3_());

    }
}
