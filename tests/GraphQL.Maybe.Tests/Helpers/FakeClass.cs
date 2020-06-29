namespace GraphQL.Maybe.Tests.Helpers
{
    internal class FakeClass
    {
        public Maybe<string> Name;

#pragma warning disable CS0649
        protected Maybe<float> Price;
#pragma warning restore CS0649  
    }
}
