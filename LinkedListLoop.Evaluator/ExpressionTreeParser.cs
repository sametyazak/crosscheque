using System;
using System.Collections.Generic;
using System.Text;
using antlr;
using antlr.collections;
using System.Collections;
using log4net;

namespace LinkedLoopList.Evaluator
{

    enum ExpressionType
    {
        Number,
        Text,
        Bool
    }

    internal class ExpressionTreeParser : TreeParser
    {

        /// <summary>
        /// Expression variable finder. Tree walker uses this to find any unknown variables.
        /// </summary>
        IExpressionPropertyFinder finderInstance;

        /// <summary>
        /// Context for this expression.
        /// </summary>
        ExpressionContext context;

        /// <summary>
        /// Ctor for tree walker.
        /// </summary>
        /// <param name="context">Expression context</param>
        /// <param name="finderInstance">Finder instance</param>
        public ExpressionTreeParser(ExpressionContext context, IExpressionPropertyFinder finderInstance)
        {
            this.finderInstance = finderInstance;
            this.context = context;
        }

        #region Expression Type Handling Helper Method
        //bool IsNumberType(object obj) {
        //    return IsNumberType(obj.GetType());
        //}

        bool IsNumberType(Type t)
        {
            return GetExpressionType(t) == ExpressionType.Number;
        }

        //bool IsTextType(object obj) {
        //    return IsTextType(obj.GetType());
        //}

        bool IsTextType(Type t)
        {
            return GetExpressionType(t) == ExpressionType.Text;
        }

        //bool IsBoolType(object obj) {
        //    return IsBoolType(obj.GetType());
        //}

        bool IsBoolType(Type t)
        {
            return GetExpressionType(t) == ExpressionType.Bool;
        }

        ExpressionType GetExpressionType(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return ExpressionType.Number;

                case TypeCode.String:
                case TypeCode.Char:
                    return ExpressionType.Text;

                case TypeCode.Boolean:
                    return ExpressionType.Bool;

                default:
                    string msg = string.Format("Unsupported type: '{0}'", Type.GetTypeCode(t));
                    throw new Exception(msg);
            }
        }

        #endregion

        /// <summary>
        /// Evauates logical operators less than, equal, etc.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <param name="op">Operator</param>
        /// <returns>A bool instance that is the result of evaluation</returns>
        bool EvaluateEquality(Type aType, object a, Type bType, object b, string op)
        {
            if (a == null || b == null)
            {
                if (a == null && b == null)
                {
                    switch (op)
                    {
                        case "<": return false;
                        case "<=": return true;
                        case ">": return false;
                        case ">=": return true;
                        case "=": return true;
                        case "!=": return false;
                    }
                }
                else if ((a == null && b != null) || (a != null && b == null))
                {
                    switch (op)
                    {
                        case "<": return false;
                        case "<=": return false;
                        case ">": return false;
                        case ">=": return false;
                        case "=": return false;
                        case "!=": return true;
                    }
                }
            }
            else if (IsNumberType(aType) && IsNumberType(bType))
            {
                decimal da = Convert.ToDecimal(a);
                decimal db = Convert.ToDecimal(b);
                switch (op)
                {
                    case "<": return da < db;
                    case "<=": return da <= db;
                    case ">": return da > db;
                    case ">=": return da >= db;
                    case "=": return da == db;
                    case "!=": return da != db;
                }
                // below is impossible, only to surpass compiler error 'not all code paths return a value'
                return false;
            }
            else if (IsTextType(aType) && IsTextType(bType))
            {
                // Text types only supports '=' and '!=' operators
                string sa = Convert.ToString(a);
                string sb = Convert.ToString(b);
                if (op == "=")
                    return sa == sb;
                else if (op == "!=")
                    return sa != sb;
                else if (op == ">")
                    return (string.Compare(sa, sb) > 0);
                else if (op == ">=")
                    return (string.Compare(sa, sb) >= 0);
                else if (op == "<")
                    return (string.Compare(sa, sb) < 0);
                else if (op == "<=")
                    return (string.Compare(sa, sb) <= 0);
                else
                    throw new Exception(string.Format("'{0}' operand is not compatible with string", op));
            }
            else if (IsBoolType(aType) && IsBoolType(bType))
            {
                bool ba = Convert.ToBoolean(a);
                bool bb = Convert.ToBoolean(b);

                // Bool types only supports '=' and '!=' operators
                if (op == "=")
                    return (ba == bb);
                else if (op == "!=")
                    return (ba != bb);
                else
                    throw new Exception(string.Format("'{0}' operand is not compatible with bool", op));
            }

            throw new Exception(
                string.Format("'{0}' operand is not compatible with '{1}' and '{2}' ", op, a.GetType().ToString(), b.GetType().ToString()));
        }

        /// <summary>
        /// Evaluates arithmetic expressions (this also includes string concatenation).
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <param name="op">Operator</param>
        /// <returns>Either a decimal instance or a string instance that represents the result of the evaluation</returns>
        object EvaluateArithmetic(Type aType, object a, Type bType, object b, string op, out Type resultType)
        {
            if (IsNumberType(aType) && IsNumberType(bType))
            {
                decimal da = Convert.ToDecimal(a);
                decimal db = Convert.ToDecimal(b);
                resultType = typeof(decimal);
                switch (op)
                {
                    case "+":
                        return da + db;
                    case "-":
                        return da - db;
                    case "/":
                        return da / db; // TODO: division by zero kontrolü eklenmeli
                    case "*":
                        return da * db;
                    case "%":
                        return da % db;
                }
                // below is impossible, only to surpass compiler error 'not all code paths return a value'
                return null;
            }
            else if ((IsTextType(aType) || IsTextType(bType)) && op == "+")
            {
                resultType = typeof(string);
                return string.Concat(a, b);
            }

            throw new Exception(string.Format("'{0}' operand is not compatible with '{1}' and '{2}'",
                op, aType, bType));
        }

        public object Evaluate(AST tree)
        {
            Type resultType;
            return InternalEvaluate(tree, out resultType);
        }

        /// <summary>
        /// Evaluates the tree, returns the resulting object.
        /// </summary>
        /// <param name="tree">AST tree to parse</param>
        /// <returns>Object depending on the type of expression</returns>
        object InternalEvaluate(AST tree, out Type resultType)
        {

            //AST returnedTree = null;
            object result = null;
            object a, b;

            try
            {

                if (tree == null)
                    tree = ASTNULL;

                switch (tree.Type)
                {
                    case ExpressionTokenTypes.GT:
                    case ExpressionTokenTypes.GTE:
                    case ExpressionTokenTypes.LTH:
                    case ExpressionTokenTypes.LTE:
                    case ExpressionTokenTypes.EQUAL:
                    case ExpressionTokenTypes.NOT_EQUAL:
                        {
                            AST tmpAST = tree;

                            tree = tree.getFirstChild();
                            Type aType;
                            a = InternalEvaluate(tree, out aType);

                            tree = tree.getNextSibling();
                            Type bType;
                            b = InternalEvaluate(tree, out bType);

                            tree = retTree_;
                            tree = tmpAST;
                            tree = tree.getNextSibling();

                            result = EvaluateEquality(aType, a, bType, b, tmpAST.getText());
                            resultType = typeof(bool);
                        }
                        break;

                    case ExpressionLexer.LITERAL_IN:
                    case ExpressionLexer.LITERAL_NOT:
                        {
                            AST tmpAST = tree; // NOT or IN

                            tree = tree.getFirstChild();
                            Type aType;
                            a = InternalEvaluate(tree, out aType);
                            ExpressionType leftType = GetExpressionType(a.GetType());

                            AST tmp = tree.getNextSibling();
                            ArrayList list = new ArrayList();
                            while (tmp != null)
                            {
                                Type oType;
                                object o = InternalEvaluate(tmp, out oType);
                                if (GetExpressionType(o.GetType()) != leftType)
                                {
                                    string msg = string.Format("Data type not match, '{0}'({1}) - '{2}'({3})",
                                        tmp.getText(), GetExpressionType(a.GetType()), tree.getText(), GetExpressionType(o.GetType()));
                                    throw new Exception(msg);
                                }
                                list.Add(o);
                                tmp = tmp.getNextSibling();
                            }

                            result = false;
                            //foreach(object o in list) {
                            //    if(a.Equals(o)) {
                            //        result = true;
                            //    }
                            //}
                            foreach (object o in list)
                            {
                                if (a.Equals(o) && tmpAST.Type == ExpressionLexer.LITERAL_IN)
                                    result = true;
                                else
                                {
                                    if (a.Equals(o) && tmpAST.Type == ExpressionLexer.LITERAL_NOT)
                                    {
                                        result = false;
                                        break;
                                    }
                                    else
                                    {
                                        if (!a.Equals(o) && tmpAST.Type == ExpressionLexer.LITERAL_NOT)
                                            result = true;
                                    }
                                }
                            }
                            resultType = typeof(bool);
                        }
                        break;

                    case ExpressionLexer.MULT:
                    case ExpressionLexer.DIV:
                    case ExpressionLexer.MINUS:
                    case ExpressionLexer.PLUS:
                    case ExpressionLexer.MOD:
                        {
                            AST tmpAST = tree;

                            tree = tree.getFirstChild();
                            Type aType;
                            a = InternalEvaluate(tree, out aType);

                            //tree = retTree_;
                            tree = tree.getNextSibling();
                            Type bType;
                            b = InternalEvaluate(tree, out bType);

                            tree = retTree_;
                            tree = tmpAST;
                            tree = tree.getNextSibling();

                            result = EvaluateArithmetic(aType, a, bType, b, tmpAST.getText(), out resultType);
                        }
                        break;

                    case ExpressionTokenTypes.FALSE:
                        resultType = typeof(bool);
                        result = false;
                        break;

                    case ExpressionTokenTypes.TRUE:
                        resultType = typeof(bool);
                        result = true;
                        break;

                    case ExpressionLexer.ID:
                        if (tree.getFirstChild() == null)
                        {
                            // this is a normal identifier
                            result = finderInstance.FindPropertyValue(tree.getText());
                            resultType = finderInstance.FindPropertyType(tree.getText());

                            tree = tree.getNextSibling();
                        }
                        else
                        {
                            // this sucks! it is a function unfortunately
                            string functionName = tree.getText();
                            AST tmp = tree.getFirstChild();
                            ArrayList args = new ArrayList();
                            while (tmp != null)
                            {
                                string text = tmp.getText();
                                if (text == "(" || text == ")")
                                {
                                    tmp = tmp.getNextSibling();
                                    continue;
                                }
                                Type tmpType;
                                args.Add(InternalEvaluate(tmp, out tmpType)); // tmp is the argument as AST.
                                tmp = tmp.getNextSibling();
                            }

                            // Handling functions was the most delightful improvement in EL for me.
                            //result = ExecuteFunction(functionName, args, out resultType);

                            throw new Exception("functions not supported yet!");
                            tree = tree.getNextSibling();
                        }
                        break;

                    case ExpressionLexer.LITERAL_AND:
                    case ExpressionLexer.LITERAL_OR:
                        {
                            AST tmpAST = tree;

                            tree = tree.getFirstChild();
                            AST aAST = tree;
                            Type aType;
                            a = InternalEvaluate(tree, out aType);

                            tree = retTree_;
                            AST bAST = tree;
                            Type bType;
                            b = InternalEvaluate(tree, out bType);

                            tree = retTree_;
                            tree = tmpAST;
                            tree = tree.getNextSibling();

                            if (a is bool && b is bool)
                            {
                                bool ba = Convert.ToBoolean(a);
                                bool bb = Convert.ToBoolean(b);

                                if (tmpAST.Type == ExpressionLexer.LITERAL_AND)
                                {
                                    result = ba && bb;
                                }
                                else if (tmpAST.Type == ExpressionLexer.LITERAL_OR)
                                {
                                    result = ba || bb;
                                }
                                resultType = typeof(bool);
                            }
                            else
                            {
                                throw new Exception(
                                    string.Format("'{0}' is not bool - e0001", tmpAST.getText()));
                            }
                        }
                        break;

                    case ExpressionLexer.STRING_LITERAL:
                        {
                            // remove the beginning and ending quotes (")
                            result = tree.getText().Substring(1, tree.getText().Length - 2);
                            resultType = typeof(string);

                            tree = tree.getNextSibling();
                        }
                        break;

                    case ExpressionLexer.NUMBER:
                        {
                            string str = tree.getText().Replace('.', ','); // TODO: IFormatProvider ?
                            result = decimal.Parse(str);
                            resultType = typeof(decimal);

                            tree = tree.getNextSibling();
                        }
                        break;

                    case ExpressionLexer.QUESTION_MARK:
                        {
                            AST tmpAST = tree;

                            tmpAST = tree.getFirstChild();
                            Type aType;
                            a = InternalEvaluate(tmpAST, out aType); // condition
                            if (a is bool)
                            {
                                bool condition = (bool)a;
                                if (condition == true)
                                {
                                    result = InternalEvaluate(tmpAST.getNextSibling(), out resultType);
                                }
                                else
                                {
                                    tmpAST = tmpAST.getNextSibling();
                                    result = InternalEvaluate(tmpAST.getNextSibling(), out resultType);
                                }
                            }
                            else
                            {
                                throw new Exception("Ternary condition not bool");
                            }

                            tree = tree.getNextSibling();
                        }
                        break;

                    default:
                        throw new NoViableAltException(tree);
                }

            }
            catch (RecognitionException ex)
            {
                throw new Exception("Expression could not be parsed", ex);
            }

            retTree_ = tree;
            return result;
        }


    }

}