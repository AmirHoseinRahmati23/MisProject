using MudBlazor;

namespace ThemeStatics;

public static class DefaultSnackBars
{
    public static void AddUnknownError(this ISnackbar snackBar, Action<SnackbarOptions> options = null!)
    {
        var markdown = "<h3>ماسفانه خطایی رخ داده است</h3>" +
                       "<ul>" +
                       "<li>- اتصال اینترنت خود را بررسی کنید</li>" +
                       "<li>در صورت برطرف نشدن مشکل دقایلی دیگر دوباره امتحان کنید</li>";
        snackBar.Add(markdown, Severity.Error, options);
    }

    public static void AddErrorList(this ISnackbar snackBar, IEnumerable<string> errorList, Action<SnackbarOptions> options = null!)
    {
        var itemsMarkup = string.Empty;
        foreach (var error in errorList)
        {
            itemsMarkup += $"<li>{error}</li>";
        }
        var markup = $@"<div><h3>خطا:</h3><ul>{itemsMarkup}</ul></div>";

        snackBar.Add(markup, Severity.Error, options);
    }
}