using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedLoopList.Evaluator
{

    public class ExpressionContext {

        Dictionary<string, object> dict = new Dictionary<string, object>();

        public void Add(string key, object obj) {
            dict.Add(key, obj);
        }

        public bool ContainsKey(string key) {
            return dict.ContainsKey(key);
        }

        public object this[string key] {
            get {
                object value;
                if(dict.TryGetValue(key, out value)) {
                    return value;
                } else {
                    throw new Exception(string.Format("'{0}' key is not found in Expressioncontext", key));
                }
            }
            set {
                dict.Add(key, value);
            }
        }
    }

}
