namespace LiveDev.Domain
{
    public class Definition
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string ReturnValue { get; set; }

        public override string ToString()
        {
            return "public class " + ClassName + "{public " + ReturnValue + " " + MethodName +
                   "(){//put content here.}}";
        }
    }
}