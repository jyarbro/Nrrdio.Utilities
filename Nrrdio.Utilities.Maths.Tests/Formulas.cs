namespace Nrrdio.Utilities.Maths.Tests;

[TestClass]
public class Formulas {
    [TestMethod]
    public void Lerp() {
        Assert.AreEqual(2, Formula.Lerp(1, 3, 0.5));
        Assert.AreEqual(4, Formula.Lerp(1, 3, 1.5));
    }
}