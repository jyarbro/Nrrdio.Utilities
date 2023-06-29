using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nrrdio.Utilities.Maths;

namespace Nrrdio.Utilities.Tests;

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

		Assert.AreEqual((true, new Point(3, 2), default(Point)), segment1.Intersects(segment2));
		Assert.AreEqual((true, new Point(3, 2), default(Point)), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void LargeVerticalIntersects() {
		var segment1 = new Segment(new Point(0, 0), new Point(0, 600));
		var segment2 = new Segment(new Point(-80.896208128707386, 192.00151880011086), new Point(246.73060356878347, 16.227559135922846));

		Assert.AreEqual((true, new Point(0, 148.60016605084175), default(Point)), segment1.Intersects(segment2));
		Assert.AreEqual((true, new Point(0, 148.60016605084175), default(Point)), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void NoIntersect() {
		var segment1 = new Segment(new Point(1, -2), new Point(2.5, 1));
		var segment2 = new Segment(new Point(1, 4), new Point(4, 1));

		Assert.AreEqual((false, default(Point), default(Point)), segment1.Intersects(segment2));
		Assert.AreEqual((false, default(Point), default(Point)), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void LargeNoIntersect() {
		var segment1 = new Segment(new Point(-113.68370987427628, 300), new Point(-59.179092628201211, 276.83033152363953));
		var segment2 = new Segment(new Point(0, 0), new Point(0, 600));

		Assert.AreEqual((false, default(Point), default(Point)), segment1.Intersects(segment2));
		Assert.AreEqual((false, default(Point), default(Point)), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void IntersectsAtEnd() {
		var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
		var segment2 = new Segment(new Point(1, 4), new Point(3, 2));

		Assert.AreEqual((true, new Point(3, 2), default(Point)), segment1.Intersects(segment2));
		Assert.AreEqual((true, new Point(3, 2), default(Point)), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void IntersectsHorizontal() {
		var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
		var segment2 = new Segment(new Point(0, 2), new Point(4, 2));

		Assert.AreEqual((true, new Point(3, 2), default(Point)), segment1.Intersects(segment2));
		Assert.AreEqual((true, new Point(3, 2), default(Point)), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void IntersectsVertical() {
		var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
		var segment2 = new Segment(new Point(3, 4), new Point(3, 0));

		Assert.AreEqual((true, new Point(3, 2), default(Point)), segment1.Intersects(segment2));
		Assert.AreEqual((true, new Point(3, 2), default(Point)), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void ColinearOverlap() {
		var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
		var segment2 = new Segment(new Point(3, 2), new Point(3.5, 3));

		Assert.AreEqual((true, segment2.Point1, segment2.Point2), segment1.Intersects(segment2));
		Assert.AreEqual((true, segment2.Point1, segment2.Point2), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void ColinearReverseOverlap() {
		var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
		var segment2 = new Segment(new Point(3.5, 3), new Point(3, 2));

		Assert.AreEqual((true, segment2.Point1, segment2.Point2), segment1.Intersects(segment2));
		Assert.AreEqual((true, segment2.Point2, segment2.Point1), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void ColinearDisjoint() {
		var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
		var segment2 = new Segment(new Point(5, 6), new Point(6, 8));

		Assert.AreEqual((false, default(Point), default(Point)), segment1.Intersects(segment2));
		Assert.AreEqual((false, default(Point), default(Point)), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void PartialOverlap() {
		var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
		var segment2 = new Segment(new Point(3, 2), new Point(5, 6));

		Assert.AreEqual((true, segment2.Point1, segment1.Point2), segment1.Intersects(segment2));
		Assert.AreEqual((true, segment2.Point1, segment1.Point2), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void PartialOverlapReverse() {
		var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
		var segment2 = new Segment(new Point(5, 6), new Point(3, 2));

		Assert.AreEqual((true, segment1.Point2, segment2.Point2), segment1.Intersects(segment2));
		Assert.AreEqual((true, segment2.Point2, segment1.Point2), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void Parallel() {
		var segment1 = new Segment(new Point(2, 0), new Point(4, 4));
		var segment2 = new Segment(new Point(2, 2), new Point(4, 6));

		Assert.AreEqual((false, default(Point), default(Point)), segment1.Intersects(segment2));
		Assert.AreEqual((false, default(Point), default(Point)), segment2.Intersects(segment1));
	}

	[TestMethod]
	public void Contains() {
		var segment = new Segment(new Point(2, 8), new Point(4.59234123, 12));

		Assert.IsFalse(segment.Contains(new Point(0, .00004)));
		Assert.IsTrue(segment.Contains(new Point(6.59234123 / 2, 10)));
		Assert.IsFalse(segment.Contains(new Point(6, 9)));
	}

	[TestMethod]
	public void AngleTo() {
		var segment1 = new Segment(new Point(5, 10), new Point(10, 10));
		var segment2 = new Segment(new Point(5, 10), new Point(10, 15));

		Assert.AreEqual(45D, segment1.AngleTo(segment2));

        segment1 = new Segment(new Point(100, 5), new Point(0, 8));
        segment2 = new Segment(new Point(100, 0), new Point(0, 0));

        Assert.AreEqual(1.718, Math.Round(segment1.AngleTo(segment2), 3));
    }
}
