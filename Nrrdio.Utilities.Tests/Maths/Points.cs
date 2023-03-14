using Nrrdio.Utilities.Maths;

namespace Nrrdio.Utilities.Tests;

[TestClass]
public class Points {
	[TestMethod]
	public void Subtraction() {
		var point1 = new Point(10, 5);
		var point2 = point1 - new Point(1, 0);

		Assert.AreEqual(new Point(9, 5), point2);
	}

	[TestMethod]
	public void AreEqual() {
		var point1 = new Point(3, 3);
		var point2 = new Point(3, 3);
		var point3 = new Point(3, 3.00000000000000000000000001);

		Assert.AreEqual(point1, point2);
		Assert.IsTrue(point1 == point2);

		Assert.AreEqual(point1, point3);
		Assert.IsTrue(point1 == point3);
	}

	[TestMethod]
	public void AreNotEqual() {
		var point1 = new Point(3, 4);
		var point2 = new Point(3, 4.00001);
		var point3 = new Point(4, 3);

		Assert.AreNotEqual(point1, point2);
		Assert.IsTrue(point1 != point2);

		Assert.AreNotEqual(point1, point3);
		Assert.IsTrue(point1 != point3);
	}

	[TestMethod]
	public void LeftOfPositiveLine() {
		var line = new Segment(new Point(2, 0), new Point(4, 10));

		Assert.IsTrue(new Point(0, 0).NearLine(line) > 0);
		Assert.IsFalse(new Point(3, 3).NearLine(line) > 0);
		Assert.IsTrue(new Point(1, 9).NearLine(line) > 0);
		Assert.IsTrue(new Point(0, 100).NearLine(line) > 0);
	}

	[TestMethod]
	public void RightOfNegativeLine() {
		var line = new Segment(new Point(4, 10), new Point(2, 0));

		Assert.IsTrue(new Point(0, 0).NearLine(line) < 0);
		Assert.IsFalse(new Point(3, 3).NearLine(line) < 0);
		Assert.IsTrue(new Point(1, 9).NearLine(line) < 0);
		Assert.IsTrue(new Point(0, 100).NearLine(line) < 0);
	}

	[TestMethod]
	public void RightOfLine() {
		var line = new Segment(new Point(2, 0), new Point(4, 10));

		Assert.IsTrue(new Point(3, -4).NearLine(line) < 0);
		Assert.IsFalse(new Point(2, 3).NearLine(line) < 0);
		Assert.IsTrue(new Point(9, 3).NearLine(line) < 0);
		Assert.IsTrue(new Point(100, 0).NearLine(line) < 0);
	}

	[TestMethod]
	public void LeftOfNegativeLine() {
		var line = new Segment(new Point(4, 10), new Point(2, 0));

		Assert.IsTrue(new Point(3, -4).NearLine(line) > 0);
		Assert.IsFalse(new Point(2, 3).NearLine(line) > 0);
		Assert.IsTrue(new Point(9, 3).NearLine(line) > 0);
		Assert.IsTrue(new Point(100, 0).NearLine(line) > 0);
	}

	[TestMethod]
	public void OnLine() {
		var line = new Segment(new Point(2, 0), new Point(4, 4));

		Assert.IsTrue(new Point(2.5, 1).NearLine(line) == 0);
		Assert.IsTrue(new Point(3, 2).NearLine(line) == 0);
		Assert.IsTrue(new Point(3.5, 3).NearLine(line) == 0);
	}

	[TestMethod]
	public void Lerps() {
		var point1 = new Point(2, 0);
		var point2 = new Point(4, 4);

		Assert.AreEqual(new Point(3, 2), point1.Lerp(point2, 0.5));
		Assert.AreEqual(new Point(3.5, 3), point1.Lerp(point2, 0.75));
	}

	[TestMethod]
	public void Sort() {
		var points = new List<Point> {
				new Point(4, 4),
				new Point(2, 0),
				new Point(4, -1),
				new Point(6, 3),
			};

		var shuffled = new List<Point> {
				points[3],
				points[0],
				points[2],
				points[1],
			};

		shuffled = shuffled.Sort(new Polygon(points).Circumcircle.Center);

		foreach (var item in shuffled) {
			Console.WriteLine(item);
		}

		Assert.AreEqual(new Point(6, 3), shuffled[0]);
		Assert.AreEqual(new Point(4, 4), shuffled[1]);
		Assert.AreEqual(new Point(2, 0), shuffled[2]);
		Assert.AreEqual(new Point(4, -1), shuffled[3]);
	}

	[TestMethod]
	public void SortComplex() {
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

		var shuffled = new List<Point> {
				points[3],
				points[6],
				points[4],
				points[1],
				points[5],
				points[2],
				points[7],
				points[0],
			};

		var polygon = new Polygon(points);

		shuffled = shuffled.Sort(polygon.Circumcircle.Center);

		var start = shuffled.IndexOf(points[0]);

		for (int i = 0; i < shuffled.Count; i++) {
			var j = (i + start) % shuffled.Count;
			Assert.AreEqual(points[i], shuffled[j]);
		}
	}
}
