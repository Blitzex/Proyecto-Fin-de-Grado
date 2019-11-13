using FuzzyLogicPCL.FuzzySets;
using System;
using System.Collections.Generic;

namespace FuzzyLogicPCL
{
    public class FuzzyRule
    {
        List<FuzzyExpression> Premises;
        FuzzyExpression Conclusion;

        public FuzzyRule(List<FuzzyExpression> _prem, FuzzyExpression _concl)
        {
            Premises = _prem;
            Conclusion = _concl;
        }

        public FuzzyRule(string ruleStr, FuzzySystem fuzzySystem)
        {
            // To Uppercase
            ruleStr = ruleStr.ToUpper();

            // Split premises and conclusion
            String[] rule = ruleStr.Split(new String[] { " THEN " }, StringSplitOptions.RemoveEmptyEntries);
            if (rule.Length == 2)
            {
                // Compute and add premises
                rule[0] = rule[0].Remove(0, 2); // On enlève "IF"
                String[] prem = rule[0].Trim().Split(new String[] { " AND " }, StringSplitOptions.RemoveEmptyEntries);
                Premises = new List<FuzzyExpression>();
                foreach (String exp in prem)
                {
                    String[] res = exp.Split(new String[] { " IS " }, StringSplitOptions.RemoveEmptyEntries);
                    if (res.Length == 2)
                    {
                        FuzzyExpression fexp = new FuzzyExpression(fuzzySystem.LinguisticVariableByName(res[0]), res[1]);
                        Premises.Add(fexp);
                    }
                }
                // Add conclusion
                String[] conclu = rule[1].Split(new String[] { " IS " }, StringSplitOptions.RemoveEmptyEntries);
                if (conclu.Length == 2)
                {
                    Conclusion = new FuzzyExpression(fuzzySystem.LinguisticVariableByName(conclu[0]), conclu[1]);
                }
            }
        }

        internal FuzzySet Apply(List<FuzzyValue> Problem)
        {
            // Compute degree (for the whole rule) : min(degree for each premise)
            double degree = 1;
            foreach (FuzzyExpression premise in Premises)
            {
                double localDegree = 0;
                LinguisticValue val = null;
                // Search premise in problem : is there a value in the problem ? (officially : yes but...)
                foreach (FuzzyValue pb in Problem)
                {
                    if (premise.Lv == pb.Lv)
                    {
                        // We have found the Linguistic Variable, we search for the Linguistic Value and stop
                        val = premise.Lv.LinguisticValueByName(premise.LinguisticValueName);
                        if (val != null)
                        {
                            localDegree = val.DegreeAtValue(pb.Value); // this is fuzzyfication here
                            break;
                        }
                    }
                }
                if (val == null)
                {
                    return null; // problem here : we don't have the information in the problem
                }

                // Change overall degree and go on for the next premise
                degree = Math.Min(degree, localDegree);
            }

            // We know how much the rule is true, so we compute the resulting fuzzy set * degree
            return Conclusion.Lv.LinguisticValueByName(Conclusion.LinguisticValueName).Fs * degree;
        }
    }
}
