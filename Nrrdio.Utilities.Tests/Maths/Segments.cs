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
            var segment2 = new Segment(new Point(1, 4), new Point(4, 1));

            var intersection = (true, new Point(3, 2), default(Point));
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void IntersectsAtEnd() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(1, 4), new Point(3, 2));

            var intersection = (true, new Point(3, 2), default(Point));
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void IntersectsHorizontal() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(0, 2), new Point(4, 2));

            var intersection = (true, new Point(3, 2), default(Point));
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void IntersectsVertical() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(3, 4), new Point(3, 0));

            var intersection = (true, new Point(3, 2), default(Point));
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void ColinearOverlap() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(3, 2), new Point(3.5, 3));

            var intersection = (true, segment2.Point1, segment2.Point2);
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void ColinearDisjoint() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(5, 6), new Point(6, 8));

            var intersection = (false, default(Point), default(Point));
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void OverlapsReverse() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(3.5, 3), new Point(3, 2));

            var intersection = (true, segment2.Point1, segment2.Point2);
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void PartialOverlap() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(3, 2), new Point(5, 6));

            var intersection = (true, segment2.Point1, segment1.Point2);
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void PartialOverlapReverse() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(5, 6), new Point(3, 2));

            var intersection = (true, segment1.Point2, segment2.Point2);
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void Parallel() {
            var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
            var segment2 = new Segment(new Point(2, 2), new Point(4, 6));

            var intersection = (false, default(Point), default(Point));
            Assert.AreEqual(intersection, segment1.Intersects(segment2));
        }

        [TestMethod]
        public void Contains() {
            var segment = new Segment(new Point(2, 8), new Point(4.59234123, 12));

            Assert.IsFalse(segment.Contains(new Point(0, .00004)));
            Assert.IsTrue(segment.Contains(new Point(6.59234123 / 2, 10)));
            Assert.IsFalse(segment.Contains(new Point(6, 9)));
        }
    }
}
