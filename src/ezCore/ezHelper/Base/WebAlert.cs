namespace ez.Core.Helpers
{
    public static class WebAlert
    {
        public static string ShowIframe(string url, string title, int width, bool showClose, string closed = "null", string callback = "")
        {
            return string.Format("ShowIframe('{0}','{1}',{2},{3},{4});{1}", url, title, width, showClose.ToString().ToLower(), closed);
        }

        public static string ShowTipLoading(string msg, bool closePage = false, bool isParent = true, string callback = "", bool isRefresh = true)
        {
            return isParent
                ? string.Format(closePage ? "ParentShowTipLoading('{0}');ParentCloseMessage({2});{1}" : "ShowTipLoading('{0}');{1}", msg, callback, isRefresh == true ? 1 : 0)
                : string.Format(closePage ? "ShowTipLoading('{0}');CloseMessage({2});{1}" : "ShowTipLoading('{0}');{1}", msg, callback, isRefresh == true ? 1 : 0);
        }

        public static string ShowTipMessage(string msg, int iconType, bool closePage = false, bool isParent = true, string callback = "", bool isRefresh = true)
        {
            return isParent
                ? string.Format(closePage ? "ParentShowTip('{0}',{1}); setTimeout(function() {{ParentCloseMessage({3});{2}}}, 1500);" : "ParentShowTip('{0}',{1});{2}", msg, iconType, callback, isRefresh == true ? 1 : 0)
                : string.Format(closePage ? "ShowTip('{0}',{1}); setTimeout(function() {{CloseMessage({3});{2}}}, 1500);" : "ShowTip('{0}',{1});{2}", msg, iconType, callback, isRefresh == true ? 1 : 0);
        }
    }
}
