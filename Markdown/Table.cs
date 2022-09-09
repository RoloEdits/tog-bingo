﻿using System.Text;

namespace Markdown;

public class Table
{
    private byte Columns { get; init; }
    private byte Rows { get; init; }
    private byte Bonus { get; init; }
    private short ColumnOffset { get; init; }
    private short BonusColumns { get; init; }

    public Table(byte columns, byte rows, byte bonus)
    {
        Columns = columns;
        Rows = rows;
        Bonus = bonus;
        ColumnOffset = (short)(Columns + 1);
        BonusColumns = (short)(Columns - Bonus);
    }

    public static string CreateDynamic<T>(string corner, List<T> data, Table table)
    {
        var builder = new StringBuilder();

        WriteHeader(corner, table, builder);
        WriteDivider(table, builder);
        WriteRows(table, data, builder);

        return builder.ToString();
    }

    private static void WriteHeader(string corner, Table table, StringBuilder builder)
    {
        for (var headerColumn = 0; headerColumn < table.ColumnOffset; headerColumn++)
        {
            if (headerColumn == 0)
            {
                builder.Append($"| {corner} |");
            }
            else if (headerColumn >= table.BonusColumns + 1)
            {
                builder.Append(" Bonus |");
            }
            else
            {
                builder.Append($" {headerColumn.ToString()} |");
            }
        }

        builder.Append(Environment.NewLine);
    }

    private static void WriteDivider(Table table, StringBuilder builder)
    {
        for (var i = 0; i < table.ColumnOffset; i++)
        {
            builder.Append(" :---: |");
        }

        builder.Append(Environment.NewLine);
    }

    private static void WriteRows<T>(Table table, IReadOnlyList<T> data, StringBuilder builder)
    {
        var absoluteIndex = 0;
        short endColumnSquare = table.Columns;

        var labels = GetLabel(table.Rows);

        for (var row = 0; row < table.Rows; row++)
        {
            // Writes out row label.
            builder.Append($"| **{labels[row]}** |");
            // Writes out each square.
            for (var currentIndex = absoluteIndex; currentIndex < endColumnSquare; currentIndex++)
            {
                // Writes out each square's value.
                builder.Append($" {data[currentIndex]} |");

                absoluteIndex = currentIndex + 1;
            }

            endColumnSquare += table.Columns;
            // Goes to next row.
            builder.Append(Environment.NewLine);
        }
    }

    private static List<string> GetLabel(byte rows)
    {
        var label = new List<string>();
        if (rows < 27)
        {
            for (var ones = 'A'; ones <= 'Z'; ones++)
            {
                label.Add($"{ones}");
            }

            return label;
        }
        else
        {
            for (var ones = 'A'; ones <= 'Z'; ones++)
            {
                label.Add($"{ones}");
            }

            for (var tens = 'A'; tens <= 'Z'; tens++)
            {
                for (var ones = 'A'; ones <= 'Z'; ones++)
                {
                    label.Add($"{tens}{ones}");
                    if (label.Count >= 64)
                    {
                        return label;
                    }
                }
            }

            return label;
        }
    }
}