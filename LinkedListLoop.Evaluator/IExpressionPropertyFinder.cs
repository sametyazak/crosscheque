using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedLoopList.Evaluator
{

	public interface IExpressionPropertyFinder {
		
        object FindPropertyValue(string propertyName);
        
        Type FindPropertyType(string propertyName);
		
        string[] GetAllPropertyNames();

	}

}
