using System;
using System.Collections.Generic;
using ADODB;
using InsERT;

public static class CountriesCodes
{
    public static Dictionary<string, int> LoadCountries(Subiekt subiekt)
    {
        var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        string sql = "SELECT pa_Id, pa_Nazwa FROM sl_Panstwo";

        var conn = (Connection)subiekt.Baza.Polaczenie;
        var rs = new Recordset();

        rs.Open(sql, conn, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly);

        while (!rs.EOF)
        {
            int id = Convert.ToInt32(rs.Fields["pa_Id"].Value);
            string name = rs.Fields["pa_Nazwa"].Value.ToString();

            if (!dict.ContainsKey(name))
                dict[name] = id;

            rs.MoveNext();
        }

        rs.Close();
        return dict;
    }
}
