using System;

namespace FuzzyLogicPCL
{
    public class FuzzyExpression //Declara las variables lingüísticas
    {
        internal LinguisticVariable Lv { get; set; }
        internal String LinguisticValueName { get; set; }

        public FuzzyExpression(LinguisticVariable _lv, String _value)
        {
            Lv = _lv;
            LinguisticValueName = _value;
        }
    }
}
