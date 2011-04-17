using System.Collections.Generic;

namespace LiveDev.Domain
{
    public class CorrectionResult
    {
        public string Result { get; set; }
        public List<string> Errors { get; set; }

        public CorrectionResult()
        {
            Errors = new List<string>();
        }

        public virtual bool HasErrors()
        {
            return Errors.Count != 0;
        }
    }
}