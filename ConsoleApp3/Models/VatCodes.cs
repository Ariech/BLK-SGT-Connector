using ADODB;
using System;
using System.Collections.Generic;


    public static class VatCodes
    {

    public static Dictionary<string, int> LoadVatRates(dynamic subiekt)
    {
        var vatRates = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        string sql = "SELECT vat_Id, vat_Symbol FROM dbo.sl_StawkaVAT";

        var conn = (Connection)subiekt.Baza.Polaczenie;
        var rs = new Recordset();

        rs.Open(sql, conn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly);

        while (!rs.EOF)
        {
            int id = Convert.ToInt32(rs.Fields["vat_Id"].Value);
            string symbol = rs.Fields["vat_Symbol"].Value.ToString();

            if (!vatRates.ContainsKey(symbol))
                vatRates.Add(symbol, id);

            rs.MoveNext();
        }

        rs.Close();
        return vatRates;
    }
}

