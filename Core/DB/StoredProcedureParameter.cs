
namespace Core.Db
{
    using System;

    public class StoredProcedureParameter
    {
        public StoredProcedureParameter(String name, Object value)
        {
            this.ParameterName = name;
            this.Value = value;
        }

        public String ParameterName { get; private set; }
        public Object Value { get; private set; }
    }
}
