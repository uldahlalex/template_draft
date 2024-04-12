using FluentAssertions;

namespace tests;

public static class AssertionHelpers
{
    public static void ShouldNotContainNulls<T>(this T obj)
    {
        foreach (var property in typeof(T).GetProperties())
        {
            var propertyValue = property.GetValue(obj);
            propertyValue.Should().NotBeNull($"because property {property.Name} should not be null");
        }
    }
}