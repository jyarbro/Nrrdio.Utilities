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
		Assert.AreNotEqual(segment2, segment3);
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

        segment1 = new Segment(new Point(401.04278015968674, 67.0196425998768), new Point(551.28296413574037, 205.27863549802379));
        segment2 = new Segment(new Point(406.78548879217476, 72.304387939124567), new Point(435.14459119245265, 98.401939430294789));

        Assert.AreEqual((true, segment2.Point1, segment2.Point2), segment2.Intersects(segment1));
        Assert.AreEqual((true, segment2.Point1, segment2.Point2), segment1.Intersects(segment2));

        segment1 = new Segment(new Point(286.40309161863138, 239.852318841589), new Point(247.286618350403, 259.86559502957749));
        segment2 = new Segment(new Point(397.43104630611151, 183.0467600553726), new Point(166.71963994898061, 301.0863149887258));

        Assert.AreEqual((true, segment1.Point1, segment1.Point2), segment2.Intersects(segment1));
        Assert.AreEqual((true, segment1.Point1, segment1.Point2), segment1.Intersects(segment2));

        segment1 = new Segment(new Point(247.286618350403, 259.86559502957749), new Point(166.71963994898061, 301.0863149887258));
        segment2 = new Segment(new Point(397.43104630611151, 183.0467600553726), new Point(166.71963994898061, 301.0863149887258));

        Assert.AreEqual((true, segment1.Point1, segment1.Point2), segment2.Intersects(segment1));
        Assert.AreEqual((true, segment1.Point1, segment1.Point2), segment1.Intersects(segment2));
    }

    [TestMethod]
    public void ColinearOverlapSameStart() {
        var segment1 = new Segment(new Point(463.34334836776395, 163.34334836776392), new Point(600, 258.11552765609));
        var segment2 = new Segment(new Point(463.34334836776395, 163.34334836776392), new Point(508.84201564160634, 194.896937605855));

        Assert.AreEqual((true, segment1.Point1, segment2.Point2), segment2.Intersects(segment1));
        Assert.AreEqual((true, segment1.Point1, segment2.Point2), segment1.Intersects(segment2));
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
	public void AngleIsNumber() {
		var origin = new Point(0, 0);
        var segment1 = new Segment(origin, new Point(1000, 0));
		var increment = .001d;

        for (double x = -1000; x < 1000; x += increment) {
			var segment2 = new Segment(origin, new Point(x, 1000));

			var angle = segment1.AngleTo(segment2);
			Assert.IsFalse(double.IsNaN(angle));
		}

        for (double x = -1000; x < 1000; x += increment) {
            var segment2 = new Segment(origin, new Point(x, -1000));

            var angle = segment1.AngleTo(segment2);
            Assert.IsFalse(double.IsNaN(angle));
        }

        for (double y = -1000; y < 1000; y += increment) {
            var segment2 = new Segment(origin, new Point(1000, y));

            var angle = segment1.AngleTo(segment2);
            Assert.IsFalse(double.IsNaN(angle));
        }

        for (double y = -1000; y < 1000; y += increment) {
            var segment2 = new Segment(origin, new Point(-1000, y));

            var angle = segment1.AngleTo(segment2);
            Assert.IsFalse(double.IsNaN(angle));
        }
    }

    [TestMethod]
	public void AngleTo() {
		var segment1 = new Segment(new Point(5, 10), new Point(10, 10));
		var segment2 = new Segment(new Point(5, 10), new Point(10, 15));

        var angle = segment1.AngleTo(segment2);
        Assert.AreEqual(45D, angle);

        segment1 = new Segment(new Point(100, 5), new Point(0, 8));
        segment2 = new Segment(new Point(100, 0), new Point(0, 0));

        angle = segment1.AngleTo(segment2);
        Assert.AreEqual(1.718, Math.Round(angle, 3));

		segment1 = new Segment(new Point(205.51224777884937, 408.80471583332934), new Point(135.27451178530475, 386.92074581254468));
		segment2 = new Segment(new Point(135.27451178530475, 386.92074581254468), new Point(65.036775791760121, 365.03677579176));

        angle = segment1.AngleTo(segment2);
        Assert.AreEqual(0, Math.Round(angle, 3));

        segment1 = new Segment(new Point(89.150129014188011, 302.67028436078027), new Point(97.754420007461519, 335.78651332747489));
        segment2 = new Segment(new Point(97.754420007461519, 335.78651332747489), new Point(97.846244302003811, 336.139927032932));

		angle = segment1.AngleTo(segment2);
        Assert.AreEqual(0, Math.Round(angle, 3));
    }

    [TestMethod]
	public void Midpoint() {
		var point1 = new Point(-3, 5);
		var point2 = new Point(8, -1);
		var segment1 = new Segment(point1, point2);

		Assert.AreEqual(new Point(2.5, 2), segment1.Midpoint);

		point1 = new Point(-2, 5);
		point2 = new Point(6, -1);
		segment1 = new Segment(point1, point2);

		Assert.AreEqual(new Point(2, 2), segment1.Midpoint);
	}
}
