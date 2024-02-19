using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

// Tracks operation performance
// Implemented by using(new PerformanceMonitor("WatchName")){...}
public class PerformanceMonitor : IDisposable {
    private readonly string _text;
    private readonly Stopwatch _watch;
	
    public PerformanceMonitor(string text) {
        _text = text;
        _watch = Stopwatch.StartNew();
    }
	
    public void Dispose() {
        _watch.Stop();
        Debug.Log($"{_text}: {_watch.ElapsedMilliseconds}");
    }
}