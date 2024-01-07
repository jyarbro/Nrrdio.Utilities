using System.Diagnostics;

namespace Nrrdio.Utilities.Maths.Tests;

[TestClass]
public class CatmullRomSplines {
    [TestMethod]
    public void Performance() {
        var timer = new Stopwatch();

        var point1 = new Point(1, 2);
        var point2 = new Point(2, 3);
        var point3 = new Point(3, 2);
        var point4 = new Point(4, 0);

        var uniformSpline = new CatmullRomSpline(point1, point2, point3, point4);
        var centripetalSpline = new CentripetalCatmullRomSpline(point1, point2, point3, point4);

        for (var i = 0; i < 1000000; i++) {
            _ = uniformSpline.Interpolate(0.5f);
        }

        timer.Start();
        for (var i = 0; i < 1000000; i++) {
            _ = uniformSpline.Interpolate(0.5f);
        }
        timer.Stop();

        Console.WriteLine($"v1 took {timer.ElapsedMilliseconds} milliseconds");

        for (var i = 0; i < 1000000; i++) {
            _ = centripetalSpline.Interpolate(0.5f);
        }

        timer.Restart();
        for (var i = 0; i < 1000000; i++) {
            _ = centripetalSpline.Interpolate(0.5f);
        }
        timer.Stop();

        Console.WriteLine($"v2 {timer.ElapsedMilliseconds} milliseconds");
    }

    [TestMethod]
    public void Interpolate() {
        var point1 = new Point(1,2);
        var point2 = new Point(2,3);
        var point3 = new Point(3,2);
        var point4 = new Point(4,0);

        var spline = new CatmullRomSpline(point1, point2, point3, point4, 0.5);

        var value1 = spline.Interpolate(0);
        var value2 = spline.Interpolate(0.25f);
        var value3 = spline.Interpolate(0.5f);
        var value4 = spline.Interpolate(0.75f);
        var value5 = spline.Interpolate(1);

        Console.WriteLine(value1.X);
        Console.WriteLine(value1.Y);
        Console.WriteLine("---");
        Console.WriteLine(value2.X);
        Console.WriteLine(value2.Y);
        Console.WriteLine("---");
        Console.WriteLine(value3.X);
        Console.WriteLine(value3.Y);
        Console.WriteLine("---");
        Console.WriteLine(value4.X);
        Console.WriteLine(value4.Y);
        Console.WriteLine("---");
        Console.WriteLine(value5.X);
        Console.WriteLine(value5.Y);

        Assert.AreEqual(point2, value1);
        Assert.AreEqual(new Point(2.25, 2.9140625), value2);
        Assert.AreEqual(new Point(2.5, 2.6875), value3);
        Assert.AreEqual(new Point(2.75, 2.3671875), value4);
        Assert.AreEqual(point3, value5);
    }

    [TestMethod]
    public void Angle() {
        var point1 = new Point(1, 2);
        var point2 = new Point(3, 3);
        var point3 = new Point(3, 2);
        var point4 = new Point(3, 0);

        var spline = new CatmullRomSpline(point1, point2, point3, point4);

        for (var i = 0f; i < 1; i += .1f) {
            var angle = spline.AngleAt(i);
            Console.WriteLine($"{i:0.0}: Angle {angle}");
        }

        Console.WriteLine("---");

        point1 = new Point(0, 0);
        point2 = new Point(1, 2);
        point3 = new Point(2, 3);
        point4 = new Point(3, 2);

        spline = new CatmullRomSpline(point1, point2, point3, point4);

        for (var i = 0f; i < 1; i += .1f) {
            var angle = spline.AngleAt(i);
            Console.WriteLine($"{i:0.0}: Angle {angle}");
        }
    }
}