using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nrrdio.Utilities.Maths;
using System.Collections.Generic;

namespace Nrrdio.Utilities.Tests {
    [TestClass]
    public class Polygons {
        [TestMethod]
        public void Winding() {
            var points = new List<Point> {
                new Point(2, 0),
                new Point(4, 4),
                new Point(6, 3),
                new Point(4, -1),
            };

            var polygon = new Polygon(points);

            Assert.AreEqual(Polygon.EWinding.CLOCKWISE, polygon.Winding);
        }

        [TestMethod]
        public void Area() {
            var polygon = new Polygon(new List<Point> {
                new Point(2, 0),
                new Point(2, 4),
                new Point(4, 4),
                new Point(4, 0),
            });

            Assert.AreEqual(8, polygon.Area);

            polygon = new Polygon(new List<Point> {
                new Point(2, 0),
                new Point(4, 4),
                new Point(6, 3),
                new Point(4, -1),
            });

            Assert.AreEqual(10, polygon.Area);
        }

        [TestMethod]
        public void Centroid() {
            var polygon = new Polygon(new List<Point> {
                new Point(2, 0),
                new Point(2, 4),
                new Point(4, 4),
                new Point(4, 0),
            });

            Assert.AreEqual(new Point(3, 2), polygon.Centroid);

            polygon = new Polygon(new List<Point> {
                new Point(2, 0),
                new Point(4, 4),
                new Point(6, 3),
                new Point(4, -1),
            });

            Assert.AreEqual(new Point(4, 1.5), polygon.Centroid);

            polygon = new Polygon(new List<Point> {
                new Point(2, 0),
                new Point(2, 4),
                new Point(4, 4),
                new Point(4, 3),
                new Point(5, 3),
                new Point(5, 1),
                new Point(4, 1),
                new Point(4, 0),
            });

            Assert.AreEqual(new Point(3.3, 2), polygon.Centroid);
        }

        [TestMethod]
        public void CentroidReversed() {
            var points = new List<Point> {
                new Point(4, 4),
                new Point(2, 4),
                new Point(2, 0),
                new Point(4, 0),
                new Point(4, 1),
                new Point(5, 1),
                new Point(5, 3),
                new Point(4, 3),
            };

            var polygon = new Polygon(points);

            Assert.AreEqual(new Point(3.3, 2), polygon.Centroid);
        }

        [TestMethod]
        public void ContainsOne() {
            var points = new List<Point> {
                new Point(4, 4),
                new Point(2, 4),
                new Point(2, 0),
                new Point(4, 0),
                new Point(4, 1),
                new Point(5, 1),
                new Point(5, 3),
                new Point(4, 3),
            };

            var polygon = new Polygon(points);

            var testPoint = new Point(2.05, .0005);
            Assert.IsTrue(polygon.Contains(testPoint));

            testPoint = new Point(4.5, 2.9999999);
            Assert.IsTrue(polygon.Contains(testPoint));

            testPoint = new Point(4.5, 3.1);
            Assert.IsFalse(polygon.Contains(testPoint));
        }

        [TestMethod]
        public void ContainsMany() {
            var points = new List<Point> {
                new Point(4, 4),
                new Point(2, 4),
                new Point(2, 0),
                new Point(4, 0),
                new Point(4, 1),
                new Point(5, 1),
                new Point(5, 3),
                new Point(4, 3),
            };

            var polygon = new Polygon(points);

            var testPoints = new List<Point> {
                new Point(3, 1),
                new Point(4, 2)
            };

            Assert.IsTrue(polygon.Contains(testPoints));

            testPoints.Add(new Point(4.5, 3.0000001));

            Assert.IsFalse(polygon.Contains(testPoints));
        }

        [TestMethod]
        public void ContainsEdges() {
            var polygon = new Polygon(new List<Point> {
                new Point(4, 4),
                new Point(2, 4),
                new Point(2, 0),
                new Point(4, 0),
                new Point(4, 1),
                new Point(5, 1),
                new Point(5, 3),
                new Point(4, 3),
            });

            var reversePolygon = new Polygon(new List<Point> {
                new Point(4, 3),
                new Point(5, 3),
                new Point(5, 1),
                new Point(4, 1),
                new Point(4, 0),
                new Point(2, 0),
                new Point(2, 4),
                new Point(4, 4),
            });

            var testPoint = new Point(4.5, 3);
            Assert.IsTrue(polygon.Contains(testPoint));
            Assert.IsTrue(reversePolygon.Contains(testPoint));

            testPoint = new Point(3, 0);
            Assert.IsTrue(polygon.Contains(testPoint));
            Assert.IsTrue(reversePolygon.Contains(testPoint));

            testPoint = new Point(4, 3);
            Assert.IsTrue(polygon.Contains(testPoint));
            Assert.IsTrue(reversePolygon.Contains(testPoint));

            testPoint = new Point(5, 2);
            Assert.IsTrue(polygon.Contains(testPoint));
            Assert.IsTrue(reversePolygon.Contains(testPoint));

            testPoint = new Point(2, 2);
            Assert.IsTrue(polygon.Contains(testPoint));
            Assert.IsTrue(reversePolygon.Contains(testPoint));

            testPoint = new Point(2, 4);
            Assert.IsTrue(polygon.Contains(testPoint));
            Assert.IsTrue(reversePolygon.Contains(testPoint));

            testPoint = new Point(3, 4);
            Assert.IsTrue(polygon.Contains(testPoint));
            Assert.IsTrue(reversePolygon.Contains(testPoint));
        }
    }
}