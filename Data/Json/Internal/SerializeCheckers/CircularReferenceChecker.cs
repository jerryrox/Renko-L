using System;
using System.Collections;
using System.Collections.Generic;
using Renko.Diagnostics;

namespace Renko.Data.Internal
{
    /// <summary>
    /// Checks for circular references in JsonData.
    /// There is a significant limitation in which objects with type other than JsonData are not deeply checked.
    /// </summary>
    public class CircularReferenceChecker {

        private Stack<object> stack;


        private CircularReferenceChecker() {
            stack = new Stack<object>(16);
        }

        /// <summary>
        /// Returns true if there is a circular referencing.
        /// </summary>
        public static bool Process(JsonData data) {
            return new CircularReferenceChecker().Evaluate(data);
        }

        private bool Evaluate(object obj) {
            Type type = obj.GetType();

            // object must be a class.
            if(!type.IsClass)
                return false;

            JsonObject jo;
            JsonArray ja;
            JsonData jd;

            // Deeper enumerations
            if((jo = obj as JsonObject) != null)
                return EvaluateObject(jo);
            else if((ja = obj as JsonArray) != null)
                return EvaluateArray(ja);
            else if((jd = obj as JsonData) != null)
                return EvaluateData(jd);
			
            return EvaluateRaw(obj);
        }

        private bool EvaluateObject(JsonObject obj) {
            if(stack.Contains(obj))
                return true;
            stack.Push(obj);

            foreach(var data in obj.Values) {
                if(Evaluate(data.Value))
                    return true;
            }
            stack.Pop();
            return false;
        }

        private bool EvaluateArray(JsonArray array) {
            if(stack.Contains(array))
                return true;
            stack.Push(array);

            for(int i=0; i<array.Count; i++) {
                if(Evaluate(array[i].Value))
                    return true;
            }
            stack.Pop();
            return false;
        }

        private bool EvaluateData(JsonData data) {
            return Evaluate(data.Value);
        }

        private bool EvaluateRaw(object raw) {
            if(!raw.GetType().IsClass)
                return false;
            return stack.Contains(raw);
        }
    }
}
