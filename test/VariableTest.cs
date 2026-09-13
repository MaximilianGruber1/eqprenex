
using Xunit;
using eqprenex.Language;
using eqprenex.Language.Utilities;
using eqprenex.Parsing;

namespace eqprenex.Tests
{
    public class VariableTest
    {
        [Fact]
        public void SingleCharacter_NoIndex()
        {
            var v = new Variable("a");
            Assert.Equal("a", v.Name);
            Assert.Equal("a", v.Stem);
            Assert.Equal("", v.Index);
        }

        [Fact]
        public void MultiCharacter_NoIndex()
        {
            var v = new Variable("abc");
            Assert.Equal("abc", v.Name);
            Assert.Equal("abc", v.Stem);
            Assert.Equal("", v.Index);
        }

        [Fact]
        public void SingleCharacter_SingleIndex()
        {
            var v = new Variable("a3");
            Assert.Equal("a3", v.Name);
            Assert.Equal("a", v.Stem);
            Assert.Equal("3", v.Index);
        }

        [Fact]
        public void MultiCharacter_SingleIndex()
        {
            var v = new Variable("abc9");
            Assert.Equal("abc9", v.Name);
            Assert.Equal("abc", v.Stem);
            Assert.Equal("9", v.Index);
        }

        [Fact]
        public void SingleCharacter_MultiIndex()
        {
            var v = new Variable("a123");
            Assert.Equal("a123", v.Name);
            Assert.Equal("a", v.Stem);
            Assert.Equal("123", v.Index);
        }

        [Fact]
        public void MultiCharacter_MultiIndex()
        {
            var v = new Variable("abc123");
            Assert.Equal("abc123", v.Name);
            Assert.Equal("abc", v.Stem);
            Assert.Equal("123", v.Index);
        }

        [Fact]
        public void IndexZero()
        {
            var v = new Variable("a0");
            Assert.Equal("a0", v.Name);
            Assert.Equal("a", v.Stem);
            Assert.Equal("0", v.Index);
        }

        [Fact]
        public void IndexStartsWith0()
        {
            var v = new Variable("a012");
            Assert.Equal("a012", v.Name);
            Assert.Equal("a", v.Stem);
            Assert.Equal("012", v.Index);
        }

        [Fact]
        public void DigitsInStem()
        {
            var v = new Variable("a0b34c789");
            Assert.Equal("a0b34c789", v.Name);
            Assert.Equal("a0b34c", v.Stem);
            Assert.Equal("789", v.Index);
        }

        [Fact]
        public void OnlySingleDigit()
        {
            var v = new Variable("1");
            Assert.Equal("1", v.Name);
            Assert.Equal("1", v.Stem);
            Assert.Equal("", v.Index);
        }

        [Fact]
        public void OnlyMultipleDigits()
        {
            var v = new Variable("012345");
            Assert.Equal("012345", v.Name);
            Assert.Equal("0", v.Stem);
            Assert.Equal("12345", v.Index);
        }
    }

    public class VariableCounterTest
    {
        private void Test(int count, string formula)
        {
            IFormula f = new Parser(formula).Parse();
            var ctr = new VariableOccurenceCounter();
            var actual = ctr.Count(f);

            Assert.Equal(count, actual);
        }


        [Fact]
        public void Simple()
        {
            Test(1, "a");
            Test(1, "!a");
            Test(2, "a & b");
            Test(2, "a | b");
            Test(2, "a -> b");
            Test(2, "a <- b");
            Test(2, "a <-> b");
            Test(2, "?a a");
            Test(2, "?a a");
        }

        [Fact]
        public void Complex()
        {
            Test(
                24,
                "?a ?b #c #d (!(a -> !c) & !(b <-> !d) <-> e <-> g <-> !(!f & h)) <-> " +
                "?a ?b #c #d (c <-> !(!a <-> !(!b <-> !d)) <-> !(h -> !(!g & !(e <-> f))))");
            Test(
                72,
                "?a ?b ?c #d #e #f (!(e <-> !(!(d & !(a -> !b)) & (!c | f))) <-> g -> (!j <-> !(!h & !l & !(!i <-> k)))) <-> " +
                "#a #b #c ?d ?e ?f (!c & !(!b & !d) & !(f | !(a | !e)) <-> !(!(g <-> k) & !(!h <- !(i | (!j -> !l))))) <-> " +
                "?a ?b ?c #d #e #f ((c <-> d & (a -> e)) & b & f <-> l & (!i | g & !(k | !(h & !j)))) <-> " +
                "?a ?b ?c #d #e #f (!(!a <-> d <- f) & !(b & c & e) <-> !(g -> !j <-> k <-> !(l <-> h & !i)))");
        }
    }
}
