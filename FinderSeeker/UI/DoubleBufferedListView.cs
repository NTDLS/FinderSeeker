namespace FinderSeeker.UI
{
    /// <summary>
    /// A list view that does not flicker when it is repainted.
    /// </summary>
    public class DoubleBufferedListView : ListView
    {
        public DoubleBufferedListView()
        {
            DoubleBuffered = true;
        }
    }
}
