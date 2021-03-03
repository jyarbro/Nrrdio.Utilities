using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nrrdio.Utilities.Maths;

namespace Nrrdio.Utilities.Tests {
    [TestClass]
    public class Segments {
        [TestMethod]
        public void Equality() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment3 = new Segment(new Point(4, 4), new Point(2, 0));
            var segment4 = new Segment(new Point(6, 6), new Point(10, 10));

            Assert.AreEqual(segment1, segment2);
            Assert.AreEqual(segment2, segment3);
            Assert.AreNotEqual(segment3, segment4);
        }

        [TestMethod]
        public void Intersects() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(0, 2), new Point(4, 2));

            var intersection = (true, new Point(3, 2), default(Point));
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void Overlaps() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(3, 2), new Point(3.5, 3));

            var intersection = (true, new Point(3, 2), new Point(3.5, 3));
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void Parallel() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(2, 2), new Point(4, 6));

            var intersection = (false, default(Point), default(Point));
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }
    }
}
