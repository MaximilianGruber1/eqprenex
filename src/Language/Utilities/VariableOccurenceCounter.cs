namespace eqprenex.Language.Utilities
{
    public class VariableOccurenceCounter
    {
        public int Count(IFormula formula)
        {
            Stack<IFormula> stack = [];
            stack.Push(formula);
            int result = 0;

            while (stack.Count > 0)
            {
                var f = stack.Pop();

                if (f is Variable)
                {
                    result++;
                }
                else if (f is Not not)
                {
                    stack.Push(not.Inner);
                }
                else if (f is BinaryOperator b)
                {
                    stack.Push(b.Left);
                    stack.Push(b.Right);
                }
                else if (f is Quantifier q)
                {
                    result++;
                    stack.Push(q.Inner);
                }
            }

            return result;
        }
    }
}
