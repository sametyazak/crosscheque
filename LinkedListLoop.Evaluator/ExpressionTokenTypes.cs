namespace LinkedLoopList.Evaluator
{
	public class ExpressionTokenTypes
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
		
	}
}
