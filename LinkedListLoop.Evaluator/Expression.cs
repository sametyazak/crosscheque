using AST = antlr.collections.AST;
using CharBuffer = antlr.CharBuffer;
using System.Collections.Generic;
using System.IO;
using System.Text;
using antlr;
using System;
using log4net;
using System.Reflection;
using System.Web;
using System.Configuration;

namespace LinkedLoopList.Evaluator
{

    public class Expression
    {

        /// <summary>
        /// Cache for generated ASTs.
        /// </summary>
        static Dictionary<string, CommonAST> astCache = new Dictionary<string, CommonAST>();

        /// <summary>
        /// Log handle.
        /// </summary>
        static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Static ctor loads all built-in and custom functions.
        /// </summary>
        static Expression()
        {

        }

        public static bool EvaluateLogical(string expr, IExpressionPropertyFinder finder)
        {
            object result = Evaluate(new ExpressionContext(), expr, finder);
            if (!(result is bool))
            {
                throw new Exception("EvaluationError:'" + expr + "' is not a logical expression");
            }
            return (bool)result;
        }

        public static object Evaluate(ExpressionContext context, string expr, IExpressionPropertyFinder finder)
        {
            //try {
            CommonAST ast;
            if (!astCache.ContainsKey(expr))
            {
                ExpressionParser parser = CreateParser(expr);
                parser.expr();
                ast = parser.getAST();
                lock (astCache)
                {
                    if (!astCache.ContainsKey(expr))
                        astCache.Add(expr, ast);
                }
            }
            else
            {
                ast = astCache[expr];
            }

            //Console.WriteLine(ast.ToTree());
            try
            {
                ExpressionTreeParser my = new ExpressionTreeParser(context, finder);
                return my.Evaluate(ast);
            }
            catch (Exception e)
            {
                string msg = string.Format("An error accured during evaluation of '{0}'", expr);
                log.Fatal(msg, e);
                throw new Exception(msg, e);
            }
        }

        /// <summary>
        /// Creates a parser for the given string.
        /// </summary>
        static ExpressionParser CreateParser(string expr)
        {
            try
            {
                ExpressionLexer lexer = new ExpressionLexer(new StringReader(expr));
                return new ExpressionParser(lexer);
            }
            catch
            {
                throw new Exception(string.Format("'{0}' expression has occured an error in parser", expr));
            }
        }

    }

}
