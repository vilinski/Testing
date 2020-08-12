using AutoFixture.Xunit2;
using DotNet.Testing.AutoMoqFixture;
using TestImplementation.UnderTestClasses;
using Xunit;


namespace TestImplementation
{
    public class AutoMoqFixtureAttributeTests
    {

        [Theory, AutoMoqFixture(false)]
        public void Resolve_ByDefault_FirstClass(FirstClass firstClass)
        {
            var result = firstClass.TriggerMethod();

            Assert.True(result);
            Assert.NotNull(firstClass.GetSecondClass);
            Assert.NotNull(firstClass.GetSecondClass.Property);
            Assert.NotNull(firstClass.GetSecondClass.GetMyInterface.Model);
            Assert.Equal(0, firstClass.GetSecondClass.GetMyInterface.Model.MyInteger);
            Assert.Null(firstClass.GetSecondClass.GetMyInterface.Model.MyString);
        }

        [Theory, AutoMoqFixture(true)]
        public void Resolve_ConfigureMember_FirstClass(FirstClass firstClass)
        {
            var result = firstClass.TriggerMethod();

            Assert.True(result);
            Assert.NotNull(firstClass.GetSecondClass);
            Assert.NotNull(firstClass.GetSecondClass.Property);
            Assert.NotNull(firstClass.GetSecondClass.GetMyInterface.Model);
            Assert.NotEqual(0, firstClass.GetSecondClass.GetMyInterface.Model.MyInteger);
            Assert.NotNull(firstClass.GetSecondClass.GetMyInterface.Model.MyString);
        }

        [Theory, AutoMoqFixture(17)]
        public void Resolve_WithRepeatCount_FirstClass(FirstClass firstClass)
        {
            int expectedRepeatCount = 17;

            Assert.Equal(expectedRepeatCount, firstClass.StrArray.Length);
        }

        [Theory, AutoMoqFixture(true)]
        public void Resolve_ByTransient_SecondClass(SecondClass secondClass, FirstClass firstClass)
        {
            Assert.NotEqual(firstClass.GetSecondClass.GetMyInterface.Model.MyString, secondClass.GetMyInterface.Model.MyString);
            Assert.NotEqual(firstClass.GetSecondClass.GetMyInterface.Model.MyInteger, secondClass.GetMyInterface.Model.MyInteger);
            Assert.NotEqual(firstClass.GetSecondClass.Property, secondClass.Property);
        }


        [Theory, AutoMoqFixture(true)]
        public void Resolve_BySingle_SecondClass([Frozen]SecondClass secondClass, FirstClass firstClass)
        {
            Assert.Equal(firstClass.GetSecondClass.GetMyInterface.Model.MyString, secondClass.GetMyInterface.Model.MyString);
            Assert.Equal(firstClass.GetSecondClass.GetMyInterface.Model.MyInteger, secondClass.GetMyInterface.Model.MyInteger);
            Assert.Equal(firstClass.GetSecondClass.Property, secondClass.Property);
        }
    }
}
