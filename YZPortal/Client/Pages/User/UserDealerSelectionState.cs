namespace YZPortal.Client.Pages.User
{
    public class UserDealerSelectionState
    {
        public bool ShowingConfigureDialog { get; private set; }
        public YZPortal.Client.Models.Dealers.Dealers dealers { get; private set; } = new YZPortal.Client.Models.Dealers.Dealers();

        public void ShowDealerSelectionDialog()
        {

            ShowingConfigureDialog = true;
        }

        public void CancelDealerSelectionDialog()
        {
            ShowingConfigureDialog = false;
        }
    }
}
