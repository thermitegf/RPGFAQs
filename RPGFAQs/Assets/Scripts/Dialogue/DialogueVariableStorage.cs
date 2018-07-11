using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace Assets.Scripts
{
    class DialogueVariableStorage : VariableStorageBehaviour
    {
        Dictionary<string, Value> variables = new Dictionary<string, Value>();

        [Serializable]
        public class DefaultVariable
        {
            public string Name;
            public string value;
            public Value.Type type;
        }

        public DefaultVariable[] defaultVariables;

        void Awake()
        {
            ResetToDefaults();
        }

        public override void ResetToDefaults()
        {
            Clear();

            foreach (var variable in defaultVariables)
            {
                object value;

                switch (variable.type)
                {
                    case Value.Type.Number:
                        var f = 0.0f;
                        float.TryParse(variable.value, out f);
                        value = f;
                        break;
                    case Value.Type.String:
                        value = variable.value;
                        break;
                    case Value.Type.Bool:
                        var b = false;
                        bool.TryParse(variable.value, out b);
                        value = b;
                        break;
                    case Value.Type.Variable:
                        Debug.Log("Eh.");
                        continue;
                    case Value.Type.Null:
                        value = null;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var v = new Value(value);

                SetValue($"${variable.Name}", v);
            }
        }

        public override void SetValue(string variableName, Value value)
        {
            variables[variableName] = new Value(value);
        }

        public override Value GetValue(string variableName)
        {
            if (!variables.ContainsKey(variableName))
            {
                return Value.NULL;
            }
            return variables[variableName];
        }

        public override void Clear()
        {
            variables.Clear();
        }

        void Update()
        {

        }
    }
}
