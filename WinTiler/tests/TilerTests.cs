using System.Drawing;
using System.Collections.Generic;
using WinTiler.Core;
using Xunit;

public class TilerTests {
    [Fact]
    public void TilerDoesNotThrow() {
        var cfg = new Config();
        var t = new Tiler(cfg);
        var list = new List<ManagedWindow>();
        var rect = new Rectangle(0,0,800,600);
        t.ApplyLayout(list, rect); // should not throw
    }
}
