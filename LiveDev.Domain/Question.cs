using System;

namespace LiveDev.Domain
{
    public class Question
    {
        public virtual int Id { get; set; }
        public virtual string TextQuestion { get; set; }
        public virtual string SourceCode { get; set; }
        
        private Definition _contractDefinition;
        public virtual Definition ContractDefinition
        {
            get { return _contractDefinition;}
            set { _contractDefinition = value;}
        }

        public Question()
        {
        }

        public Question(Definition contractDefinition)
        {
            _contractDefinition = contractDefinition;
        }

        public virtual string GetContractDefinitionSourceCode()
        {
            return ContractDefinition.ToString();
        }
    }
}