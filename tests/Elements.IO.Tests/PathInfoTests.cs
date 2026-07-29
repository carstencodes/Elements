namespace HedgeCraft.Elements.IO.Tests;

using System.Threading.Tasks;

using TUnit;
using TUnit.Assertions;
using HedgeCraft.Elements.IO;

public class PathInfoTests
{
    [Test]
    public async Task TestPathInfoCreate()
    {
        int x = 1;
        await Assert.That(x).IsGreaterThan(0);
    }
}
