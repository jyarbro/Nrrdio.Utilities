using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nrrdio.Utilities.Maths;
using System.Linq;

namespace Nrrdio.Utilities.Tests {
    [TestClass]
    public class Hexagons {
        [TestMethod]
        public void One() {
            var hexagon = new Hexagon(new Point(0, 0), 1, 1);
            Assert.AreEqual(6, hexagon.Vertices.Count());
        }

        [TestMethod]
        public void Two() {
            var hexagon = new Hexagon(new Point(0, 0), 2, 1);
            Assert.AreEqual(12, hexagon.Vertices.Count());
        }

        [TestMethod]
        public void Three() {
            var hexagon = new Hexagon(new Point(0, 0), 3, 1);

            Assert.AreEqual(18, hexagon.Vertices.Count());
        }

        [TestMethod]
        public void Four() {
            var hexagon = new Hexagon(new Point(0, 0), 4, 1);

            Assert.AreEqual(24, hexagon.Vertices.Count());
        }

        [TestMethod]
        public void SixVertices() {
            var hexagon = new Hexagon(new Point(0, 0), 6, 1);

            var vertices = hexagon.Vertices.Where(v => (hexagon.Centroid - v).RadialMagnitude == hexagon.Radius);

            Assert.AreEqual(6, vertices.Count());
        }

        [TestMethod]
        public void SixApothems() {
            var hexagon = new Hexagon(new Point(0, 0), 6, 1);

            var vertices = hexagon.Vertices.Where(v => (hexagon.Centroid - v).RadialMagnitude == hexagon.Apothem);

            Assert.AreEqual(6, vertices.Count());
        }

        [TestMethod]
        public void Apothems() {
            var hexagon = new Hexagon(new Point(0, 0), 2, 1);

            var vertices = hexagon.Vertices.Where(v => (hexagon.Centroid - v).RadialMagnitude != hexagon.Radius).ToList();

            for (var i = 0; i < vertices.Count; i++) {
                var current = (hexagon.Centroid - vertices[i]).RadialMagnitude;
                Assert.AreEqual($"{1.7320508075688772f:#############}", $"{current:#############}");
            }
        }

        [TestMethod]
        public void Edges() {
            var hexagon = new Hexagon(new Point(0, 0), 2, 1);

            Assert.AreEqual(12, hexagon.Edges.Count());
        }
    }
}
