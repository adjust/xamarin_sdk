
internal class AdjustUtil {
    internal static void iterateTwoPairList(
        List<string>? twoPairList, Action<string, string> twoPairApply) 
    {
        if (twoPairList is null) { return; }

        for (int i = 0; i + 1 < twoPairList.Count; i = i + 2) {
            twoPairApply(twoPairList[i], twoPairList[i + 1]);
        }
    }
}