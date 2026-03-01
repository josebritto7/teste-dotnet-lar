using Bogus;

namespace Lar.TesteDotNet.Tests.Shared.Common;

public abstract class Test
{
    protected static Faker Faker = new("pt_BR");
}