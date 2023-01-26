namespace YZPortal.API.Controllers.Memberships.Invites
{
    public static class InvitesHelper
    {
        public static readonly string emptyErrorMsg = false.ToString();

        public static string FormulateErrorMsgStringFormulator(this string errorMessage, int rowIndex, string errorMsgType)
        {
            return errorMessage == emptyErrorMsg ? $"Line number {rowIndex} contains {errorMsgType}."
                : errorMessage.Remove(errorMessage.Length - 1, 1) + $", {errorMsgType}.";
        }

        public static List<Import.Sheet> FormulateErrorMsg(this Import.Sheet row, List<Import.Sheet> failedOutput, string errorMsgType)
        {
            row.HasError = true;

            if (failedOutput.FirstOrDefault(y => y.Index == row.Index) != null)
            {
                var fO = failedOutput.FirstOrDefault(y => y.Index == row.Index);
                fO.ErrorMsg = fO.ErrorMsg.FormulateErrorMsgStringFormulator(row.Index, errorMsgType);
            }
            else
            {
                row.ErrorMsg = row.ErrorMsg.FormulateErrorMsgStringFormulator(row.Index, errorMsgType);
                failedOutput.Add(row);
            }

            return failedOutput;
        }

    }
}
